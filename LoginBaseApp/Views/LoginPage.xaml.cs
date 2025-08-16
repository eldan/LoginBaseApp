
using LoginBaseApp.Helper;
using LoginBaseApp.Models;
using LoginBaseApp.Service;
using LoginBaseApp.ViewModels;

namespace LoginBaseApp.Views;

public partial class LoginPage : ContentPage
{
	

	public LoginPage(LoginPageViewModel vm)
	{
		
		InitializeComponent();
		BindingContext = vm ;
	}

	

	
}
