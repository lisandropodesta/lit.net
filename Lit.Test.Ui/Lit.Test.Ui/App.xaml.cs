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
        }

        protected override void OnStart()
        {
            var data = new Calculator();

            var parent = (Element)MainPage.FindByName("WorkArea") ?? MainPage;

            Mapper.Create(parent, data);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
