using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Lit.Test.Ui
{
    using Lit.xamarin;
    using Lit.Test.Ui.Demo;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            var data = new Calculator();

            var parent = MainPage.FindByName("WorkArea") as StackLayout;

            Mapper.Create(parent, data);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
