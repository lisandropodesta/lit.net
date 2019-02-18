namespace Lit.Test.Ui.Demo
{
    using Lit.xamarin;

    [LitUi(ControlType.Container)]
    public class Calculator
    {
        public const string OnKeyPressedTag = @"OnKeyPressed";

        protected class UiOnClick : LitUiAttribute
        {
            public UiOnClick()
            {
                CtrlType = ControlType.Button;
                OnClickCommand = OnKeyPressedTag;
                CommandParameterAutoRegEx = "_(.*)";
            }
        }

        #region User interface

        [LitUi(ControlType.Label, Docking = Placement.Top)]
        public VisualProperty<string> Screen { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_0 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_1 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_2 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_3 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_4 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_5 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_6 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_7 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_8 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_9 { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Dot { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Add { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Sub { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Mul { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Div { get; set; }

        [UiOnClick]
        public VisualProperty<string> Input_Res { get; set; }

        /// <summary>
        /// Key pressed event.
        /// </summary>
        public ControlEvent OnKeyPressed { get { return null; } set { KeyPressed(value); } }

        #endregion

        #region Private members

        private readonly CalculatorStack stack = new CalculatorStack(10);

        #endregion

        public Calculator()
        {
        }

        /// <summary>
        /// Process a key pressed.
        /// </summary>
        private void KeyPressed(ControlEvent eventData)
        {
            var uiattr = eventData.Property.Attribute;
            switch (uiattr.OnClickCommand)
            {
                case OnKeyPressedTag:
                    stack.PutKey(uiattr.CommandParameter);
                    Screen.Value = stack.Screen;
                    break;
            }
        }
    }
}
