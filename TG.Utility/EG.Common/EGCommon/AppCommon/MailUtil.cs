using EG.Business.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EG.Utility.AppCommon
{
    /// <summary>
    /// Util for send mail 
    /// </summary>
    public class MailUtil
    {
        public Action SendCompletedHandler;

        private static readonly bool MAIL_SEND_ENABLED = ConfigCache.GetBoolAppConfig("MAIL_SEND_ENABLED");
        private static readonly int MAIL_TIMEOUT = ConfigCache.GetIntAppConfig("MAIL_TIMEOUT") * 1000;


        private int _port = 25;                          //smtp port
        private string _strHost = string.Empty;          //SMTP server address
        private string _strAccount = string.Empty;       //SMTP account
        private string _strPwd = string.Empty;           //SMTP password
        private string _strFrom = string.Empty;          //sender address
        private string[] _strto = null;                  //to addresss
        private string[] _strreplyto = null;             //reply to addresss

        #region Contructors

        public MailUtil()
        {
            _strHost = ConfigCache.GetAppConfig("MAIL_HOST");
            _strAccount = ConfigCache.GetAppConfig("MAIL_ACCOUNT");
            _strPwd = ConfigCache.GetAppConfig("MAIL_PASSWORD");
            _strFrom = ConfigCache.GetAppConfig("MAIL_FROM");
            _strto = ConfigCache.GetAppConfig("MAIL_TO").Split(new char[] { ',' });
            _port = ConfigCache.GetIntAppConfig("MAIL_PORT");
            _strreplyto = ConfigCache.GetAppConfig("MAIL_REPLYTO").Split(new char[] { ',' });
        }

        public MailUtil(string host, string account, string password, string from, string[] to, int port)
        {
            this._strHost = host;
            this._strAccount = account;
            this._strPwd = password;
            this._strFrom = from;
            this._strto = to;
            this._port = port;
        }
        #endregion //Contructors

        #region Send Mail

        /// <summary>
        /// send mail
        /// </summary>
        /// <param name="to">receivers mail-addredss</param>
        /// <param name="subject">email subject</param>
        /// <param name="content">email content</param>
        /// <returns>status of send mail</returns>
        public bool SendMail(string[] to, string subject, string content)
        {
            return SendMail(to, subject, content, null, true);
        }

        /// <summary>
        ///  send mail
        /// </summary>
        /// <param name="to">receiver mail-address</param>
        /// <param name="title">email subject</param>
        /// <param name="content">email content</param>
        /// <returns>status of send mail</returns>
        public bool SendMail(string to, string title, string content)
        {
            return SendMail(new string[] { to }, title, content); ;
        }

        /// <summary>
        /// send mail
        /// </summary>
        /// <param name="title">mail subject</param>
        /// <param name="content">mail content</param>
        /// <returns>status of send mail</returns>
        public bool SendMail(string title, string content, bool isAsync)
        {
            return SendMail(this._strto, title, content, null, isAsync);
        }

        /// <summary>
        /// send mail
        /// </summary>
        /// <param name="title">mail subject</param>
        /// <param name="content">mail content</param>
        /// <param name="attachments">email attachments</param>
        /// <returns>status of send mail</returns>
        public bool SendMail(string title, string content, string[] attachments, bool isAsync)
        {
            return SendMail(this._strto, title, content, attachments, isAsync);
        }

        /// <summary>
        /// send mail to any people
        /// </summary>
        /// <param name="to">receivers mail-addredss</param>
        /// <param name="subject">email subject</param>
        /// <param name="content">email content</param>
        /// <param name="attachments">email attachments</param>
        /// <param name="isAsync">is ascync or not</param>
        /// <returns>status of send mail</returns>
        public bool SendMail(string[] to, string subject, string content, string[] attachments, bool isAsync)
        {
            if (!MAIL_SEND_ENABLED)
            {
                throw new Exception("Mail Send Function is disable!");
                //return false;
            }

            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//send the mail style
            _smtpClient.Host = _strHost; //smtp server
            _smtpClient.Port = _port;
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new System.Net.NetworkCredential(_strAccount, _strPwd);//user name & password

            MailMessage _mailMessage = new MailMessage();

            _mailMessage.From = new MailAddress(_strFrom);

            foreach (string toTemp in to)
            {
                _mailMessage.To.Add(toTemp);
            }


            _mailMessage.Subject = subject;
            _mailMessage.Body = content;
            _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            _mailMessage.IsBodyHtml = true;
            _mailMessage.Priority = MailPriority.Normal;
            _mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            if (_strreplyto != null)
            {
                foreach (var item in _strreplyto)
                {
                    _mailMessage.ReplyToList.Add(item);
                }
            }

            if (attachments != null)
            {
                foreach (var item in attachments)
                {
                    _mailMessage.Attachments.Add(new Attachment(item));
                }
            }

            try
            {
                if (isAsync)
                {
                    _smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    string userState = null;
                    _smtpClient.SendAsync(_mailMessage, userState);
                    /*
                    System.Timers.Timer checkingTimer = new System.Timers.Timer(MAIL_TIMEOUT);//实例化Timer类，设置间隔时间为10000毫秒； 
                    checkingTimer.Elapsed += new System.Timers.ElapsedEventHandler(SendEmailTimeout);//到达时间的时候执行事件； 
                    checkingTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 
                    checkingTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                    checkingTimer.Container
                    checkingTimer.Start();
                     */
                }
                else
                {
                    _smtpClient.Timeout = MAIL_TIMEOUT;
                    _smtpClient.Send(_mailMessage);
                }
            }

            catch (Exception e)
            {
                //网络原因或者发件邮件配置错误导致发送失败

                //string errorMsg = "\nServer:" + _strHost + ":" + _port;
                //errorMsg += "\nFrom:" + ":" + _strFrom;
                //errorMsg += "\nTo:" + ":" + to;
                //errorMsg += "\n";

                throw e;

                //return false;
            }

            return true;
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            // String token = (string)e.UserState;

            if (SendCompletedHandler != null)
                SendCompletedHandler();
        }

        #endregion //Send Mail

        #region Verify Mail

        /// <summary>
        /// use regex to verify email address
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns></returns>
        public bool VerifyWithRegex(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            if (re.IsMatch(emailAddress))
                return true;
            else
                return false;
        }

        /// <summary>
        /// use telnet to connect email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool TelnetEmailServer(string emailAddress)
        {
            bool result = false;

            // get hostname
            var arrHost = emailAddress.Split('@');
            var hostName = arrHost[1];

            // get dnsname
            IList<string> dnsNames = GetDNSNames(hostName);
            if (dnsNames == null || dnsNames.Count == 0)
            {
                return result;
            }

            //telnet email server
            string smtpHostName = dnsNames[0];
            int smtpHost = 25;

            TcpClient tcpClient = new TcpClient(smtpHostName, smtpHost);

            string CRLF = "\r\n";
            byte[] dataBuffer;
            string responseString;

            NetworkStream netStream = tcpClient.GetStream();
            StreamReader reader = new StreamReader(netStream);
            responseString = reader.ReadLine();

            // Perform HELO to SMTP Server and get Response
            dataBuffer = Encoding.ASCII.GetBytes("HELO server.com" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            responseString = reader.ReadLine();

            // Perform MAIL FROM to a special email and get Response
            dataBuffer = Encoding.ASCII.GetBytes("MAIL FROM:<eriketse@gmail.com>" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            responseString = reader.ReadLine();

            // Read Response of the RCPT TO Message to know from email server if the email exist or not 
            dataBuffer = Encoding.ASCII.GetBytes("RCPT TO:<" + emailAddress + ">" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            responseString = reader.ReadLine();

            //check if the response string include 550 
            if (!responseString.Contains("250"))//responseString.Contains("550")
            {
                // Mail Address Does not Exist

                result = false;
            }
            else
            {
                result = true;
            }

            // QUITE Connection
            dataBuffer = Encoding.ASCII.GetBytes("QUITE" + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            
            // close tcp connection
            tcpClient.Close();

            return result;
        }

        /// <summary>
        /// use nslookup to get hostname
        /// notes:the system must include nslookup
        /// </summary>
        /// <param name="hostname">email's hostname</param>
        /// <returns></returns>
        public IList<string> GetDNSNames(string hostname)
        {
            IList<string> emailExchanger = new List<string>();

            string output;

            //create nslookup process & run
            var startInfo = new ProcessStartInfo("nslookup");
            startInfo.Arguments = string.Format("-type=MX {0}", hostname);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (var cmd = Process.Start(startInfo))
            {
                output = cmd.StandardOutput.ReadToEnd();
            }

            //use regex to filter output string
            string pattern = @"mail exchanger =\s*([\w\-\=\.]*)";
            MatchCollection matches = Regex.Matches(output, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Success)
                    emailExchanger.Add(match.Groups[1].Value);
            }

            return emailExchanger;
        }

        #endregion //Verify Mail
    }
}
