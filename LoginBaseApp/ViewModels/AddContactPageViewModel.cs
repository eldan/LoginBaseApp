

using LoginBaseApp.Models;
using LoginBaseApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginBaseApp.ViewModels
{

  public partial class AddContactPageViewModel : ViewModelBase
  {
    IApplicationService serviceProvider;

    private User user;

    private bool canSave;

    public bool CanSave
    {
      get { return canSave; }
      set { canSave = value; OnPropertyChanged(); }
    }

    private string userName;
    public string UserName
    {
      get { return userName; }
      set
      {
        if (userName != value)
        {
          userName = value;

          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private string fullName;
    public string FullName
    {
      get { return fullName; }
      set
      {
        if (fullName != value)
        {
          fullName = value;

          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private string email;
    public string Email
    {
      get { return email; }
      set
      {
        if (email != value)
        {
          email = value;

          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private string address;
    public string Address
    {
      get { return address; }
      set
      {
        if (address != value)
        {
          address = value;
          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private ObservableCollection<Instrument> instruments;
    public ObservableCollection<Instrument> Instruments
    {
      get { return instruments; }
      set
      {
        instruments = value;
      }
    }

    private string phoneNumber;
    public string PhoneNumber
    {
      get { return phoneNumber; }
      set
      {
        if (phoneNumber != value)
        {
          phoneNumber = value;

          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private string avatar;
    public string Avatar
    {
      get { return avatar; }
      set
      {
        if (avatar != value)
        {
          avatar = value;

          OnPropertyChanged();
          CheckIfCanSave();
        }
      }
    }

    private DateTime birthDate;
    public DateTime BirthDate
    {
      get
      {
        return birthDate;
      }
      set
      {
        if (birthDate != value)
        {
          birthDate = value;

          OnPropertyChanged();

          CheckIfCanSave();
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
          if (value.Length > 0)
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

    private bool isBusy;
    public bool IsBusy
    {
      get { return isBusy; }
      set { isBusy = value; OnPropertyChanged(); }
    }

    private Instrument selectedInstrument;
    public Instrument SelectedInstrument
    {
      get => selectedInstrument;
      set
      {
        selectedInstrument = value;
        CheckIfCanSave();
        OnPropertyChanged();
      }
    }


    public ICommand ResetUserDataCommand
    {
      get;
    }
    public ICommand NavCommand
    {
      get;
    }

    public ICommand SaveUserDataCommand
    {
      get;
    }
    public ICommand GetInstrumentsCommand { get; }

    public ICommand NavBackCommand

    {
      get;
    }
    public ICommand ChangePhotoAvatarCommand
    {
      get;
    }
    public ICommand AddContactFromDeviceCommand { get; }

   
    private bool CheckValidity()
    {
      ErrorMsg = "";
      if (FullName != null)
      {
        if (FullName.Length <= 3)
        {
          ErrorMsg = "שם קצר מדי";
          return false;
        }
        if (SelectedInstrument == null)
        {
          ErrorMsg = "בחר כלי נגינה";
          return false;
        }
        else
        {
          return true;
        }

      }
      return false;
    }
    private void CheckIfCanSave()
    {

      if (CheckValidity())
      {
        CanSave = true;
      }

      else
      {
        CanSave = false;
      }
    }

    public AddContactPageViewModel(IApplicationService serviceProvider)
    {
      this.serviceProvider = serviceProvider;

      SaveUserDataCommand = new Command(async () => await SaveUserData());
      NavCommand = new Command(async () => await OpenNavApp());
      NavBackCommand = new Command(async () => await Shell.Current.GoToAsync("///UserListPage"));
      
      ChangePhotoAvatarCommand = new Command(async () => ChangePhotoAvatar());
      AddContactFromDeviceCommand = new Command(async () => await AddContactFromDevice());
      Instruments = new ObservableCollection<Instrument>();
      GetInstrumentsCommand = new Command(async () => await LoadInstrumentsAsync());
      GetInstrumentsCommand.Execute(null);
    }

    private async Task LoadInstrumentsAsync()
    {
      var instrumentsList = await serviceProvider.GetInstuments();
      foreach (var instrument in instrumentsList)
      {
        Instruments.Add(instrument);
      }


    }
    private async Task AddContactFromDevice()
    {
      try
      {
        if (Permissions.CheckStatusAsync<Permissions.ContactsRead>().Result != PermissionStatus.Granted)
        {
          await Permissions.RequestAsync<Permissions.ContactsRead>();
        }

        var contact = await Contacts.Default.PickContactAsync();

        if (contact == null)
          return;

        string id = contact.Id;
        string namePrefix = contact.NamePrefix;
        string givenName = contact.GivenName;
        string middleName = contact.MiddleName;
        string familyName = contact.FamilyName;
        string nameSuffix = contact.NameSuffix;
        string displayName = contact.DisplayName;
        List<ContactPhone> phones = contact.Phones; // List of phone numbers
        List<ContactEmail> emails = contact.Emails; // List of email addresses


        FullName = contact.GivenName + " " + contact.MiddleName + " " + contact.FamilyName;
        PhoneNumber = phones[0].ToString();
        Email = emails[0].ToString();


        //TODO add to similliar page as Details Page

      }
      catch (Exception ex)
      {
        // Most likely permission denied
      }
    }
    private async Task ChangePhotoAvatar()
    {
      string opnCamera = "צלם";
      string opnGallery = "בחר מהספרייה";
      string command = await Shell.Current.DisplayActionSheet("בחר", "אישור", "ביטול", opnGallery, opnCamera);
      if (command == opnCamera)
      {
        if (MediaPicker.Default.IsCaptureSupported)
        {
          FileResult photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions() { Title = opnGallery });

          if (photo != null)
          {
            // save the file into local storage
            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);
            avatar = localFilePath;
            Avatar = avatar;
            OnPropertyChanged(nameof(Avatar));
          }
        }
      }
      else
      // pick photo from Gallery
      {
        FileResult photo = await MediaPicker.Default.PickPhotoAsync();
        if (photo != null)
        {
          Avatar = photo.FullPath;
          OnPropertyChanged(nameof(Avatar));
        }
      }
    }

    private async Task SaveUserData()
    {
      User tempUser = new User()
      {
        
        BirthDate = BirthDate,
        IsAdmin = false,
      };
     
      UserName = Guid.NewGuid().ToString("N");
      tempUser.Username = UserName;
      tempUser.FullName = FullName;
      if (Avatar != null) tempUser.Avatar = Avatar; else tempUser.Avatar = "";
      if (PhoneNumber != null) tempUser.PhoneNumber = PhoneNumber; else tempUser.PhoneNumber = "";
      if (Address != null) tempUser.Address = Address; else tempUser.Address = "";
      if (Email != null) tempUser.Email = Email; else tempUser.Email = "";
      if (SelectedInstrument != null) tempUser.Instrument = SelectedInstrument; else tempUser.Instrument = instruments[0];

      IsBusy = true;
      bool tf = await serviceProvider.AddUser(tempUser);
      IsBusy = false;
     
      if (!tf)
      {
        Shell.Current.DisplayAlert("Oh", "Couldn't save... try again", "Got it");
      }
      else
      {
        NavBackCommand.Execute(null);
      }
    }

    private async Task OpenNavApp()
    {
      bool isValidAddress = await Launcher.TryOpenAsync("waze://ul?q=" + Address + "&navigate=yes");
    }
  }
}

