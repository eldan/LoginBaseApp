using LoginBaseApp.Helper;
using LoginBaseApp.Service;
using LoginBaseApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;


namespace LoginBaseApp.ViewModels
{
  public partial class RegisterPageViewModel : ViewModelBase
  {
    IApplicationService serviceProvider;

    private string? username;
    public string? Username
    {
      get { return username; }
      set
      {
        if (username != value)
        {
          username = value;
          OnPropertyChanged();
        }
      }
    }


    private string? password;
    public string? Password
    {
      get { return password; }
      set
      {
        if (password != value)
        {
          password = value;
          OnPropertyChanged();
        }
      }
    }


    private string? fullName;
    public string? FullName
    {
      get { return fullName; }
      set
      {
        if (fullName != value)
        {
          fullName = value;
          OnPropertyChanged();
        }
      }
    }


    private string? address;
    public string? Address
    {
      get { return address; }
      set
      {
        if (address != value)
        {
          address = value;
          OnPropertyChanged();
        }
      }
    }


    private string? phoneNumber;
    public string? PhoneNumber
    {
      get { return phoneNumber; }
      set
      {
        if (phoneNumber != value)
        {
          phoneNumber = value;
          OnPropertyChanged();
        }
      }
    }


    private string? email;
    public string? Email
    {
      get { return email; }
      set
      {
        if (email != value)
        {
          email = value;
          OnPropertyChanged();
        }
      }
    }


    private DateTime? birthDate;
    public DateTime? BirthDate
    {
      get { return birthDate; }
      set
      {
        if (birthDate != value)
        {
          birthDate = value;
          OnPropertyChanged();

          TimeSpan difference = DateTime.Today - birthDate.Value;
          int days = (int)difference.TotalDays;

          CurrentAge = (days/365).ToString();

        }
      }
    }

    private bool messageIsVisible;
    public bool MessageIsVisible
    {
      get => messageIsVisible;
      set
      {
        if (messageIsVisible != value)
        {
          messageIsVisible = value;
          OnPropertyChanged();
        }
      }
    }

    private string? errorMsg;
    public string? ErrorMsg
    {
      get { return errorMsg; }
      set
      {
        if (errorMsg != value)
        {
          errorMsg = value;
          if (value.Length>0)
          {
            MessageIsVisible = true;
          }
          else
          {
            MessageIsVisible = false;
          }
          OnPropertyChanged();
        }
      }
    }


    private string? currentAge;
    public string? CurrentAge
    {
      get { return currentAge; }
      set
      {
        if (currentAge != value)
        {
          currentAge = value;
          OnPropertyChanged();
        }
      }

    
  }
    public ICommand RegisterCommand{ get; }
    public ICommand LinkLoginCommand { get; }
    public RegisterPageViewModel(IApplicationService service, IServiceProvider provider)
    {
      this.serviceProvider = service;
      RegisterCommand = new Command(()=>TryRegister());

      LinkLoginCommand = new Command(() =>
      {
        var loginVM = provider.GetService<LoginPageViewModel>();
        Application.Current.MainPage = new LoginPage(loginVM);
      });
    }
    private async Task TryRegister()
    {
      string errorMessage = string.Empty;
      bool isValid;
      if (username != null)
      {
        isValid = Regex.IsMatch(username, @"^\S+$");
        if (!isValid) errorMessage += "אסור רווחים" + "\n";

        isValid = Regex.IsMatch(username, @"^(?!\d)");
        if (!isValid) errorMessage += "אסור אות ראשונה מספר" + "\n";
      }
      else
      {
        errorMessage += "חייב לרשום שם משתמש" + "\n";
      }


      if (password != null)
      {
        isValid = Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).+$");
        if (!isValid) errorMessage += "סיסמה חייבת להכיל לפחות אות גדולה אחת ומספר" + "\n";
      }
      else
      {
        errorMessage += "חייב לרשום שם סיסמא" + "\n";
      }


      if (BirthDate != null)
      {
        TimeSpan difference = DateTime.Today - birthDate.Value;
        int days = (int)difference.TotalDays;

        CurrentAge = (days / 365).ToString();
        isValid = int.Parse(currentAge)>=18;
        if (!isValid) errorMessage += "הנך חייב להיות גדול מ 18" + "\n";
      }
      else
      {
        errorMessage += "חייב לרשום שם גיל" + "\n";
      }
      // remove the last  char that are \n, so it wont be displayed
      if (errorMessage.Length > 0)
        errorMessage = errorMessage.Remove(errorMessage.Length - 1, 1);
      ErrorMsg = errorMessage;
      if (errorMessage == string.Empty)
      {
        bool succeed = await serviceProvider.Register(username, password, fullName, address,email,phoneNumber, (DateTime)birthDate, false);
        if (succeed)
        {
          LinkLoginCommand.Execute(null);
        }
      }
    }
  
  }
}
