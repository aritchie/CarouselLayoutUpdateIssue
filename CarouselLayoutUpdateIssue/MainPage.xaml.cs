namespace CarouselLayoutUpdateIssue;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}


    protected override void OnAppearing()
    {
        base.OnAppearing();
		(BindingContext as MainPageViewModel)?.OnAppearing();
    }
}


