using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Pages;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _confirmPassword;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _errorMessage;

        // Commands
        public ICommand RegisterCommand { get; private set; }
        public ICommand MoveToLoginPageCommand { get; private set; }

        // Services
        private IAuthService _authService;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;

            // Commands setup
            RegisterCommand = new Command(() => Register());
            MoveToLoginPageCommand = new Command(() => GoToLoginPage());
        }

        private async void GoToLoginPage()
        {
            if(DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//D{nameof(LoginPage)}");
            }   
        }

        private async void Register()
        {
            if (!ValidInputs())
            {
                ErrorMessage = "Please enter valid details";
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ErrorMessage = "Passwords do not match";
                return;
            }

            if (!await _authService.Register(Name, Password, Username)) 
            {
                ErrorMessage = "Something when wrong with registering...";
                return;
            };

            if(DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//D{nameof(HomePage)}");
            }   
        }

        private bool ValidInputs()
        {
            // Consider checking for valid email
            // Consider checking for valid password
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Username)) return false;
            if (String.IsNullOrEmpty(Password)) return false;
            if (String.IsNullOrEmpty(ConfirmPassword)) return false;
            return true;
        }
    }
}
