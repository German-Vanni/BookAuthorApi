﻿namespace BookAuthor.Api.Model.Paging
{
    public class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = Math.Abs(value) > maxPageSize ? maxPageSize : Math.Abs(value);
            }
        }
    }
}
