namespace LitCalc
{
    using Xamarin.Forms;
    using Lit.calc;
    using Lit.xamarin;

    public partial class MainPage : ContentPage
    {
        public Calculator Calculator { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Calculator = new Calculator();

            var parent = FindByName("WorkArea") as StackLayout;

            Mapper.Create(parent, Calculator);

            Calculator.Update();
        }
    }
}
