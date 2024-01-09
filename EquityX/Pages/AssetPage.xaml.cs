using EquityX.ViewModels;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;

namespace EquityX.Pages;

public partial class AssetPage : ContentPage
{

    ChartEntry[] entries = new[]
    {
        new ChartEntry(200)
        {
            Label = "January",
            ValueLabel = "200",
            Color = SKColor.Parse("#266489")
        },
        new ChartEntry(400)
        {
            Label = "February",
            ValueLabel = "400",
            Color = SKColor.Parse("#68B9C0")
        },
    };
    
    public AssetPage(AssetViewModel assestViewModel)
	{
		InitializeComponent();
		BindingContext = assestViewModel;

        // May need to do the timer here 
        chartView.Chart = new LineChart()
		{
			Entries = entries,
        };

        var theme = Application.Current.RequestedTheme;

        if (theme == AppTheme.Dark) 
        { 
            chartView.Chart.BackgroundColor = SKColor.Parse("#191919");
        } 
        else
        {
            chartView.Chart.BackgroundColor = SKColor.Parse("#F6F6F6");
        }
    }
}