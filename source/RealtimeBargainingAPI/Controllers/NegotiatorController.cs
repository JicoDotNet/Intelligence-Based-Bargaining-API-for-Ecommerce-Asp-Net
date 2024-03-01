using AIBasedRealtimeBargaining.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealtimeBargainingAPI.Logic;
using System.Net;

namespace RealtimeBargainingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiatorController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Negotiate(RequestCommand command)
        {
            try
            {
                if (command.TokenKey != GenericLogic.DefaultToken)
                {
                    return Unauthorized(new ResponseCommand { IsSuccess = false, StatusCode = HttpStatusCode.Unauthorized, Message = "You are Unauthorized.", _response = null });
                }
                if (command == null
                        || string.IsNullOrEmpty(command.Tenant)
                        || command.OfferPrice <= 0
                        || command.ThresholdPrice <= 0
                        || command.ThresholdPrice >= command.OfferPrice
                        || command.CustomerId <= 0
                        || command.ProductId <= 0
                        || command.ProposedCost <= 0)
                {
                    return Ok(new ResponseCommand { IsSuccess = false, StatusCode = HttpStatusCode.Forbidden, Message = "Param Mismatch", _response = null });
                }
                NegotiatorLogic negotiator = new NegotiatorLogic();
                ResponseCommand response = new ResponseCommand()
                {
                    IsSuccess = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    _response = negotiator.NextValue(command)
                };
                return Ok(response);
            }
            catch (Exception Ex)
            {
                return Problem(Ex.Message, null, 500);// (new ResponseCommand { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = Ex.Message, _response = null });
            }
        }
    }
}
