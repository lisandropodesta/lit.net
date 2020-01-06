namespace Lit.Calc
{
    using Lit.Ui;

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

        enum Rows
        {
            Debug,
            Screen,
            RadixSelection,
            Ctrl,
            Keys0,
            Keys1,
            Keys2,
            Keys3,
            Keys4,
            Keys5
        }

        [LitUi(ControlType.Label, Row = (int)Rows.Debug, Col = 0, ColSpan = -1, ContentLayout = LayoutMode.TopRight, Property = TargetProperty.Text)]
        public VisualProperty<string> Debug { get; set; }

        [LitUi(ControlType.Label, Row = (int)Rows.Screen, Col = 0, ColSpan = -1, ContentLayout = LayoutMode.TopRight, Property = TargetProperty.Text)]
        public VisualProperty<string> Screen { get; set; }

        [UiOnClick(Row = (int)Rows.RadixSelection, Col = 0)]
        public bool Input_BIN { get; set; }

        [UiOnClick(Row = (int)Rows.RadixSelection, Col = 1)]
        public bool Input_DEC { get; set; }

        [UiOnClick(Row = (int)Rows.RadixSelection, Col = 2)]
        public bool Input_OCT { get; set; }

        [UiOnClick(Row = (int)Rows.RadixSelection, Col = 3)]
        public bool Input_HEX { get; set; }

        [UiOnClick(Row = (int)Rows.Ctrl, Col = 0, Text = @"CE")]
        public bool Input_CE { get; set; }

        [UiOnClick(Row = (int)Rows.Ctrl, Col = 1, Text = @"C")]
        public bool Input_CA { get; set; }

        [UiOnClick(Row = (int)Rows.Ctrl, Col = 2, Text = @"<-")]
        public bool Input_BCK { get; set; }

        //[UiOnClick(Row = (int)Rows.Ctrl, Col = 3, Text = @"")]
        //public bool Input_ { get; set; }

        [UiOnClick(Row = (int)Rows.Keys0, Col = 0, Text = @"D")]
        public bool Input_D { get; set; }

        [UiOnClick(Row = (int)Rows.Keys0, Col = 1, Text = @"E")]
        public bool Input_E { get; set; }

        [UiOnClick(Row = (int)Rows.Keys0, Col = 2, Text = @"F")]
        public bool Input_F { get; set; }

        [UiOnClick(Row = (int)Rows.Keys0, Col = 3, Text = @"%")]
        public bool Input_MOD { get; set; }

        [UiOnClick(Row = (int)Rows.Keys1, Col = 0, Text = @"A")]
        public bool Input_A { get; set; }

        [UiOnClick(Row = (int)Rows.Keys1, Col = 1, Text = @"B")]
        public bool Input_B { get; set; }

        [UiOnClick(Row = (int)Rows.Keys1, Col = 2, Text = @"C")]
        public bool Input_C { get; set; }

        [UiOnClick(Row = (int)Rows.Keys1, Col = 3, Text = @"/")]
        public bool Input_Div { get; set; }

        [UiOnClick(Row = (int)Rows.Keys2, Col = 0)]
        public bool Input_7 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys2, Col = 1)]
        public bool Input_8 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys2, Col = 2)]
        public bool Input_9 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys2, Col = 3, Text = @"x")]
        public bool Input_Mul { get; set; }

        [UiOnClick(Row = (int)Rows.Keys3, Col = 0)]
        public bool Input_4 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys3, Col = 1)]
        public bool Input_5 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys3, Col = 2)]
        public bool Input_6 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys3, Col = 3, Text = @"-")]
        public bool Input_Sub { get; set; }

        [UiOnClick(Row = (int)Rows.Keys4, Col = 0)]
        public bool Input_1 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys4, Col = 1)]
        public bool Input_2 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys4, Col = 2)]
        public bool Input_3 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys4, Col = 3, Text = @"+")]
        public bool Input_Add { get; set; }

        [UiOnClick(Row = (int)Rows.Keys5, Col = 0, Text = @"+/-")]
        public bool Input_Sgn { get; set; }

        [UiOnClick(Row = (int)Rows.Keys5, Col = 1)]
        public bool Input_0 { get; set; }

        [UiOnClick(Row = (int)Rows.Keys5, Col = 2, Text = @".")]
        public bool Input_Dot { get; set; }

        [UiOnClick(Row = (int)Rows.Keys5, Col = 3, Text = @"=")]
        public bool Input_Res { get; set; }

        /// <summary>
        /// Key pressed event.
        /// </summary>
        [LitUi(ControlType.None)]
        public ControlEvent OnKeyPressed { get { return null; } set { KeyPressed(value); } }

        #endregion

        #region Private members

        private readonly CalculatorEngine stack = new CalculatorEngine(Radix.Dec);

        #endregion

        public Calculator()
        {
        }

        /// <summary>
        /// Send text to the calculator.
        /// </summary>
        /// <param name="text"></param>
        public void PutKey(string text)
        {
            stack.PutKey(text);
            Update();
        }

        public void Update()
        {
            Debug.Value = stack.Debug;
            Screen.Value = stack.Screen;
        }

        /// <summary>
        /// Process a key pressed.
        /// </summary>
        private void KeyPressed(ControlEvent e)
        {
            switch (e.CommandName)
            {
                case OnKeyPressedTag:
                    PutKey(e.CommandParameter);
                    break;
            }
        }
    }
}
