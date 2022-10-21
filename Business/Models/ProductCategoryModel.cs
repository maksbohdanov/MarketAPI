using System.Collections.Generic;

namespace Business.Models
{
    public class ProductCategoryModel: BaseModel
    {
        public string CategoryName { get; set; }
        public ICollection<int> ProductIds { get; set; }
    }
}
