using GoodWareMixWebClient.Model;

namespace GoodWareMixWebClient.ViewModel
{
    public class AttributeViewModel
    {
        public PagedResponse<List<AttributeEntity>> AttributePagedResponse { get; set; }
        public AttributeEntity Attribute { get; set; }
    }
}
