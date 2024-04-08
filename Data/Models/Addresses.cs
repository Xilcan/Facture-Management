using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class Addresses
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Proszę podać państwo")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać miasto")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać ulice")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać numer budynku")]
        public string HouseNumber { get; set; } = string.Empty;

        public string? LocalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać kod pocztowy")]
        public string PosteCode { get; set; } = string.Empty;
    }
}
