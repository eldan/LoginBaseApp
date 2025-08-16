using LoginBaseApp.Models;
using LoginBaseApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginBaseApp.ViewModels
{
    public class BandsPageViewModel : ViewModelBase
    {
    IApplicationService serviceProvider;
    List<User> users;
    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users
    {
      get => _users;
      set
      {
        _users = value;
        OnPropertyChanged(nameof(Users));
      }
    }


    ObservableCollection<User> selectedUsers;
    public ObservableCollection<User> SelectedUsers
    {
      get { return selectedUsers; }
      set { selectedUsers = value;
        
      }
    }

    ObservableCollection<User> unselectedUsers;
    public ObservableCollection<User> UnselectedUsers
    {
      get { return unselectedUsers; }
      set { unselectedUsers = value;
    
      }
    }


    private Band band;
  

    
    private bool isLoading;

    public bool IsLoading
    {
      get { return isLoading; }
      set { isLoading = value; }
    }
    public ICommand LoadListCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand RemoveUserCommand { get; }

    public BandsPageViewModel(IApplicationService serviceProvider) 
    {
      this.serviceProvider = serviceProvider;
      Users = new();
      LoadListCommand = new Command(async () => await LoadList());
      AddUserCommand = new Command<User>((u) => AddUser(u));
      RemoveUserCommand = new Command<User>((u) => RemoveUser(u));

      LoadListCommand.Execute(null);
    }

    private void RemoveUser(User u)
    {
      SelectedUsers.Remove(u);
      UnselectedUsers.Add(u);
      Band b = new Band() { Id = band.Id, Name = band.Name, BandMembers = SelectedUsers.ToList() };
      var tf = serviceProvider.SaveBand(b);

    }

    private void AddUser(User u)
    {
      SelectedUsers.Add(u);
      UnselectedUsers.Remove(u);
      Band b = new Band() { Id = band.Id, Name = band.Name, BandMembers = SelectedUsers.ToList() };
      var tf = serviceProvider.SaveBand(b);
    }

    private async Task LoadList()
    {
      if (!IsLoading)
      {
        try
        {
          IsBusy = true;
          IsLoading = true;

          Users.Clear(); // Clear existing items

          var _tmpusers = await serviceProvider.GetUsers();
          Users = new ObservableCollection<User>(_tmpusers);
          band = await serviceProvider.GetBand();
          UnselectedUsers = new();
          SelectedUsers = new();
          SelectedUsers = new ObservableCollection<User>(band.BandMembers);
          var unselectedUsers = Users
        .Where(user => !SelectedUsers.Any(selected => selected.Username == user.Username))
        .ToList();

          UnselectedUsers = new ObservableCollection<User>(unselectedUsers);

          OnPropertyChanged(nameof(SelectedUsers));
          OnPropertyChanged(nameof(UnselectedUsers));



        }
        catch (Exception ex)
        {
          await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
          IsLoading = false;
          IsBusy = false;
        }
      }
    }
  }
}
