using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AuthenticationDTOS;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationServies _authenticationServies;

        public AuthenticationController(IAuthenticationServies authenticationServies) 
        {
            _authenticationServies = authenticationServies;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO) {

            var result = await _authenticationServies.LoginAsync(loginDTO);

            return HandleResult(result);

        }


        [HttpPost("Register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) {

            var result = await _authenticationServies.RegisterAsync(registerDTO);

            return HandleResult(result); 
        
        }

        // check email exist
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> EmailExist(string email) { 
        
            var result = await _authenticationServies.CheckEmailExistAsync(email);

            return Ok(result);
        }

        // get current user
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser() {

            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationServies.GetCurrentUserAsync(email!);
            return HandleResult(result);
        }


        // get current user address
        [Authorize]
        [HttpGet("UserAddress")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress() {

            var result = await _authenticationServies.GetUserAddressAsync(GeEmailFromTokenClaims());

            return HandleResult(result);
        }

        // update Current user address
        [Authorize]
        [HttpPut("UpdateAddress")]

        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO) {

            var result = await _authenticationServies.UpdateUserAddressAsync(
                                      GeEmailFromTokenClaims(), addressDTO);
            return HandleResult(result);
        }

    }
}
