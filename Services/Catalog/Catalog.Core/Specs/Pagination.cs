﻿namespace Catalog.Core.Specs
{
    public class Pagination<T> where T : class
    {
        public Pagination() { }
        public Pagination(int pageIndex, int PageSize, int count, IReadOnlyList<T> data) 
        {
            PageIndex = pageIndex;
            this.PageSize = PageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
