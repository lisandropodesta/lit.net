using System;
using System.Reflection;
using System.ComponentModel;
using Xamarin.Forms;
using Lit.DataType;

namespace Lit.Ui.Xamarin
{
    public class XamarinMapper : Mapper<Layout<View>, Grid, View>
    {
        private static readonly object theLock = new object();

        /// <summary>
        /// Instance setup.
        /// </summary>
        public static void Initialize()
        {
            lock (theLock)
            {
                if (Instance == null)
                {
                    Instance = new XamarinMapper();
                }
            }
        }

        protected override View CreateControl(ControlType? controlType)
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

        protected override int GetColumnsCount(Grid grid)
        {
            return grid.ColumnDefinitions.Count;
        }

        protected override int GetRowsCount(Grid grid)
        {
            return grid.RowDefinitions.Count;
        }

        protected override void AddChild(Layout<View> parent, View control)
        {
            parent.Children.Add(control);
        }

        protected override void AddChild(Grid grid, View control, int col, int row)
        {
            grid.Children.Add(control, col, row);
        }

        protected override void SetRowSpan(View control, int rowSpan)
        {
            Grid.SetRowSpan(control, rowSpan);
        }

        protected override void SetColumnSpan(View control, int colSpan)
        {
            Grid.SetColumnSpan(control, colSpan);
        }

        protected override ControlType? GetControlType(View control)
        {
            if (control is Label)
                return ControlType.Label;

            if (control is Button)
                return ControlType.Button;

            if (control is Layout)
                return ControlType.Container;

            return null;
        }

        protected override bool UpdateControl(bool setup, ControlType? controlType, View control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
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

                            return UpdateLabel(label, controlProps, attr, propInfo, data, dataProps);
                        }

                        return false;
                    }

                case ControlType.Button:
                    {
                        if (control is Button button)
                        {
                            if (setup)
                            {
                                SetupButton(button, controlProps, attr, propInfo, data, dataProps);
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
                UpdatePropertyValue(control, controlProps, nameof(control.Text), attr.Text, true);
                UpdatePropertyValue(control, controlProps, nameof(control.HorizontalOptions), GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, nameof(control.VerticalOptions), GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);
            }
        }

        /// <summary>
        /// Update a Label control.
        /// </summary>
        private static bool UpdateLabel(Label control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo,
            object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            if (attr != null)
            {
                switch (attr.Property)
                {
                    case TargetProperty.None:
                        break;

                    case TargetProperty.Text:
                        UpdatePropertyValue(control, controlProps, nameof(control.Text), data, true);
                        break;
                }
            }

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
        private static void SetupButton(Button control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo,
            object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            SetupCommon(control, controlProps, attr, propInfo);

            if (attr != null)
            {
                var text = attr.Text ?? attr.GetAutoCommandParameter(propInfo);
                UpdatePropertyValue(control, controlProps, nameof(control.Text), text, true);

                UpdatePropertyValue(control, controlProps, nameof(control.HorizontalOptions), GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, nameof(control.VerticalOptions), GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);

                control.Clicked += delegate (object sender, EventArgs e)
                {
                    if (!string.IsNullOrEmpty(attr.OnClickCommand))
                    {
                        if (dataProps.ContainsKey(attr.OnClickCommand))
                        {
                            var cmdPropInfo = dataProps[attr.OnClickCommand].PropertyInfo;
                            var parameter = attr.CommandParameter ?? attr.GetAutoCommandParameter(propInfo);

                            var ev = new ControlEvent
                            {
                                Sender = sender,
                                CommandName = attr.OnClickCommand,
                                CommandParameter = parameter,
                                Property = dataProps[propInfo.Name]
                            };

                            cmdPropInfo.SetValue(data, ev, null);
                        }
                    }
                };
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
                                    var propValue = propInfo.GetValue(data, null);
                                    var isVisualProperty = TypeHelper.IsSubclassOf(propInfo.PropertyType, typeof(VisualProperty<>));

                                    if (isVisualProperty)
                                    {
                                        if (propValue == null)
                                        {
                                            propValue = Activator.CreateInstance(propInfo.PropertyType);
                                            propInfo.SetValue(data, propValue, null);
                                        }
                                    }

                                    var visualProp = isVisualProperty ? propValue as VisualProperty : null;
                                    var childData = isVisualProperty ? visualProp.GetData() : propAttr.OwnDataContext ? propValue : data;
                                    var child = CreateControl(control, propAttr, propInfo, childData);

                                    if (isVisualProperty)
                                    {
                                        visualProp.Bind(child);
                                        visualProp.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                                        {
                                            UpdateControl(sender as View, propAttr, propInfo, visualProp.GetData(), false);
                                        };
                                    }
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
        /// Common setup for a control.
        /// </summary>
        private static void SetupCommon(View control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo)
        {
            if (attr != null)
            {
            }
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
    }
}
