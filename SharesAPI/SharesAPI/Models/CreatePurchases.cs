using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class CreatePurchase
    {
        public int Quantity { get; set; }
        public string Symbol { get; set; }
    }
}
