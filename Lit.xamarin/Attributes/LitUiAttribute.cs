namespace Lit.xamarin
{
    using System;

    /// <summary>
    /// Visual attributes for automatic screen construction.
    /// </summary>
    public class LitUiAttribute : Attribute
    {
        /// <summary>
        /// Control in which to render, in case is null then information renders in current control.
        /// </summary>
        public ControlType? CtrlType { get; set; }

        /// <summary>
        /// Target property to be assigned, only used when ControlType is null.
        /// </summary>
        public TargetProperty? Property { get; set; }

        /// <summary>
        /// Style to be assigned.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Docking mode.
        /// </summary>
        public Placement Docking { get; set; }

        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Text placement.
        /// </summary>
        public Placement TextPlacement { get; set; }

        /// <summary>
        /// Command to be executed on click.
        /// </summary>
        public string OnClickCommand { get; set; }

        /// <summary>
        /// Makes command parameter be extracted from property name.
        /// </summary>
        public string CommandParameterAutoRegEx { get; set; }

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
    }
}
