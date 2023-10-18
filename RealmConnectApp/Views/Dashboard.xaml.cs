using RealmConnectApp.ViewModels;

namespace RealmConnectApp.Views;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();
		BindingContext = new DashboardVM();
	}
}