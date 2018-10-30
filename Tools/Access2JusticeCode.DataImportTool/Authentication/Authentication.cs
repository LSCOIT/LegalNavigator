using Microsoft.Identity.Client;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access2Justice.DataImportTool.Authentication
{
    public class Authentication
    {
        private PublicClientApplication app = null;
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];

        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        private static string todoListScope = ConfigurationManager.AppSettings["todo:TodoListScope"];
        private static string todoListBaseAddress = ConfigurationManager.AppSettings["todo:TodoListBaseAddress"];
        private static string[] scopes = new string[] { todoListScope };
        
        public Authentication()
        {
            app = new PublicClientApplication(clientId, authority, TokenCacheHelper.GetUserCache());
        }

        public async Task<AuthenticationResult> Login()
        {

            var accounts = await app.GetAccountsAsync().ConfigureAwait(false);
            
            // clear the cache
            while (accounts.Any())
            {
                await app.RemoveAsync(accounts.First());
                accounts = await app.GetAccountsAsync();
            }

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenAsync(scopes, accounts.FirstOrDefault(), UIBehavior.SelectAccount, string.Empty);
                return result;
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode == "access_denied")
                {
                    // The user canceled sign in, take no action.
                }
                else
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageBox.Show(message);
                }
                return null;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception : " + ex.InnerException.Message;
                }
                MessageBox.Show(message);
                return null;
            }

        }

        public async Task<string> Logout()
        {
            var accounts = await app.GetAccountsAsync().ConfigureAwait(false);

            while (accounts.Any())
            {
                await app.RemoveAsync(accounts.First());
                accounts = await app.GetAccountsAsync();
            }
            return "";
        }
    }
}
