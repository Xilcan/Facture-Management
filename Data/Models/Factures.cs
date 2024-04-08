namespace Data.Models
{
    internal class Factures
    {
        public long Id { get; set; }

        public long NumberFactures { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime SaleDate { get; set; } = DateTime.Now;

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string Comments { get; set; } = string.Empty;


        private DateTime _lastModification;

        public DateTime? LastModification
        {
            get 
            { 
                return _lastModification; 
            }
            set
            {
                if (value == null)
                {
                    _lastModification = CreationDate;
                }
                else
                {
                    _lastModification = value.Value;
                }
            }
        }

        public long? CustomerId { get; set; }

        public long? CompanyId { get; set; }

        public virtual Customers? Customer { get; set; }

        public virtual Companies? Company { get; set; }

        public virtual ICollection<FactureDetail>? FactureDetails { get; set; }

        public virtual ICollection<Payments>? Payments { get; set; }
    }
}
