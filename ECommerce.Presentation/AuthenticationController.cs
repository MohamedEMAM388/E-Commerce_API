using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AuthenticationDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
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

            return HandleResult<UserDTO>(result);

        }


        [HttpPost("Register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) {

            var result = await _authenticationServies.RegisterAsync(registerDTO);

            return HandleResult<UserDTO>(result);
        
        }

    }
}
