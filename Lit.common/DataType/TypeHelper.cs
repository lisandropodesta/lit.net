namespace Lit.DataType
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    public static class TypeHelper
    {
        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, object> GetPropertiesValues<TAttr>(object obj) where TAttr : class
        {
            var props = GetProperties<TAttr>(obj.GetType());
            return GetPropertiesValues(props, obj);
        }

        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, object> GetPropertiesValues(IEnumerable<PropertyInfo> props, object obj)
        {
            var dict = new Dictionary<string, object>();
            return props?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.Name, p.GetValue(obj));
                return d;
            });
        }

        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, object> GetPropertiesValues<TAttr>(IEnumerable<ReflectionProperty<TAttr>> props, object obj) where TAttr : class
        {
            var dict = new Dictionary<string, object>();
            return props?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.PropertyInfo.Name, p.PropertyInfo.GetValue(obj));
                return d;
            });
        }

        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, object> GetPropertiesValues<TAttr>(IReflectionProperties<TAttr> props, object obj) where TAttr : class
        {
            var dict = new Dictionary<string, object>();
            return props?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.Key, p.Value.PropertyInfo.GetValue(obj));
                return d;
            });
        }

        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, Tuple<TAttr, object>> GetPropertiesValuesWithAttr<TAttr>(IEnumerable<ReflectionProperty<TAttr>> props, object obj) where TAttr : class
        {
            var dict = new Dictionary<string, Tuple<TAttr, object>>();
            return props?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.PropertyInfo.Name, new Tuple<TAttr, object>(p.Attribute, p.PropertyInfo.GetValue(obj)));
                return d;
            });
        }

        /// <summary>
        /// Get a list of properties having a custom attribute with their values.
        /// </summary>
        public static IDictionary<string, Tuple<TAttr, object>> GetPropertiesValuesWithAttr<TAttr>(IReflectionProperties<TAttr> props, object obj) where TAttr : class
        {
            var dict = new Dictionary<string, Tuple<TAttr, object>>();
            return props?.Aggregate(dict, (d, p) =>
            {
                d.Add(p.Key, new Tuple<TAttr, object>(p.Value.Attribute, p.Value.PropertyInfo.GetValue(obj)));
                return d;
            });
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static IList<PropertyInfo> SetPropertiesValues<TAttr>(object obj, IDictionary<string, object> values, bool checkChanged = false) where TAttr : class
        {
            var props = GetProperties<TAttr>(obj.GetType());
            return SetPropertiesValues(props, obj, values, checkChanged);
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static IList<PropertyInfo> SetPropertiesValues(IEnumerable<PropertyInfo> props, object obj, IDictionary<string, object> values, bool checkChanged = false)
        {
            var changed = checkChanged ? new List<PropertyInfo>() : null;
            if (props != null)
            {
                foreach (var p in props)
                {
                    if (values.TryGetValue(p.Name, out object value))
                    {
                        if (SetPropertyValue(p, obj, value, checkChanged))
                        {
                            changed.Add(p);
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static IList<PropertyInfo> SetPropertiesValues<TAttr>(IEnumerable<ReflectionProperty<TAttr>> props, object obj, IDictionary<string, object> values, bool checkChanged = false) where TAttr : class
        {
            var changed = checkChanged ? new List<PropertyInfo>() : null;
            if (props != null)
            {
                foreach (var p in props)
                {
                    if (values.TryGetValue(p.PropertyInfo.Name, out object value))
                    {
                        if (SetPropertyValue(p.PropertyInfo, obj, value, checkChanged))
                        {
                            changed.Add(p.PropertyInfo);
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static IList<PropertyInfo> SetPropertiesValues<TAttr>(IReflectionProperties<TAttr> props, object obj, IDictionary<string, object> values, bool checkChanged = false) where TAttr : class
        {
            var changed = checkChanged ? new List<PropertyInfo>() : null;
            if (props != null)
            {
                foreach (var p in props)
                {
                    if (values.TryGetValue(p.Key, out object value))
                    {
                        if (SetPropertyValue(p.Value.PropertyInfo, obj, value, checkChanged))
                        {
                            changed.Add(p.Value.PropertyInfo);
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static bool SetPropertyValue<TAttr>(IEnumerable<ReflectionProperty<TAttr>> props, string propName, object obj, object value, bool checkChanged = false) where TAttr : class
        {
            var item = props?.FirstOrDefault(i => i.PropertyInfo.Name == propName);
            return SetPropertyValue(item?.PropertyInfo, obj, value, checkChanged);
        }

        /// <summary>
        /// Set properties having a custom attribute with a dictionary value.
        /// </summary>
        public static bool SetPropertyValue<TAttr>(IReflectionProperties<TAttr> props, string propName, object obj, object value, bool checkChanged = false) where TAttr : class
        {
            var item = props?.FirstOrDefault(i => i.Value.PropertyInfo.Name == propName);
            return SetPropertyValue(item?.Value.PropertyInfo, obj, value, checkChanged);
        }

        /// <summary>
        /// Set a property of a object optionally detecting changes.
        /// </summary>
        public static bool SetPropertyValue(PropertyInfo propinfo, object obj, object value, bool checkChanged = false)
        {
            if (propinfo != null)
            {
                var changed = !checkChanged;
                if (checkChanged)
                {
                    var prevValue = propinfo.GetValue(obj);
                    changed = !value.Equals(prevValue);
                    checkChanged = !changed;
                }

                propinfo.SetValue(obj, value);
                return changed;
            }

            return false;
        }

        /// <summary>
        /// Get properties.
        /// </summary>
        public static IReflectionProperties GetPropertiesDict(Type type)
        {
            var dict = new ReflectionProperties();
            return type?.GetRuntimeProperties()?.Aggregate(dict, (d, p) =>
            {
                d[p.Name] = new ReflectionProperty(p);
                return d;
            });
        }

        /// <summary>
        /// Get properties having a custom attribute.
        /// </summary>
        public static IReflectionProperties<TAttr> GetPropertiesDict<TAttr>(Type type) where TAttr : class
        {
            var dict = new ReflectionProperties<TAttr>();
            return type?.GetRuntimeProperties()?.Aggregate(dict, (d, p) =>
            {
                TAttr attr = null;
                if (typeof(TAttr) == typeof(object) || (attr = GetAttribute<TAttr>(p)) != null)
                {
                    d[p.Name] = new ReflectionProperty<TAttr>(p, attr);
                }

                return d;
            });
        }

        /// <summary>
        /// Get a list of properties info that has a custom attribute.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetProperties<TAttr>(Type type) where TAttr : class
        {
            return type?.GetRuntimeProperties()?.Where(i => HasAttribute<TAttr>(i));
        }

        /// <summary>
        /// Get a class property info by an enum name.
        /// </summary>
        public static PropertyInfo GetPropertyInfo<TEnum>(Type type, TEnum enumValue)
        {
            var name = Enum.GetName(typeof(TEnum), enumValue);
            return GetPropertyInfo(type, name);
        }

        /// <summary>
        /// Get a class property info by name.
        /// </summary>
        public static PropertyInfo GetPropertyInfo(Type type, string name)
        {
            return !string.IsNullOrEmpty(name) ? type.GetRuntimeProperty(name) : null;
        }

        /// <summary>
        /// Get a list of fields info that has a custom attribute, attribute value is also returned.
        /// </summary>
        public static IEnumerable<ReflectionMember<TAttr>> GetFieldsWithAttr<TAttr>(Type type) where TAttr : class
        {
            return type?.GetRuntimeFields()?.Select(i =>
            {
                var attr = GetAttribute<TAttr>(i);
                return attr != null ? new ReflectionMember<TAttr>(i, attr) : null;
            }).Where(t => t != null);
        }

        /// <summary>
        /// Get a list of fields info that has a custom attribute.
        /// </summary>
        public static IEnumerable<FieldInfo> GetFields<TAttr>(Type type) where TAttr : class
        {
            return type?.GetRuntimeFields()?.Where(i => HasAttribute<TAttr>(i));
        }

        /// <summary>
        /// Determines whether an enum value has an attribute or not.
        /// </summary>
        public static bool FieldHasAttribute<TAttr>(Type enumType, object value) where TAttr : class
        {
            return GetFieldAttribute<TAttr>(enumType, value) != null;
        }

        /// <summary>
        /// Determines whether an enum value has an attribute or not.
        /// </summary>
        public static bool FieldHasAttribute<TAttr>(Type type, string name) where TAttr : class
        {
            return GetAttribute<TAttr>(type.GetRuntimeField(name)) != null;
        }

        /// <summary>
        /// Get an attribute from an enum value.
        /// </summary>
        public static TAttr GetFieldAttribute<TAttr>(Type enumType, object value) where TAttr : class
        {
            if (IsEnumType(enumType))
            {
                return GetFieldAttribute<TAttr>(enumType, Enum.GetName(enumType, value));
            }

            return null;
        }

        /// <summary>
        /// Get an attribute from an enum value.
        /// </summary>
        public static TAttr GetFieldAttribute<TAttr>(Type type, string name) where TAttr : class
        {
            return GetAttribute<TAttr>(type.GetRuntimeField(name));
        }

        /// <summary>
        /// Determines whether a member info has an attribute or not.
        /// </summary>
        public static bool HasAttribute<TAttr>(MemberInfo memberInfo) where TAttr : class
        {
            return GetAttribute<TAttr>(memberInfo) != null;
        }

        /// <summary>
        /// Get an attribute from a member info.
        /// </summary>
        public static TAttr GetAttribute<TAttr>(MemberInfo memberInfo) where TAttr : class
        {
            if (memberInfo != null)
            {
                var attrs = CustomAttributeExtensions.GetCustomAttributes(memberInfo);
                if (attrs != null)
                {
                    foreach (var attr in attrs)
                    {
                        if (attr is TAttr)
                        {
                            return attr as TAttr;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether a type has an attribute or not.
        /// </summary>
        public static bool HasAttribute<TAttr>(Type type) where TAttr : class
        {
            return GetAttribute<TAttr>(type) != null;
        }

        /// <summary>
        /// Get an attribute from a type.
        /// </summary>
        public static TAttr GetAttribute<TAttr>(Type type, bool inherit = false) where TAttr : class
        {
            if (type != null)
            {
                var attrs = type.GetTypeInfo().GetCustomAttributes(inherit);
                if (attrs != null)
                {
                    foreach (var attr in attrs)
                    {
                        if (attr is TAttr)
                        {
                            return attr as TAttr;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether a type is an enum.
        /// </summary>
        public static bool IsEnumType(Type type)
        {
            return type != null && type.GetTypeInfo().IsEnum;
        }
    }
}
