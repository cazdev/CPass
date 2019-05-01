using CMessage;

namespace CPass.Model
{
    /// <summary>
    /// Strage of encrypted information about Entry
    /// </summary>
    public class Entry
    {
        public string Name { get => StringCipher.Decrypt(Name, ViewModel.ViewModel.pass); set => StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
        public string UserName { get => StringCipher.Decrypt(UserName, ViewModel.ViewModel.pass); set => StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
        public string Password { get => StringCipher.Decrypt(Password, ViewModel.ViewModel.pass); set => StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
        public Custom[] customFields;
        public bool IsCustom => customFields.Length == 0;

        public Entry(string name, string userName, string password)
        {
            Name = name;
            UserName = userName;
            Password = password;
        }

        public Entry(string name, string userName, string password, Custom[] customFields) : this(name, userName, password)
        {
            this.customFields = customFields;
        }
    }
    /// <summary>
    /// Field for custom Entry
    /// </summary>
    public struct Custom
    {
        public string Name;
        public string Value;
        /*example:
         * Name - website
         * Value - facebook.com
         */
    }
}
