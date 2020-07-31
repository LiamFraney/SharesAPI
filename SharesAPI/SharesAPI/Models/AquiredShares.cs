using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class AquiredShares
     {
    [Key]
    public Guid ID { get; set; }
     public double quantity { get; set; }
     public string type { get; set; }
     public AquiredShares(){
         ID = Guid.NewGuid();
     }
     }
}
