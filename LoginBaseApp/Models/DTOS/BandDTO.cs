using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginBaseApp.Models.DTOS
{
  public class BandDTO
  {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    // Serialized!!!!!
    public string BandMemberIdsSerialized { get; set; }

    // Only for serialization purposes, not stored in the database!!!!!
    [Ignore]
    public List<int> BandMemberIds
    {
      get => BandMemberIdsSerialized?.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList() ?? new List<int>();
      set => BandMemberIdsSerialized = string.Join(",", value);
    }
    //Not realy necessary, using Serialized
    //[OneToMany(CascadeOperations = CascadeOperation.All)]
    //public List<UserDTO> BandMembers { get; set; } = new List<UserDTO>();

    public BandDTO() { }

    
    public BandDTO(Band band)
    {
      Id = band.Id;
      Name = band.Name;
      BandMemberIds = band.BandMembers.Select(i => i.Id).ToList();
    }

    public Band ToBand(List<User> users)
    {
      Band band = new Band
      {
        Id = this.Id,
        Name = this.Name,
        BandMembers = users.Where(u => this.BandMemberIds.Contains(u.Id)).ToList()
      };
      return band;
    }
  }
}
