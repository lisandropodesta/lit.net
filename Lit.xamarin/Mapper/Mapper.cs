namespace Lit.xamarin
{
    using System;
    using Xamarin.Forms;
    using Lit.DataType;
    using System.Reflection;

    public static class Mapper
    {
        public static object Create(Layout<View> parent, object data)
        {
            return Create(parent, data.GetType(), data);
        }

        public static View Create(Layout<View> parent, Type template, object data)
        {
            /*if (IsVisualObjectType(templateType))
            {
                var control = (VisualElement)Activator.CreateInstance(templateType);
                control.Parent = parent;

                AssignProperties(control, data);
                // < Label Text = "Welcome to Lit.xamarin!" HorizontalOptions = "Center" VerticalOptions = "CenterAndExpand" />

                return control;
            }*/

            return SetupControl(parent, template, null, data);
        }

        /*private static void AssignProperties(View control, object data)
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
        private static View SetupControl(Layout<View> parent, Type template, PropertyInfo propInfo, object data)
        {
            var attr = TypeHelper.GetAttribute<LitUiAttribute>(template);
            return SetupControl(parent, attr, propInfo, data);
        }

        /// <summary>
        /// Setup a control, it may create a new one.
        /// </summary>
        private static View SetupControl(Layout<View> parent, LitUiAttribute attr, PropertyInfo propInfo, object data)
        {
            var control = attr != null ? CreateControl(attr.CtrlType) : null;

            if (control != null)
            {
                if (attr.HasGridSpecification && parent is Grid grid)
                {
                    var left = attr.Col;
                    var right = attr.Col + attr.ColSpan < 0 ? grid.ColumnDefinitions.Count + attr.ColSpan : attr.ColSpan - 1;
                    var top = attr.Row;
                    var bottom = attr.Row + attr.RowSpan < 0 ? grid.RowDefinitions.Count + attr.RowSpan : attr.RowSpan - 1;

                    grid.Children.Add(control, attr.Col, attr.Row);

                    var colSpan = attr.ColSpan;
                    if (colSpan < 0)
                    {
                        colSpan = 1 + grid.ColumnDefinitions.Count + attr.ColSpan - attr.Col;
                    }
                    if (colSpan != 1)
                    {
                        Grid.SetColumnSpan(control, colSpan);
                    }

                    var rowSpan = attr.RowSpan;
                    if (rowSpan < 0)
                    {
                        rowSpan = 1 + grid.RowDefinitions.Count + attr.RowSpan - attr.Row;
                    }
                    if (rowSpan != 1)
                    {
                        Grid.SetRowSpan(control, rowSpan);
                    }
                }
                else
                {
                    parent.Children.Add(control);
                }
            }
            else if (attr == null || attr.CtrlType == ControlType.None)
            {
                control = parent;
            }

            UpdateControl(control, attr, propInfo, data, true);

            return control;
        }

        /// <summary>
        /// Create a new control.
        /// </summary>
        private static View CreateControl(ControlType? controlType)
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
        /// Update and optionally setup a control.
        /// </summary>
        private static bool UpdateControl(View control, LitUiAttribute attr, PropertyInfo propInfo, object data, bool setup)
        {
            var controlType = attr?.CtrlType;
            if (controlType == ControlType.None)
            {
                controlType = GetControlType(control);
            }

            var controlProps = controlType != ControlType.None ? ReflectionPropertiesCache.Get(control.GetType()) : null;

            var dataProps = data != null ? ReflectionPropertiesCache<LitUiAttribute>.Get(data.GetType()) : null;

            switch (controlType)
            {
                case ControlType.None:
                    return false;

                case ControlType.Label:
                    {
                        if (control is Label label)
                        {
                            if (setup)
                            {
                                SetupLabel(label, controlProps, attr, propInfo);
                            }

                            return UpdateLabel(label, controlProps, data, dataProps);
                        }

                        return false;
                    }

                case ControlType.Button:
                    {
                        if (control is Button button)
                        {
                            if (setup)
                            {
                                SetupButton(button, controlProps, attr, propInfo);
                            }

                            return UpdateButton(button, controlProps, data, dataProps);
                        }

                        return false;
                    }

                case ControlType.Container:
                    {
                        if (control is Layout<View> container)
                        {
                            if (setup)
                            {
                                SetupContainer(container, controlProps, attr, propInfo, data, dataProps);
                            }

                            return UpdateContainer(container, controlProps, data, dataProps);
                        }

                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// Setup a Label control.
        /// </summary>
        private static void SetupLabel(Label control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo)
        {
            SetupCommon(control, controlProps, attr, propInfo);

            if (attr != null)
            {
                UpdatePropertyValue(control, controlProps, @"Text", attr.Text, true);
                UpdatePropertyValue(control, controlProps, "HorizontalOptions", GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, "VerticalOptions", GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);
            }
        }

        /// <summary>
        /// Update a Label control.
        /// </summary>
        private static bool UpdateLabel(Label control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            if (dataProps != null)
            {
                foreach (var p in dataProps)
                {
                    if (p.Value.Attribute.CtrlType != ControlType.None)
                    {
                        switch (p.Value.Attribute.Property)
                        {
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Setup a Button control.
        /// </summary>
        private static void SetupButton(Button control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo)
        {
            SetupCommon(control, controlProps, attr, propInfo);

            if (attr != null)
            {
                var text = attr.Text ?? attr.GetAutoCommandParameter(propInfo);
                UpdatePropertyValue(control, controlProps, @"Text", text, true);

                UpdatePropertyValue(control, controlProps, "HorizontalOptions", GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, "VerticalOptions", GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);
            }
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
        private static void SetupContainer(Layout<View> control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo containerPropInfo,
            object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            SetupCommon(control, controlProps, attr, containerPropInfo);

            if (dataProps != null)
            {
                for (var step = 1; step <= 2; step++)
                {
                    foreach (var pair in dataProps)
                    {
                        var reflectionProperty = pair.Value;
                        var propInfo = reflectionProperty.PropertyInfo;
                        var propValue = propInfo.GetValue(data, null);
                        var propAttr = reflectionProperty.Attribute;

                        switch (step)
                        {
                            case 1:
                                {
                                    if (propAttr.HasGridSpecification && control is Grid grid)
                                    {
                                        var maxCol = propAttr.Col + (propAttr.ColSpan > 0 ? propAttr.ColSpan - 1 : 0);
                                        while (grid.ColumnDefinitions.Count <= maxCol)
                                        {
                                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                                        }

                                        var maxRow = propAttr.Row + (propAttr.RowSpan > 0 ? propAttr.RowSpan - 1 : 0);
                                        while (grid.RowDefinitions.Count <= maxRow)
                                        {
                                            grid.RowDefinitions.Add(new RowDefinition());
                                        }
                                    }
                                }
                                break;

                            case 2:
                                {
                                    /*if (propInfo.PropertyType.IsSubclassOf(typeof(VisualProperty<>)))
                                    {

                                    }*/

                                    var child = SetupControl(control, propAttr, propInfo, propValue);
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update a container control.
        /// </summary>
        private static bool UpdateContainer(Layout<View> control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
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
        /// Common setup for a control.
        /// </summary>
        private static void SetupCommon(View control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo)
        {
            if (attr != null)
            {
            }
        }

        /// <summary>
        /// Get the type of a control assuming attr is right.
        /// </summary>
        private static ControlType? GetControlType(View control)
        {
            if (control is Label)
                return ControlType.Label;

            if (control is Button)
                return ControlType.Button;

            if (control is Layout)
                return ControlType.Container;

            return null;
        }

        /// <summary>
        /// Translate text options to horizontal LayoutOptions
        /// </summary>
        private static LayoutOptions? GetHorizontalOptions(LayoutMode mode, LayoutMode textMode)
        {
            var alignment = GetHorizontalAlignment(textMode != LayoutMode.None ? textMode : mode);
            if (alignment.HasValue)
            {
                var expands = GetHorizontalExpands(mode);
                return new LayoutOptions(alignment.Value, expands);
            }

            return null;
        }

        /// <summary>
        /// Translate text options to vertical LayoutOptions
        /// </summary>
        private static LayoutOptions? GetVerticalOptions(LayoutMode mode, LayoutMode textMode)
        {
            var alignment = GetVerticalAlignment(textMode != LayoutMode.None ? textMode : mode);
            if (alignment.HasValue)
            {
                var expands = GetVerticalExpands(mode);
                return new LayoutOptions(alignment.Value, expands);
            }

            return null;
        }

        /// <summary>
        /// Translate a layout mode to horizontal LayoutAlignment
        /// </summary>
        private static LayoutAlignment? GetHorizontalAlignment(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.TopLeft:
                case LayoutMode.Left:
                case LayoutMode.LeftCenter:
                case LayoutMode.BottomLeft:
                    return LayoutAlignment.Start;

                case LayoutMode.TopCenter:
                case LayoutMode.Center:
                case LayoutMode.BottomCenter:
                    return LayoutAlignment.Center;

                case LayoutMode.TopRight:
                case LayoutMode.Right:
                case LayoutMode.RightCenter:
                case LayoutMode.BottomRight:
                    return LayoutAlignment.End;

                case LayoutMode.Top:
                case LayoutMode.Bottom:
                case LayoutMode.Fill:
                    return LayoutAlignment.Fill;

                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return null;
            }
        }

        /// <summary>
        /// Translate a layout mode to vertical LayoutAlignment
        /// </summary>
        private static LayoutAlignment? GetVerticalAlignment(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.Top:
                case LayoutMode.TopLeft:
                case LayoutMode.TopCenter:
                case LayoutMode.TopRight:
                    return LayoutAlignment.Start;

                case LayoutMode.Left:
                case LayoutMode.LeftCenter:
                case LayoutMode.Center:
                case LayoutMode.RightCenter:
                case LayoutMode.Right:
                    return LayoutAlignment.Center;

                case LayoutMode.BottomLeft:
                case LayoutMode.Bottom:
                case LayoutMode.BottomCenter:
                case LayoutMode.BottomRight:
                    return LayoutAlignment.End;

                case LayoutMode.Fill:
                    return LayoutAlignment.Fill;

                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return null;
            }
        }

        /// <summary>
        /// Translate a layout mode to horizontal expansion
        /// </summary>
        private static bool GetHorizontalExpands(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.Top:
                case LayoutMode.Bottom:
                case LayoutMode.Fill:
                    return true;

                case LayoutMode.TopLeft:
                case LayoutMode.Left:
                case LayoutMode.LeftCenter:
                case LayoutMode.BottomLeft:
                case LayoutMode.TopCenter:
                case LayoutMode.Center:
                case LayoutMode.BottomCenter:
                case LayoutMode.TopRight:
                case LayoutMode.Right:
                case LayoutMode.RightCenter:
                case LayoutMode.BottomRight:
                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Translate a layout mode to vertical expansion
        /// </summary>
        private static bool GetVerticalExpands(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.Left:
                case LayoutMode.Right:
                case LayoutMode.Fill:
                    return true;

                case LayoutMode.Top:
                case LayoutMode.Bottom:
                case LayoutMode.TopLeft:
                case LayoutMode.LeftCenter:
                case LayoutMode.BottomLeft:
                case LayoutMode.TopCenter:
                case LayoutMode.Center:
                case LayoutMode.BottomCenter:
                case LayoutMode.TopRight:
                case LayoutMode.RightCenter:
                case LayoutMode.BottomRight:
                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return false;
            }
        }
    }
}
