using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using redrgb.DB;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using System.IO.Compression;

namespace LocalVersionControl
{
    /// <summary>
    /// Records the changes to file or folders
    /// </summary>
    public class ModificationRecorder
    {
        DirectoryInfo logDirectory;
        Project pro;
        RootFolder root;
        string xmlPath;
        changeLog log;
        Thread updateThread;

        public event EventHandler updateChangeList;

        /// <summary>
        /// gets the changeLog
        /// </summary>
        public changeLog Log
        {
            get { return log; }
        }

        /// <summary>
        /// gets the project
        /// </summary>
        public Project getProject
        {
            get { return pro; }
        }

        /// <summary>
        /// gets whether the project is in the process of updateing
        /// </summary>
        public bool isUpdating
        {
            get
            {
                if (updateThread != null && updateThread.IsAlive)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// load the change log and the snapshot and creates them if they dont exist
        /// </summary>
        /// <param name="pro"></param>
        public ModificationRecorder(Project pro)
        {
            try
            {
                this.pro = pro;
                logDirectory = new DirectoryInfo(pro.Location + @"\" + SettingsStore.Default.logFolderName);
                if (!logDirectory.Exists)
                {
                    logDirectory.Create();
                    logDirectory.Refresh();
                    logDirectory.Attributes = logDirectory.Attributes | FileAttributes.Hidden;
                }
                root = new RootFolder("", pro.Location, File.GetLastWriteTime(pro.Location));
                xmlPath = Path.Combine(logDirectory.FullName, "fileList.xml");
                if (File.Exists(xmlPath))
                {
                    try
                    {
                        using (XmlReader reader = XmlReader.Create(xmlPath))
                        {
                            root.load(reader);
                        }
                        System.Diagnostics.Debug.WriteLine(root.print());
                    }
                    catch
                    {

                    }
                }
                log = new changeLog(Path.Combine(logDirectory.FullName, "filelog.list"));
                pro.LastDate = log.getLastModTime();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Starts the update on a different thread
        /// </summary>
        public void update()
        {
            if (updateThread == null || !updateThread.IsAlive)
            {
                updateThread = new Thread(delegate()
                {
                    logDirectory.Refresh();
                    if (!logDirectory.Exists)
                    {
                        reload(pro);
                        log.readFile();
                    }
                    root.removeDeleted(log);
                    if (updateProcess(pro.Location))
                    {
                        saveList();
                        log.writeFile();
                    }
                    if (updateChangeList != null)
                        updateChangeList.Invoke(this, EventArgs.Empty);
                });
                updateThread.IsBackground = true;
                updateThread.Start();


            }
            return;

        }

        /// <summary>
        /// Does the actual updating using recursion
        /// </summary>
        /// <param name="path">current location</param>
        /// <param name="root">First folder</param>
        /// <returns>success</returns>
        private bool updateProcess(string path, bool root = true)
        {
            try
            {
                if (!pro.excludeLocationContains(path))
                {
                    // Process the list of files found in the directory.
                    string[] fileEntries = Directory.GetFiles(path);
                    foreach (string fileName in fileEntries)
                    {
                        if (!pro.excludeLocationContains(fileName))
                        {
                            DateTime fileDate = File.GetLastWriteTime(fileName);
                            TrackedFile file = this.root.followPath(fileName);
                            if (file != null)
                            {
                                if (string.IsNullOrEmpty(file.Checksum))
                                {
                                    //generates checksum if it is empty (usualy caused when file is in use when being inserted)
                                    string checksum = GenerateChecksum(fileName);
                                    file.update(checksum, makeSnapshot(fileName), fileDate);
                                    pro.LastDate = DateTime.Now;

                                }
                                if (file.dateChanged(fileDate))
                                {
                                    string checksum = GenerateChecksum(fileName);
                                    if (file.checksumChanged(checksum))
                                    {
                                        log.add(getRelative(fileName), WatcherChangeTypes.Changed, file.Snapshot, false);
                                        file.update(checksum, makeSnapshot(fileName), fileDate);
                                        pro.LastDate = DateTime.Now;
                                    }
                                }

                            }
                            else
                            {
                                string checksum = GenerateChecksum(fileName);
                                this.root.additem(fileName, checksum, makeSnapshot(fileName), fileDate);
                                log.add(getRelative(fileName), WatcherChangeTypes.Created, "", false);
                                pro.LastDate = DateTime.Now;
                            }
                        }
                    }

                    // Recurse into subdirectories of this directory.
                    string[] subdirEntries = Directory.GetDirectories(path);
                    foreach (string subdir in subdirEntries)
                    {
                        // Do not iterate through reparse points
                        if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint && !pro.excludeLocationContains(subdir))
                        {
                            if (this.root.additem(subdir, "", "", Directory.GetLastWriteTime(subdir)))
                            {
                                log.add(getRelative(subdir), WatcherChangeTypes.Created, "", true);
                                pro.LastDate = DateTime.Now;
                            }
                            //DirectoryInfo dir = new DirectoryInfo(subdir);
                            if (!updateProcess(subdir, false))
                                return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// makes a new snapshot of the file given
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns>snapshot path</returns>
        private string makeSnapshot(string path)
        {
            try
            {
                string snapshotPath = Path.Combine(logDirectory.FullName, "snapshots");
                if (!Directory.Exists(snapshotPath))
                {
                    Directory.CreateDirectory(snapshotPath);
                }
                string fileName = Path.Combine(snapshotPath, DateTime.Now.ToString("yyyy-MM-d--HH-mm-ss") + "_" + Path.GetFileName(path) + ".gz");

                if (File.Exists(fileName))
                {
                    fileName = Path.Combine(snapshotPath, DateTime.Now.ToString("yyyy-MM-d--HH-mm-ss") + "_" + Guid.NewGuid().ToString() + "_" + Path.GetFileName(path) + ".gz");
                }

                using (FileStream source = File.Open(path, FileMode.Open, FileAccess.Read))
                using (FileStream destination = File.Create(fileName))
                using (GZipStream compress = new GZipStream(destination, CompressionMode.Compress))
                {

                    //source.Seek(0, SeekOrigin.Begin);
                    source.CopyTo(compress);

                }
                //compress.Flush();
                //destination.Flush();

                //File.Copy(path, fileName, false);
                return getRelative(fileName);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// restores the snapshot at source to teh the dest
        /// </summary>
        /// <param name="sourcePath">snapshot location</param>
        /// <param name="destPath">destination location</param>
        /// <returns>success</returns>
        private bool restoreSnapshot(string sourcePath, string destPath)
        {

            try
            {
                if (!Path.GetExtension(sourcePath).Equals(".gz"))
                {
                    File.Move(sourcePath, destPath);
                }
                else
                {
                    using (FileStream source = File.Open(getFull(sourcePath), FileMode.Open, FileAccess.Read))
                    using (FileStream destination = File.Open(destPath, FileMode.Create))
                    using (GZipStream unCompress = new GZipStream(source, CompressionMode.Decompress))
                    {
                        // source.Seek(0, SeekOrigin.Begin);
                        unCompress.CopyTo(destination);
                        //unCompress.Flush();
                        //destination.Flush();
                    }

                }
                //File.Copy(path, fileName, false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// passes notifcation to correct lcoation and save the snapshot and change log.
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="type">type of change</param>
        /// <param name="renamePath">old path</param>
        public void notifyFileChange(string path, WatcherChangeTypes type, string renamePath = "")
        {
            if (!pro.excludeLocationContains(path))
            {
                try
                {
                    switch (type)
                    {
                        case WatcherChangeTypes.Changed:
                            changeFileFolder(path);
                            break;
                        case WatcherChangeTypes.Created:
                            addFileFolder(path);
                            break;
                        case WatcherChangeTypes.Deleted:
                            deleteFileFolder(path);
                            break;
                        case WatcherChangeTypes.Renamed:
                            renameFileFolder(path, renamePath);
                            break;
                        default:
                            break;
                    }
                    saveList();
                    log.writeFile();
                    if (updateChangeList != null)
                        updateChangeList.Invoke(this, EventArgs.Empty);
                }
                catch
                {

                }

            }
        }

        /// <summary>
        /// updates change log and snapshot with a added file/folder
        /// </summary>
        /// <param name="path"></param>
        public void addFileFolder(string path)
        {
            string checksum = "";
            string snapshot = "";
            if (File.Exists(path))
            {
                checksum = GenerateChecksum(path);
                snapshot = makeSnapshot(path);
                log.add(getRelative(path), WatcherChangeTypes.Created, snapshot, false);
            }
            else
            {
                log.add(getRelative(path), WatcherChangeTypes.Created, "", true);
            }
            pro.LastDate = DateTime.Now;
            root.additem(path, checksum, snapshot, File.GetLastWriteTime(path));

            System.Diagnostics.Debug.WriteLine(root.print());
        }

        /// <summary>
        /// updates change log and snapshot with a deleted file/folder
        /// </summary>
        /// <param name="path"></param>
        public void deleteFileFolder(string path)
        {
            //TrackedFile file = root.followPath(path);
            //log.add(path, WatcherChangeTypes.Deleted, file.Snapshot, file.);
            // root.remove(path);
            TrackedFile item = root.followPath(path);
            item.removeDeleted(Path.GetDirectoryName(path), log);
            pro.LastDate = DateTime.Now;
        }

        /// <summary>
        /// updates change log and snapshot with a renamed file/folder
        /// </summary>
        /// <param name="path"></param>
        private void renameFileFolder(string oldPath, string newPath)
        {
            TrackedFile item = root.followPath(oldPath);
            if (item != null)
            {
                item.Name = Path.GetFileName(newPath);
                log.add(getRelative(newPath), WatcherChangeTypes.Renamed, getRelative(oldPath), item.isDir);
                pro.LastDate = DateTime.Now;
            }


            System.Diagnostics.Debug.WriteLine(root.print());
        }

        /// <summary>
        /// updates change log and snapshot with a changed file/folder
        /// </summary>
        /// <param name="path"></param>
        private void changeFileFolder(string path)
        {
            TrackedFile item = root.followPath(path);
            if (item != null)
            {
                string checksum = "";
                DateTime fileDate = File.GetLastWriteTime(path);
                if (File.Exists(path))
                {
                    if (item.dateChanged(fileDate))
                    {
                        checksum = GenerateChecksum(path);

                        if (item.checksumChanged(checksum))
                        {
                            string snapshot = makeSnapshot(path);
                            log.add(getRelative(path), WatcherChangeTypes.Changed, item.Snapshot, item.isDir);
                            item.update(checksum, snapshot, fileDate);
                            pro.LastDate = DateTime.Now;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// saves the snapshot to xml
        /// </summary>
        public void saveList()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            logDirectory.Refresh();
            if (!logDirectory.Exists)
                logDirectory.Create();
            using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
            {
                root.save(writer);
            }
        }

        /// <summary>
        /// reload the snapshot and the log
        /// </summary>
        /// <param name="pro"></param>
        public void reload(Project pro)
        {
            this.pro = pro;
            logDirectory = new DirectoryInfo(pro.Location + @"\" + SettingsStore.Default.logFolderName);
            if (!logDirectory.Exists)
            {
                logDirectory.Create();
                logDirectory.Refresh();
                logDirectory.Attributes = logDirectory.Attributes | FileAttributes.Hidden;
            }
            root = new RootFolder("", pro.Location, File.GetLastWriteTime(pro.Location));
            xmlPath = Path.Combine(logDirectory.FullName, "fileList.xml");
            if (File.Exists(xmlPath))
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(xmlPath))
                    {
                        root.load(reader);
                    }
                    System.Diagnostics.Debug.WriteLine(root.print());
                }
                catch
                {

                }
            }
            log = new changeLog(Path.Combine(logDirectory.FullName, "filelog.list"));
        }

        /// <summary>
        /// undoes the last change
        /// </summary>
        /// <returns>the log entry undone</returns>
        public changeLogEntry revertsChange()
        {
            try
            {
                changeLogEntry lastEntry = log.remove();
                if (lastEntry != null)
                {
                    switch (lastEntry.Type)
                    {
                        case WatcherChangeTypes.Changed:

                            if (!lastEntry.isDir)
                            {
                                //File.Copy(lastEntry.Prev, lastEntry.Name, true);
                                //File.Delete(lastEntry.Prev);
                                restoreSnapshot(lastEntry.Prev, getFull(lastEntry.Name));
                                TrackedFile file = root.followPath(getFull(lastEntry.Name));
                                if (file != null)
                                {
                                    File.Delete(getFull(file.Snapshot));
                                    file.update(GenerateChecksum(getFull(lastEntry.Name)), lastEntry.Prev, File.GetLastWriteTime(getFull(lastEntry.Name)));
                                }
                            }
                            break;
                        case WatcherChangeTypes.Created:


                            if (lastEntry.isDir)
                                Directory.Delete(lastEntry.Name);
                            else
                            {
                                TrackedFile file = root.remove(getFull(lastEntry.Name));
                                if (file != null && File.Exists(file.Snapshot))
                                    File.Delete(getFull(file.Snapshot));

                                File.Delete(getFull(lastEntry.Name));
                                //File.Delete(lastEntry.Prev);
                            }
                            break;
                        case WatcherChangeTypes.Deleted:
                            if (lastEntry.isDir)
                            {
                                Directory.CreateDirectory(getFull(lastEntry.Name));
                                root.additem(getFull(lastEntry.Name), "", lastEntry.Prev, File.GetLastAccessTime(getFull(lastEntry.Name)));
                            }
                            else
                            {
                                if (!Directory.Exists(Path.GetDirectoryName(getFull(lastEntry.Name))))
                                {
                                    Directory.CreateDirectory(getFull(lastEntry.Name));
                                }

                                //File.Copy(lastEntry.Prev, lastEntry.Name);
                                restoreSnapshot(lastEntry.Prev, getFull(lastEntry.Name));
                                //File.Delete(lastEntry.Prev);

                                root.additem(getFull(lastEntry.Name), GenerateChecksum(getFull(lastEntry.Name)), lastEntry.Prev, File.GetLastAccessTime(getFull(lastEntry.Name)));
                            }

                            break;
                        case WatcherChangeTypes.Renamed:

                            if (lastEntry.isDir)
                            {
                                Directory.Move(getFull(lastEntry.Name), getFull(lastEntry.Prev));
                            }
                            else
                            {
                                File.Move(lastEntry.Name, lastEntry.Prev);
                            }
                            TrackedFile item = root.followPath(getFull(lastEntry.Name));
                            if (item != null)
                            {
                                item.Name = Path.GetFileName(getFull(lastEntry.Prev));
                            }

                            break;
                        default:
                            break;
                    }
                    saveList();
                    if (updateChangeList != null)
                        updateChangeList.Invoke(this, EventArgs.Empty);
                }
                return lastEntry;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// generates a cheksum of the given file using MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns>generated checksum</returns>
        private string GenerateChecksum(string path)
        {
            MD5 md5 = null;
            FileStream stream = null;
            try
            {
                md5 = MD5.Create();
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                return Convert.ToBase64String(md5.ComputeHash(stream));
            }
            catch
            {
                return "";
            }
            finally
            {
                if (md5 != null)
                {
                    md5.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }


        /// <summary>
        /// deletes all loging of the projects changes
        /// </summary>
        /// <returns>success</returns>
        public bool removeTrack()
        {
            try
            {
                logDirectory.Delete(true);
                reload(pro);
                if (updateChangeList != null)
                    updateChangeList.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets time project last changed
        /// </summary>
        /// <returns>time and date last changed</returns>
        public DateTime getLastModTime()
        {
            return log.getLastModTime();
        }

        /// <summary>
        /// makes full path Relative to project location
        /// </summary>
        /// <param name="path">full path</param>
        /// <returns>Relative path</returns>
        public string getRelative(string path)
        {
            System.Uri file = new Uri(path);
            string locationPath = pro.Location;
            if (!locationPath.EndsWith("\\"))
                locationPath += "\\";
            System.Uri folder = new Uri(locationPath);

            Uri relativeUri = folder.MakeRelativeUri(file);
            return Uri.UnescapeDataString(relativeUri.ToString());
        }

        /// <summary>
        /// Makes Full path from path Relative to project location
        /// </summary>
        /// <param name="path">Relative path</param>
        /// <returns>full path</returns>
        public string getFull(string path)
        {
            return Path.Combine(pro.Location, path);
        }

    }
}
