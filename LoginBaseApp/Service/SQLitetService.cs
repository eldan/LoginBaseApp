using LoginBaseApp.Models;
using LoginBaseApp.Models.DTOS;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instrument = LoginBaseApp.Models.Instrument;

namespace LoginBaseApp.Service
{
  public class SQLitetService : IApplicationService
  {
    const string DB_NAME = "LoginBaseApp_0000.db3";
    List<User> users = new List<User>();
    List<Instrument> instruments = new List<Instrument>();
    List<Band> bands = new List<Band>();
    User loginUser;

    public const SQLite.SQLiteOpenFlags FLAGS =
      SQLite.SQLiteOpenFlags.ReadWrite |
      SQLite.SQLiteOpenFlags.Create |
      SQLite.SQLiteOpenFlags.SharedCache;

    string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DB_NAME);

    SQLiteAsyncConnection dbConn;


    public async Task LoadSampleDataAsync()
    {
      List<UserDTO> tmpusers = new List<UserDTO>();
      List<Instrument> tmpinstruments = new List<Instrument>();
      List<BandDTO> tmpbands = new List<BandDTO>();
      tmpinstruments.Add(new Instrument() { Id = 1, Name = "לא מוגדר", Picture = "sol.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 2, Name = "סקסופון", Picture = "sax.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 3, Name = "כלי הקשה", Picture = "drums.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 4, Name = "קלידים", Picture = "piano.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 5, Name = "בס", Picture = "bass.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 6, Name = "זמר/ת", Picture = "singer.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 7, Name = "חצוצרן", Picture = "trumpet.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 8, Name = "טרומבון", Picture = "trombone.jpg" });
      tmpinstruments.Add(new Instrument() { Id = 9, Name = "גיטרה", Picture = "guitar.jpg" });
      User u1 = new User { Id = 1, Username = "admin", Password = "admin", Address = "המעיין 8 קדימה", Email = "ad@ad.com", FullName = "אלדן הילדסהיים", PhoneNumber = "972-33-22-1111", BirthDate = new DateTime(1969, 8, 19), IsAdmin = true, Avatar = "https://avatars.githubusercontent.com/u/619414?v=4&size=64", Instrument = tmpinstruments[1] };
      tmpusers.Add(new UserDTO(u1));
      User u2 = new User { Id = 2, Username = "user1", Password = "password1", Address = "hamayan 6, kadima", Email = "usr1@gmail.com", FullName = "לואי ארמסטרונג", PhoneNumber = "972-33-22-2211", BirthDate = new DateTime(1985, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDJJyrR6i2ot7Gl2AIoOSE8PeF4FKCwtLXOi0pigFLjq-iHe9x1z4FpQ01pCec5SUVcec&usqp=CAU", Instrument = tmpinstruments[6] };
      tmpusers.Add(new UserDTO(u2 ));
      tmpusers.Add(new UserDTO(new User { Id = 3, Username = "user2", Password = "password2", Address = "רחוק מאוד 19, פלורידה", Email = "usr2@gmail.com", FullName = "בילי הולידי", PhoneNumber = "888-33-22-2221", BirthDate = new DateTime(1915, 4, 7), IsAdmin = false, Avatar = "https://assets.vogue.com/photos/603970803d808cc9eac3bdda/4:3/w_2160,h_1620,c_limit/GettyImages-104476113%20(1).jpg", Instrument = tmpinstruments[5] }) );
      tmpusers.Add(new UserDTO(new User { Id = 4, Username = "user3", Password = "password3", Address = "רחוק מאוד 19, פלורידה", Email = "usr3@gmail.com", FullName = "ג'ון קולטרן", PhoneNumber = "111-33-2232221", BirthDate = new DateTime(1921, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQlVZWAjzznqC7LiR8KeHVZhXXlRaJP8vA9NRKQQLW7yBnqtUtcSuoX-k3qXihIcqAnbSrBucuoDLa2tSUsmM49NfPoV4oPYaElMB0BVTw", Instrument = tmpinstruments[4] }) );
      tmpusers.Add(new UserDTO(new User { Id = 5, Username = "user4", Password = "password4", Address = "רחוק מאוד 19, פלורידה", Email = "usr4@gmail.com", FullName = "דיזי גילספי", PhoneNumber = "050-999-333", BirthDate = new DateTime(1930, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRyB_I7Y5yX4ecEP5b3veI0RZUPFKFhhVXR_A&s", Instrument = tmpinstruments[5] }) );
      tmpusers.Add(new UserDTO(new User { Id = 6, Username = "user5", Password = "password5", Address = "רחוק מאוד 19, פלורידה", Email = "usr5@gmail.com", FullName = "מיילס דיוויס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1926, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1DavBc1KsQrQ4aZyhrPMObcZFGQDZLwLOKTKVxwcGkk1cV4M5RgRedvLKoWwsvNSX-HY&usqp=CAU", Instrument = tmpinstruments[5] }) );
      tmpusers.Add(new UserDTO(new User { Id = 7, Username = "user6", Password = "password6", Address = "רחוק מאוד 19, פלורידה", Email = "usr6@gmail.com", FullName = "תלוניוס מונק", PhoneNumber = "050-999-333", BirthDate = new DateTime(2001, 8, 19), IsAdmin = false, Avatar = "https://t3.gstatic.com/images?q=tbn:ANd9GcT22mKB1nwyDeqqsAsA23Bf2q7JWgHjvwdhvg3ygLjYJ6KF058arg", Instrument = tmpinstruments[5] }) );
      tmpusers.Add(new UserDTO(new User { Id = 8, Username = "user7", Password = "password7", Address = "רחוק מאוד 19, פלורידה", Email = "usr7@gmail.com", FullName = "דייב ברובק", PhoneNumber = "050-999-333", BirthDate = new DateTime(1920, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/licensed-image?q=tbn:ANd9GcQuewH4ABkrEpKj0SJwT4pIGw828sQTNTcbpPvChwEgrpfqptmgpDcD9CNxGanyUvKgmESao2mCvf9fl7k", Instrument = tmpinstruments[5] }) );
      tmpusers.Add(new UserDTO(new User { Id = 9, Username = "user8", Password = "password8", Address = "רחוק מאוד 19, פלורידה", Email = "usr8@gmail.com", FullName = "פרנק סינטרה", PhoneNumber = "050-999-333", BirthDate = new DateTime(1915, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcT7-Al4hUd_J_N40lhEYTGuRbzASwoU-GsmjmZGjVuNR1IdiL8V6IcOlnKF5zaSCJ2vXITw-x7lwMTfi26SUdiqLtlShPbSEAs-bowWjn0", Instrument = tmpinstruments[6] }) );
      tmpusers.Add(new UserDTO(new User { Id = 10, Username = "user9", Password = "password9", Address = "רחוק מאוד 19, פלורידה", Email = "usr9@gmail.com", FullName = "סטן גטס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1927, 8, 19), IsAdmin = false, Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHytwccW5-kwYk3v1HdICIC7Y828-nP7wJ4Q&s", Instrument = tmpinstruments[1] }) );
      tmpusers.Add(new UserDTO(new User { Id = 11, Username = "user10", Password = "password10", Address = "רחוק מאוד 19, פלורידה", Email = "usr41@gmail.com", FullName = "ג'אנגו ריינהארדט", PhoneNumber = "050-999-333", BirthDate = new DateTime(1910, 8, 19), IsAdmin = false, Avatar = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Django_Reinhardt_%28Gottlieb_07301%29.jpg/250px-Django_Reinhardt_%28Gottlieb_07301%29.jpg", Instrument = tmpinstruments[8] }) );
      tmpusers.Add(new UserDTO(new User { Id = 12, Username = "user11", Password = "password11", Address = "רחוק מאוד 19, פלורידה", Email = "usr42@gmail.com", FullName = "צרלז מיגוס", PhoneNumber = "050-999-333", BirthDate = new DateTime(1910, 8, 19), IsAdmin = false, Avatar = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Charles_Mingus_1976_cropped.jpg/250px-Charles_Mingus_1976_cropped.jpg", Instrument = tmpinstruments[4] }) );
     

      Band band = new Band { Id=1,Name= "Generic Name", BandMembers = new List<User>() { u1 } };
      tmpbands.Add(new BandDTO(band));

      await Init();

      try
      {
        await dbConn.DeleteAllAsync<UserDTO>();
        await dbConn.DeleteAllAsync<Instrument>();
        await dbConn.DeleteAllAsync<BandDTO>();

      }
      catch (Exception ex)
      {
        throw ex;
      }

      try
      {
        if ((await dbConn.Table<Instrument>().CountAsync()) == 0)
        {
          await dbConn.InsertAllAsync(tmpinstruments);
        }
        if ((await dbConn.Table<UserDTO>().CountAsync()) == 0)
        {
          await dbConn.InsertAllAsync(tmpusers);
        }
        if ((await dbConn.Table<BandDTO>().CountAsync()) == 0)
        {
          await dbConn.InsertAllAsync(tmpbands);
        }
        // TESTING
        var instruments = await dbConn.GetAllWithChildrenAsync<Instrument>();
        var usersDTO = await dbConn.GetAllWithChildrenAsync<UserDTO>();
        var bandsDTO = await dbConn.GetAllWithChildrenAsync<BandDTO>();

        // TEST Users
        List<User> debugusers = new List<User>();
        foreach (var u in usersDTO)
        {
          debugusers.Add(u.ToUser(instruments));
        }
        //TEST Bands
        List<Band> debugbands = new List<Band>();
        foreach (var b in bandsDTO)
        {
          debugbands.Add(b.ToBand(debugusers));
        }
        Console.WriteLine("Initial Data Inserted");
      }
      catch (Exception ex)
      {
       
        throw ex;
      
      }
      
    }


    private async Task Init()//Lazy loading of the database
    {
      //->>>>>>>>>>>>>>>>>>>> TODO for debugging only - add remarks
      if (dbConn is not null)
        return;
      try
      {
        dbConn = new SQLiteAsyncConnection(DatabasePath, FLAGS);
        
        await dbConn.CreateTableAsync<UserDTO>();
        await dbConn.CreateTableAsync<Instrument>();
        await dbConn.CreateTableAsync<BandDTO>();

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        throw ex;
      }
      finally{
        // TESTING

      }
    }

    public async Task<bool> AddUser(User user)
    {
      try
      {
        await Init();
        await dbConn.InsertAsync(new UserDTO(user));
        return true;
      }
      catch (Exception ex)
      {

        throw ex;
      }
     
    }

    public async Task<bool> DeleteUser(User user)
    {
      await Init();
      try
      {
        int userIdToBeErased = user.Id;
        await dbConn.DeleteAsync(new UserDTO(user));
        var bands = await dbConn.Table<BandDTO>().ToListAsync();

        foreach (var band in bands)
        {
          if (band.BandMemberIds.Contains(userIdToBeErased))
          {
            band.BandMemberIds.Remove(userIdToBeErased);
            await dbConn.UpdateAsync(band);
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
      throw new NotImplementedException();
    }

    public async Task<Band> GetBand()
    {
      try
      {
        await Init();
        var bandDTOs = await dbConn.GetAllWithChildrenAsync<BandDTO>();
        bands = new List<Band>();
        foreach (var b in bandDTOs)
        {
          bands.Add(b.ToBand(users));
        }
        return bands[0];
      }
      catch (Exception ex)
      {
        return new List<Band>()[0];
      }
      return new List<Band>()[0];
    }

    public List<User> GetCashedUsers()
    {
      return users;
    }

    public async Task<List<Instrument>> GetInstuments()
    {
      if (instruments.Count > 0)
        return instruments; // Return cached instruments if available
      try
      {
        await Init();
        var instruments = await dbConn.GetAllWithChildrenAsync<Instrument>();
        return instruments;
      }
      catch (Exception ex)
      {
        return new List<Instrument>();
      }
      return new List<Instrument>();
    }

    public User GetLoginUser()
    {
      return loginUser;
    }

    public async Task<List<User>> GetUsers()
    {
      try
      {
        await Init();
        instruments = await dbConn.GetAllWithChildrenAsync<Instrument>();
        var usersDTO = await dbConn.GetAllWithChildrenAsync<UserDTO>();
        List<User> tmpusers = new List<User>();
        users = new List<User>();
        foreach (var u in usersDTO)
        {
          tmpusers.Add(u.ToUser(instruments));
        }
        users = tmpusers;//This is the cached users
        return users;
      }
      catch (Exception ex)
      {
        return new List<User>();
      }
      return new List<User>();
    }

    public async Task<bool> Login(string username, string password)
    {
      await Init();
      try
      {
        var usersDTO = await dbConn.Table<UserDTO>()
        .Where(u => u.Username == username && u.Password == password)
        .FirstOrDefaultAsync();
        var instruments = await dbConn.GetAllWithChildrenAsync<Instrument>();
        loginUser = usersDTO.ToUser(instruments);
        return true;


        if (usersDTO == null)
          return false;

      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public async Task<bool> Register(string username, string password, string fullName, string address, string email, string phoneNumber, DateTime birthDate, bool isAdmin)
    {
      await Init(); // 👈 Lazy-load the database before proceeding


      // For example:
      var newUser = new User
      {
        Username = username,
        Password = password,
        FullName = fullName,
        Address = address,
        Email = email,
        PhoneNumber = phoneNumber,
        BirthDate = birthDate,
        IsAdmin = isAdmin
      };

      try
      {
        await Init();
        await dbConn.InsertAsync(new UserDTO(newUser));
        return true;

       
      }
      catch (Exception ex)
      {
        // Handle or log the error
        return false;
      }
    }

    public async Task<bool> SaveBand(Band band)
    {
      BandDTO bandDTO = new BandDTO(band);
      try
      {
        await Init();
        await dbConn.UpdateAsync(bandDTO);
      }
      catch (Exception ex)
      {

        throw ex;
      }
      
      return true;
    }

    public async Task<bool> UpdateUser(User user)
    {
      await Init();
      try
      {
        await dbConn.UpdateAsync(new UserDTO(user));
        return true;
      }
      catch (Exception)
      {

        throw;
      }
      

    }

    public async Task InitializeAsync()
    {
      await Init();
      throw new NotImplementedException();
    }
  }
}
