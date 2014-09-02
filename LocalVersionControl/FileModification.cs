using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using redrgb.DB;
using System.Text.RegularExpressions;
using System.Xml;

namespace LocalVersionControl
{
    /// <summary>
    /// snapshot of file
    /// </summary>
    public class TrackedFile
    {
        string name;
        DateTime lastUpdate;
        TrackedFolder parent;
        string checksum;
        string snapshot;
        protected bool directory;
        protected RootFolder root;
        /// <summary>
        /// gets or sets name of file
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// gets or sets time and date of last update
        /// </summary>
        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }
        /// <summary>
        /// gets or sets parent of file
        /// </summary>
        public TrackedFolder Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        /// <summary>
        /// gets or sets checksum of file
        /// </summary>
        public string Checksum
        {
            get { return checksum; }
            set { checksum = value; }
        }
        /// <summary>
        /// gets or sets snapshot of file
        /// </summary>
        public string Snapshot
        {
            get { return snapshot; }
            set { snapshot = value; }
        }
        /// <summary>
        /// returns false
        /// </summary>
        public bool isDir
        {
            get { return directory; }
        }

        /// <summary>
        /// create a files using values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="checksum"></param>
        /// <param name="snapshot"></param>
        /// <param name="lastUpdate"></param>
        public TrackedFile(string name, TrackedFolder parent, string checksum, string snapshot, DateTime lastUpdate)
        {
            if(parent!=null)
                this.root = parent.getRoot();
            this.name = name;
            this.parent = parent;
            this.checksum = checksum;
            this.snapshot = snapshot;
            this.directory = false;
            this.lastUpdate = lastUpdate;
        }

        /// <summary>
        /// compares date to see if has changed
        /// </summary>
        /// <param name="date"></param>
        /// <returns>true if date is later then lastUpdate </returns>
        public bool dateChanged(DateTime date)
        {
            if (!lastUpdate.Equals(DateTime.MinValue))
                return lastUpdate.CompareTo(date) < 0;
            else
                return true;
        }
        /// <summary>
        /// checks if checksum is the smae
        /// </summary>
        /// <param name="newchecksum"></param>
        /// <returns>if cehcksum are the same</returns>
        public bool checksumChanged(string newchecksum)
        {
            return !checksum.Equals(newchecksum);
        }
        /// <summary>
        /// follows the path to the file needed
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <returns>this file</returns>
        public virtual TrackedFile followPath(string[] path, int pos)
        {
            return this;
        }

        /// <summary>
        /// updates the files
        /// </summary>
        /// <param name="checksum"></param>
        /// <param name="snapshot"></param>
        /// <param name="date"></param>
        public void update(string checksum, string snapshot, DateTime date)
        {
            this.checksum = checksum;
            this.snapshot = snapshot;
            this.lastUpdate = date;
        }

        /// <summary>
        /// remove this file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public virtual TrackedFile remove(string[] path, int pos)
        {
            if (parent.removeItem(this))
                return this;
            else
                return null;
        }

        /// <summary>
        /// remove because file was deleted by adding to log
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public virtual bool removeDeleted(string path, changeLog log)
        {
            string currentPath = Uri.UnescapeDataString(Path.Combine(path, Name));
            //string currentPath = path;
            if (!File.Exists(currentPath))
            {
                log.add(getRelative(currentPath), WatcherChangeTypes.Deleted, snapshot, false);
                Parent.removeItem(this);
                return true;
            }
            return false;
        }


        /// <summary>
        /// makes full path Relative to project location
        /// </summary>
        /// <param name="path">full path</param>
        /// <returns>Relative path</returns>
        public string getRelative(string path)
        {
            System.Uri file = new Uri(path);
            string locationPath = root.RootPath;
            if (!locationPath.EndsWith("\\"))
                locationPath += "\\";
            System.Uri folder = new Uri(locationPath);

            Uri relativeUri = folder.MakeRelativeUri(file);
            return Uri.UnescapeDataString(relativeUri.ToString());
        }

