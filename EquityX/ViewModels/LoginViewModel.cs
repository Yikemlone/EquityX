using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Pages;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        // Commands
        public ICommand LoginCommand { get; private set; }
        public ICommand GoToRegisterCommand { get; private set; }

        // Services
        private IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;

            // Commands setup
            LoginCommand = new Command(() => Login());
            GoToRegisterCommand = new Command(() => GoToRegister());
        }
        
        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        private async void GoToRegister()
        {
            if(DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//D{nameof(RegisterPage)}");
            }
        }

        /// <summary>
        /// Logs the user in and takes them to the home page if attempted login is successful
        /// </summary>
        private async void Login()
        {
            if(!ValidInputs())
            {
                ErrorMessage = "Please enter a valid email and password";
                return;
            }

            if (!await _authService.Login(Username, Password))
            {
                ErrorMessage = "No user by those credintials";
                return;
            };

            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//D{nameof(HomePage)}");
            }
        }

        /// <summary>
        /// Ensures that the user has entered a valid email and password
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidInputs() 
        { 
            if(String.IsNullOrEmpty(Password)) return false;
            if(String.IsNullOrEmpty(Username)) return false;
            return true;
        }
    }
}
