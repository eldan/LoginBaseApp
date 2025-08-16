using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Models;


public class Band
{
  [PrimaryKey]
  public int Id { get; set; }
 
  public  string Name { get; set; }

  [Ignore]
  public List<User> BandMembers { get; set; } = new List<User>();
}
