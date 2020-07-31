using SharesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SharesAPI.DatabaseAccess
{
    public class SqlSharesRepository : ISharesRepository
    {
        private AppDbContext Context { get; }

        public SqlSharesRepository(AppDbContext context)
        {
            Context = context;
        }

        public Share Add(Share Share)
        {
            Share.availableShares = 10000;
            Share.lastUpdated = DateTime.Now;
            Context.Shares.Add(Share);
            Context.SaveChanges();
            return Share;
        }

        public Share Delete(string Symbol)
        {
            Share Share = Context.Shares.Find(Symbol);
            if (Share != null)
            {
                Context.Shares.Remove(Share);
                Context.SaveChanges();
            }
            return Share;
        }

        public IEnumerable<Share> GetShares()
        {
            return Context.Shares;
        }

        public async System.Threading.Tasks.Task UpdateAllShares()
        {
            string url = "https://api.worldtradingdata.com/api/v1/stock?symbol=FB,GOOGL,AMZN,AAPL,MSFT&api_token=U3IUGrdIPEAC1DLccCoX8KyWml0yxreXY1G5xeccGilkd1nK22wSJ1PmIVRM";
            using (HttpClient client = new HttpClient())
            {
                try{
                    client.Timeout = TimeSpan.FromSeconds(4);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        ApiRequest content = response.Content.ReadAsAsync<ApiRequest>().Result;
                        foreach (Share share in content.data)
                        {
                            Update(share);
                        }
                    }
                }
                catch{

                }
            }
        }

        public Share GetShare(string Symbol)
        {
            return Context.Shares.FirstOrDefault(emp => emp.symbol == Symbol);
        }

        public Share Update(Share ShareChanges)
        {
            Share outdatedShare = GetShare(ShareChanges.symbol);
            if (outdatedShare != null){
                outdatedShare.name = ShareChanges.name;
                outdatedShare.currency = ShareChanges.currency;
                outdatedShare.lastUpdated = DateTime.Now;
                outdatedShare.price = ShareChanges.price;
                outdatedShare.symbol = ShareChanges.symbol;

                Context.SaveChanges();
            }
            else {
                Add(ShareChanges);
            }
            return outdatedShare;
        }
    }
}
