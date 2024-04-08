using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class CompanyAddress : Addresses
    {
        public long CompanyId { get; set; }

        public virtual required Companies Company { get; set; }
    }
}
