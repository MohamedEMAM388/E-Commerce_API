using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.AuthenticationDTOS
{
    public record RegisterDTO
        // application Validation only in DTOS
        ([EmailAddress] string Email ,
        string DisplayName ,
        string UserName ,
        string Password ,
        [Phone] string PhoneNumber
    );

}
