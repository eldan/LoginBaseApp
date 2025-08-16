using LoginBaseApp.Helper;
using LoginBaseApp.Service;
using LoginBaseApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;

namespace LoginBaseApp.ViewModels
{
	/// <summary>
	/// ה-ViewModel עבור דף ההתחברות. מנהל את הלוגיקה והמצב של הדף.
	/// </summary>
	public class LoginPageViewModel : ViewModelBase
	{
    // שדה פרטי לשמירת שירות ההתחברות שהוזרק
    IServiceProvider provider;
    IApplicationService appService;

    private string? _userName = "";
		private string? _password = "";

		public string? UserName
		{
			get => _userName;
			set
			{
				if (_userName != value)
				{
					_userName = value;
					OnPropertyChanged(); // מודיע ל-UI על שינוי כדי לעדכן את התצוגה
					(LoginCommand as Command)?.ChangeCanExecute(); // בודק מחדש אם ניתן להפעיל את כפתור ההתחברות
				}
			}
		}
		public string? Password
		{
			get => _password;
			set
			{
				if (_password != value)
				{
					_password = value;
					OnPropertyChanged(); // מודיע ל-UI על שינוי
					(LoginCommand as Command)?.ChangeCanExecute(); // בודק מחדש אם ניתן להפעיל את כפתור ההתחברות
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

		private Color? messageColor;
		public Color? MessageColor
		{
			get => messageColor;
			set
			{
				if (messageColor != value)
				{
					messageColor = value;
					OnPropertyChanged();
				}
			}
		}

		private bool isPassword;
		public bool IsPassword
		{
			get => isPassword;
			set
			{
				if (isPassword != value)
				{
					isPassword = value;
					OnPropertyChanged();
				}
			}
		}

    private string? showPasswordIcon;
    public string? ShowPasswordIcon
    {
      get => showPasswordIcon;
      set
      {
        if (showPasswordIcon != value)
        {
          showPasswordIcon = value;
          OnPropertyChanged();
        }
      }
    }

    private string? loginMessage;
		public string? LoginMessage
		{
			get => loginMessage;
			set
			{
				if (loginMessage != value)
				{
					loginMessage = value;
					OnPropertyChanged();
				}
			}
		}

    public LoginPageViewModel(IApplicationService service, IServiceProvider provider)
		{

			this.appService = service;
			this.provider = provider;
			// אתחול הפקודות והגדרת ערכים ראשוניים
			ShowPasswordCommand = new Command(TogglePasswordVisiblity);
      LoginCommand = new Command(async () => await Login(), CanLogin);
      LinkRegisterCommand = new Command(() =>
      {
        var registerVm = provider.GetService<RegisterPageViewModel>();
        Application.Current.MainPage = new RegisterPage(registerVm);
       
      });



      //ShowPasswordIcon = FontHelper.CLOSED_EYE_ICON; // הגדרת אייקון ברירת מחדל
      IsPassword = true; // הגדרת שדה הסיסמה כמוסתר כברירת מחדל
      loadUserDataTask = GetUserNameFromSecureStorageAsync();
      

    }
    private async Task GetUserNameFromSecureStorageAsync()
    {
      UserName = await SecureStorage.Default.GetAsync("userName");
      
    }


    public bool CanLogin()
		{
			return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));
		}

		public ICommand ShowPasswordCommand
		{
			get;
		}
    public ICommand LinkRegisterCommand
    {
      get;
    }

    public ICommand LoginCommand
		{
			get;
		}

    private Task loadUserDataTask; // this is for trying to load login data from secure storage, see constructor
    /// <summary>
    /// מחליף את מצב התצוגה של הסיסמה (מוסתר/גלוי) ומעדכן את האייקון בהתאם.
    /// </summary>
    private void TogglePasswordVisiblity()
		{
			IsPassword = !IsPassword; // הופך את הערך הבוליאני
      // עובדים עם  coonverter  אז לא צריך להגדיר אייקון
      //if (IsPassword)
      //	ShowPasswordIcon = FontHelper.CLOSED_EYE_ICON;
      //else
      //	ShowPasswordIcon = FontHelper.OPEN_EYE_ICON;
    }

    private async Task Login()
		{
			IsBusy = true; // מסמן שהאפליקציה בתהליך (להצגת מחוון טעינה)
			MessageIsVisible = true; // מציג את אזור הודעת המשוב

			// קורא לשירות ההתחברות עם הפרטים שהוזנו
			if (await appService.Login(UserName!, Password!))
			{
				// במקרה של הצלחה
				LoginMessage = AppMessages.LoginMessage;
				MessageColor = Colors.White;
        ((App)Application.Current!).CurrentUser = appService.GetLoginUser();

        // save the user name in secure storage
        await SecureStorage.Default.SetAsync("userName", UserName!); //userName is the key

        var shellMvvm = provider.GetService<AppShellViewModel>()!;
        if (UserName == "admin")
        {
          shellMvvm.IsAdmin = true; // אם המשתמש הוא אדמין, מגדיר את המאפיין המתאים
        }
        else
        {
          shellMvvm.IsAdmin = false; // אם המשתמש אינו אדמין, מגדיר את המאפיין המתאים
        }
        Application.Current.Windows[0].Page = new AppShell(shellMvvm);

      }
			else
			{
				// במקרה של כישלון
				LoginMessage = AppMessages.LoginErrorMessage;
				MessageColor = Color.FromArgb("#ff3366");

      }
      IsBusy = false; // מסיים את מצב "עסוק"
		}
	}
}
