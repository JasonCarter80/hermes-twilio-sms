function SendSMS(from, to, message) {
  
  var xmlhttp;
  if (window.XMLHttpRequest)
    {  xmlhttp=new XMLHttpRequest();
    }
  else
    {  xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
    }

  xmlhttp.onreadystatechange=function()
    {
    if (xmlhttp.readyState==4 && xmlhttp.status==200)
      {
      alert(xmlhttp.responseText);
      }
    }
  var url = "/TwilioSMS/api/sms/" + from + "/" + to + "/" + message;
  xmlhttp.open("GET",url, true);
  xmlhttp.send();
}