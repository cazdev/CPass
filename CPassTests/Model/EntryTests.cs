using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CPass.Model.Tests
{
    [TestClass()]
    public class EntryTests
    {
        private readonly Random rnd = new Random();

        [TestMethod()]
        public void EntryTest()
        {
            string name = CreateRandomString();
            string username = CreateRandomString();
            string pass = CreateRandomString();

            Entry entry = new Entry(name, username, pass);

            Assert.AreEqual(entry.Name, name);
            Assert.AreEqual(entry.UserName, username);
            Assert.AreEqual(entry.Password, pass);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            string name = CreateRandomString();
            string username = CreateRandomString();
            string pass = CreateRandomString();

            Entry entry = new Entry(name, username, pass);

            Assert.AreEqual(entry.ToString(), $"{name} - User: {username}, Pass: {pass}");
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