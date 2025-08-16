using LoginBaseApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LoginBaseApp.Service
{

  public class FakeDBService : IApplicationService
  {
    // רשימה המשמשת כמסד נתונים מדמה
    List<User> users = new List<User>();
    List<Instrument> instruments = new List<Instrument>();
    List<Band> bands = new List<Band>();
    User loginUser;


    /// <summary>
    /// בנאי המאתחל את "מסד הנתונים" עם משתמשים לדוגמה.
    /// </summary>
    public FakeDBService()
    {
      instruments.Add(new Instrument() { Id = 1, Name = "סקסופון", Picture = "sax.jpg" });
      instruments.Add(new Instrument() { Id = 2, Name = "כלי הקשה", Picture = "drums.jpg" });
      instruments.Add(new Instrument() { Id = 3, Name = "קלידים", Picture = "piano.jpg" });
      instruments.Add(new Instrument() { Id = 4, Name = "בס", Picture = "bass.jpg" });
      instruments.Add(new Instrument() { Id = 5, Name = "זמר/ת", Picture = "singer.jpg" });
      instruments.Add(new Instrument() { Id = 6, Name = "חצוצרן", Picture = "trumpet.jpg" });
      instruments.Add(new Instrument() { Id = 7, Name = "טרומבון", Picture = "trombone.jpg" });
      instruments.Add(new Instrument() { Id = 8, Name = "גיטרה", Picture = "guitar.jpg" });


      users.Add(new User { Id = 1, Username = "admin", Password = "admin", Address = "המעיין 8 קדימה", Email = "ad@ad.com", FullName = "אלדן הילדסהיים", PhoneNumber = "972-33-22-1111", BirthDate = new DateTime(1969, 8, 19), IsAdmin = true, Avatar = "https://avatars.githubusercontent.com/u/619414?v=4&size=64", Instrument = instruments[0] });
      users.Add(new User { Id = 2, Username = "user1", Password = "password1", Address = "hamayan 6, kadima", Email = "usr1@gmail.com", FullName = "לואי ארמסטרונג", PhoneNumber = "972-33-22-2211", BirthDate = new DateTime(1985, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDJJyrR6i2ot7Gl2AIoOSE8PeF4FKCwtLXOi0pigFLjq-iHe9x1z4FpQ01pCec5SUVcec&usqp=CAU", Instrument = instruments[5] });
      users.Add(new User { Id = 3, Username = "user2", Password = "password2", Address = "רחוק מאוד 19, פלורידה", Email = "usr2@gmail.com", FullName = "בילי הולידי", PhoneNumber = "888-33-22-2221", BirthDate = new DateTime(1915, 4, 7), IsAdmin = false, Avatar = "https://assets.vogue.com/photos/603970803d808cc9eac3bdda/4:3/w_2160,h_1620,c_limit/GettyImages-104476113%20(1).jpg", Instrument = instruments[4] });
      users.Add(new User { Id = 4, Username = "user3", Password = "password3", Address = "רחוק מאוד 19, פלורידה", Email = "usr3@gmail.com", FullName = "ג'ון קולטרן", PhoneNumber = "111-33-2232221", BirthDate = new DateTime(1921, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQlVZWAjzznqC7LiR8KeHVZhXXlRaJP8vA9NRKQQLW7yBnqtUtcSuoX-k3qXihIcqAnbSrBucuoDLa2tSUsmM49NfPoV4oPYaElMB0BVTw", Instrument = instruments[3] });
      users.Add(new User { Id = 5, Username = "user4", Password = "password4", Address = "רחוק מאוד 19, פלורידה", Email = "usr4@gmail.com", FullName = "דיזי גילספי", PhoneNumber = "050-999-333", BirthDate = new DateTime(1930, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRyB_I7Y5yX4ecEP5b3veI0RZUPFKFhhVXR_A&s", Instrument = instruments[4] });
      users.Add(new User { Id = 6, Username = "user5", Password = "password5", Address = "רחוק מאוד 19, פלורידה", Email = "usr5@gmail.com", FullName = "מיילס דיוויס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1926, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1DavBc1KsQrQ4aZyhrPMObcZFGQDZLwLOKTKVxwcGkk1cV4M5RgRedvLKoWwsvNSX-HY&usqp=CAU", Instrument = instruments[4] });
      users.Add(new User { Id = 7, Username = "user6", Password = "password6", Address = "רחוק מאוד 19, פלורידה", Email = "usr6@gmail.com", FullName = "תלוניוס מונק", PhoneNumber = "050-999-333", BirthDate = new DateTime(2001, 8, 19), IsAdmin = false, Avatar = "https://t3.gstatic.com/images?q=tbn:ANd9GcT22mKB1nwyDeqqsAsA23Bf2q7JWgHjvwdhvg3ygLjYJ6KF058arg", Instrument = instruments[4] });
      users.Add(new User { Id = 7, Username = "user7", Password = "password7", Address = "רחוק מאוד 19, פלורידה", Email = "usr7@gmail.com", FullName = "דייב ברובק", PhoneNumber = "050-999-333", BirthDate = new DateTime(1920, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/licensed-image?q=tbn:ANd9GcQuewH4ABkrEpKj0SJwT4pIGw828sQTNTcbpPvChwEgrpfqptmgpDcD9CNxGanyUvKgmESao2mCvf9fl7k", Instrument = instruments[4] });
      users.Add(new User { Id = 7, Username = "user8", Password = "password8", Address = "רחוק מאוד 19, פלורידה", Email = "usr8@gmail.com", FullName = "פרנק סינטרה", PhoneNumber = "050-999-333", BirthDate = new DateTime(1915, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcT7-Al4hUd_J_N40lhEYTGuRbzASwoU-GsmjmZGjVuNR1IdiL8V6IcOlnKF5zaSCJ2vXITw-x7lwMTfi26SUdiqLtlShPbSEAs-bowWjn0", Instrument = instruments[5] });
      users.Add(new User { Id = 7, Username = "user9", Password = "password9", Address = "רחוק מאוד 19, פלורידה", Email = "usr9@gmail.com", FullName = "סטן גטס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1927, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHytwccW5-kwYk3v1HdICIC7Y828-nP7wJ4Q&s", Instrument = instruments[0] });
      users.Add(new User { Id = 7, Username = "user10", Password = "password10", Address = "רחוק מאוד 19, פלורידה", Email = "usr41@gmail.com", FullName = "ג'אנגו ריינהארדט", PhoneNumber = "050-999-333", BirthDate = new DateTime(1910, 8, 19), IsAdmin = false, Avatar = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Django_Reinhardt_%28Gottlieb_07301%29.jpg/250px-Django_Reinhardt_%28Gottlieb_07301%29.jpg", Instrument = instruments[7] });
      users.Add(new User { Id = 7, Username = "user11", Password = "password11", Address = "רחוק מאוד 19, פלורידה", Email = "usr42@gmail.com", FullName = "צרלז מיגוס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1910, 8, 19), IsAdmin = false, Avatar = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Charles_Mingus_1976_cropped.jpg/250px-Charles_Mingus_1976_cropped.jpg", Instrument = instruments[3] });

      bands.Add(new Band() { Id = 1, Name = "הלהקה הראשונה", BandMembers = new List<User>() { users[0], users[1], users[2] } });
     

    }


    public async Task<bool> Login(string username, string password)
    {
      // חיפוש המשתמש הראשון ברשימה שתואם לשם המשתמש והסיסמה שהתקבלו
      var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
      // אם נמצא משתמש (התוצאה אינה null), ההתחברות הצליחה
      loginUser = (User)user;
      return user != null;
    }
    public async Task<bool> Register(string username, string password, string fullName, string address, string email, string phoneNumber, DateTime birtdate, bool isadmin)
    {
      User user = new User() { Username = username, Password = password, Address = address, Email = email, FullName = fullName, PhoneNumber = phoneNumber, BirthDate = birtdate, IsAdmin = true };
      users.Add(user);
      return user != null;
    }

    public async Task<List<User>> GetUsers()
    {
      //this will fetch from database
      await Task.Delay(250); // סימול של עיכוב בטעינת הנתונים (כמו קריאה לבסיס נתונים)
      return DeepCopyNonAdminUserList();
    }

    public List<User> GetCashedUsers()
    {
      return DeepCopyNonAdminUserList();
    }
    private List<User> DeepCopyNonAdminUserList()
    {
      List<User> usersCopy = new List<User>();
      ;
      foreach (var user in users)
      {
        if (!(bool)user.IsAdmin)
        {
          User userCopy = new User
          {
            Username = user.Username,
            Password = user.Password,
            Address = user.Address,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate,
            IsAdmin = user.IsAdmin,
            Avatar = user.Avatar,
            Instrument = user.Instrument
          };
          usersCopy.Add(userCopy);
        }

      }
      return usersCopy;
    }

    public async Task<bool> DeleteUser(User user)
    {
      foreach (var u in users)
      {
        if (u.Username == user.Username)
        {
          users.Remove(u);
          break;
        }
      }
      return true;
    }

    public async Task<bool> UpdateUser(User user)
    {
      foreach (var u in users)
      {
        if (u.Username == user.Username)
        {
          u.Password = user.Password;
          u.Address = user.Address;
          u.Email = user.Email;
          u.FullName = user.FullName;
          u.PhoneNumber = user.PhoneNumber;
          u.BirthDate = user.BirthDate;
          u.IsAdmin = user.IsAdmin;
          u.Avatar = user.Avatar;
          u.Instrument = user.Instrument;
          break;
        }
      }
      return true;
    }

    public User GetLoginUser()
    {
      return loginUser;
    }

    public async Task<bool> AddUser(User user)
    {
      users.Add(user);
      return true;
    }

    public async Task<List<Instrument>> GetInstuments()
    {
      return instruments;
    }

    public async Task<Band> GetBand()
    {
      return bands[0];
    }


    public async Task<bool> SaveBand(Band band)
    {
      bands[0] = band;
      return true;
    }

    public Task InitializeAsync()
    {
      throw new NotImplementedException();
    }

    public Task LoadSampleDataAsync()
    {
      throw new NotImplementedException();
    }
  }
}
