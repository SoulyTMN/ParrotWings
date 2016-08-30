using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWIngs.Models
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public string PayeeName { get; set; }
        public string PayeeEmail { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}