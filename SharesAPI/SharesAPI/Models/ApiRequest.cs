using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class ApiRequest
    {
        public int symbols_requested;
        public int symbols_returned;
        public List<Share> data;
    }
}
