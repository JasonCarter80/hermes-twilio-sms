using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;
using TelcoData;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace Twilio_SMS.Controllers
{




    public class IncomingController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Incoming()
        {
             var twilioResponse = new TwilioResponse();
            twilioResponse.Sms("Hello");
            SendEmail(null, null);
            return Request.CreateResponse(HttpStatusCode.OK, twilioResponse.Element);
        }
        public HttpResponseMessage Incoming(SMSMessage sms)
        {
            var cnam = TelcoData.TelcoData.GetExchange(sms.From);

            var message = String.Format("Hello {0}, how is {1}, {2} today?", sms.Status, cnam.City,
                                        cnam.State);
            var response = new TwilioResponse();
            try
            {
                SendEmail(sms, cnam);
            }
            catch (Exception ex)
            {
                response.Sms(ex.Message);    
            }
            
            
            return Request.CreateResponse(HttpStatusCode.OK, response.Element);
        }

        private void SendEmail(SMSMessage sms, ExchangeData callerDetails)
        {
            if (sms==null) sms = new SMSMessage();
            if (callerDetails==null) callerDetails = new ExchangeData();

            var fromAddress = new MailAddress(Properties.Settings.Default.EmailTo, "Incoming SMS");
            var toAddress = new MailAddress(Properties.Settings.Default.EmailTo, "SMS Email Queue");
            var ccAddress = new MailAddress(Properties.Settings.Default.EmailCC);

            var subject = string.Format("Incoming SMS: {0}", sms.From ?? "Unknown");
            var getBody = CreateHtmlBody(sms, callerDetails);

            var smtp = new SmtpClient();


            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = getBody,
                IsBodyHtml = true

            })
            {
                message.Bcc.Add(ccAddress);
                smtp.Send(message);
            }
        }

        private static string CreateHtmlBody(SMSMessage callDetails, ExchangeData callerDetails)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<h2>SMS From: {0}</h2>", callDetails.From);
            sb.AppendFormat("<h3>Call Details</h3><table>");
            sb.AppendFormat("<tr><td><b>{0}</b></td><td><h3>{1}</h3></td></tr>", "Message", callDetails.Body);
            sb.AppendFormat("<tr><td><b>{0}</b></td><td>{1}, {2}</td></tr>", "Location", callerDetails.City, callerDetails.State );
            sb.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "Recieved Time", callDetails.DateSent );
            sb.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "Sent To Account", callDetails.To);
            sb.AppendFormat("</table>");
            return sb.ToString();
        }
    }


}
