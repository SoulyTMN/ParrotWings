using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWIngs.Models
{
    public class UserAccountDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public double Balance { get; set; }
    }
}