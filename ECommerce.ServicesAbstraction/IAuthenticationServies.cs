using Shared.CommonResponses;
using Shared.DTOS.AuthenticationDTOS;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IAuthenticationServies
    {
        // login method => take email & password -- return displayname , email , token (jwt)
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);

        //  register method => take email , displayname , username , password , phone number
        //  -- return displayname , email , token (jwt)
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);

        // check email exist 
        Task<bool> CheckEmailExistAsync(string email);

        // get  current user by email
        // return not Found if user not exist
        Task<Result<UserDTO>> GetCurrentUserAsync(string email);

        // get current User addres by email 
        Task<Result<AddressDTO>> GetUserAddressAsync(string Email);

        // update Address for current user
        Task<Result<AddressDTO>> UpdateUserAddressAsync(string email, AddressDTO addressDTO);

    }
}