        /// <summary>
        /// addes entry to xml file
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>sucesss</returns>
        public virtual bool save(XmlWriter writer)
        {
            writer.WriteStartElement("File");
            writer.WriteAttributeString("Name", this.Name);
            writer.WriteAttributeString("Checksum", this.Checksum);
            writer.WriteAttributeString("Snapshot", this.Snapshot);
            if (!this.lastUpdate.Equals(DateTime.MinValue))
                writer.WriteAttributeString("Date", this.LastUpdate.ToFileTime().ToString());

            writer.WriteEndElement();
            return true;
        }

        /// <summary>
        /// returns a string containing the name
        /// </summary>
        /// <returns>string to print</returns>
        public virtual string print()
        {
            string result = "File element:" + Name + Environment.NewLine;

            return result;
        }
    }

    /// <summary>
    /// snapshot of folder
    /// </summary>
    public class TrackedFolder : TrackedFile
    {
        protected List<TrackedFile> contents;

        public RootFolder getRoot()
        {
            return this.root;
        }
        /// <summary>
        /// create a folder using values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="checksum"></param>
        /// <param name="snapshot"></param>
        /// <param name="lastUpdate"></param>
        public TrackedFolder(string name, TrackedFolder parent, DateTime date)
            : base(name, parent, "", "", date)
        {
            if(parent!=null)
                this.root = parent.getRoot();
            contents = new List<TrackedFile>();
            directory = true;
            this.LastUpdate = date;
        }

        /// <summary>
        /// adds the item to this folder
        /// </summary>
        /// <param name="item"></param>
        public virtual void additem(TrackedFile item)
        {
            if (contents == null)
                contents = new List<TrackedFile>();
            contents.Add(item);
        }

        /// <summary>
        /// adds the item from values to given location in tree
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="checksum"></param>
        /// <param name="snapshot"></param>
        /// <param name="date"></param>
        /// <returns>success</returns>
        public bool additem(string[] path, int pos, bool dir, string checksum, string snapshot, DateTime date)
        {
            if (path.Length == pos + 1 && !dir)
            {
                if (this.getitem(path[pos]) == null)
                {
                    this.additem(new TrackedFile(path[pos], this, checksum, snapshot, date));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                bool added = false;
                TrackedFolder insertFolder = (TrackedFolder)getitem(path[pos]);
                if (insertFolder == null)
                {
                    insertFolder = new TrackedFolder(path[pos], this, date);
                    this.additem(insertFolder);
                    added = true;
                }
                if (path.Length > ++pos)
                {
                    added = insertFolder.additem(path, pos, dir, checksum, snapshot, date);
                }
                return added;
            }
        }

        /// <summary>
        /// gets item with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>item with name</returns>
        public TrackedFile getitem(string name)
        {
            foreach (TrackedFile file in contents)
            {
                if (file.Name.Equals(name))
                {
                    return file;
                }
            }
            return null;
        }

        /// <summary>
        /// gets item by following path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <returns>item at path</returns>
        public override TrackedFile followPath(string[] path, int pos)
        {
            if (pos > path.Length - 1)
                return this;
            else if (pos <= path.Length - 1)
            {
                TrackedFile fileItem = (TrackedFile)getitem(path[pos]);
                if (fileItem != null)
                    return fileItem.followPath(path, ++pos);
            }
            return null;
        }
        /// <summary>
        /// removes specific item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>sucesss</returns>
        public bool removeItem(TrackedFile item)
        {
            return contents.Remove(item);
        }

        /// <summary>
        /// remove item at path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <returns>item removed</returns>
        public override TrackedFile remove(string[] path, int pos)
        {
            if (pos > path.Length - 1)
            {
                Parent.removeItem(this);
                return this;
            }
            else if (pos <= path.Length - 1)
            {
                TrackedFile fileItem = (TrackedFile)getitem(path[pos]);
                if (fileItem != null)
                {
                    return fileItem.remove(path, ++pos);
                }
            }
            return null;
        }
        /// <summary>
        /// remove item at path making entry in log
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pos"></param>
        /// <returns>success</returns>
        public override bool removeDeleted(string path, changeLog log)
        {
            string currentPath = Uri.UnescapeDataString(Path.Combine(path, Name));
            bool res = false;
            foreach (TrackedFile item in contents.Reverse<TrackedFile>())
            {
                res = item.removeDeleted(currentPath, log);
            }

            if (!Directory.Exists(currentPath))
            {
                log.add(getRelative(currentPath), WatcherChangeTypes.Deleted, "", true);
                if (Parent != null)
                    Parent.removeItem(this);
                return true;
            }
            return res;
        }
        /// <summary>
        /// saves folder to xml adding contained file and folder.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>success</returns>
        public override bool save(XmlWriter writer)
        {
            writer.WriteStartElement("Folder");
            writer.WriteAttributeString("Name", this.Name);
            if (!LastUpdate.Equals(DateTime.MinValue))
                writer.WriteAttributeString("Date", this.LastUpdate.ToFileTime().ToString());
            foreach (TrackedFile item in contents)
            {
                item.save(writer);
            }
            writer.WriteEndElement();
            return true;
        }

        /// <summary>
        /// loads  folder to xml loading contained file and folder.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>success</returns>
        public virtual bool load(XmlReader reader)
        {
            string name;
            string dateString;
            while (reader.Read())
            {
                // Only detect start elements.
                if (reader.IsStartElement())
                {
                    // Get element name and switch on it.
                    switch (reader.Name)
                    {
                        case "Folder":
                            // Detect this element.
                            name = reader.GetAttribute("Name");
                            dateString = reader.GetAttribute("Date");
                            if (name != null)
                            {
                                DateTime date = DateTime.MinValue;
                                if (dateString != null)
                                {
                                    long time = 0;
                                    if (long.TryParse(dateString, out time))
                                    {
                                        date = DateTime.FromFileTime(time);
                                    }
                                }

                                TrackedFolder folder = new TrackedFolder(name, this, date);
                                folder.load(reader);
                                additem(folder);
                            }
                            break;
                        case "File":
                            // Detect this article element.
                            Console.WriteLine("Start <article> element.");
                            // Search for the attribute name on this current node.
                            name = reader.GetAttribute("Name");
                            string readChecksum = reader.GetAttribute("Checksum");
                            string snapshot = reader.GetAttribute("Snapshot");
                            dateString = reader.GetAttribute("Date");
                            if (name != null && readChecksum != null && snapshot != null)
                            {
                                DateTime date = DateTime.MinValue;
                                if (dateString != null)
                                {
                                    long time = 0;
                                    if (long.TryParse(dateString, out time))
                                    {
                                        date = DateTime.FromFileTime(time);
                                    }
                                }
                                additem(new TrackedFile(name, this, readChecksum, snapshot, date));
                            }
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// returns a string containing the name and the item it contains
        /// </summary>
        /// <returns>string to print</returns>
        public override string print()
        {
            string result = "Folder element:" + Name + Environment.NewLine;
            foreach (TrackedFile item in contents)
            {
                result += Environment.NewLine + item.print();
            }

            return result;
        }
    }

    /// <summary>
    /// the root folder of the snapshot
    /// </summary>
    public class RootFolder : TrackedFolder
    {
        string rootPath;
        public string RootPath
        {
            get { return rootPath; }
        }

        /// <summary>
        /// create the root folder using values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="date"></param>
        public RootFolder(string name, string path, DateTime date)
            : base(name, null, date)
        {
            this.root = this;
            this.rootPath = path;
        }

        /// <summary>
        /// add specific item
        /// </summary>
        /// <param name="item"></param>
        public override void additem(TrackedFile item)
        {
            if (contents == null)
                contents = new List<TrackedFile>();
            contents.Add(item);
        }

        /// <summary>
        /// spilt path to array of directories
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] spiltPath(string path)
        {
            
                Uri uriBase = new Uri(rootPath);
                Uri uriPath = new Uri(path);
                Uri relativePath = uriBase.MakeRelativeUri(uriPath);

            return relativePath.ToString().Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
        }

        /// <summary>
        /// add an item to path usign given values
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checksum"></param>
        /// <param name="snapshot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool additem(string path, string checksum, string snapshot, DateTime date)
        {
            string newName = Path.GetFileName(path);
            bool dir = !File.Exists(path);

            string[] pathArray = spiltPath(path);

            if (pathArray.Length <= 2)
            {
                if (this.getitem(pathArray[1]) == null)
                {
                    if (dir)
                    {
                        this.additem(new TrackedFolder(pathArray[1], this, date));
                    }
                    else
                    {
                        this.additem(new TrackedFile(pathArray[1], this, checksum, snapshot, date));

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                TrackedFolder insertFolder = (TrackedFolder)getitem(pathArray[1]);
                if (insertFolder == null)
                {
                    insertFolder = new TrackedFolder(pathArray[1], this, date);
                    this.additem(insertFolder);
                }

                return insertFolder.additem(pathArray, 2, dir, checksum, snapshot, date);
            }
        }

        /// <summary>
        /// use path to get item in tree
        /// </summary>
        /// <param name="path"></param>
        /// <returns>itme</returns>
        public TrackedFile followPath(string path)
        {
            string[] pathArray = spiltPath(path);
            int pos = 1;
            if (pos > pathArray.Length - 1)
                return this;
            else if (pos < path.Length - 1)
            {
                TrackedFile foundFolder = getitem(pathArray[pos]);
                if (foundFolder != null)
                    return foundFolder.followPath(pathArray, 2);
            }
            return null;
        }

        /// <summary>
        /// use path to remove item in tree
        /// </summary>
        /// <param name="path"></param>
        /// <returns>item</returns>
        public TrackedFile remove(string path)
        {
            string[] pathArray = spiltPath(path);
            int pos = 1;


            if (pos > pathArray.Length - 1)
            {
                return null;
            }
            else if (pos <= pathArray.Length - 1)
            {
                TrackedFile fileItem = (TrackedFile)getitem(pathArray[pos]);
                if (fileItem != null)
                    return fileItem.remove(pathArray, ++pos);
            }
            return null;
        }

        /// <summary>
        /// use path to remove item in tree storing change in log
        /// </summary>
        /// <param name="path"></param>
        /// <returns>success</returns>
        public bool removeDeleted(changeLog log)
        {
            string currentPath = Uri.UnescapeDataString(rootPath);
            bool res = false;
            foreach (TrackedFile item in contents.Reverse<TrackedFile>())
            {
                res = item.removeDeleted(rootPath, log);
            }
            return res;
        }

        /// <summary>
        /// saves the tree stucture to XML
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>success</returns>
        public override bool save(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("FileList");
            foreach (TrackedFile item in contents)
            {
                item.save(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            return true;
        }

        /// <summary>
        /// loads the tree stucture from XML
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>success</returns>
        public override bool load(XmlReader reader)
        {
            string name;
            string dateString;
            while (reader.Read())
            {
                // Only detect start elements.
                if (reader.IsStartElement())
                {
                    // Get element name and switch on it.
                    switch (reader.Name)
                    {
                        case "Folder":
                            // Detect this element.
                            name = reader.GetAttribute("Name");
                            dateString = reader.GetAttribute("Date");
                            if (name != null)
                            {
                                DateTime date = DateTime.MinValue;
                                if (dateString != null)
                                    DateTime.TryParse(dateString, out date);
                                TrackedFolder folder = new TrackedFolder(name, this, date);
                                folder.load(reader);
                                additem(folder);
                            }
                            break;
                        case "File":
                            // Detect this article element.
                            Console.WriteLine("Start <article> element.");
                            // Search for the attribute name on this current node.
                            name = reader.GetAttribute("Name");
                            dateString = reader.GetAttribute("Date");
                            string readChecksum = reader.GetAttribute("Checksum");
                            string snapshot = reader.GetAttribute("Snapshot");

                            if (name != null && readChecksum != null && snapshot != null)
                            {
                                DateTime date = DateTime.MinValue;
                                if (dateString != null)
                                {
                                    long time = 0;
                                    if (long.TryParse(dateString, out time))
                                    {
                                        date = DateTime.FromFileTime(time);
                                    }
                                }

                                additem(new TrackedFile(name, this, readChecksum, snapshot, date));
                            }
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// returns a string containing the name and type of all items contained
        /// </summary>
        /// <returns>string to print</returns>
        public override string print()
        {
            string result = "Root element:" + Name + Environment.NewLine;
            foreach (TrackedFile item in contents)
            {
                result += Environment.NewLine + item.print();
            }

            return result;
        }
    }


}
