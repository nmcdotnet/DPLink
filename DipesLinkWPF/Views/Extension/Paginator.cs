using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.Views.Extension
{
    public class Paginator : IDisposable
    {
        private bool disposedValue;

        public DataTable SourceData { get; set; }

        public static int PageSize { get; private set; }
        public int CurrentPage { get; set; } = 0;
        public int TotalPages => (int)Math.Ceiling(SourceData.Rows.Count / (double)PageSize);

        public Paginator(DataTable sourceData, int pageSize = 500)
        {
            SourceData = sourceData;
            PageSize = pageSize;
        }

        public DataTable GetPage(int pageNumber)
        {
            DataTable page = SourceData.Clone(); // Clone structure, not data
            int startIndex = pageNumber * PageSize;
            int endIndex = Math.Min((pageNumber + 1) * PageSize, SourceData.Rows.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                page.ImportRow(SourceData.Rows[i]);
            }

            return page;
        }

        public int GetCurrentPageNumber(int rowIndex)
        {
            rowIndex = Math.Max(0, rowIndex);
            int pageNumber = rowIndex / PageSize;
            return pageNumber;
        }

        public bool NextPage()
        {
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage++;
                return true;
            }
            return false;
        }

        public bool PreviousPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                return true;
            }
            return false;
        }

        public bool IsLastRowInPage(int rowIndex)
        {
            return rowIndex % PageSize == PageSize - 1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SourceData?.Dispose();
                }

                // Release unmanaged resources here.
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
