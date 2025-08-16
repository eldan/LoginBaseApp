using LoginBaseApp.Service;
using LoginBaseApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginBaseApp.ViewModels
{
  public class AppShellViewModel : ViewModelBase
  {
    IServiceProvider provider;

    private bool isAdmin;
    public bool IsAdmin
    {
     // get => (Application.Current as App)?.CurrentUser?.IsAdmin ?? false;
     get => isAdmin;
      set
      {
        if (value != null)
        {
          isAdmin = value;
          OnPropertyChanged();
        }
      }
    }


    private string stam;

    public string Stam
    {
      get { return stam; }
      set { stam = value; }
    }


    public ICommand LogoutCommand
    {
      get;
    }

    public AppShellViewModel(IServiceProvider provider)
    {
      this.provider = provider;

      LogoutCommand = new Command(Logout);

    }

    private void Logout()
    {
      (Application.Current as App)!.CurrentUser = null;
      OnPropertyChanged(nameof(IsAdmin));
      Page loginPage = provider.GetService<LoginPage>()!;
      Application.Current.Windows[0].Page = loginPage; // החלפת הדף הנוכחי לדף ההתחברות
    }

    public bool IsLogin()
    {
      if ((Application.Current as App)!.CurrentUser != null)
      {
        return true;
      }
      return false;
    }
    public bool IsNotLogin()
    {
      if ((Application.Current as App)!.CurrentUser != null)
      {
        return false;
      }
      return true;
    }
  }
}
