using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LocalVersionControl
{
    /// <summary>
    /// tracks the changes to files/folders
    /// </summary>
    public class changeLog
    {
        Stack<changeLogEntry> entrys;
        string logPath;

        /// <summary>
        /// gets the list of log entrys
        /// </summary>
        public Stack<changeLogEntry> Entrys
        {
            get { return entrys; }
        }

        /// <summary>
        /// sets the logPath and load the file
        /// </summary>
        /// <param name="path"></param>
        public changeLog(string path)
        {
            entrys = new Stack<changeLogEntry>();
            logPath = path;
            readFile();
        }
        /// <summary>
        /// gets the time of the last change
        /// </summary>
        /// <returns></returns>
        public DateTime getLastModTime()
        {
            if (entrys.Count > 0)
                return entrys.Peek().Date;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// adds a new change
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="prev"></param>
        /// <param name="dir"></param>
        public void add(string name, WatcherChangeTypes type, string prev, bool dir)
        {
            entrys.Push(new changeLogEntry(name, DateTime.Now, type, prev, dir));
        }

        /// <summary>
        /// removes the last change
        /// </summary>
        /// <returns>the entry removed</returns>
        public changeLogEntry remove()
        {
            changeLogEntry res = null;
            if (entrys.Count > 0)
            {
                res = entrys.Pop();
                writeFile(true);
            }
            return res;
        }

        /// <summary>
        /// gets the entrys from the log file
        /// </summary>
        public void readFile()
        {
            StreamReader fileReader = null;
            try
            {

                if (File.Exists(logPath))
                {
                    fileReader = new StreamReader(logPath, Encoding.Unicode);
                    string line;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        string[] values = line.Split(new char[] { ',' });
                        if (values.Length >= 5)
                        {
                            DateTime tempDate;
                            WatcherChangeTypes type;
                            if (!DateTime.TryParse(values[1], out tempDate))
                            {
                                tempDate = DateTime.MinValue;
                            }
                            if (Enum.TryParse(values[2], out type))
                            {
                                entrys.Push(new changeLogEntry(values[0], tempDate,type, values[3], Convert.ToBoolean(values[4]), false));
                            }
                            
                        }
                    }

                }
            }
            catch
            {
            }
            finally
            {
                if (fileReader != null)
                    fileReader.Close();
            }
        }

        /// <summary>
        /// writes the entrys to the log file
        /// </summary>
        /// <param name="force"></param>
        public void writeFile(bool force = false)
        {
            StreamWriter fileWritter = null;
            try
            {

                fileWritter = new StreamWriter(logPath, !force, Encoding.Unicode);
                foreach (changeLogEntry entry in entrys.Reverse())
                {
                    if (entry.IsNew||force)
                    {
                        fileWritter.WriteLine(entry.ToString());
                        entry.IsNew = false;
                    }
                }

            }
            catch
            {
            }
            finally
            {
                if (fileWritter != null)
                    fileWritter.Close();
            }

        }
   
    }

    /// <summary>
    /// an entry in the log
    /// </summary>
    public class changeLogEntry
    {
        string name;
        DateTime date;
        string prev;
        WatcherChangeTypes type;
        bool dir;
        bool isNew;

        /// <summary>
        /// gets the name
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// gets the date
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }
        /// <summary>
        /// gets the prevoius state 
        /// </summary>
        public string Prev
        {
            get { return prev; }
        }
        /// <summary>
        /// gets if directory
        /// </summary>
        public bool isDir
        {
            get { return dir; }
        }
        /// <summary>
        /// gets type of change
        /// </summary>
        public WatcherChangeTypes Type
        {
            get { return type; }
        }
        /// <summary>
        /// gets and sets if new
        /// </summary>
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        /// <summary>
        /// create an entry usign given values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <param name="prev"></param>
        /// <param name="dir"></param>
        /// <param name="isNew"></param>
        public changeLogEntry(string name, DateTime date, WatcherChangeTypes type, string prev, bool dir, bool isNew = true)
        {
            this.name = name;
            this.date = date;
            this.prev = prev;
            this.type = type;
            this.dir = dir;
            this.isNew = isNew;
        }

        /// <summary>
        /// gets entry as string
        /// </summary>
        /// <returns>entry as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append(",");
            sb.Append(date);
            sb.Append(",");
            sb.Append(type);
            sb.Append(",");
            sb.Append(prev);
            sb.Append(",");
            sb.Append(dir);
            return sb.ToString();
        }
    }
}
