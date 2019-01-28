using System.Collections.Generic;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using Microsoft.AspNetCore.Identity;

namespace CoreNG.Domain.Accounts.Requests
{
    public class SeedAuthDbRequest
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;

        public SeedAuthDbRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seed()
        {
            SeedRoles();
            _context.SaveChanges();

            SeedUsers();
            _context.SaveChanges();
        }

        private void SeedUsers()
        {
            var users = new List<string>()
            {
                "admin:password123:Admin",
                "user:password123:User"
            };

            foreach (var user in users)
            {
                var parts = user.Split(":");

                var userName = parts[0];
                var password = parts[1];
                var role = parts[2];
                
                if (!_context.Users.Any(x => x.UserName == userName))
                {
                    var u = new ApplicationUser();
                    u.UserName = userName;
                    u.Email = $"{userName}@127.0.0.1";
                    var task = _userManager.CreateAsync(u);
                    task.Wait();
                    task = _userManager.AddPasswordAsync(u, password);
                    task.Wait();

                    var userRole = _context.Roles.Single(x => x.Name == role);
                    var ur = new IdentityUserRole<string>();
                    ur.UserId = u.Id;
                    ur.RoleId = userRole.Id;
                    _context.UserRoles.Add(ur);
                    task.Wait();
                }
            }
        }
        private void SeedRoles()
        {
            var roles = new List<string>()
            {
                "Admin",
                "User"
            };

            foreach (var role in roles)
            {
                if (!_context.Roles.Any(x => x.Name == role))
                {
                    var r = new IdentityRole();
                    r.Name = role;
                    _context.Roles.Add(r);
                }    
            }
        }
    }
}