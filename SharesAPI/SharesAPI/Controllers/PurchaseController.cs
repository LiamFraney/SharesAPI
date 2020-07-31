using Microsoft.AspNetCore.Mvc;
using SharesAPI.DatabaseAccess;
using SharesAPI.Models;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseController : ControllerBase
    {
    private readonly ISharesRepository _sharesRepository;
    private readonly IUsersRepository _userRepository;
        public PurchaseController(ISharesRepository sharesRepository, IUsersRepository usersRepository)
        {
            _sharesRepository = sharesRepository;
            _userRepository = usersRepository;
        }

        [ProducesResponseType(typeof(User),200)]
        [ProducesResponseType(typeof(string),400)]
        [ProducesResponseType(typeof(string),403)]
        [ProducesResponseType(typeof(string),404)]
        [HttpPost()]
        public IActionResult Create([FromBody] CreatePurchase createPurchase){
            string username = Request.Headers["username"];
            string password = Request.Headers["password"];
            User user = _userRepository.GetUser(username);
            if(user == null){
                return NotFound("Username and password combination not found.");
            }
            User validUser = _userRepository.Authenticate(username, password);
            if(validUser == null){
                return Forbid("it is forbidden.");
            }
            Share updatedShare = _sharesRepository.GetShare(createPurchase.Symbol);
            if(updatedShare == null){
                return NotFound("Share not found");
            }
            if(updatedShare.availableShares - createPurchase.Quantity < 0){
                return BadRequest("Requested amount exceeds the amount we currently have");
            }
            updatedShare.availableShares -= createPurchase.Quantity;
            Share share = _sharesRepository.Update(updatedShare);

            User updatedUser = _userRepository.PurchaseShare(user.username, createPurchase.Quantity, share);
            if(updatedUser == null){
                return BadRequest("You require more funds");
            }
            return Ok(updatedUser);
        }


    }
}
