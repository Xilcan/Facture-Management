using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class CustomerAddress : Addresses
    {
        public long CustomerId { get; set; }

        public virtual required Customers Customer { get; set; }
    }
}
