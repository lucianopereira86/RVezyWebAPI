using System;

namespace RVezy.Domain.Domain.Models
{
    public class PageOptions
    {
        public PageOptions()
        {
            Page = 1;
            Count = 10;
        }
        public PageOptions(int page, int count)
        {
            Page = page;
            Count = count;
        }
        public int Page { get; set; }

        public int Count { get; set; }
    }

    public class PageOptionsResponse
    {
        public PageOptionsResponse()
        {
        }

        public PageOptionsResponse(PageOptions input, int totalRecords)
        {
            CurrentCount = input.Count;
            CurrentPage = input.Page;
            CurrentUpperRange = (input.Page * input.Count);
            CurrentLowerRange = CurrentUpperRange - (input.Count - 1);
            NextPage = (((int)Math.Ceiling((double)totalRecords / (double)input.Count)) > input.Page) ? input.Page + 1 : input.Page;
            PreviousPage = input.Page > 0 ? input.Page - 1 : 0;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((double)totalRecords / (double)input.Count);
        }

        public int CurrentPage { get; set; }
        public int CurrentCount { get; set; }
        public int TotalRecords { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentLowerRange { get; set; }
        public int CurrentUpperRange { get; set; }
    }
}
