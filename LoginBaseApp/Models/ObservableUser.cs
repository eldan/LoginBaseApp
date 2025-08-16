using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Models
{
  public class ObservableUser : INotifyPropertyChanged
  {
    User user;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableUser(User u)
    {
      this.user = u;
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public User ToUserClass()
    {
      return this.user;
    }

    public string UserName
    {
      get => user.Username;
      set
      {
        user.Username = value;
        OnPropertyChanged();
      }
    }

    public string Avatar
    {
      get => user.Avatar;
      set
      {
        user.Avatar = value;
        OnPropertyChanged();
      }
    }

    public string FullName
    {
      get => user.FullName;
      set
      {
        user.FullName = value;
        OnPropertyChanged();
      }
    }

    public string Password
    {
      get => user.Password;
      set
      {
        user.Password = value;
        OnPropertyChanged();
      }
    }

    public string Address
    {
      get => user.Address;
      set
      {
        user.Address = value;
        OnPropertyChanged();
      }
    }


    public string Email
    {
      get => user.Email;
      set
      {
        user.Email = value;
        OnPropertyChanged();
      }
    }


    public string PhoneNumber
    {
      get => user.PhoneNumber;
      set
      {
        user.PhoneNumber = value;
        OnPropertyChanged();
      }
    }


    public bool? IsAdmin
    {
      get => user.IsAdmin;
      set
      {
        user.IsAdmin = value;
        OnPropertyChanged();
      }
    }


    public DateTime? BirthDate
    {
      get => user.BirthDate;
      set
      {
        user.BirthDate = value;
        OnPropertyChanged();
      }
    }

    public Instrument? Instrument
    {
      get => user.Instrument;
      set
      {
        user.Instrument = value;
        OnPropertyChanged();
      }
    }

  }
}
