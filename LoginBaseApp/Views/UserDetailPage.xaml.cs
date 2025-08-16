using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class UserDetailPage : ContentPage
{
	public UserDetailPage(UserDetailPageViewModel vm)
	{
		InitializeComponent();
    BindingContext = vm;
  }
}
