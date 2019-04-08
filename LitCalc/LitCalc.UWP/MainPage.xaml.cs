namespace LitCalc.UWP
{
    using System.Collections.Generic;
    using Windows.System;
    using Windows.UI.Xaml.Input;

    public sealed partial class MainPage
    {
        private LitCalc.App app;

        private readonly Dictionary<VirtualKey, string> keys = new Dictionary<VirtualKey, string>
        {
            { VirtualKey.Number0, "0" },
            { VirtualKey.NumberPad0, "0" },
            { VirtualKey.Number1, "1" },
            { VirtualKey.NumberPad1, "1" },
            { VirtualKey.Number2, "2" },
            { VirtualKey.NumberPad2, "2" },
            { VirtualKey.Number3, "3" },
            { VirtualKey.NumberPad3, "3" },
            { VirtualKey.Number4, "4" },
            { VirtualKey.NumberPad4, "4" },
            { VirtualKey.Number5, "5" },
            { VirtualKey.NumberPad5, "5" },
            { VirtualKey.Number6, "6" },
            { VirtualKey.NumberPad6, "6" },
            { VirtualKey.Number7, "7" },
            { VirtualKey.NumberPad7, "7" },
            { VirtualKey.Number8, "8" },
            { VirtualKey.NumberPad8, "8" },
            { VirtualKey.Number9, "9" },
            { VirtualKey.NumberPad9, "9" },
            { VirtualKey.A, "A" },
            { VirtualKey.B, "B" },
            { VirtualKey.C, "C" },
            { VirtualKey.D, "D" },
            { VirtualKey.E, "E" },
            { VirtualKey.F, "F" },
            { VirtualKey.Decimal, "." },
            { VirtualKey.Back, "BCK" },
            { VirtualKey.Add, "ADD" },
            { VirtualKey.Subtract, "SUB" },
            { VirtualKey.Multiply, "MUL" },
            { VirtualKey.Divide, "DIV" },
            { VirtualKey.Escape, "CA" }
        };

        public MainPage()
        {
            this.InitializeComponent();

            app = new LitCalc.App();
            LoadApplication(app);

            PreviewKeyDown += MainPage_PreviewKeyDown;
        }

        private void MainPage_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!keys.TryGetValue(e.Key, out string text))
            {
                text = e.Key.ToString();
            }

            if (!string.IsNullOrEmpty(text))
            {
                app.PutInputKey(text);
            }
        }
    }
}
