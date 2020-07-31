using System;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class Share
     {
     [Key]
     [Required]
    public string symbol { get; set; }
    [Required]
    public string name { get; set; }
    public string currency { get; set; }
    public DateTime lastUpdated {get;set;}
    public double price { get; set; }
    public string shares { get; set; }
    public int availableShares { get; set; }
     }
}