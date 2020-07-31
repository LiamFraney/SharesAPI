using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class User
     {
     [Key]
     public string username { get; set;}
     public string password { get; set; }
     public double pennies { get; set; }
     public List<AquiredShares> AquiredShares { get; set; }
     }
}
