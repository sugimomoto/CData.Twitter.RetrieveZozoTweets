using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CData.Twitter.RetrieveZozoTweets;

namespace CData.Twitter.RetrieveZozoTweets.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WriteJsonFile()
        {
            var folder = @"C:\Work\ZozoTweets\";

            Program.writeJsonInLocalFolder(folder,"HelloJson",1);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetAccessTokenTest()
        {
            var accessToken = Program.getAccessToken();
            Console.WriteLine(accessToken);
            Assert.IsNotNull(accessToken);
        }


        [TestMethod]
        public void GetNextTokenTest()
        {
            var accessToken = Program.getAccessToken();
            Console.WriteLine(accessToken);
            Assert.IsNotNull(accessToken);
        }


    }
}
