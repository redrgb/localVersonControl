using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LocalVersionControl
{
    /// <summary>
    /// stores the projects setting
    /// </summary>
    public class Project
    {
        private long key;
        private string name;
        private string description;
        private string location;
        private string note;
        private DateTime time;
        private DateTime lastDate;
        private bool autoMonitor;
        private bool newEntry;
        private bool excludeLocationsChanged;
        private List<string> excludeLocations;
        private bool willMonitor;
        private FileMonitor fMonitor;

        private ModificationRecorder modRecord;

        public event EventHandler updateProjectModDate;

        /// <summary>
        /// gets or sets key used by database
        /// </summary>
        public long Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// gets or sets name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// gets or sets Description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// gets or sets location
        /// </summary>
        public string Location
        {
            get { return location; }
            set { location = value; 

                    if (fMonitor != null)
                    {
                        fMonitor.Location = location;
                    }
            }
        }

        /// <summary>
        /// gets or sets note
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        /// <summary>
        /// gets or sets last modifcation data
        /// </summary>
        public DateTime LastDate
        {
            get { return lastDate; }
            set {
                lastDate = value;
                if (updateProjectModDate != null)
                    updateProjectModDate.Invoke(this, EventArgs.Empty);
            }

        }
        /// <summary>
        /// gets creation time
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// gets or set if set to start monitoring automaticly
        /// </summary>
        public bool AutoMonitor
        {
            get { return autoMonitor; }
            set { autoMonitor = value; }
        }
        
        /// <summary>
        /// gets or sets if newentry
        /// </summary>
        public bool NewEntry
        {
            get { return newEntry; }
            set { newEntry = value; }
        }

        /// <summary>
        /// gets or sets if will start monitoring after update
        /// </summary>
        public bool WillMonitor
        {
            get { return willMonitor; }
            set { willMonitor = value; }
        }

        /// <summary>
        /// gets if Modrecorder has been created
        /// </summary>
        public bool ModrecorderCreated
        {
            get { return modRecord!=null; }
        }
        public ModificationRecorder ModRecorder
        {
            get {
                if (modRecord == null)
                {
                    modRecord = new ModificationRecorder(this);
                }
                return modRecord;
            }

        }
        /// <summary>
        /// gets or sets Exclude locations
        /// </summary>
        public List<string> ExcludeLocations
        {
            get
            {
                excludeLocationsChanged = false;
                return excludeLocations;
            }
            set
            {
                excludeLocationsChanged = true;
                excludeLocations = value;
            }
        }
        
        /// <summary>
        /// gets if ExcludeLocation has been changed
        /// </summary>
        public bool ExcludeLocationsChanged
        {
            get { return excludeLocationsChanged; }
        }

        /// <summary>
        /// gets if monitoring
        /// </summary>
        public bool isMonitoring
        {
            get
            {
                if (fMonitor == null)
                {
                    return false;
                }
                else
                {
                    return fMonitor.isMonitoring;
                }
            }
        }

        /// <summary>
        /// creates projects useing given values
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="note"></param>
        /// <param name="time"></param>
        /// <param name="autoMonitor"></param>
        public Project(long key, string name, string description, string location, string note, DateTime time,bool autoMonitor)
        {
            this.key = key;
            this.name = name;
            this.description = description;
            this.location = location;
            this.note = note;
            this.time = time;
            newEntry = false;
            this.autoMonitor = autoMonitor;
            //if (autoMonitor)
            //{
             //   this.willMonitor = true;
            //    this.update();
            //}
        }

        /// <summary>
        /// set up blank project
        /// </summary>
        public Project()
        {
            time = DateTime.Now;
            lastDate = DateTime.Now;
            newEntry = true;
            autoMonitor = false;
            key = -1;
        }

        /// <summary>
        /// loads exclusion list
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool loadExcludeLocation(string path)
        {

            if (excludeLocations == null)
            {
                excludeLocations = new List<string>();
            }
            excludeLocations.Add(path.ToLower().Trim(new char[] { ' ', '\\', '/' }));
            return true;
        }

        /// <summary>
        /// finds if path is should be excluded
        /// </summary>
        /// <param name="path"></param>
        /// <returns>exclude path from log</returns>
        public bool excludeLocationContains(string path)
        {
            string[] excludeExtension = SettingsStore.Default.DefaultExclude.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (Path.GetFullPath(path).ToLower().Contains(SettingsStore.Default.logFolderName.ToLower()))
            {
                return true;
            }
            else if (excludeExtension.Contains(Path.GetExtension(path).ToLower().TrimStart(new char[] { '.' })))
            {
                return true;
            }
            else if (excludeLocations == null)
            {
                return false;
            }
            else
            {
                bool isFile = File.Exists(path);
                foreach (string value in excludeLocations)
                {
                    if (isFile)
                    {
                        if (Path.GetFullPath(path).Equals(Path.GetFullPath(value),StringComparison.CurrentCultureIgnoreCase))
                        {
                            return true;
                        }else if (Path.GetFullPath(Path.GetDirectoryName(path)).StartsWith(Path.GetFullPath(value),StringComparison.CurrentCultureIgnoreCase))
                        {
                           return true;
                        }

                    }
                    else
                    {
                        if (Path.GetFullPath(path).StartsWith(Path.GetFullPath(value), StringComparison.CurrentCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// starts updating the project
        /// </summary>
        public void update()
        {
            if (isMonitoring)
                stopMonitoring();
            ModRecorder.update();
        }

        /// <summary>
        /// gets the name as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// starts the projects monitoring for changes
        /// </summary>
        /// <returns>success</returns>
        public bool startMonitoring()
        {
            willMonitor = false;
            if (fMonitor == null)
            {
                fMonitor = new FileMonitor(this);
            }
            return fMonitor.start();
        }

        /// <summary>
        /// stops the projects monitoring for changes
        /// </summary>
        /// <returns>success</returns>
        public bool stopMonitoring()
        {
            willMonitor = false;
            if (fMonitor != null)
            {
                return fMonitor.stop();
            }
            return false;
        }
    }
}
