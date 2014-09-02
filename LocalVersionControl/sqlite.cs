using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace redrgb.DB
{
    /// <summary>
    /// sqlite database class
    /// </summary>
    public class sqlite : dbAccess
    {
        SQLiteConnection connection = null;

        /// <summary>
        /// sets up the database
        /// </summary>
        /// <param name="host">database file</param>
        public sqlite(string host)
            : base(host)
        {
            priKeyName = "rowid";
        }

        /// <summary>
        /// connects to the database
        /// </summary>
        /// <param name="pass">password</param>
        /// <returns>state of connection</returns>
        public override ConnectionState connect(string pass="")
        {

            //SQ fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            this.connection = new SQLiteConnection("Data Source=" + this.host);

            this.connection.Open();

            return this.connection.State;
        }

        /// <summary>
        /// executes command
        /// </summary>
        /// <param name="command"></param>
        /// <returns>rows affected or -1 for error</returns>
        public override int execute(string command)
        {

            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
            int status;
            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            try
            {
                status = sqliteCommand.ExecuteNonQuery();
            }
            catch
            {
                //this.connection.Open();
               // status = sqliteCommand.ExecuteNonQuery();
                status = -1;
            }

            return status;
        }

        /// <summary>
        /// executes command using Parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parms"></param>
        /// <returns>rows affected or -1 for error</returns>
        public override int execute(string command, Parameters parms)
        {
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
            int status;
            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            sqliteCommand.Parameters.AddRange(parms.getParameters());
            try
            {
                status = sqliteCommand.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            return status;
        }

        /// <summary>
        /// executes a pre created DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns>rows affected or -1 for error</returns>
        public int execute(DbCommand command)
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }

            command.Connection = this.connection;

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// gets the number of rows in table
        /// </summary>
        /// <param name="table"></param>
        /// <returns>number of rows</returns>
        public override int GetCount(string table)
        {

            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
            SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT COUNT(*) FROM `" + table + "`", this.connection);


            return (int)sqliteCommand.ExecuteScalar();
        }

        /// <summary>
        /// Runs query command given
        /// </summary>
        /// <param name="command"></param>
        /// <returns>results of query</returns>
        public override DbDataReader query(string command)
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            SQLiteDataReader results = sqliteCommand.ExecuteReader();


            this.lastQuery = results;

            return results;
        }

        /// <summary>
        /// Runs query command given using parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parms"></param>
        /// <returns>results of query</returns>
        public override DbDataReader query(string command, Parameters parms)
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            sqliteCommand.Parameters.AddRange(parms.getParameters());
            SQLiteDataReader results = sqliteCommand.ExecuteReader();


            this.lastQuery = results;

            return results;
        }

        /// <summary>
        /// fills a data table using query given
        /// </summary>
        /// <param name="command"></param>
        /// <returns>datatable</returns>
        public DataTable fill(string command)
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }

            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            SQLiteDataAdapter sqliteAd = new SQLiteDataAdapter(sqliteCommand);

            DataTable dt = new DataTable();
            sqliteAd.Fill(dt);

            this.lastQuery = sqliteCommand.ExecuteReader();

            return dt;
        }

        /// <summary>
        /// fills a data table using query given
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parms"></param>
        /// <returns>datatable</returns>
        public DataTable fill(string command, Parameters parms)
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }

            SQLiteCommand sqliteCommand = new SQLiteCommand(command, this.connection);
            sqliteCommand.Parameters.AddRange(parms.getParameters());
            SQLiteDataAdapter sqliteAd = new SQLiteDataAdapter(sqliteCommand);

            DataTable dt = new DataTable();
            sqliteAd.Fill(dt);

            this.lastQuery = sqliteCommand.ExecuteReader();

            return dt;
        }

        /// <summary>
        /// gets last id inserted
        /// </summary>
        /// <returns>lastId</returns>
        public override long lastID()
        {
            return this.connection.LastInsertRowId;
        }
    }

    /// <summary>
    /// class to hold SQLiteParameter for easy passing to database
    /// </summary>
    public class SqliteParameters : Parameters
    {
        private Dictionary<string,SQLiteParameter> values;

        /// <summary>
        /// creates empty set
        /// </summary>
        public SqliteParameters()
        {
            this.values = new Dictionary<string, SQLiteParameter>();
        }

        /// <summary>
        /// creates parameter suing values given
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public SqliteParameters(string name, object value)
        {
            this.values = new Dictionary<string, SQLiteParameter>();
            this.add(name, value);
        }

        /// <summary>
        /// add new parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>success</returns>
        public override bool add(string name, object value)
        {
            try
            {
                this.values.Add(name,new SQLiteParameter(name, value));
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// edits existing value using name as key
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>success</returns>
        public override bool edit(string name, object value)
        {
            try
            {
                this.values[name].Value = value;
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// get the parameters
        /// </summary>
        /// <returns>arrays of parameter</returns>
        public override DbParameter[] getParameters()
        {
            SQLiteParameter[] array = new SQLiteParameter[this.values.Count];
            this.values.Values.CopyTo(array, 0);
            return array;
        }
    }
}


