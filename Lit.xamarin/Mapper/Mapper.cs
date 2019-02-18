namespace Lit.xamarin
{
    using System;
    using Xamarin.Forms;
    using Lit.DataType;

    public static class Mapper
    {
        public static object Create(Element parent, object data)
        {
            return Create(parent, data.GetType(), data);
        }

        public static Element Create(Element parent, Type template, object data)
        {
            /*if (IsVisualObjectType(templateType))
            {
                var control = (VisualElement)Activator.CreateInstance(templateType);
                control.Parent = parent;

                AssignProperties(control, data);
                // < Label Text = "Welcome to Lit.xamarin!" HorizontalOptions = "Center" VerticalOptions = "CenterAndExpand" />

                return control;
            }*/

            return SetupControl(parent, template, data);
        }

        /*private static void AssignProperties(Element control, object data)
        {
            //var x = TypeHelper.GetPropertiesDict<LitUiAttribute>(control.GetType());
        }*/

        /*private static bool IsVisualObjectType(Type type)
        {
            return type != null && type.IsSubclassOf(typeof(VisualElement));
        }*/

        /// <summary>
        /// Setup a control, it may create a new one.
        /// </summary>
        private static Element SetupControl(Element parent, Type template, object data)
        {
            var attr = TypeHelper.GetAttribute<LitUiAttribute>(template);
            return SetupControl(parent, attr, data);
        }

        /// <summary>
        /// Setup a control, it may create a new one.
        /// </summary>
        private static Element SetupControl(Element parent, LitUiAttribute attr, object data)
        {
            var control = attr != null ? CreateControl(attr.CtrlType) : null;

            if (control != null)
            {
                control.Parent = parent;
            }
            else if (attr == null || !attr.CtrlType.HasValue)
            {
                control = parent;
            }

            UpdateControl(control, attr, data);

            return control;
        }

        /// <summary>
        /// Create a new control.
        /// </summary>
        private static Element CreateControl(ControlType? controlType)
        {
            switch (controlType ?? ControlType.None)
            {
                case ControlType.None:
                    return null;

                case ControlType.Label:
                    return new Label();

                case ControlType.Button:
                    return new Button();

                case ControlType.Container:
                default:
                    return new Grid();
            }
        }

        /// <summary>
        /// Setup control properties.
        /// </summary>
        private static void SetupControl(Element control, LitUiAttribute attr)
        {
            var controlType = attr?.CtrlType ?? GetControlType(control);
            var controlProps = controlType != ControlType.None ? ReflectionPropertiesCache.Get(control.GetType()) : null;

            switch (controlType)
            {
                case ControlType.None:
                    break;

                case ControlType.Label:
                    SetupLabel(control as Label, controlProps, attr);
                    break;

                case ControlType.Button:
                    SetupButton(control as Button, controlProps, attr);
                    break;

                case ControlType.Container:
                    SetupContainer(control as Layout, controlProps, attr);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Update control properties.
        /// </summary>
        private static bool UpdateControl(Element control, LitUiAttribute attr, object data)
        {
            var controlType = attr?.CtrlType ?? GetControlType(control);
            var controlProps = controlType != ControlType.None ? ReflectionPropertiesCache.Get(control.GetType()) : null;

            var dataProps = data != null ? ReflectionPropertiesCache<LitUiAttribute>.Get(data.GetType()) : null;

            switch (controlType)
            {
                case ControlType.None:
                    return false;

                case ControlType.Label:
                    return UpdateLabel(control as Label, controlProps, data, dataProps);

                case ControlType.Button:
                    return UpdateButton(control as Button, controlProps, data, dataProps);

                case ControlType.Container:
                    return UpdateContainer(control as Layout, controlProps, data, dataProps);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Setup a Label control.
        /// </summary>
        private static void SetupLabel(Label control, IReflectionProperties controlProps, LitUiAttribute attr)
        {
            if (attr != null)
            {
                UpdatePropertyValue(control, controlProps, @"Text", attr.Text, true);
            }
        }

        /// <summary>
        /// Update a Label control.
        /// </summary>
        private static bool UpdateLabel(Label control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            foreach (var p in dataProps)
            {
                if (p.Value.Attribute.CtrlType.HasValue)
                {
                    switch (p.Value.Attribute.Property)
                    {

                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Setup a Button control.
        /// </summary>
        private static bool SetupButton(Button control, IReflectionProperties controlProps, LitUiAttribute attr)
        {
            var changed = false;
            if (attr != null)
            {
                if (attr.Text != null)
                {
                    if (control.Text != attr.Text)
                    {
                        control.Text = attr.Text;
                        changed = true;
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Update a Button control.
        /// </summary>
        private static bool UpdateButton(Button control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            return changed;
        }

        /// <summary>
        /// Setup a container control.
        /// </summary>
        private static void SetupContainer(Layout control, IReflectionProperties controlProps, LitUiAttribute attr)
        {
            if (attr != null)
            {
            }
        }

        /// <summary>
        /// Update a container control.
        /// </summary>
        private static bool UpdateContainer(Layout control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            return changed;
        }

        /// <summary>
        /// Update a control property's value usign reflection information.
        /// </summary>
        private static bool UpdatePropertyValue(object control, IReflectionProperties props, string propName, object value, bool onlyIfNotNull)
        {
            if (!onlyIfNotNull || value != null)
            {
                if (props.TryGetValue(propName, out ReflectionProperty prop))
                {
                    var currValue = prop.PropertyInfo.GetValue(control, null);
                    if (currValue != value)
                    {
                        prop.PropertyInfo.SetValue(control, value, null);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the type of a control assuming attr is right.
        /// </summary>
        private static ControlType? GetControlType(Element control)
        {
            if (control is Label)
                return ControlType.Label;

            if (control is Button)
                return ControlType.Button;

            if (control is Layout)
                return ControlType.Container;

            return null;
        }
    }
}
