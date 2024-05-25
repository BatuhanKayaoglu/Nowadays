namespace Nowadays.API.Pagination
{
    // After Pagination, I create this viewModel in order to return the data about the page while also returning the information about the page.
    public class PagedViewModel<T> where T:class 
    {
        public PagedViewModel() : this(new List<T>(), new Page()) // I created this ctor so that I can create a new one if the results and pageInfo information I want are not received.
        {

        }
        public PagedViewModel(IList<T> results, Page pageInfo)
        {
            Results = results;
            PageInfo = pageInfo;
        }

        public IList<T> Results { get; set; } 
        public Page PageInfo { get; set; } 
    }
}
