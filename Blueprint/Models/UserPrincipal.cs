using Blueprint.DbOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Blueprint.Models
{
    public class UserPrincipal : IPrincipal
    {
        private readonly IIdentity identity;

        public UserPrincipal(IIdentity identity)
        {
            using (var context =  new BaseDA("YourCSName"))
            {
                this.identity = identity;

                var user = context.GetUserInfo(identity.Name);

                Username = user.Username;
                Role = user.Role;
            }
        }

        public string Username { get; set; }
        public string Role { get; set; }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return Role == role;
        }
    }
}