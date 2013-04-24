using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using Twilio;
using Twilio.TwiML;

namespace Twilio_SMS.Controllers
{
    public class SmsController : ApiController
    {
        public string Get(string from, string to, string message)
        {
            var accountSID = Properties.Settings.Default.AccountSID;
            var authToken = Properties.Settings.Default.AuthToken;
            var allowedNumbers = Properties.Settings.Default.FromNumbers;
            var twilio = new TwilioRestClient(accountSID, authToken);
            
            from = CleanPhone(from);
            if (!allowedNumbers.Contains(from))
                throw new Exception("From Number Not in Your List of Allowed Numbers");
            
            to = CleanPhone(to);
            var a = twilio.SendSmsMessage(from, to, message);
            return a.Status;
        }

        public SmsMessageResult Get(string from)
        {
            var accountSID = Properties.Settings.Default.AccountSID;
            var authToken = Properties.Settings.Default.AuthToken;
            //var allowedNumbers = Properties.Settings.Default.FromNumbers;
            var twilio = new TwilioRestClient(accountSID, authToken);
            from = CleanPhone(from);
            var a = twilio.ListSmsMessages(from, null, null, null, null);
            return a;
        }

        string CleanPhone(string phone)
        {
            var digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
            
        }

    }
}