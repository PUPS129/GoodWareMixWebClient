using GoodWareMixWebClient.Model;

namespace GoodWareMixWebClient.ViewModel
{
    public class ProductViewModel
    {
        public PagedResponse<List<Product>> ProductPagedResponse { get; set; }
        public Product Product { get; set; }
    }
}
