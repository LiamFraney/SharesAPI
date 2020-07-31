using SharesAPI.Models;
using System.Collections.Generic;

namespace SharesAPI.DatabaseAccess
{
    public interface IUsersRepository
    {
        User GetUser(string username);
        User Authenticate(string username, string password);
        User Add(CreateUser username);
        User Update(User userChanges);
        User Delete(string username);
        User PurchaseShare(string username, int quantity, Share share);
    }
}
