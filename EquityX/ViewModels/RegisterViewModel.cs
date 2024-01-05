using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Pages;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

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
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
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

            if (!await _authService.Register(Name, Password, Email)) 
            {
                ErrorMessage = "Something when wrong with registering...";
                return;
            };
        }

        private bool ValidInputs()
        {
            // Consider checking for valid email
            // Consider checking for valid password
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Email)) return false;
            if (String.IsNullOrEmpty(Password)) return false;
            if (String.IsNullOrEmpty(ConfirmPassword)) return false;
            return true;
        }
    }
}
