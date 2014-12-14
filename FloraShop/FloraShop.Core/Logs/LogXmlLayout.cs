#region Reference Declaration

using System;
using FloraShop.Core;
using log4net.Layout;
using System.Xml;
using log4net.Core;

#endregion

namespace FloraShop.Core.Logs
{
    public class LogXmlLayout : XmlLayoutBase
    {
        public LogXmlLayout()
            : base()
        {

        }

        public LogXmlLayout(bool Location)
            : base(Location)
        {

        }

        /// <summary>
        /// Formats the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="loggingEvent">The logging event.</param>
        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
        {
            SiteException cmsException = loggingEvent.ExceptionObject as SiteException;

            if (cmsException != null)
            {
                string dateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";
                writer.WriteStartElement("ClawLogEntry");//Write ClawLogEntry
                writer.WriteElementString("ExceptionId", cmsException.ExceptionID.ToString());
                writer.WriteElementString("ExceptionType", Enum.GetName(typeof(SiteExceptionType), cmsException.ExceptionType));
                writer.WriteElementString("UserId", cmsException.CurrentUserId ?? "");
                writer.WriteElementString("DateCreated", DateTime.Now.ToString(dateTimeFormat));
                writer.WriteElementString("LastOccured", DateTime.Now.ToString(dateTimeFormat));
                writer.WriteElementString("UserIp", cmsException.IPAddress);
                writer.WriteElementString("UserAgent", cmsException.UserAgent);

                writer.WriteStartElement("RequestUrl");
                writer.WriteCData(cmsException.HttpPathAndQuery);
                writer.WriteEndElement(); //RequestUrl

                writer.WriteStartElement("Referer");
                writer.WriteCData(cmsException.HttpReferrer);
                writer.WriteEndElement(); //Referer

                writer.WriteElementString("Verb", cmsException.HttpVerb);

                writer.WriteStartElement("StackTrace");
                writer.WriteCData(cmsException.StackTrace);
                writer.WriteEndElement(); //StackTrace

                writer.WriteStartElement("Message");
                writer.WriteCData(Globals.HtmlEncode(cmsException.ToString()));
                writer.WriteEndElement(); //Message

                writer.WriteEndElement(); //ClawLogEntry
            }
        }
    }
}