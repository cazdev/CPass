using System.Net;
using System.Security;

namespace CPass.ViewModel
{
    public class ViewModel
    {
        public bool AddNew = false;
        public bool removeBool = true;
        public bool wipeBool = false;
        public bool settingsBool = false;
        public bool switchLoading = false;
        public bool copyBool = false;

        public static SecureString SecString = new NetworkCredential("", "TljO3NBhu088VIE36VSAmzJqfo1qSKFg48D9VzHAAythPimmy/+KRCys1pqBptpqB4WEczUNbVG645WSWy77cCgcW7v05ur+qyWzF+ZlIA/fIhdwCJG8s41oEWkT58re").SecurePassword;
        public static string pass => new NetworkCredential("", SecString).Password;
    }
}
