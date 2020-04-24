using CinemaApp.DomainEntity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaApp.Customer.MVCLayer.ViewModel
{
    public class MovieHallsData
    {
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public int RowAndColumn { get; set; }
        public DateTime MovieDateTime { get; set; }
        public IEnumerable<MovieHall> MovieHallDatas { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}