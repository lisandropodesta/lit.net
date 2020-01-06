using System;
using Lit.DataType;
using System.Reflection;

namespace Lit.Ui
{
    public abstract class Mapper<T, TGrid, TControl>
        where T : class
        where TGrid : class
        where TControl : class
    {
        protected static Mapper<T, TGrid, TControl> Instance { get; set; }

        /// <summary>
        /// Creates a control from a template.
        /// </summary>
        public static object Create(T parent, object data)
        {
            return Create(parent, data.GetType(), data);
        }

        /// <summary>
        /// Creates a control from a template.
        /// </summary>
        public static object Create(T parent, Type template, object data)
        {
            return CreateControl(parent, template, null, data);
        }

        /// <summary>
        /// Setup a control, it may create a new one.
        /// </summary>
        protected static object CreateControl(T parent, Type template, PropertyInfo propInfo, object data)
        {
            var attr = TypeHelper.GetAttribute<LitUiAttribute>(template);
            return CreateControl(parent, attr, propInfo, data);
        }

        /// <summary>
        /// Setup a control, it may create a new one.
        /// </summary>
        protected static object CreateControl(T parent, LitUiAttribute attr, PropertyInfo propInfo, object data)
        {
            var control = attr != null ? Instance.CreateControl(attr.CtrlType) : null;

            if (control != null)
            {
                if (attr.HasGridSpecification && parent is TGrid grid)
                {
                    var colCount = Instance.GetColumnsCount(grid);
                    var rowCount = Instance.GetRowsCount(grid);
                    var left = attr.Col;
                    var right = attr.Col + attr.ColSpan < 0 ? colCount + attr.ColSpan : attr.ColSpan - 1;
                    var top = attr.Row;
                    var bottom = attr.Row + attr.RowSpan < 0 ? rowCount + attr.RowSpan : attr.RowSpan - 1;

                    Instance.AddChild(grid, control, attr.Col, attr.Row);

                    var colSpan = attr.ColSpan;
                    if (colSpan < 0)
                    {
                        colSpan = 1 + colCount + attr.ColSpan - attr.Col;
                    }
                    if (colSpan != 1)
                    {
                        Instance.SetColumnSpan(control, colSpan);
                    }

                    var rowSpan = attr.RowSpan;
                    if (rowSpan < 0)
                    {
                        rowSpan = 1 + rowCount + attr.RowSpan - attr.Row;
                    }
                    if (rowSpan != 1)
                    {
                        Instance.SetRowSpan(control, rowSpan);
                    }
                }
                else
                {
                    Instance.AddChild(parent, control);
                }

                UpdateControl(control, attr, propInfo, data, true);
            }
            /*else if (attr == null || attr.CtrlType == ControlType.None)
            {
                control = parent;
            }*/

            return control;
        }

        /// <summary>
        /// Update and optionally setup a control.
        /// </summary>
        protected static bool UpdateControl(TControl control, LitUiAttribute attr, PropertyInfo propInfo, object data, bool setup)
        {
            var controlType = attr?.CtrlType;
            if (controlType == ControlType.None)
            {
                controlType = Instance.GetControlType(control);
            }

            var controlProps = controlType != ControlType.None ? ReflectionPropertiesCache.Get(control.GetType()) : null;

            var dataProps = data != null ? ReflectionPropertiesCache<LitUiAttribute>.Get(data.GetType()) : null;

            return Instance.UpdateControl(setup, controlType, control, controlProps, attr, propInfo, data, dataProps);
        }

        /// <summary>
        /// Update a control property's value usign reflection information.
        /// </summary>
        protected static bool UpdatePropertyValue(object control, IReflectionProperties props, string propName, object value, bool onlyIfNotNull)
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
        /// Translate a layout mode to horizontal expansion
        /// </summary>
        protected static bool GetHorizontalExpands(LayoutMode mode)
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
        protected static bool GetVerticalExpands(LayoutMode mode)
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

        #region Abstract methods

        /// <summary>
        /// Create a new control.
        /// </summary>
        protected abstract TControl CreateControl(ControlType? controlType);

        /// <summary>
        /// Get grid columns count.
        /// </summary>
        protected abstract int GetColumnsCount(TGrid element);

        /// <summary>
        /// Get grid rows count.
        /// </summary>
        protected abstract int GetRowsCount(TGrid element);

        /// <summary>
        /// Adds a child to an element.
        /// </summary>
        protected abstract void AddChild(T parent, TControl control);

        /// <summary>
        /// Adds a child to an element.
        /// </summary>
        protected abstract void AddChild(TGrid grid, TControl control, int col, int row);

        /// <summary>
        /// Set row span for a element inside a grid.
        /// </summary>
        protected abstract void SetRowSpan(TControl control, int rowSpan);

        /// <summary>
        /// Set col span for a element inside a grid.
        /// </summary>
        protected abstract void SetColumnSpan(TControl control, int colSpan);

        /// <summary>
        /// Get the type of a control assuming attr is right.
        /// </summary>
        protected abstract ControlType? GetControlType(TControl control);

        /// <summary>
        /// Updates (optionally also setup) a control.
        /// </summary>
        protected abstract bool UpdateControl(bool setup, ControlType? controlType, TControl control, IReflectionProperties controlProps, LitUiAttribute attr, PropertyInfo propInfo, object data, IReflectionProperties<LitUiAttribute> dataProps);

        #endregion
    }
}