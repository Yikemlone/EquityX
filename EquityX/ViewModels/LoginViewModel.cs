using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Pages;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        public ICommand LoginCommand { get; private set; }
        public ICommand GoToRegisterCommand { get; private set; }

        private IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;

            LoginCommand = new Command(() => Login());
            GoToRegisterCommand = new Command(() => GoToRegister());
        }
        
        private async void GoToRegister()
        {
            // TODO: Set up Idom check for desktop or phone
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        private async void Login()
        {
            if(!ValidInputs())
            {
                ErrorMessage = "Please enter a valid email and password";
                return;
            }

            if (!await _authService.Login(Email, Password)) 
            {
                ErrorMessage = "No user by those credintials";
            };
        }

        private bool ValidInputs() 
        { 
            if(String.IsNullOrEmpty(Password)) return false;
            if(String.IsNullOrEmpty(Email)) return false;
            return true;
        }
    }
}
