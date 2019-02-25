namespace Lit.xamarin
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Visual attributes for automatic screen construction.
    /// </summary>
    public class LitUiAttribute : Attribute
    {
        #region Private fields

        private int? row;

        private int? rowSpan;

        private int? col;

        private int? colSpan;

        #endregion

        /// <summary>
        /// Control in which to render, in case is null then information renders in current control.
        /// </summary>
        public ControlType CtrlType { get; set; }

        /// <summary>
        /// Target property to be assigned, only used when ControlType is null.
        /// </summary>
        public TargetProperty Property { get; set; }

        /// <summary>
        /// Data context sets as property value.
        /// </summary>
        public bool OwnDataContext { get; set; }

        /// <summary>
        /// Style to be assigned.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Layout mode.
        /// </summary>
        public LayoutMode LayoutMode { get; set; }

        /// <summary>
        /// Check if the control has row/col specification.
        /// </summary>
        public bool HasGridSpecification { get { return row.HasValue || rowSpan.HasValue || col.HasValue || colSpan.HasValue; } }

        /// <summary>
        /// Row placing in container.
        /// </summary>
        public int Row { get { return row ?? 0; } set { row = value; } }

        /// <summary>
        /// Row span in container.
        /// </summary>
        public int RowSpan { get { return rowSpan ?? 1; } set { rowSpan = value; } }

        /// <summary>
        /// Column placing in container.
        /// </summary>
        public int Col { get { return col ?? 0; } set { col = value; } }

        /// <summary>
        /// Column span in container.
        /// </summary>
        public int ColSpan { get { return colSpan ?? 1; } set { colSpan = value; } }

        /// <summary>
        /// Text placement.
        /// </summary>
        public LayoutMode ContentLayout { get; set; }

        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Command to be executed on click.
        /// </summary>
        public string OnClickCommand { get; set; }

        /// <summary>
        /// Makes command parameter be extracted from property name.
        /// </summary>
        public string AutoCommandParameterRegEx { get; set; }

        /// <summary>
        /// Parameter for commands ejecution.
        /// </summary>
        public string CommandParameter { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LitUiAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LitUiAttribute(ControlType type)
        {
            CtrlType = type;
        }

        public string GetAutoCommandParameter(PropertyInfo propInfo)
        {
            if (!string.IsNullOrEmpty(AutoCommandParameterRegEx))
            {
                var regex = new Regex(AutoCommandParameterRegEx);
                var match = regex.Match(propInfo.Name);
                return match.Success ? match.Value : null;
            }

            return null;
        }
    }
}
