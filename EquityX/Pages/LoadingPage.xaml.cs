using EquityX.Services;
using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class LoadingPage : ContentPage
{
    private IAuthService _authService;

	public LoadingPage(IAuthService authService)
	{
		InitializeComponent();
        _authService = authService;
	}

    /// <summary>
    /// Once navigated to, checks if the user is authenticated and navigates to the appropriate page
    /// </summary>
    /// <param name="args"></param>
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (await _authService.IsAuthenticated())
        {
            NavigatePlatformHome();
        }
        else
        {
            NavigatePlatformLogin();
        }
    }

    /// <summary>
    /// Navigates to the login page based on the platform
    /// </summary>
    private async void NavigatePlatformLogin()
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

    /// <summary>
    /// Navigates to the home page based on the platform
    /// </summary>
    private async void NavigatePlatformHome()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//D{nameof(HomePage)}");
        }
    }
}