using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LocalVersionControl
{

    /// <summary>
    /// handles the set up of monitoring files
    /// </summary>
    public class FileMonitor
    {
        private string location;
        private Project project;
        private FileSystemWatcher fWatcher;

       // private List<string> excludePaths;

        /// <summary>
        /// gets location
        /// </summary>
        public string Location
        {
            get { return location; }
            set 
            {
                if (fWatcher != null)
                {
                    fWatcher.Path = location;
                }
                location = value;
            }
        }

        /// <summary>
        /// gets whether currentle monitoring
        /// </summary>
        public bool isMonitoring
        {
            get
            {
                if (fWatcher == null||!fWatcher.EnableRaisingEvents)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// set up file monitor with the location and project
        /// </summary>
        /// <param name="pro"></param>
        public FileMonitor(Project pro)
        {
            this.Location = pro.Location;
            this.project = pro;

        }

        /// <summary>
        /// starts the process of watching for file changes
        /// </summary>
        /// <returns>success</returns>
        public bool start()
        {
            if (fWatcher == null)
            {
                try
                {
                    fWatcher = new FileSystemWatcher(location);
                    fWatcher.IncludeSubdirectories = true;
                    fWatcher.Changed += new FileSystemEventHandler(fWatcher_Changed);
                    fWatcher.Renamed += new RenamedEventHandler(fWatcher_Renamed);
                    fWatcher.Created += new FileSystemEventHandler(fWatcher_Changed);
                    fWatcher.Deleted += new FileSystemEventHandler(fWatcher_Changed);
                    fWatcher.NotifyFilter = NotifyFilters.FileName  | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
                    fWatcher.EnableRaisingEvents = true;
                }
                catch
                {
                    fWatcher = null;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// stops the process of watching for file changes
        /// </summary>
        /// <returns>success</returns>
        public bool stop()
        {
            try
            {
                fWatcher.Changed -= new FileSystemEventHandler(fWatcher_Changed);
                fWatcher.Renamed -= new RenamedEventHandler(fWatcher_Renamed);
                fWatcher.Created -= new FileSystemEventHandler(fWatcher_Changed);
                fWatcher.Deleted -= new FileSystemEventHandler(fWatcher_Changed);
                fWatcher.EnableRaisingEvents = false;
            }
            catch
            {
                return false;
            }
            finally
            {
                fWatcher = null;
            }
            return true;
        }

        /// <summary>
        /// passess event to ModificationRecorder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void  fWatcher_Changed(object sender, FileSystemEventArgs e)
        {
                 project.ModRecorder.notifyFileChange(e.FullPath, e.ChangeType);
            
        }   
        
        /// <summary>
        /// passess rename event to ModificationRecorder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fWatcher_Renamed(object sender, RenamedEventArgs e)
        {
                project.ModRecorder.notifyFileChange(e.OldFullPath, e.ChangeType,e.FullPath);
        }
    }
}
