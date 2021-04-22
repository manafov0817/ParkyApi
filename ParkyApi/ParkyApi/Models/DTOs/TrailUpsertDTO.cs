using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ParkyApi.Models.Trail;

namespace ParkyApi.Models.DTOs
{
    public class TrailUpsertDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DiffucltyType Diffuclty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public double Elevation { get; set; }

    }
}
