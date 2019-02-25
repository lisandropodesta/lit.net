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
                LayoutMode = LayoutMode.Fill;
                OnClickCommand = OnKeyPressedTag;
                AutoCommandParameterRegEx = @"(?!.*_)(.*)";
            }
        }

        #region User interface

        [LitUi(ControlType.Label, Row = 0, Col = 0, ColSpan = -1, ContentLayout = LayoutMode.TopRight, Text = "0", Property = TargetProperty.Text)]
        public VisualProperty<string> Screen { get; set; }

        [UiOnClick(Row = 1, Col = 0)]
        public bool Input_CE { get; set; }

        [UiOnClick(Row = 1, Col = 1)]
        public bool Input_C { get; set; }

        [UiOnClick(Row = 1, Col = 2, Text = @"%")]
        public bool Input_MOD { get; set; }

        [UiOnClick(Row = 1, Col = 3, Text = @"/")]
        public bool Input_Div { get; set; }

        [UiOnClick(Row = 2, Col = 0)]
        public bool Input_7 { get; set; }

        [UiOnClick(Row = 2, Col = 1)]
        public bool Input_8 { get; set; }

        [UiOnClick(Row = 2, Col = 2)]
        public bool Input_9 { get; set; }

        [UiOnClick(Row = 2, Col = 3, Text = @"x")]
        public bool Input_Mul { get; set; }

        [UiOnClick(Row = 3, Col = 0)]
        public bool Input_4 { get; set; }

        [UiOnClick(Row = 3, Col = 1)]
        public bool Input_5 { get; set; }

        [UiOnClick(Row = 3, Col = 2)]
        public bool Input_6 { get; set; }

        [UiOnClick(Row = 3, Col = 3, Text = @"-")]
        public bool Input_Sub { get; set; }

        [UiOnClick(Row = 4, Col = 0)]
        public bool Input_1 { get; set; }

        [UiOnClick(Row = 4, Col = 1)]
        public bool Input_2 { get; set; }

        [UiOnClick(Row = 4, Col = 2)]
        public bool Input_3 { get; set; }

        [UiOnClick(Row = 4, Col = 3, Text = @"+")]
        public bool Input_Add { get; set; }

        [UiOnClick(Row = 5, Col = 0, Text = @"+/-")]
        public bool Input_Sgn { get; set; }

        [UiOnClick(Row = 5, Col = 1)]
        public bool Input_0 { get; set; }

        [UiOnClick(Row = 5, Col = 2, Text = @".")]
        public bool Input_Dot { get; set; }

        [UiOnClick(Row = 5, Col = 3, Text = @"=")]
        public bool Input_Res { get; set; }

        /// <summary>
        /// Key pressed event.
        /// </summary>
        [LitUi(ControlType.None)]
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
        private void KeyPressed(ControlEvent e)
        {
            switch (e.CommandName)
            {
                case OnKeyPressedTag:
                    stack.PutKey(e.CommandParameter);
                    Screen.Value = stack.Screen;
                    break;
            }
        }
    }
}
