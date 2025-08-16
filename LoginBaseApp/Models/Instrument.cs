using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Models;

[SQLite.Table("InstrumentTbl")]//לא חייבים להגדיר את השם של הטבלה, אם לא נכתוב הוא ישתמש בשם של המחלקה
public class Instrument
    {
  [PrimaryKey]
  [AutoIncrement]
  public int Id { get; set; }

  
      public  string Name { get; set; }

  
      public string? Picture { get; set; }

    }
