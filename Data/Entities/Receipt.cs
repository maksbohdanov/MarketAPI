using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Receipt: BaseEntity
    {
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
