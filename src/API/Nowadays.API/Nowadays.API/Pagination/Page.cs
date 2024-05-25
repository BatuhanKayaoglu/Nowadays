using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Drawing.Printing;
using System.Reflection.Metadata;

namespace Nowadays.API.Pagination
{
    public class Page
    {
        #region Summary - 1 
        /// <summary>
        // This constructor creates the first page of a page by default when called without any parameters. The total number of rows is not specified,
        // so the default value is 0. This can often be used to load pagination information.
        // Scenario: User wants to browse all data in the database, but does not need information about paging configuration. 
        // Depending on the scenario, this constructor creates the first page with a totalRowCount of 0 by default.
        /// </summary>
        #endregion
        public Page() : this(0)
        {

        }


        #region Summary - 2 
        /// <summary>
        // When this constructor is called specifying the total number of rows, it sets the first page and pageSize to 10 by default.
        // Scenario: A user wants to see the first 10 elements of a page and knows how many elements there are in total.
        /// </summary>
        #endregion
        public Page(int totalRowCount) : this(1, 10, totalRowCount)
        {

        }

        #region Summary - 3
        /// <summary>
        //Scenario: A user wants to see data on a specific page size (pageSize) but does not want to specify which page it is on.
        #endregion
        public Page(int pageSize, int totalRowCount) : this(1, pageSize, totalRowCount) 
        {

        }

        #region Summary - 4
        /// <summary>
        // When this constructor is called specifying a specific page number, page size, and total number of rows, it creates the specified page.
        // Scenario: A user wants to see data at a specific page size (pageSize) on a specific page number (currentPage).
        /// </summary>
        #endregion
        public Page(int currentPage, int pageSize, int totalRowCount)
        {
            if (currentPage < 1)
                throw new ArgumentException("Invalid page number!");

            if (pageSize < 1)
                throw new ArgumentException("Invalid page size!");

            TotalRowCount = totalRowCount;
            CurrentPage = currentPage;
            PageSize = pageSize;

        }



        public int CurrentPage { get; set; }   
        public int PageSize { get; set; }
        public int TotalRowCount { get; set; } 
        public int TotalPageCount => (int)Math.Ceiling((double)TotalRowCount / PageSize); // We find the total number of pages by using TotalRowCount/PageSize.
        public int Skip => (CurrentPage - 1) * PageSize; 
    }
}
