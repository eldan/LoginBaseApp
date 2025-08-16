using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace LoginBaseApp.Models.DTOS;


  [SQLite.Table("UserDTOTbl")]//לא חייבים להגדיר את השם של הטבלה, אם לא נכתוב הוא ישתמש בשם של המחלקה
  public class UserDTO
  {
  [PrimaryKey]
  [AutoIncrement]
  public int Id { get; set; }

    public string? Username
    {
      get; set;
    }

    public string? Avatar
    {
      get; set;
    }

    public string? FullName
    {
      get; set;
    }
    
    public string? Password
    {
      get; set;
    }

    public string? Address
    {
      get; set;
    }

    public string? Email
    {
      get; set;
    }
    public string? PhoneNumber
    {
      get; set;
    }

    public bool? IsAdmin
    {
      get; set;
    }
    public DateTime? BirthDate
    {
      get; set;
    }

    // Instrument? Instrument { get; set; }
    [ForeignKey(typeof(Instrument))]
    public int InstrumentId { get; set; } // Foreign key

    public UserDTO() { }
    public UserDTO(User u)
    {
      InstrumentId = u.Instrument?.Id ?? 0;
      Id = u.Id;
      Username = u.Username;
      Avatar = u.Avatar;
      FullName = u.FullName;
      Password = u.Password;  //TODO add HashPassword
      Address = u.Address;
      Email = u.Email;
      PhoneNumber = u.PhoneNumber;
      IsAdmin = u.IsAdmin;
      BirthDate = u.BirthDate;
  }


  //Not realy necessary, using Serialized
  //[ManyToOne]
  //public BandDTO Band { get; set; }

  public User ToUser(List<Instrument> instruments)
  {
      return new User
      {
          Id = this.Id,
          Username = this.Username,
          Avatar = this.Avatar,
          FullName = this.FullName,
          Password = this.Password,
          Address = this.Address,
          Email = this.Email,
          PhoneNumber = this.PhoneNumber,
          IsAdmin = this.IsAdmin,
          BirthDate = this.BirthDate,
          Instrument = instruments.FirstOrDefault(i => i.Id == this.InstrumentId)
      };
  }

}
