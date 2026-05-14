using Shared.CommonResponses;
using Shared.DTOS.AuthenticationDTOS;
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

    }
}
