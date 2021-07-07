using CMessage;
using System;

namespace CPass.Model
{
    [Serializable]
    /// <summary>
    /// Strage of encrypted information about Entry
    /// </summary>
    public class Entry
    {
        private string name, username, password;
        public string Name { get => StringCipher.Decrypt(name, ViewModel.ViewModel.pass); set => name = StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
        public string UserName { get => StringCipher.Decrypt(username, ViewModel.ViewModel.pass); set => username = StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
        public string Password { get => StringCipher.Decrypt(password, ViewModel.ViewModel.pass); set => password = StringCipher.Encrypt(value, ViewModel.ViewModel.pass); }
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

        public override string ToString()
        {
            return $"{Name} - User: {UserName}, Pass: {Password}";
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
