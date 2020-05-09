using Scheduler.NPoco;
using NPoco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace Logging
{
    public class Logger
    {
        public static int Info(string msg, Guid? session=null)
        {
            int Result = -1;
            if (ConfigurationManager.AppSettings["logger"].ToLower() == "true")
            {
                StackTrace stackTrace = new StackTrace(true);
                string CallerClass = stackTrace.GetFrame(1).GetMethod().Name;
                string CallerLineNumber = stackTrace.GetFrame(1).GetFileLineNumber().ToString();
                Console.WriteLine("Info - " + CallerClass + " - " + CallerLineNumber);

                Result = LogMsg("Info", msg, "", CallerClass, CallerLineNumber, session);
            }
            return Result;
        }

        public static int Info(string msg, dynamic value, Guid? session = null)
        {
            int Result = -1;
            if (ConfigurationManager.AppSettings["logger"].ToLower() == "true")
            {
                StackTrace stackTrace = new StackTrace(true);
                string CallerClass = stackTrace.GetFrame(1).GetMethod().Name;
                string CallerLineNumber = stackTrace.GetFrame(1).GetFileLineNumber().ToString();
                Console.WriteLine("Info - " + CallerClass + " - " + CallerLineNumber);

                Result = LogMsg("Info", msg, value, CallerClass, CallerLineNumber, session);
            }
            return Result;
        }

        public static int Error(Exception ex, Guid? session = null)
        {
            int Result = -1;
            if (ConfigurationManager.AppSettings["logger"].ToLower() == "true")
            {
                StackTrace stackTrace = new StackTrace(true);
                string CallerClass = stackTrace.GetFrame(1).GetMethod().Name;
                string CallerLineNumber = stackTrace.GetFrame(1).GetFileLineNumber().ToString();
                Console.WriteLine("Info - " + CallerClass + " - " + CallerLineNumber);

                Result = LogMsg("Error", ex.GetType().ToString(), ex, CallerClass, CallerLineNumber, session);
            }
            return Result;
        }

        public static int Value(string title, dynamic value, Guid? session = null)
        {
            int Result = -1;
            if (ConfigurationManager.AppSettings["logger"].ToLower() == "true")
            {
                StackTrace stackTrace = new StackTrace(true);
                string CallerClass = stackTrace.GetFrame(1).GetMethod().Name;
                string CallerLineNumber = stackTrace.GetFrame(1).GetFileLineNumber().ToString();
                Console.WriteLine("Info - " + CallerClass + " - " + CallerLineNumber);

                Result = LogMsg("Value", title, value, CallerClass, CallerLineNumber, session);
            }
            return Result;
        }

        public static int LogMsg(string kind, string msg, dynamic value, string callerClass, string callerLineNumber, Guid? session = null)
        {
            int Result = -1;
            if (ConfigurationManager.AppSettings["logger"].ToLower() == "true")
            {
                using (IDatabase db = new NPocoDatabase("CommunicationCenter"))
                {
                    session = (session == null) ? Guid.NewGuid() : session;

                    var sql = Sql.Builder;
                    switch (kind.ToLower())
                    {
                        case "info":
                            {
                                msg = msg.Replace("{value}", (string)value);

                                Log Log = new Log()
                                {
                                    Guidfield=Guid.NewGuid(),
                                    DateCreated = DateTimeOffset.Now,
                                    Kind = "Info",
                                    Msg = msg,
                                    Value = (string)value,
                                    CallerClass = callerClass,
                                    CallerLineNumber = callerLineNumber,
                                    Session = (Guid)session
                                };
                                db.Insert<Log>(Log);
                                Result = Log.Id;
                            }
                            Console.WriteLine(msg);
                            break;
                        case "error":
                            {
                               //var Json = new JavaScriptSerializer().Serialize(value);
                                Log Log = new Log()
                                {
                                    Guidfield = Guid.NewGuid(),
                                    DateCreated = DateTimeOffset.Now,
                                    Kind = "Error",
                                    Msg = msg,
                                    Value = "",//Json,
                                    CallerClass = callerClass,
                                    CallerLineNumber = callerLineNumber,
                                    Session = (Guid)session
                                };
                                db.Insert<Log>(Log);
                                Result = Log.Id;
                            }
                            Console.WriteLine(msg);
                            break;
                        case "value":
                            {
                                var Json = new JavaScriptSerializer().Serialize(value);
                                Log Log = new Log()
                                {
                                    Guidfield = Guid.NewGuid(),
                                    DateCreated = DateTimeOffset.Now,
                                    Kind = "Value",
                                    Msg = msg,
                                    Value = Json,
                                    CallerClass = callerClass,
                                    CallerLineNumber = callerLineNumber,
                                    Session = (Guid)session
                                };
                                db.Insert<Log>(Log);
                                Result = Log.Id;
                                Console.WriteLine(Json);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return Result;
        }
    }

    public class Log
    {
        public int Id { get; set; }
        public Guid Guidfield { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string Kind { get; set; }
        public string Msg { get; set; }
        public string Value { get; set; }
        public string CallerClass { get; set; }
        public string CallerLineNumber { get; set; }
        public Guid Session { get; set; }
    }
}
