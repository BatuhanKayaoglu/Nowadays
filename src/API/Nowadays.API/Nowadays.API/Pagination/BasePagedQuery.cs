namespace Nowadays.API.Pagination
{
    // When a query is sent to us from outside, for example, I want the 3rd page of this data. He should send me some information.
    // In this information, he/she should send information about which page he/she wants and how many elements he/she wants on this page.
    public class BasePagedQuery
    {
        public BasePagedQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
