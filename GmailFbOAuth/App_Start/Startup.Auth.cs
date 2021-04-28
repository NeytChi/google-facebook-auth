using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Net;
using System.Security.Claims;

namespace GmailFbOAuth
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            var fb = new FacebookAuthenticationOptions
            {
                AppId = "1983015365196183",
                AppSecret = "627c57744cdd227ee3b611c6b1a44df3",
                AuthenticationType = "Facebook",
                SignInAsAuthenticationType = "ExternalCookie",
                UserInformationEndpoint = "https://graph.facebook.com/v10.0/me?fields=id,name,birthday,last_name,first_name,email,hometown",
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = async ctx =>
                    {
                        if (ctx.User["birthday"] != null)
                        {
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, ctx.User["birthday"].ToString()));
                        }
                        if (ctx.User["hometown"] != null)
                        {
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.StreetAddress, ctx.User["hometown"].ToString()));
                        }
                    }  
                }
            };
            fb.Fields.Add("birthday");
            fb.Fields.Add("hometown");
            fb.Scope.Add("email");
            fb.Scope.Add("user_birthday");
            fb.Scope.Add("user_hometown");
            app.UseFacebookAuthentication(fb);
            /*
            app.UseFacebookAuthentication(
               appId: "1983015365196183",
               appSecret: "627c57744cdd227ee3b611c6b1a44df3");
            */
            
            var gg = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "677385549022-213opd8o4usk576fklmd30od41ncrm79.apps.googleusercontent.com",
                ClientSecret = "SRG2sjVW7SFUKHEA3qmzutZ7",

                Provider = new GoogleOAuth2AuthenticationProvider
                {

                    OnAuthenticated = async ctx =>
                    {
                        ctx.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, "birthday"));
                        ctx.Identity.AddClaim(new Claim(ClaimTypes.StreetAddress, "streetaddress"));
                        if (ctx.User["hometown"] != null)
                        {
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.StreetAddress, ctx.User["hometown"].ToString()));
                        }
                        ctx.Identity.AddClaim(new Claim("GoogleAccessToken", ctx.AccessToken));
                    }
                }
            };
            gg.Scope.Add("profile");
            /*app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "677385549022-213opd8o4usk576fklmd30od41ncrm79.apps.googleusercontent.com",
                ClientSecret = "SRG2sjVW7SFUKHEA3qmzutZ7"
            });*/
            app.UseGoogleAuthentication(gg);
        }
    }
}