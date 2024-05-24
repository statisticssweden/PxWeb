using System.Web;

namespace PXWeb.Management
{
    public class AuthorizationUtil
    {
        //Verify that user is authorized to view the table
        public static bool IsAuthorized(string databaseId, string menu, string selection)
        {
            var dbcfg = Settings.Current.Database[databaseId];
            if (dbcfg != null)
            {
                if (dbcfg.Protection.IsProtected)
                {
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        //TODO AuthenticationMethod
                        //PXWeb.Security.IAuthentication auth = GetAuthentication(dbcfg.Protection.AuthenticationMethod);
                        //auth.Autenticate();
                    }
                    if (!dbcfg.Protection.AuthorizationMethod.IsAuthorized(databaseId, menu, selection))
                    {
                        //HttpContext.Current.Response.Redirect(LinkManager.CreateLink("~/Menu.aspx", new LinkManager.LinkItem() {Key = "msg", Value = "UnauthorizedTable" }));
                        return false;
                    }
                }
            }
            return true;
        }
    }
}