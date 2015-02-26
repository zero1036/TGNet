using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Utility.AppCommon;
using System.Security.Cryptography;


namespace EG.Utility.Test
{
    [TestClass]
    public class SecurityTest
    {
        [TestMethod]
        public void Security()
        {
            Security s = new Security();
            Assert.AreEqual("abcdefghijkl123456", s.Decrypt(s.Encrypt("abcdefghijkl123456", "abc123bv"), "abc123bv"), "error");
        }

        [TestMethod]
        public void BytesToStr_Test()
        {
            byte[] b1 = new byte[] { 147, 193, 237, 244, 109, 65, 15 };

            string s = Convert.ToBase64String(b1);

            byte[] b2 = Convert.FromBase64String(s);

        }

        [TestMethod]
        public void SecurityTestKeyLength()
        {
            Security s = new Security();
            Assert.AreEqual("abcdefghijkl123456", s.Decrypt(s.Encrypt("abcdefghijkl123456", "123"), "123"), "error");
        }

    }


}
