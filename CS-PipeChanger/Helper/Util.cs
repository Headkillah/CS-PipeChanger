using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace PipeChanger
{
    public class Util
    {
        public const string modPrefix = "Pipe Changer " + PipeChanger.MODVERSION;
        private static string m_lastLog;
        private static int m_duplicates = 0;

        public static void DebugPrint(params string[] args)
        {
            DateTime now = DateTime.Now;
            long millis = now.Ticks / 10000;
            string s = String.Format("[" + modPrefix + "] {0, -42} {1, 22} {2, 2}  {3}", String.Join(" ", args), now, Thread.CurrentThread.ManagedThreadId, millis / 1000);
            Debug.Log(s);
            WriteLog(s);
        }

        public static void Log(string message)
        {
            if (message == m_lastLog)
            {
                m_duplicates++;
            }
            else if (m_duplicates > 0)
            {
                Debug.Log(modPrefix + "(x" + (m_duplicates + 1) + ")");
                Debug.Log(modPrefix + message);
                WriteLog(message);
                m_duplicates = 0;
            }
            else
            {
                Debug.Log(modPrefix + message);
                WriteLog(message);
            }
            m_lastLog = message;
        }

        public static void LogException(Exception e)
        {
            //Log("Intercepted exception (not game breaking):");
            Debug.LogException(e);
            WriteLog(e.InnerException.Message);
        }

        public static void WriteLog(string s)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(PipeChanger.LogFile, FileMode.Append))
            {
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine(s);
                sw.Flush();
            }
        }
    }
}