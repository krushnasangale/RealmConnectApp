using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RealmConnectApp.Views;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmConnectApp.ViewModels
{
    public partial class LoginPageVM : BaseViewModel
    {
        public LoginPageVM()
        {
            EmailText = "test@test.com";
            PasswordText = "test123";
            Login();
        }

        [ObservableProperty]
        string emailText;

        [ObservableProperty]
        string passwordText;

        private async Task StartDashboard()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Dashboard());
        }

        [RelayCommand]
        async void CreateAccount()
        {
            await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(EmailText, PasswordText);
            await Login();
        }

        [RelayCommand]
        public async Task Login()
        {
            var user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(EmailText, PasswordText));
            if(user != null)
            {
                var userid = user.Id;
                Preferences.Set("userId", userid);
                await StartDashboard();
            }
            else await Application.Current.MainPage.DisplayAlert("Error", "Login failed, try again", "OK");
        }
    }
}
