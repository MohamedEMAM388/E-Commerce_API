using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.IDentityData.DataSeed
{
    public class IdentityDataIntializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataIntializer> _logger;

        public IdentityDataIntializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<IdentityDataIntializer> logger)  
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InitializeDataAsync()
        {
            try{

                if (!_roleManager.Roles.Any()) { 
                
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin"});
                }

                if (!_userManager.Users.Any()) {

                    var user01 = new ApplicationUser() {

                        DisplayName = "Mohamed Hisham",
                        UserName = "MohamedHisham",
                        Email = "MohamedHisham@gmail.com",
                        PhoneNumber = "01021840100",
                    };

                    var user02 = new ApplicationUser()
                    {

                        DisplayName = "Ahmed Ali",
                        UserName = "AhmedAli",
                        Email = "AhmedAli@gmail.com",
                        PhoneNumber = "01022840100",
                    };

                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "Admin");

                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occurred while seeding identity data : {ex.Message} happened");
            }
        }
    }
}
