using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CinemaApp.Customer.MVCLayer.ViewModel
{
    public class MoviePaymentDetails
    {
        public int Id { get; set; }
        public double CurrentBalance { get; set; }
        public double RemainingBalance { get; set; }
        public double TransferAmount { get; set; }
        public TransferMode TransferMode { get; set; }

    }
    public enum TransferMode
    {
        [Display(Name = "Interbank Giro Transfer")] IBG,
        [Display(Name = "Instant Transfer")] IBGT
    }
}