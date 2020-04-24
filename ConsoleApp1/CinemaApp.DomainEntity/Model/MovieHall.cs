using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.DomainEntity.Model
{
    public class MovieHall
    {
        public int Id { get; set; }
        [Display(Name = "Movie Hall Id")]
        public int MovieHallId { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        [Display(Name ="Movie Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, dd MMMM yyyy HH:mm:ss}")]
        public DateTime MovieDateTime { get; set; }
    }
}
