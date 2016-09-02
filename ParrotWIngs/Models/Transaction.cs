using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ParrotWIngs.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string PayeeId { get; set; }
        public ApplicationUser Payee { get; set; }
        public double ResultingPayeeBalance { get; set; }
        [Required]
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }
        public double ResultingRecipientBalance { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}