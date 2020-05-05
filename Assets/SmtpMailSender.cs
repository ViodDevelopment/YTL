using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class SmtpMailSender : MonoBehaviour
{

    public void SendSmtpMail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("pekeno1811@gmail.com");
        mail.To.Add("pekeno1811@gmail.com");
        mail.Subject = "Test Smtp Mail";
        mail.Body = "Testing SMTP mail from GMAIL";
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("pekeno1811@gmail.com", "sergikovic63") as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
    }
}
