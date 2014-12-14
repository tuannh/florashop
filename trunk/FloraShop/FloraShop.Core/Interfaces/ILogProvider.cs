using FloraShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Interfaces
{
    public interface ILogProvider
    {
        void LogException(SiteException exception, int logId);

        void LogException(Exception exception, int logId);

        void DeleteExceptions(string logID);

        List<SiteException> GetAllExceptions(int pageIndex, int pageSize, out int totalRecords);

        List<SiteException> GetAllExceptions(string FileName, int pageIndex, int pageSize, out int totalRecords);

        SiteException GetException(string logID);

        SiteException GetException(string FileName, string logID);
    }
}
