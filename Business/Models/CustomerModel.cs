using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class CustomerModel: BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int DiscountValue { get; set; }

        public ICollection<int> ReceiptsIds { get; set; }
    }
}
