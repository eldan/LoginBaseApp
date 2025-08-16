using LoginBaseApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Service
{
	/// <summary>
	/// ממשק (Interface) המגדיר את החוזה עבור שירותי התחברות.
	/// כל מחלקה שתממש ממשק זה חייבת לספק לוגיקת התחברות.
	/// </summary>
	public interface IApplicationService
	{
    public  Task<bool> Register(string username, string password, string fullName, string address, string email, string phoneNumber, DateTime birthDate, bool isAdmin);
    public Task<bool> Login(string username, string password);
    public Task<bool> UpdateUser(User user);
    public Task<bool> AddUser(User user);
    public Task<bool> DeleteUser(User user);
    
    public Task<List<User>> GetUsers();
    public Task<List<Instrument>> GetInstuments();
    public Task<Band> GetBand();
    public Task<bool> SaveBand(Band band);

    public List<User> GetCashedUsers();
    public User GetLoginUser();
    public Task InitializeAsync();
    public  Task LoadSampleDataAsync();
  }
}
