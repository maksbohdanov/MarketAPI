using System.Collections.Generic;


namespace Business.Models
{
    public class ProductModel: BaseModel
    {
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public ICollection<int> ReceiptDetailIds { get; set; }
    }
}
