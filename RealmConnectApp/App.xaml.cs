using RealmConnectApp.AppConfig;
using RealmConnectApp.Views;

namespace RealmConnectApp
{
    public partial class App : Application
    {
        public static Realms.Sync.App RealmApp;
        public App()
        {
            InitializeComponent();

            RealmApp = Realms.Sync.App.Create(ApplicationConfig.RealmAppId);
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}