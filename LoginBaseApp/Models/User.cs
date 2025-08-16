using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Models;


public class User
{

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
  [Ignore]
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

  public Instrument? Instrument { get; set; }
}
