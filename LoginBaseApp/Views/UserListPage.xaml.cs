using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class UserListPage : ContentPage
{
 

  public UserListPage(UserListPageViewModel vm)
	{
		InitializeComponent();
    BindingContext = vm;
  }
  protected override void OnAppearing()
  {
    base.OnAppearing();
    var vm = BindingContext as UserListPageViewModel;
    vm.LoadListCommand.Execute(null);
  }
}

