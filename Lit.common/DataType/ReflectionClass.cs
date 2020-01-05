using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// ReflectionClass is a base class that inspects its properties looking for an attribute and builds a property list that allows
    /// automated interaction.
    /// </summary>
    public class ReflectionClass<T> where T : Attribute
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event EventHandler<PropertyInfo> PropertyChanged;

        /// <summary>
        /// Static dictionary to avoid repetition of information between several instances of the same class.
        /// </summary>
        private readonly static IDictionary<Type, IReflectionProperties<T>> typesDict = new Dictionary<Type, IReflectionProperties<T>>();

        /// <summary>
        /// Dictionary of properties with associated attributes.
        /// </summary>
        protected readonly IReflectionProperties<T> properties;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReflectionClass()
        {
            var type = GetType();
            properties = GetProperties(type);
        }

        /// <summary>
        /// Get the properties of a generic type.
        /// </summary>
        public static IReflectionProperties<T> GetProperties(Type type)
        {
            IReflectionProperties<T> properties;

            lock (typesDict)
            {
                if (!typesDict.ContainsKey(type))
                {
                    properties = TypeHelper.GetPropertiesDict<T>(type);
                    typesDict.Add(type, properties);
                }
                else
                {
                    properties = typesDict[type];
                }
            }

            return properties;
        }

        /// <summary>
        /// Get information of a property.
        /// </summary>
        public ReflectionProperty<T> GetProperty(string name)
        {
            return properties[name];
        }

        /// <summary>
        /// Get a dictionary of properties and associated attributes.
        /// </summary>
        public IDictionary<string, T> GetPropertiesDict()
        {
            var dict = new Dictionary<string, T>();
            return properties?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.Value.PropertyInfo.Name, p.Value.Attribute);
                return d;
            });
        }

        /// <summary>
        /// Get current values as a dictionary.
        /// </summary>
        public IDictionary<string, object> GetPropertiesValues()
        {
            return TypeHelper.GetPropertiesValues(properties, this);
        }

        /// <summary>
        /// Get current values as a dictionary.
        /// </summary>
        public IDictionary<string, Tuple<T, object>> GetPropertiesValuesWithAttr()
        {
            return TypeHelper.GetPropertiesValuesWithAttr(properties, this);
        }

        /// <summary>
        /// Copy current values from a similar object.
        /// </summary>
        public void SetPropertiesValues(ReflectionClass<T> source)
        {
            foreach (var p in properties)
            {
                var pi = p.Value.PropertyInfo;
                SetPropertyValue(pi, pi.GetValue(source));
            }
        }

        /// <summary>
        /// Set current values with a dictionary.
        /// </summary>
        public void SetPropertiesValues(IDictionary<string, object> values)
        {
            foreach (var i in values)
            {
                var pi = properties[i.Key];
                if (pi != null)
                {
                    SetPropertyValue(pi.PropertyInfo, i.Value);
                }
            }
        }

        /// <summary>
        /// Set current value of a property.
        /// </summary>
        protected void SetPropertyValue(string name, object value)
        {
            var item = properties[name];
            if (item != null)
            {
                SetPropertyValue(item.PropertyInfo, value);
            }
        }

        /// <summary>
        /// Get the value of a property.
        /// </summary>
        protected object GetPropertyValue(string propName)
        {
            var item = properties[propName];
            if (item != null)
            {
                return GetPropertyValue(item.PropertyInfo);
            }

            return null;
        }

        /// <summary>
        /// Get the value of a property.
        /// </summary>
        protected object GetPropertyValue(PropertyInfo info)
        {
            return info.GetValue(this);
        }

        /// <summary>
        /// Set the value of a property.
        /// </summary>
        protected void SetPropertyValue(PropertyInfo info, object value)
        {
            var prevValue = info.GetValue(this);
            if (!prevValue.Equals(value))
            {
                info.SetValue(this, value);
                NotifyPropertyChanged(info);
            }
        }

        /// <summary>
        /// Notify a property changed.
        /// </summary>
        private void NotifyPropertyChanged(PropertyInfo info)
        {
            OnPropertyChanged(info);

            try
            {
                PropertyChanged?.Invoke(this, info);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Internal event on property changed.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyInfo info)
        {
        }
    }
}
