using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using redrgb.DB;
using System.Windows.Forms;

namespace LocalVersionControl
{
    /// <summary>
    /// handles the loading of the projects and the saving of changes
    /// </summary>
    public class ProjectManager
    {
        private sqlite db;
        private List<Project> projects;

        /// <summary>
        /// gets the list of projects
        /// </summary>
        public List<Project> Projects
        {
            get { return projects; }
        }

        public ProjectManager()
        {

        }

        /// <summary>
        /// loads the projects from the database
        /// </summary>
        public void loadProjects()
        {
            try
            {
                if (db == null)
                {
                    db = new sqlite("lvcontrol.db");
                    db.execute("CREATE TABLE IF NOT EXISTS projects (name, description, location, note, time DEFAULT (datetime('now')),autoMonitor)");
                    db.execute("CREATE TABLE IF NOT EXISTS excludeLocations (path, projectID)");
                }

                projects = new List<Project>();

                List<object[]> projectRaw = db.selectAll("projects", true);

                foreach (object[] cells in projectRaw)
                {
                    if (cells.Length >= 6)
                    {
                        try
                        {
                            long key = Convert.ToInt32(cells[0]);
                            string name = Convert.ToString(cells[1]);
                            string description = Convert.ToString(cells[2]);
                            string location = Convert.ToString(cells[3]);
                            string note = Convert.ToString(cells[4]);
                            bool auto = Convert.ToBoolean(cells[6]);
                            DateTime time = Convert.ToDateTime(Convert.ToString(cells[5]));
                            SqliteParameters parm = new SqliteParameters();
                            parm.add("@proID", key);
                            Project pro = new Project(key, name, description, location, note, time, auto);

                            db.query(@"SELECT path FROM excludeLocations WHERE projectID=@proID", parm);

                            List<object[]> tableData = new List<object[]>();

                            object[] values = db.readNext();
                            while (values != null)
                            {
                                pro.loadExcludeLocation(Convert.ToString(values[0]));
                                values = db.readNext();
                            }


                            projects.Add(pro);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Failed to load a Project will continue loading", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load Project unable to continue. Try restarting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// creates a new blank project
        /// </summary>
        /// <returns></returns>
        public Project createProject()
        {
            Project newProject = new Project();
            projects.Add(newProject);
            return newProject;
        }

        /// <summary>
        /// saves the changes of a project to the database
        /// </summary>
        /// <param name="pro"></param>
        public void updateProject(Project pro)
        {
            SqliteParameters parm = new SqliteParameters();
            parm.add("@newName", pro.Name);
            parm.add("@newDesc", pro.Description);
            parm.add("@newLocation", pro.Location);
            parm.add("@newNote", pro.Note);
            parm.add("@newTime", String.Format("{0:yyyy-MM-dd HH:mm:ss}", pro.Time));
            parm.add("@newAuto", pro.AutoMonitor);
            parm.add("@ID", pro.Key);
            if (pro.NewEntry)
            {
                db.execute("INSERT INTO projects (name, description, location, note, time, autoMonitor ) VALUES (@newName,@newDesc,@newLocation,@newNote,@newTime,@newAuto)", parm);
                pro.Key = db.lastID();
                pro.NewEntry = false;
            }
            else
            {
                db.execute("UPDATE projects SET name = @newName, description = @newDesc, location = @newLocation, time = @newTime, autoMonitor= @newAuto WHERE rowid=@ID", parm);
            }
            if (pro.ExcludeLocationsChanged)
            {
                parm = new SqliteParameters();
                parm.add("@proID", pro.Key);
                parm.add("@newpath", "");
                db.execute(@"DELETE FROM excludeLocations WHERE projectID=@proID", parm);
                if (pro.ExcludeLocations != null)
                {
                    foreach (string path in pro.ExcludeLocations)
                    {
                        parm.edit("@newpath", path);
                        db.execute("INSERT INTO excludeLocations (path, projectID ) VALUES (@newpath,@proID)", parm);
                    }
                }
            }
            pro.ModRecorder.reload(pro);
        }


        /// <summary>
        /// saves teh not of an applcation to the database
        /// </summary>
        /// <param name="pro"></param>
        public void updateNote(Project pro)
        {
            SqliteParameters parm = new SqliteParameters();
            parm.add("@newNote", pro.Note);
            parm.add("@ID", pro.Key);
            db.execute("UPDATE projects SET note = @newNote WHERE rowid=@ID", parm);
        }

        /// <summary>
        /// removes the project from the local list and from the database
        /// </summary>
        /// <param name="pro"></param>
        public void removeProject(Project pro)
        {
            if (!pro.NewEntry)
            {
                SqliteParameters parm = new SqliteParameters();
                parm.add("@ID", pro.Key);
                db.execute("DELETE FROM projects WHERE rowid=@ID", parm);

            }
            projects.Remove(pro);
        }
    }
}
