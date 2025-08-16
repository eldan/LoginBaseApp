using LoginBaseApp.Helper;
using LoginBaseApp.Models;
using LoginBaseApp.Service;
using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel vm)
	{
		InitializeComponent();
    BindingContext = vm;
  }
}
