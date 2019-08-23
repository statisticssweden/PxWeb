using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace PXWeb.Security
{
    public class PXWebLicenseMembershipProvider : SqlMembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            if (!base.ValidateUser(username, password))
            {
                return false;
            }

            // If authentication of all users are configured
            var user = Membership.GetUser(username);
            if (user == null)
            {
                return false;
            }

            var profile = ProfileBase.Create(user.UserName);
            var license = ((DateTime)profile.GetPropertyValue("License"));

            //Licensstyrning. En licens räcker ett år. Bortser från administratörer.
            if (Roles.IsUserInRole(user.UserName, "admin") || license.AddYears(1) > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}