using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Lit.Calc;
using Lit.Ui.Wpf;

namespace LitCalc.Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Calculator Calculator { get; set; }

        private readonly Dictionary<Key, string> keys = new Dictionary<Key, string>
        {
            { Key.D0, "0" },
            { Key.NumPad0, "0" },
            { Key.D1, "1" },
            { Key.NumPad1, "1" },
            { Key.D2, "2" },
            { Key.NumPad2, "2" },
            { Key.D3, "3" },
            { Key.NumPad3, "3" },
            { Key.D4, "4" },
            { Key.NumPad4, "4" },
            { Key.D5, "5" },
            { Key.NumPad5, "5" },
            { Key.D6, "6" },
            { Key.NumPad6, "6" },
            { Key.D7, "7" },
            { Key.NumPad7, "7" },
            { Key.D8, "8" },
            { Key.NumPad8, "8" },
            { Key.D9, "9" },
            { Key.NumPad9, "9" },
            { Key.A, "A" },
            { Key.B, "B" },
            { Key.C, "C" },
            { Key.D, "D" },
            { Key.E, "E" },
            { Key.F, "F" },
            { Key.OemPeriod, "." },
            { Key.Decimal, "." },
            { Key.Back, "BCK" },
            { Key.OemPlus, "ADD" },
            { Key.Add, "ADD" },
            { Key.OemMinus, "SUB" },
            { Key.Subtract, "SUB" },
            { Key.Multiply, "MUL" },
            { Key.Divide, "DIV" },
            { Key.Enter, "RES" },
            { Key.Escape, "CA" }
        };

        public MainWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            Calculator = new Calculator();

            var grid = new Grid();
            this.AddChild(grid);

            WpfMapper.Initialize();
            WpfMapper.Create(grid, Calculator);

            Calculator.Update();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;

            if (!keys.TryGetValue(key, out string text))
            {
                text = key.ToString();
            }

            if (!string.IsNullOrEmpty(text))
            {
                Calculator.PutKey(text);
            }
        }
    }
}
