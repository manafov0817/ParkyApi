using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models
{
    public class Trail
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public enum DiffucltyType { easy, medium, hard, expert }

        public DiffucltyType Diffuclty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public double Elevation { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}
