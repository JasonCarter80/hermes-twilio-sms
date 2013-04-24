using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TelcoData
{
    public static class TelcoData
    {
        private const string QueryUrlTemplate = @"http://www.telcodata.us/query/queryexchangexml.html?npa={0}&nxx={1}";

        public static ExchangeData GetExchange(string telePhone)
        {

            var url = BuildUrl(telePhone);
            var request = WebRequest.Create(url);
            string lookupResponse = "";
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            throw new InvalidOperationException("Response was empty.");

                        using (var reader = new StreamReader(stream))
                        {
                            lookupResponse = reader.ReadToEnd();
                            reader.Close();
                        }

                        stream.Close();
                    }

                    response.Close();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }


            return ParseReturn(lookupResponse);
        }

        private static ExchangeData ParseReturn(string response)
        {
            var settings = new XmlReaderSettings { ProhibitDtd = false, XmlResolver = null };
            var strReader = new StringReader(response);
            var xRoot = new XmlRootAttribute {ElementName = "exchangedata", IsNullable = true};
            var xmlSerializer = new XmlSerializer(typeof(ExchangeData),xRoot);
            var xmlReader = XmlReader.Create(strReader, settings);
            xmlReader.ReadToDescendant("exchangedata");
            var a = (ExchangeData)xmlSerializer.Deserialize(xmlReader);
            return a;

        }
        private static string BuildUrl(string telePhone)
        {
            telePhone = CleanPhone(telePhone);
            var phoneRegex = new Regex(@"^(?<NPA>[0-9]{3})(?<NXX>[0-9]{3})(?<EXT>[0-9]{4})");
            var mc = phoneRegex.Match(telePhone);
            var npa = mc.Groups["NPA"].Value;
            var nxx = mc.Groups["NXX"].Value;
            return String.Format(QueryUrlTemplate, npa, nxx);
        }

        private static string CleanPhone(string phone)
        {
            var digitsOnly = new Regex(@"[^\d]");
            var digits = digitsOnly.Replace(phone, "");
            return (digits.StartsWith("1")) ? digits.Substring(1) : digits;

        }
    }
}
