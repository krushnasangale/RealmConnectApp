using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RealmConnectApp.Models;
using RealmConnectApp.Views;
using Realms;
using Realms.Sync;
using System.Collections.ObjectModel;

namespace RealmConnectApp.ViewModels
{
    public partial class DashboardVM : BaseViewModel
    {
        private Realm _realm;
        private PartitionSyncConfiguration _config;

        public DashboardVM()
        {
            todoList = new ObservableCollection<Todo>();
            TodoList = new ObservableCollection<Todo>();
            emptyText = "No todos yet!";
            InitialiseRealm();
        }

        [ObservableProperty]
        ObservableCollection<Todo> todoList;

        [ObservableProperty]
        string emptyText;

        [ObservableProperty]
        string todoEntryText;

        [ObservableProperty]
        bool isRefreshing;

        public async Task InitialiseRealm()
        {
            _config = new PartitionSyncConfiguration($"{App.RealmApp.CurrentUser.Id}", App.RealmApp.CurrentUser);
            _realm = await Realm.GetInstanceAsync(_config);

            GetTodos();
            if(TodoList.Count == 0)
            {
               EmptyText = "No todos yet!";
            }
        }

        [RelayCommand]
        public async void GetTodos()
        {
            var todos = _realm.All<Todo>();
            todoList.Clear();
            foreach (var todo in todos)
            {
                todoList.Add(todo);
            }

            TodoList = new ObservableCollection<Todo>(todoList);
            IsRefreshing = false;
        }

        [RelayCommand]
        public async void AddTodo()
        {
            IsRefreshing = true;
            try
            {
                _realm.Write(() =>
                {
                    var newTodo = new Todo
                    {
                        Name = TodoEntryText,
                        Completed = "false",
                        Partition = App.RealmApp.CurrentUser.Id,
                        Owner = App.RealmApp.CurrentUser.Profile.Email
                    };
                    _realm.Add(newTodo);
                });

                TodoEntryText = string.Empty;
                GetTodos();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
        }

        [RelayCommand]
        public async void EditTodo(Todo td)
        {
            IsRefreshing = true;
            var newString = await App.Current.MainPage.DisplayPromptAsync("Edit", td.Name);
            if(newString is null || string.IsNullOrWhiteSpace(newString.ToString() )) { return; }
            try
            {
                _realm.Write(() =>
                {
                    var todo = _realm.Find<Todo>(td.Id);
                    todo.Name = UppercaseFirst(newString.ToString());
                });

                GetTodos();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
        }


        [RelayCommand]
        public async Task LogOut()
        {
            await App.RealmApp.RemoveUserAsync(App.RealmApp.CurrentUser);
            App.Current.Quit();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        private static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
