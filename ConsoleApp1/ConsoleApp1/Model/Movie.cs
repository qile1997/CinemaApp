using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string ReleaseDate { get; set; }
        public bool MovieAvailability { get; set; }

        //public Movie()
        //{
        //    this.MovieId = Program.GetId();
        //}
    }
}
