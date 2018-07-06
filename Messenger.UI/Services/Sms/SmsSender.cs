using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Messenger.UI.Services.Sms
{
    public class SmsSender : ISmsSender
    {
        private readonly SmsConfig _configs;
        private readonly ILogger<SmsSender> _logger;
        public SmsSender(IOptions<SmsConfig> configs, ILogger<SmsSender> logger)
        {
            _configs = configs.Value;
            _logger = logger;
        }

        /// <summary>
        /// Send SMS message to multiple destinations through the ViaNett HTTP API.
        /// </summary>
        /// <returns>Returns an object with the following parameters: Success, ErrorCode, ErrorMessage</returns>
        /// <param name="message">Text message</param>
        public async Task<IEnumerable<Result>> SendSMS(IEnumerable<string> destinations, string message)
        {
            var results = new List<Result>();

            foreach (var destination in destinations)
            {
                try
                {
                    results.Add(await SendSMS(destination, message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, destination, message);
                    results.Add(new Result());
                }
            }

            return results;
        }

        /// <summary>
        /// Send SMS message through the ViaNett HTTP API.
        /// </summary>
        /// <returns>Returns an object with the following parameters: Success, ErrorCode, ErrorMessage</returns>
        /// <param name="msgsender">Message sender address. Mobile number or small text, e.g. company name</param>
        /// <param name="destination">Message destination address. Mobile number.</param>
        /// <param name="message">Text message</param>
        public async Task<Result> SendSMS(string destination, string message)
        {
            // Build the URL request for sending SMS.
            var url = _configs.Url
                + "username=" + HttpUtility.UrlEncode(_configs.Username)
                + "&password=" + HttpUtility.UrlEncode(_configs.Password)
                + "&destinationaddr=" + HttpUtility.UrlEncode(destination, System.Text.Encoding.GetEncoding("ISO-8859-1"))
                + "&message=" + HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("ISO-8859-1"))
                + "&refno=1";

            // Check if the message sender is numeric or alphanumeric.            
            if (long.TryParse(_configs.Sender, out long l))
            {
                url = url + "&sourceAddr=" + _configs.Sender;
            }
            else
            {
                url = url + "&fromAlpha=" + _configs.Sender;
            }

            // Send the SMS by submitting the URL request to the server. The response is saved as an XML string.
            var serverResult = await DownloadStringAsync(url);

            // Converts the XML response from the server into a more structured Result object.
            var result = ParseServerResult(serverResult);

            return result;
        }

        /// <summary>
        /// Downloads the URL from the server, and returns the response as string.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns the http/xml response as string</returns>
        /// <exception cref="WebException">WebException is thrown if there is a connection problem.</exception>
        private async Task<string> DownloadStringAsync(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);

                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    // Failed to connect to server. Throw an exception with a customized text.
                    throw new WebException("Error occurred while connecting to server. " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Parses the XML code and returns a Result object.
        /// </summary>
        /// <param name="serverResult">XML data from a request through HTTP API.</param>
        /// <returns>Returns a Result object with the parsed data.</returns>
        private Result ParseServerResult(string serverResult)
        {
            var xDoc = new XmlDocument();
            var result = new Result();
            XmlNode node;

            xDoc.LoadXml(serverResult);
            node = xDoc.GetElementsByTagName("ack")[0];

            result.ErrorCode = int.Parse(node.Attributes["errorcode"].Value);
            result.ErrorMessage = node.InnerText;
            result.Success = (result.ErrorCode == 0);

            return result;
        }
    }
}
