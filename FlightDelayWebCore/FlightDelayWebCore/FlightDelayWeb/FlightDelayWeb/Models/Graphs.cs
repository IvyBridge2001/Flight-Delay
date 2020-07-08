using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightDelayWeb.Models
{
    public class Graphs
    {

        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }

        [EnumDataType(typeof(Airport))]
        public Airport Origin { get; set; }

        [EnumDataType(typeof(Airport))]
        public Airport Destination { get; set; }

        public float[] Probabilities { get; set; }

        public string[] Labels { get; set; }
    }
    public enum Airport
    {
        [Display(Name = "Sao Paulo")]
        SBGR = 1,
        [Display(Name = "Rio de Janeiro")]
        SBGL = 2,
        [Display(Name = "London")]
        EGLL = 3,
        [Display(Name = "New York")]
        KJFK = 4
    }

    public class jsonResponse
    {
        public string[] Labels { get; set; }
        public float[] Probabilities { get; set; }
    }
}
