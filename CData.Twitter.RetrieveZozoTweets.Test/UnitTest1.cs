using Microsoft.VisualStudio.TestTools.UnitTesting;
using CData.Twitter.RetrieveZozoTweets;
using System;

namespace CData.Twitter.RetrieveZozoTweets.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WriteJson()
        {
            Program.writeJsonInLocalFolder("Hello", @"C:\Work\ZozoTweets\", 2);

            Assert.IsTrue(true);
        }
        [TestMethod]
        public void AccessToken()
        {
            var accessToken = Program.getAccessToken();
            Console.WriteLine(accessToken);

            Assert.IsFalse(string.IsNullOrEmpty(accessToken));
        }

        [TestMethod]
        public void NextToken()
        {
            var json = "{\"results\":[{\"created_at\":\"Mon Jan 07 10:24:29 +0000 2019\"}],\"next\":\"eyJhdXRoZW50aWNpdHkiOiIyODgwMGE0NWIzNTRhZDE0NzJmM2RiZTZlNzZhNDQwMWFiODUxNjFhODQ0ZjU5NjVhNTRiOGY3ODUwNzcxMDQ5IiwiZnJvbURhdGUiOiIyMDE4MTIwODAwMDAiLCJ0b0RhdGUiOiIyMDE5MDEwNzEwMjQiLCJuZXh0IjoiMjAxOTAxMDcxMDI0MTItMTA4MjIyMTM1NTY2MjA5ODQzMi0wIn0=\",\"requestParameters\":{\"maxResults\":500,\"fromDate\":\"201812080000\",\"toDate\":\"201901071024\"}}";

            var nextToken = Program.getNextTokenByResponseJson(json);

            var actualNextToken = "eyJhdXRoZW50aWNpdHkiOiIyODgwMGE0NWIzNTRhZDE0NzJmM2RiZTZlNzZhNDQwMWFiODUxNjFhODQ0ZjU5NjVhNTRiOGY3ODUwNzcxMDQ5IiwiZnJvbURhdGUiOiIyMDE4MTIwODAwMDAiLCJ0b0RhdGUiOiIyMDE5MDEwNzEwMjQiLCJuZXh0IjoiMjAxOTAxMDcxMDI0MTItMTA4MjIyMTM1NTY2MjA5ODQzMi0wIn0=";

            Assert.IsTrue(nextToken == actualNextToken);
        }
    }
}

