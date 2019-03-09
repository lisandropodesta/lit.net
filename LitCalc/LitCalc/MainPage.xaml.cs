namespace LitCalc
{
    using Xamarin.Forms;
    using Lit.calc;
    using Lit.xamarin;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var data = new Calculator();

            var parent = FindByName("WorkArea") as StackLayout;

            Mapper.Create(parent, data);
        }
    }
}
