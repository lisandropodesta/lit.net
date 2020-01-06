using System;
using System.Reflection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Lit.DataType;

namespace Lit.Ui.Wpf
{
    public class WpfMapper : Mapper<Panel, Grid, FrameworkElement>
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
                    Instance = new WpfMapper();
                }
            }
        }

        /// <summary>
        /// Create a new control.
        /// </summary>
        protected override FrameworkElement CreateControl(ControlType? controlType)
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

        protected override void AddChild(Panel parent, FrameworkElement control)
        {
            parent.Children.Add(control);
        }

        protected override void AddChild(Grid grid, FrameworkElement control, int col, int row)
        {
            grid.Children.Add(control);
            Grid.SetRow(control, row);
            Grid.SetColumn(control, col);
        }

        protected override void SetRowSpan(FrameworkElement control, int rowSpan)
        {
            Grid.SetRowSpan(control, rowSpan);
        }

        protected override void SetColumnSpan(FrameworkElement control, int colSpan)
        {
            Grid.SetColumnSpan(control, colSpan);
        }

        protected override ControlType? GetControlType(FrameworkElement control)
        {
            if (control is Label)
                return ControlType.Label;

            if (control is Button)
                return ControlType.Button;

            if (control is Panel)
                return ControlType.Container;

            return null;
        }

        protected override bool UpdateControl(bool setup, ControlType? controlType, FrameworkElement control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo, object data, IReflectionProperties<LitUiAttribute> dataProps)
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
                        if (control is Panel container)
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
            if (attr != null)
            {
                UpdatePropertyValue(control, controlProps, nameof(control.Content), attr.Text, true);
                UpdatePropertyValue(control, controlProps, nameof(control.HorizontalAlignment), GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, nameof(control.VerticalAlignment), GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);
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
                        UpdatePropertyValue(control, controlProps, nameof(control.Content), data, true);
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
                UpdatePropertyValue(control, controlProps, nameof(control.Content), text, true);

                UpdatePropertyValue(control, controlProps, nameof(control.HorizontalAlignment), GetHorizontalOptions(attr.LayoutMode, attr.ContentLayout), true);
                UpdatePropertyValue(control, controlProps, nameof(control.VerticalAlignment), GetVerticalOptions(attr.LayoutMode, attr.ContentLayout), true);

                control.Click += delegate (object sender, RoutedEventArgs e)
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
        private static void SetupContainer(Panel control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo containerPropInfo,
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
                                            UpdateControl(sender as FrameworkElement, propAttr, propInfo, visualProp.GetData(), false);
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
        private static bool UpdateContainer(Panel control, IReflectionProperties controlProps, object data, IReflectionProperties<LitUiAttribute> dataProps)
        {
            var changed = false;

            return changed;
        }

        /// <summary>
        /// Common setup for a control.
        /// </summary>
        private static void SetupCommon(FrameworkElement control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo)
        {
            if (attr != null)
            {
            }
        }

        /// <summary>
        /// Translate text options to horizontal LayoutOptions
        /// </summary>
        private static HorizontalAlignment? GetHorizontalOptions(LayoutMode mode, LayoutMode textMode)
        {
            var alignment = GetHorizontalAlignment(textMode != LayoutMode.None ? textMode : mode);
            if (alignment.HasValue)
            {
                //var expands = GetHorizontalExpands(mode);
                //return new LayoutOptions(alignment.Value, expands);
                return alignment;
            }

            return null;
        }

        /// <summary>
        /// Translate text options to vertical LayoutOptions
        /// </summary>
        private static VerticalAlignment? GetVerticalOptions(LayoutMode mode, LayoutMode textMode)
        {
            var alignment = GetVerticalAlignment(textMode != LayoutMode.None ? textMode : mode);
            if (alignment.HasValue)
            {
                //var expands = GetVerticalExpands(mode);
                //return new LayoutOptions(alignment.Value, expands);
                return alignment;
            }

            return null;
        }

        /// <summary>
        /// Translate a layout mode to horizontal LayoutAlignment
        /// </summary>
        private static HorizontalAlignment? GetHorizontalAlignment(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.TopLeft:
                case LayoutMode.Left:
                case LayoutMode.LeftCenter:
                case LayoutMode.BottomLeft:
                    return HorizontalAlignment.Left;

                case LayoutMode.TopCenter:
                case LayoutMode.Center:
                case LayoutMode.BottomCenter:
                    return HorizontalAlignment.Center;

                case LayoutMode.TopRight:
                case LayoutMode.Right:
                case LayoutMode.RightCenter:
                case LayoutMode.BottomRight:
                    return HorizontalAlignment.Right;

                case LayoutMode.Top:
                case LayoutMode.Bottom:
                case LayoutMode.Fill:
                    return HorizontalAlignment.Stretch;

                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return null;
            }
        }

        /// <summary>
        /// Translate a layout mode to vertical LayoutAlignment
        /// </summary>
        private static VerticalAlignment? GetVerticalAlignment(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.Top:
                case LayoutMode.TopLeft:
                case LayoutMode.TopCenter:
                case LayoutMode.TopRight:
                    return VerticalAlignment.Top;

                case LayoutMode.Left:
                case LayoutMode.LeftCenter:
                case LayoutMode.Center:
                case LayoutMode.RightCenter:
                case LayoutMode.Right:
                    return VerticalAlignment.Center;

                case LayoutMode.BottomLeft:
                case LayoutMode.Bottom:
                case LayoutMode.BottomCenter:
                case LayoutMode.BottomRight:
                    return VerticalAlignment.Bottom;

                case LayoutMode.Fill:
                    return VerticalAlignment.Stretch;

                case LayoutMode.None:
                case LayoutMode.Floating:
                default:
                    return null;
            }
        }
    }
}
