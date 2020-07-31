using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharesAPI.DatabaseAccess;
using SharesAPI.Models;
using SharesAPI.Currency;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SharesController : ControllerBase
    {
    private readonly ISharesRepository _sharesRepository;
        public SharesController(ISharesRepository sharesRepository)
        {
            _sharesRepository = sharesRepository;
        }

        [ProducesResponseType(typeof(List<Share>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string money)
        {
           bool outOfDate = false;
            IEnumerable<Share> allShare = _sharesRepository.GetShares();
            foreach (Share share in allShare)
            {
                if ((DateTime.Now - share.lastUpdated).TotalMinutes > 10)
                {
                    outOfDate = true;
                    break;
                }
            }
            if (outOfDate || allShare.Count() == 0) await _sharesRepository.UpdateAllShares();

            IEnumerable<Share> updatedShares = _sharesRepository.GetShares();
            foreach(Share share in updatedShares)
            {
                if (money != share.currency && money != null){
                    double? converted = await CurrencyAPI.convert(money, share.price);
                    if (converted.HasValue)
                    {
                        share.currency = money;
                        share.price = converted.Value; 
                    }
                }
            }
            return Ok(updatedShares);
        }

        [ProducesResponseType(typeof(Share), 200)]
        [ProducesResponseType(typeof(Share), 404)]
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetShare([FromRoute]string symbol, [FromQuery] string money)
        {
            Share share = _sharesRepository.GetShare(symbol);
            if((DateTime.Now - share.lastUpdated).TotalMinutes > 1) await _sharesRepository.UpdateAllShares();

            Share updatedShare = _sharesRepository.GetShare(symbol);
            if (money != updatedShare.currency && money != null){
                double? converted = await CurrencyAPI.convert(money, updatedShare.price);
                if (converted.HasValue)
                {
                    updatedShare.currency = money;
                    updatedShare.price = converted.Value; 
                }
            }
            return Ok(updatedShare);
        }


    }
}
