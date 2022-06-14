using GoodWareMixWebClient.Model;
using GoodWareMixWebClient.Model.Entity;

namespace GoodWareMixWebClient.ViewModel
{
    public class SchedulerTaskViewModel
    {
        public PagedResponse<List<SchedulerTask>> SchedulerTaskPagedResponse { get; set; }
        public SchedulerTask SchedulerTask { get; set; }
        public ProFileSupplier ProFileSupplier { get; set; }
    }
}
