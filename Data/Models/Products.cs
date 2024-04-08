using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class Products
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TransactionTypes TransactionType {  get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 9999999999999999.99)]
        public decimal Price { get; set; } = decimal.Zero;

        public long CategoryId { get; set; }

        public virtual ICollection<FactureDetail>? FactureDetails { get; set; }

        public virtual ProductCategory Category { get; set; } = new ProductCategory();
    }
}
