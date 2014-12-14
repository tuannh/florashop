using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using System.Web;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using FloraShop.Core.Interfaces;


namespace FloraShop.Core.Logs
{
    public class LogProvider : ILogProvider
    {
        private static ILog _currentLogger = null;
        private string loggerName;
        private string configFilePath;
        public const string DefaultExceptioLogFileName = @"ExceptionLog.xml";

        public LogProvider() :
            this("RollingXmlFileLogger", SiteContext.Current.SiteConfig.ConfigFile.Log)
        {

        }

        public LogProvider(string loggerName, string configureFilePath)
        {
            this.loggerName = loggerName;
            this.configFilePath = Globals.MapPath(configureFilePath);

            if (System.IO.File.Exists(configFilePath))
            {
                XmlConfigurator.Configure(new FileInfo(configFilePath));
            }
            else
                XmlConfigurator.Configure();
        }

        public void LogException(SiteException exception, int logId)
        {
            _currentLogger = _currentLogger ?? LogManager.GetLogger(loggerName);
            try
            {
                IAppender[] appenders = _currentLogger.Logger.Repository.GetAppenders();
                foreach (IAppender appender in appenders)
                {
                    FileAppender fileAppender = appender as FileAppender;
                    if (fileAppender == null)
                        continue;
                    string filename = DefaultExceptioLogFileName;
                    {
                        string FilePath = fileAppender.File;
                        if (FilePath.StartsWith("~/") || FilePath.StartsWith("./"))
                        {
                            FilePath = FilePath.Substring(2);
                        }
                        else if (FilePath.StartsWith("/"))
                        {
                            FilePath = FilePath.Substring(1);
                        }
                        FilePath = Regex.Replace(FilePath, "(/)+", Path.DirectorySeparatorChar.ToString());
                        FilePath = Regex.Replace(FilePath, @"\\{2,}", Path.DirectorySeparatorChar.ToString());

                        if (!File.Exists(FilePath))
                            FilePath = Globals.GetApplicationPhysicalPath() + FilePath;

                        if (File.Exists(FilePath))
                        {
                            FileInfo fInfo = new FileInfo(FilePath);
                            if (!string.IsNullOrEmpty(fInfo.Name))
                                filename = fInfo.Name;
                        }
                    }
                    fileAppender.File = string.Format("{0}/{1}", LogsDir, filename);
                }

                exception.ExceptionID = Guid.NewGuid().ToString();
                _currentLogger.Error(logId, exception);
            }
            catch { }
        }

        public void LogException(Exception exception, int logId)
        {
            LogException(new SiteException(exception, exception.Message, SiteExceptionType.UnknownError), logId);
        }

        public void DeleteExceptions(string logID)
        {
        }

        public List<SiteException> GetAllExceptions(int pageIndex, int pageSize, out int totalRecords)
        {
            return GetAllExceptions(DefaultExceptioLogFileName, pageIndex, pageSize, out totalRecords);
        }

        public List<SiteException> GetAllExceptions(string FileName, int pageIndex, int pageSize, out int totalRecords)
        {
            //string myFileName = Globals.MapPath(string.Format("{0}/{1}", LogsDir, FileName));
            //List<CMSException> oExceptions = new List<CMSException>();
            //if (System.IO.File.Exists(myFileName))
            //{
            //    string fileContents = "<?xml version='1.0' encoding='utf-8'?> <CMSLogs> " + System.IO.File.ReadAllText(myFileName) + " </CMSLogs>";
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(fileContents);

            //    TextReader reader = new StringReader(fileContents);
            //    XDocument xmlDoc = XDocument.Load(reader);

            //    var query = from p in
            //                    xmlDoc.Elements("CMSLogs").Elements("CMSLogEntry")
            //                orderby (DateTime)p.Element("LastOccured") descending
            //                select p;
            //    var test = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //    foreach (var record in test)
            //    {
            //        CMSExceptionType exceptionType =
            //            (CMSExceptionType)Enum.Parse(typeof(CMSExceptionType), record.Element("ExceptionType").Value, true);

            //        CMSException oException = new CMSException(exceptionType);
            //        try
            //        {
            //            oException.ExceptionID = record.Element("ExceptionId").Value;
            //        }
            //        catch
            //        {
            //            oException.ExceptionID = "";
            //        }
            //        try
            //        {
            //            oException.CurrentUserId = record.Element("UserId").Value;
            //        }
            //        catch
            //        {
            //            oException.CurrentUserId = string.Empty;
            //        }
            //        try
            //        {
            //            oException.DateCreated = DateTime.Parse(record.Element("DateCreated").Value);
            //        }
            //        catch
            //        {
            //            oException.DateCreated = DateTime.MinValue;
            //        }
            //        try
            //        {
            //            oException.DateLastOccurred = DateTime.Parse(record.Element("LastOccured").Value);
            //        }
            //        catch
            //        {
            //            oException.DateLastOccurred = DateTime.MinValue;
            //        }
            //        try
            //        {
            //            oException.IPAddress = record.Element("UserIp").Value;
            //        }
            //        catch
            //        {
            //            oException.IPAddress = "";
            //        }
            //        try
            //        {
            //            oException.UserAgent = record.Element("UserAgent").Value;
            //        }
            //        catch
            //        {
            //            oException.UserAgent = "";
            //        }
            //        try
            //        {
            //            oException.HttpPathAndQuery = record.Element("RequestUrl").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpPathAndQuery = "";
            //        }
            //        try
            //        {
            //            oException.HttpReferrer = record.Element("Referer").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpReferrer = "";
            //        }
            //        try
            //        {
            //            oException.HttpVerb = record.Element("Verb").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpVerb = "";
            //        }
            //        try
            //        {
            //            oException.LoggedStackTrace = record.Element("StackTrace").Value;
            //        }
            //        catch
            //        {
            //            oException.LoggedStackTrace = "";
            //        }


            //        oExceptions.Add(oException);

            //    }
            //    totalRecords = query.Count();
            //    return oExceptions;
            //}
            //else
            //{
            //    totalRecords = 0;
            //    return null;
            //}
            totalRecords = 0;
            return null;
        }

