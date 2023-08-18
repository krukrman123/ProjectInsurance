﻿namespace ProjectInsurance.Models
{
    public class PagerModel
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public PagerModel()
        {

        }
        public PagerModel(int totalItems, int page, int pageSize = 5)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems/(decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 1;
            int endPage = currentPage + 1;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;      
            }
            if (endPage > TotalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 2;
                }
            }

            TotalPages = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;    
            TotalPages = totalPages;
            StartPage = startPage;  
            EndPage = endPage;
        }
    }
}
