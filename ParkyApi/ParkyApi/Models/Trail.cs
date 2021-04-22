using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models
{
    public class Trail : AbstractModel
    {
        [Required]
        public double Distance { get; set; }

        public enum DiffucltyType { easy, medium, hard, expert }

        public DiffucltyType Diffuclty { get; set; }

        [Required]
        public int NationalParkId { get; set; }
        [ForeignKey("NationalParkId")]
        public NationalPark NationalPark { get; set; }

        public double Elevation { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
