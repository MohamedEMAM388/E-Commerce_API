using AutoMapper;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Shared.CommonResponses;
using Shared.DTOS.AuthenticationDTOS;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationServies : IAuthenticationServies
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationServies(UserManager<ApplicationUser> userManager ,
           IConfiguration configuration , IMapper mapper ) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);      
            return user != null;
        }

        public async Task<Result<UserDTO>> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User Not Found" , $"User With Email {email} was not found");

            return new UserDTO(user.Email!, user.DisplayName, await CreateToken(user));
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string Email)
        {
            var user =await _userManager.Users.Include(x => x.Address)
                                         .FirstOrDefaultAsync(x => x.Email == Email);
            if (user is null)
                return Error.NotFound(
                    "User Not Found", 
                    $"User With This Email {Email} Was Not Found"
                );

            if (user.Address is null)
                return Error.NotFound(
                    "Address Not foind",
                    $"Address for User With Email {Email} Was Not Found"
                );

            return _mapper.Map<AddressDTO>(user.Address);
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

            var token = await CreateToken(user);
            return new UserDTO(user.DisplayName , user.Email! , token);

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

            var token = await CreateToken(user);
            if (identityUser.Succeeded)
                return new UserDTO(user.DisplayName , user.Email , token);

            return identityUser.Errors.Select(E => Error.InvalidCredential(E.Code, E.Description)).ToList();
        }

        public async Task<Result<AddressDTO>> UpdateUserAddressAsync(string email, AddressDTO addressDTO)
        {
           var user  = await _userManager.Users.Include(x => x.Address)
                        .FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
                return Error.NotFound(
                    "user Not Found", 
                    $"user With Email {email} Was Not Found"
                );

            if (user.Address is not null)
            {

                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.City = addressDTO.City;
                user.Address.Country = addressDTO.Country;
                user.Address.Street = addressDTO.Street;

            }
            else
                user.Address = _mapper.Map<Address>(addressDTO);

            await _userManager.UpdateAsync(user);

            return _mapper.Map<AddressDTO>(user.Address);
        }

        // ptivate method for generating token => return string

        private async Task<string> CreateToken(ApplicationUser user) {


            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email , user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),

            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var secretkey = _configuration["JWTOptions:key"]!;
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken(

                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires : DateTime.UtcNow.AddHours(1),
                signingCredentials: cred

            );


            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
