using SharesAPI.Models;
using System.Collections.Generic;

namespace SharesAPI.DatabaseAccess
{
    public interface ISharesRepository
    {
        Share GetShare(string symbol);
        IEnumerable<Share> GetShares();
        Share Add(Share symbol);
        Share Update(Share shareChanges);
        Share Delete(string symbol);
        System.Threading.Tasks.Task UpdateAllShares();
    }
}
