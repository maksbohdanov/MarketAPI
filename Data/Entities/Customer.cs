using System.Collections.Generic;

namespace Data.Entities
{
    public class Customer: BaseEntity
    {
        public int DiscountValue { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}