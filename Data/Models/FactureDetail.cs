using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class FactureDetail
    {
        public long Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 9999999999999999.99)]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public long FactureId { get; set; }

        public long ProductId { get; set; }

        public Factures Factures { get; set; } = new Factures();

        public Products Products { get; set; } = new Products();
    }
}
