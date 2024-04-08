using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class Customers
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SecoundName { get; set; }
        public string Surname { get; set; } = string.Empty;
        public ulong? Pesel {  get; set; }

        public virtual ICollection<Factures>? Factures { get; set; }
        public virtual ICollection<CustomerAddress>? Addresses { get; set; }
    }
}
