using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Common.Attribute;

namespace Common
{
    public class LogHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogHelper));
        private static readonly Dictionary<string, DateTime> SendEmailRecordDic = new Dictionary<string, DateTime>();

        static LogHelper()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Info(object msg)
        {
            Logger.Info(msg);
        }

        public static void Error(object msg)
        {
            Logger.Error(msg);
        }

        public static void Fatal(object msg)
        {
            Logger.Fatal(msg);
            SendFatalMail("yangjian@ebopark.com", msg.ToString());
        }

        /// <summary>
        /// 发送邮件-增加判断如果相同异常对一个收件人短期内只发一次
        /// </summary>
        /// <param name="email"></param>
        public static void SendFatalMail(string email, string msg)
        {
            var dicKey = email + msg;
            Info($"字典Key:{dicKey},当前字典count:{SendEmailRecordDic.Count},是否包含此Key:{SendEmailRecordDic.ContainsKey(dicKey)},字典序列化:{JsonHelper.Serialize(SendEmailRecordDic)}");
            if (SendEmailRecordDic.ContainsKey(dicKey))
            {
                if(SendEmailRecordDic[dicKey].AddHours(1)>DateTime.Now) return;//1小时内发过的就不发送
                SendEmailRecordDic.Remove(dicKey);
                Info("包含此Key，已经删除");
            }
            Info("开始发送邮件");
            SendMail(email, "yangjian@ebopark.com", "3565759bbK", "程序异常", msg);
            SendEmailRecordDic.Add(email + msg, DateTime.Now);
        }


        /// <summary>
        /// 向用户发送邮件
        /// </summary>
        /// <param name="receiveUser">接收邮件的用户</param>
        /// <param name="sendUser">发送者显求的邮箱地址,可为空</param>
        /// <param name="userPassword">发送者的登陆密码</param>
        /// <param name="mailTitle">发送标题</param>
        /// <param name="mailContent">发送的内容</param>
        [MonitorException("及时发送重要信息至个人邮箱")]//监视异常
        private static void SendMail(string receiveUser, string sendUser, string userPassword, string mailTitle, string mailContent)
        {
            var smtpClient = new SmtpClient
            {
                EnableSsl = true,//开启SSL
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "smtp.exmail.qq.com",
                Credentials = new NetworkCredential(sendUser, userPassword)
            };

            var mailMessage = new MailMessage(sendUser, receiveUser)
            {
                Subject = mailTitle,
                Body = mailContent,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.Low
            };

            smtpClient.Send(mailMessage);
        }
    }
}
