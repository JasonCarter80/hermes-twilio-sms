Hermes-Twilio-SMS
=================

Example Project to Utilize Twilio SMS from Vocalcom Hermes.Net

This project, when combined with a properly setup Twilio account can be used to send SMS messages from within a Hermes Scripting environment.

To start:
* Download this program and compile it using Visual Studio 2010 or above.
* This should be setup on the same server as your Plateform Publication, at the base NEXT to hermes_net_v4
* Enter your Account SID and Auth Token in the Web.Config
* Enter allowable 'FROM' numbers in Web.Config, these should be numbers setup in your account
* Upload Twilio.js into as an Action File in your Script.
* In an Action, create a UserDefined Function and enter the below code to call the function

```javascript
    SendSMS(@FromNumber.value, @ToNumber.value, @Message.value)
```