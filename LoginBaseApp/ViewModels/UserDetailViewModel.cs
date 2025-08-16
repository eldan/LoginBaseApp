

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

  public partial class UserDetailPageViewModel : ViewModelBase, IQueryAttributable
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

    private ObservableCollection<Instrument> instruments;

    public ObservableCollection<Instrument> Instruments
    {
      get { return instruments; }
      set { instruments = value;
      
      }
    }


    private bool CheckValidity()
    {
      ErrorMsg = "";
      if (FullName != null)
      {
        if (FullName.Length <= 3
            )

        {
          ErrorMsg = "שם קצר מדי";
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
        if (SelectedInstrument != user.Instrument ||
            FullName != user.FullName ||
            PhoneNumber != user.PhoneNumber ||
            Email != user.Email ||
            BirthDate != user.BirthDate ||
            Avatar != user.Avatar )
              {
                CanSave = true;
              }
              else
              {
                         CanSave = false;
              }
        
      }

      else
      {
        CanSave = false;
      }
    }
    private Instrument selectedInstrument;
    public Instrument SelectedInstrument
    {
      get => selectedInstrument;
      set
      {
        selectedInstrument = value;
        
        OnPropertyChanged();
        CheckIfCanSave();
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

    public ICommand NavBackCommand

    {
      get;
    }
    public ICommand ChangePhotoAvatarCommand
    {
      get;
    }
    public ICommand AddContactFromDeviceCommand { get; }
    public ICommand GetInstrumentsCommand { get; }

        public UserDetailPageViewModel(IApplicationService serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            ResetUserDataCommand = new Command(ResetUserData);
            SaveUserDataCommand = new Command(()=>SaveUserData());
            NavCommand = new Command(async () => await OpenNavApp());
            NavBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            ChangePhotoAvatarCommand = new Command(async () => ChangePhotoAvatar());
            AddContactFromDeviceCommand = new Command(async () => await AddContactFromDevice());

            // Fix for CS1660: Use an asynchronous method to populate the ObservableCollection
            Instruments = new ObservableCollection<Instrument>();
            GetInstrumentsCommand = new Command(async () => await LoadInstrumentsAsync());
            GetInstrumentsCommand.Execute(null);
        }

        private async Task LoadInstrumentsAsync()
        {
            var instrumentsList = await serviceProvider.GetInstuments();
            Instruments = new ObservableCollection<Instrument>();
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
       
        
        FullName = contact.GivenName+ " "+contact.MiddleName + " " + contact.FamilyName;
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
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
     
      if (query.TryGetValue("selectedUser", out var selectedUser))
      {
        user = selectedUser as User;
        ResetUserData();
      }

    }
    private void ResetUserData()
    {
      UserName = user.Username;
      PhoneNumber = user.PhoneNumber;
      Address = user.Address;
      Email = user.Email;
      Avatar = user.Avatar;
      FullName = user.FullName;
      BirthDate = (DateTime)user.BirthDate;
     selectedInstrument = user.Instrument;
      foreach (Instrument i in instruments)
      {
        if (selectedInstrument.Id == i.Id)
        {
          SelectedInstrument = i;
          break;
        }
      }



      CanSave = false;

    }

    private async Task SaveUserData()
    {
      User tempUser = new User()
      {
        Id = user.Id,
        Username = UserName,
        PhoneNumber = PhoneNumber,
        Address = Address,
        Email = Email,
        FullName = FullName,
        BirthDate = BirthDate,
        IsAdmin = user.IsAdmin,
        Avatar = Avatar,
        Instrument = SelectedInstrument,
      };

  
      bool tf = await serviceProvider.UpdateUser(tempUser);
     
      if (tf)
      {
        user.Username = UserName;
        user.PhoneNumber = PhoneNumber;
        user.Address = Address;
        user.Email = Email;
        user.FullName = FullName;
        user.BirthDate = BirthDate;
        user.Avatar = Avatar;
        user.Instrument = SelectedInstrument;
        CanSave = false;

        //IsBusy = true;
        //Shell.Current.DisplayAlert("Imitating save", "Todo: IsBusy has to be handled witha sync", "Got it");
        //IsBusy = false;
      }



    }

    private async Task OpenNavApp()
    {
      bool isValidAddress = await Launcher.TryOpenAsync("waze://ul?q="+Address+ "&navigate=yes");
    }
  }
}
