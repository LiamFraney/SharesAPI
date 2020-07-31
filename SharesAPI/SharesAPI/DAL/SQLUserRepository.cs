using Microsoft.EntityFrameworkCore;
using SharesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SharesAPI.DatabaseAccess
{
    public class SqlUserRepository : IUsersRepository
    {
        private AppDbContext Context { get; }

        public SqlUserRepository(AppDbContext context)
        {
            Context = context;
        }

        public User Add(CreateUser User)
        {
            User newUser = new User();
            newUser.username = User.username;
            newUser.password = User.password;
            newUser.pennies = 10000;
            newUser.AquiredShares = new List<AquiredShares>();

            Context.Users.Add(newUser);
            Context.SaveChanges();
            return newUser;
        }

        public User Delete(string username)
        {
            User User = Context.Users.Find(username);
            if (User != null)
            {
                Context.Users.Remove(User);
                Context.SaveChanges();
            }
            return User;
        }

        public User GetUser(string username)
        {
            return Context.Users.Include(x => x.AquiredShares).FirstOrDefault(emp => emp.username == username);
        }
        public User Authenticate(string username, string password){
            User SelectedUser = GetUser(username);
            if(SelectedUser.password != SelectedUser.password){
                return null;
            }
            
            return(SelectedUser);
        }
        public User PurchaseShare(string username, int quantity, Share share){
            User user = GetUser(username);
            if(user.pennies - (share.price * quantity) < 0){
                return null;
            }
            user.pennies -= share.price * quantity;

            bool update = false;
            foreach (AquiredShares aquired in user.AquiredShares){
                if (aquired.type == share.symbol){
                    update = true;
                    AquiredShares alreadyAquired = Context.AquiredShares.FirstOrDefault(x => x.ID == aquired.ID);
                    alreadyAquired.quantity += quantity;
                }
            }
            if (!update){
                AquiredShares aquired = new AquiredShares();
                aquired.type = share.symbol;
                aquired.quantity = quantity;
                Context.AquiredShares.Add(aquired);
                user.AquiredShares.Add(aquired);
            }
            Context.SaveChanges();
            User updated = GetUser(username);
            return updated;

        }

        public User Update(User UserChanges)
        {
            User outdatedUser = GetUser(UserChanges.username);
            if (outdatedUser != null){
                outdatedUser.pennies = UserChanges.pennies;
                outdatedUser.username = UserChanges.username;
                outdatedUser.password = UserChanges.password;
                outdatedUser.AquiredShares = UserChanges.AquiredShares;

                Context.SaveChanges();
            }
            return outdatedUser;
        }
    }
}
