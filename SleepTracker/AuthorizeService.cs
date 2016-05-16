using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;

namespace SleepTracker
{
    class AuthorizeService
    {
        string oauthConsumerKey;
        string oauthConsumerSecret;
        string oauthSignatureMethod;
        string oauthVersion;
        string _oauthNonce;
        string _oathTimestamp;
        public string url = "https://api.twitter.com/oauth/request_token";
        string callBack;



        public string CreateSignature()
        {

            oauthConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
            oauthConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            oauthSignatureMethod = "HMAC-SHA1";
            oauthVersion = "1.0";
            _oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            _oathTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            callBack = "https://www.google.com";


            // create oauth signature
            var baseFormat = "oauth_callback={0}&oauth_consumer_key={1}&oauth_nonce={2}&oauth_signature_method={3}" +
                        "&oauth_timestamp={4}&oauth_version={5}";

            var baseString = string.Format(baseFormat,callBack,
                                   oauthConsumerKey,
                                   _oauthNonce,
                                   oauthSignatureMethod,
                                   _oathTimestamp,
                                   oauthVersion
                                   );

            baseString = string.Concat("POST&", Uri.EscapeDataString(url), "&", Uri.EscapeDataString(baseString));
        // Console.WriteLine(baseString);

            //generate the signature key the hash will use
            string signatureKey =
                Uri.EscapeDataString(oauthConsumerSecret) + "&";
            
            var hmacsha1 = new HMACSHA1(
                new ASCIIEncoding().GetBytes(signatureKey));

            //hash the values
            string signatureString = Convert.ToBase64String(
                hmacsha1.ComputeHash(
                    new ASCIIEncoding().GetBytes(baseString)));

            Console.WriteLine(signatureString);
            return signatureString;

        }

        public string CreateAuthorizationHeaderParameter(string signature)
        {
            string authorizationHeaderParams = string.Empty;
            authorizationHeaderParams += "OAuth ";

          //  authorizationHeaderParams += "oauth_callback="
                                     //  + "\"" + Uri.EscapeDataString(callBack) + "\",";

            authorizationHeaderParams += "oauth_consumer_key="
                                         + "\"" + Uri.EscapeDataString(oauthConsumerKey) + "\",";

            authorizationHeaderParams += "oauth_nonce=" + "\"" +
                                         Uri.EscapeDataString(_oauthNonce) + "\",";

            authorizationHeaderParams += "oauth_signature_method=" + "\"" +
                Uri.EscapeDataString(oauthSignatureMethod) + "\",";

            authorizationHeaderParams += "oauth_timestamp=" + "\"" +
                                         Uri.EscapeDataString(_oathTimestamp) + "\",";

            authorizationHeaderParams += "oauth_version=" + "\"" +
                                         Uri.EscapeDataString(oauthVersion) + "\",";

            authorizationHeaderParams += "oauth_signature=" + "\""
                             + Uri.EscapeDataString(signature) + "\"";
            return authorizationHeaderParams;


        }




    }
}
