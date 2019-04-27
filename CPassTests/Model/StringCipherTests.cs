using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CMessage.Tests
{
    [TestClass()]
    public class StringCipherTests
    {
        private readonly Random rnd = new Random();
        [TestMethod()]
        public void EncryptDecryptTest()
        {
            string clearText = CreateRandomString();
            string pass = CreateRandomString();

            string encText = StringCipher.Encrypt(clearText, pass);
            string decText = StringCipher.Decrypt(encText, pass);

            Assert.AreEqual(clearText, decText);
        }

        private string CreateRandomString()
        {
            string result = "";
            for (int i = 0; i < rnd.Next(100, 200); i++)
            {
                result += (char)rnd.Next(40, 119);
            }
            return result;
        }
    }
}