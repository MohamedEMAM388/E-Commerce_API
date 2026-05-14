using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Shared.CommonResponses;
using Shared.DTOS.AuthenticationDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationServies : IAuthenticationServies
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationServies(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            // get user from db 
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            // check if user not null 
            if (user is null)
                return Error.InvalidCredential("User.InvalidCreadrntial");

            // check if password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordValid)
                return Error.InvalidCredential("User.InvalidCreadrntial");

            return new UserDTO(user.DisplayName , user.Email! , "Token");

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser() { 
            
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,

            };

            var identityUser = await _userManager.CreateAsync(user, registerDTO.Password);

            if (identityUser.Succeeded)
                return new UserDTO(user.DisplayName , user.Email , "Token");

            return identityUser.Errors.Select(E => Error.InvalidCredential(E.Code, E.Description)).ToList();
        }
    }
}
