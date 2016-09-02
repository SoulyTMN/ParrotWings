using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWIngs.Models
{
    public class MyTransactionDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CorrespondentName { get; set; }
        public double Amount { get; set; }
        public Static.TransactionTypes TransactionType { get; set; }
        public double MyResultingBalance { get; set; }

    }
}