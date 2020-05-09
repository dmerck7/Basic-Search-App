using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CW.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //// Verify User
            //if (storedPassword == context.Password)
            //{
            //    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //    identity.AddClaim(new Claim(ClaimTypes.Role, RoleCode));
            //    identity.AddClaim(new Claim("Uid", UID);    

            //    context.Validated(identity);

            //    return;
            //}
            //else
            //{
            //    context.SetError("invalid_grant", "The username or password is incorrect.");
            //    return;
            //}
        }

    }
}