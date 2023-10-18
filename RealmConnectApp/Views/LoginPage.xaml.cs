using RealmConnectApp.ViewModels;

namespace RealmConnectApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginPageVM();
	}
}