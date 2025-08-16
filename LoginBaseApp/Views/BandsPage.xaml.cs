using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class BandsPage : ContentPage
{
	public BandsPage(BandsPageViewModel vm)
	{
		InitializeComponent();
    BindingContext = vm;
  }
  protected override void OnAppearing()
  {
    base.OnAppearing();
    var vm = BindingContext as BandsPageViewModel;
    vm.LoadListCommand.Execute(null);
  }
}
