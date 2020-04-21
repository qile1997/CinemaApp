using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class Transactions
    {
        public int Id { get; set; }
        [Display(Name ="Transfer Mode")]
        public Transfer TransferMode { get; set; }
        public int UserDetailsId { get; set; }
        [Display(Name = "Transfer Amount(RM)")]
        public string TransferAmount { get; set; }
        [Display(Name = "Transaction")]
        public TransactionType Transaction { get; set; }
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }
    }
    public enum Transfer
    {
        
        [Display(Name = "Interbank Giro Transfer")] IBG,
        [Display(Name = "Instant Transfer")] IBGT
    }
    public enum TransactionType
    {
        Refund, Purchase
    }

}
