using LoginBaseApp.Helper;
using LoginBaseApp.Models;
using LoginBaseApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginBaseApp.ViewModels
{
  public class UserListPageViewModel : ViewModelBase
  {
    #region Properties
    IApplicationService serviceProvider;

    private bool isLoading;

    public bool IsLoading
    {
      get { return isLoading; }
      set { isLoading = value; }
    }

    private string searchText;

    public string SearchText
    {
      get { return searchText; }
      set { searchText = value; }
    }

    private User selectedUser;

    public User SelectedUser
    {
      get { return selectedUser; }
      set { selectedUser = value; }
    }


    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users
    {
      get => _users;
      set
      {
        _users = value;
        OnPropertyChanged(nameof(Users)); // Notify the UI
      }
    }
    #endregion


    #region ICommand
    public ICommand AddUserCommand { get; }
    public ICommand LoadListCommand { get; }
    public ICommand ResetFilterCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand TypingFilterCommand { get; }
    public ICommand ClearFilterCommand { get; }
    public ICommand ShowDetailUserCommand { get; }
    public ICommand AddContactCommand { get; }
    #endregion

    #region Constructor
    public UserListPageViewModel(IApplicationService serviceProvider)
    {
      this.serviceProvider = serviceProvider;
      Users = new();

      isLoading = false;

      ClearFilterCommand = new Command(ClearFilter);

      LoadListCommand = new Command(async () => await LoadList());
     
      DeleteUserCommand = new Command<User>(async (u) => await DeleteUser(u));
     
      TypingFilterCommand = new Command<string>(async (query) => await FilterUsers(query));

      ShowDetailUserCommand = new Command(async () => await ShowDetailUser());

      AddContactCommand = new Command(async () => await AddContact());

      //LoadListCommand.Execute(null); //moved to refreshData method
      
    }
    #endregion

    #region Methods
    private async Task AddContact()
    {
      await Shell.Current.GoToAsync("AddContactPage");

    }
    private async Task<bool> DeleteUser(User userToDelete)
    {
      IsLoading = true;
      if (userToDelete == null) return false; // Ensure user is not null, why shoult it be? ask
      if (await serviceProvider.DeleteUser(userToDelete))
      {
        Users.Remove(userToDelete);
        return true;
      }
      IsLoading = false;
      return false;
      // TODO פתוצאה של פעולה אסינק לא צריכה להופיע כאן במקרה של תקלה אלא ברמה גבוהה יותר של שגיעות משותפות
    }

    private async Task FilterUsers(string query)
    {
     // await Shell.Current.DisplayAlert("Filter Alert", "You are using filter", "OK");
      if (!string.IsNullOrEmpty(query))
      {
        IsLoading = true;
        ObservableCollection<User> _filteredUsers = new ObservableCollection<User>(Users.Where(u => u.FullName.ToLower().Contains(query.ToLower())));

        // לבצע את הקוד  בטרד של היו איי
        Application.Current?.Dispatcher.Dispatch(() =>
        {
          Users.Clear();
          foreach (var u in _filteredUsers)
          {
            Users.Add(u);
          }
          IsLoading = false;
        });
      } else
      {
        Users = new ObservableCollection<User>();
      }
    }

    private void ClearFilter()
    {
      SearchText = "";
      
      Users = new ObservableCollection<User>(serviceProvider.GetCashedUsers());
      OnPropertyChanged(nameof(ObservableUser));//TODO ask why did i have to do this, it is an ObservableCollection
      // i guess is because i made just intiate it again
    }

    private async Task ShowDetailUser()
    {
      Dictionary<string, object> param = new Dictionary<string, object>();
      param.Add("selectedUser", SelectedUser!);
      await Shell.Current.GoToAsync("UserDetailPage", param);
      SelectedUser = null;

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

          //foreach (var u in _tmpusers)
          //{
          //  Users.Add(u); // ✅ Add to the public property
          //}
          Users = new ObservableCollection<User>(_tmpusers);
          OnPropertyChanged(nameof(Users));   // so the UI rebinds to the new instance
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
    #endregion

  }
}
