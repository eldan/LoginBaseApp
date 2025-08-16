using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class AddContactPage : ContentPage
{
	public AddContactPage(AddContactPageViewModel vm)
	{
		InitializeComponent();
    BindingContext = vm;
  }
}
