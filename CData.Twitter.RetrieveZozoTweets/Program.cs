using RestSharp;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace CData.Twitter.RetrieveZozoTweets
{
    public class Program
    {
        private static int maxResult = 500;　// 最大取得件数
        private static int maxRequestLimit = 1000; // 最大リクエスト回数 500万件の場合、5,000,000 / 500 = 10,000
        private static string toDateValue =   "201901101500"; //UTC 2019/01/08 15:00 ( +09::00 で12時を示す )
        private static string fromDateValue = "201901091500"; //UTC 2019/01/07 15:00 ( +09::00 で12時を示す )

        private static string oauthUrl = "https://api.twitter.com/oauth2/token";
        private static string endpointUrl = "https://api.twitter.com/1.1/tweets/search/30day/dev.json";
        
        private static string AUTHORIZATION_HEADER_NAEM = "Authorization";
        private static string AUTHORIZATION_HEADER_VALUE = "Basic XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        private static string AUTHORIZATION_HEADER_BEARER = "Bearer";

        private const string CONTENT_TYPE_HEADER_NAME = "Content-Type";
        private const string CONTENT_TYPE_HEADER_VALUE = "application/x-www-form-urlencoded;charset=UTF-8";
        private const string BODY_BY_GET_ACCESSTOKEN = "grant_type=client_credentials";

        private static string urlParamQueryBase = "query={0}";
        private static string UrlParamMaxResultsBase = "maxResults={0}";
        private static string urlParamNextBase = "next={0}";
        private static string urlToDateBase = "toDate={0}";
        private static string urlToFromBase = "fromDate={0}";

        // #月に行くならお年玉
        private static string queryValue = "%23%E6%9C%88%E3%81%AB%E8%A1%8C%E3%81%8F%E3%81%AA%E3%82%89%E3%81%8A%E5%B9%B4%E7%8E%89";

        private static string writeFolderName = @"C:\Work\ZozoTweets\";

        private static string nextToken = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Process Start!");
            var sw = new Stopwatch();
            sw.Start();
            
            var accessToken = getAccessToken();

            // counter には現在まで出力した値を入れる。tweetlist_000003.jsonが生成されていたら、3を指定
            for (int counter = 0; counter < maxRequestLimit; counter++)
            {
                var responseJson = getResponseJson(nextToken,accessToken);
                nextToken = getNextTokenByResponseJson(responseJson);

                writeJsonInLocalFolder(responseJson,writeFolderName,counter);

                Console.WriteLine("Create : " + String.Format("{0:000000}", counter + 1) + " : " + sw.Elapsed);
            }

            Console.WriteLine("Process End!");
            Console.WriteLine("Please push any key.");
            Console.ReadKey();
        }

        public static void writeJsonInLocalFolder(string responseJson, string writeFolderName, int counter)
        {
            StreamWriter sw;

            var fileName = "tweetlist_" + String.Format("{0:000000}", counter + 1) + ".json";

            sw = new StreamWriter(writeFolderName + fileName, false);

            sw.Write(responseJson);
            sw.Close();
        }

        public static string getNextTokenByResponseJson(string responseJson)
        {
            var jsonObject = JObject.Parse(responseJson);

            return jsonObject["next"].ToString();
        }

        public static string getResponseJson(string nextToken, string accessToken)
        {
            var client = new RestClient();
            var request = new RestRequest();

            var requestUrl = $"{endpointUrl}?{String.Format(urlParamQueryBase,queryValue)}&{String.Format(UrlParamMaxResultsBase,maxResult)}&{String.Format(urlToDateBase, toDateValue)}&{String.Format(urlToFromBase, fromDateValue)}" +
                $"{(String.IsNullOrEmpty(nextToken) ? "" : "&" + String.Format(urlParamNextBase,nextToken) )}";
            
            client.BaseUrl = new Uri(requestUrl);

            request.AddHeader(AUTHORIZATION_HEADER_NAEM, AUTHORIZATION_HEADER_BEARER + " " + accessToken);

            var response = client.Execute(request);

            return response.Content;
        }

        public static string getAccessToken()
        {
            var client = new RestClient();
            var request = new RestRequest();

            client.BaseUrl = new Uri(oauthUrl);

            request.Method = Method.POST;

            request.AddHeader(AUTHORIZATION_HEADER_NAEM, AUTHORIZATION_HEADER_VALUE);
            request.AddHeader(CONTENT_TYPE_HEADER_NAME, CONTENT_TYPE_HEADER_VALUE);
            request.AddParameter(CONTENT_TYPE_HEADER_VALUE, BODY_BY_GET_ACCESSTOKEN, ParameterType.RequestBody);

            var response = client.Execute<AccessToken>(request);

            return response.Data.access_token;
        }
    }

    public class TweetResult
    {
        public string results { get; set; }
        public string next { get; set; }
        public string requestParameters { get; set; }
    }

    public class AccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }
}