        public SiteException GetException(string logID)
        {
            return GetException(DefaultExceptioLogFileName, logID);
        }

        public SiteException GetException(string FileName, string logID)
        {
            //string myFileName = Globals.MapPath(string.Format("{0}/{1}", LogsDir, FileName));
            //CMSException oException;

            //string fileContents = "<?xml version='1.0' encoding='utf-8'?> <CMSLogs> " + System.IO.File.ReadAllText(myFileName) + " </CMSLogs>";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(fileContents);

            //TextReader reader = new StringReader(fileContents);
            //XDocument xmlDoc = XDocument.Load(reader);

            //var query = from p in
            //                xmlDoc.Elements("CMSLogs").Elements("CMSLogEntry")
            //            where (string)p.Element("ExceptionId") == logID
            //            select p;
            //if (query.Count() > 0)
            //{
            //    foreach (var record in query)
            //    {
            //        CMSExceptionType exceptionType =
            //            (CMSExceptionType)
            //            Enum.Parse(typeof(CMSExceptionType), record.Element("ExceptionType").Value, true);
            //        oException = new CMSException(exceptionType);

            //        try
            //        {
            //            oException.ExceptionID = record.Element("ExceptionId").Value;
            //        }
            //        catch
            //        {
            //        }

            //        try
            //        {
            //            oException.CurrentUserId = record.Element("UserId").Value;
            //        }
            //        catch
            //        {
            //            oException.CurrentUserId = string.Empty;
            //        }
            //        try
            //        {
            //            oException.DateCreated = DateTime.Parse(record.Element("DateCreated").Value);
            //        }
            //        catch
            //        {
            //            oException.DateCreated = DateTime.MinValue;
            //        }
            //        try
            //        {
            //            oException.DateLastOccurred = DateTime.Parse(record.Element("LastOccured").Value);
            //        }
            //        catch
            //        {
            //            oException.DateLastOccurred = DateTime.MinValue;
            //        }
            //        try
            //        {
            //            oException.IPAddress = record.Element("UserIp").Value;
            //        }
            //        catch
            //        {
            //            oException.IPAddress = "";
            //        }
            //        try
            //        {
            //            oException.UserAgent = record.Element("UserAgent").Value;
            //        }
            //        catch
            //        {
            //            oException.UserAgent = "";
            //        }
            //        try
            //        {
            //            oException.HttpPathAndQuery = record.Element("RequestUrl").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpPathAndQuery = "";
            //        }
            //        try
            //        {
            //            oException.HttpReferrer = record.Element("Referer").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpReferrer = "";
            //        }
            //        try
            //        {
            //            oException.HttpVerb = record.Element("Verb").Value;
            //        }
            //        catch
            //        {
            //            oException.HttpVerb = "";
            //        }
            //        try
            //        {
            //            oException.LoggedStackTrace = record.Element("StackTrace").Value;
            //        }
            //        catch
            //        {
            //            oException.LoggedStackTrace = "";
            //        }

            //        return oException;
            //    }
            //}

            //return new CMSException(CMSExceptionType.UnknownError);

            return null;
        }

        private static string LogsDir
        {
            get
            {
                return "~/Logs";
            }
        }
    }
}