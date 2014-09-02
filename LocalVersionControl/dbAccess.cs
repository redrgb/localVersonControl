using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace redrgb.DB
{    
    /// <summary>
    /// base database class
    /// </summary>
    public abstract class dbAccess
    {
        DbConnection connection = null;

        public string host;
        public string DB;
        public string user;

        protected string priKeyName = "";
        public DbDataReader lastQuery;

        /// <summary>
        /// set up the database
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="DB"></param>
        public dbAccess(string host, string user, string pass, string DB)
        {
            this.host = host;
            this.user = user;
            this.DB = DB;
            connect(pass);
        }

        /// <summary>
        /// set up the database
        /// </summary>
        /// <param name="file"></param>
        public dbAccess(string file)
        {
            this.host = file;

            connect("");
        }

        /// <summary>
        /// connects to the database
        /// </summary>
        /// <param name="pass">password</param>
        /// <returns>state of connection</returns>
        public abstract ConnectionState connect(string pass);

        /// <summary>
        /// executes command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract int execute(string command);
        /// <summary>
        /// executes command using Parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parms"></param>
        /// <returns>rows affected or -1 for error</returns>
        public abstract int execute(string command, Parameters parms);

        /// <summary>
        /// gets the number of rows in table
        /// </summary>
        /// <param name="table"></param>
        /// <returns>number of rows</returns>
        public abstract int GetCount(string table);

        /// <summary>
        /// Runs query command given
        /// </summary>
        /// <param name="command"></param>
        /// <returns>results of query</returns>
        public abstract DbDataReader query(string command);

        /// <summary>
        /// Runs query command given using parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parms"></param>
        /// <returns>results of query</returns>
        public abstract DbDataReader query(string command, Parameters parms);

        /// <summary>
        /// gets all values in table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="priKey">include primaryKey</param>
        /// <returns>all values in table as list of object[]</returns>
        public virtual List<object[]> selectAll(string table,bool priKey=false)
        {
            string priKeyQuery = "";
            if (priKey && !string.IsNullOrEmpty(priKeyName))
                priKeyQuery = priKeyName + ", ";
            DbDataReader results = query(@"SELECT " + priKeyQuery + " * FROM " + table);
            List<object[]> tableData = new List< object[]>();
            int i = 0;
            while (results.Read())
            {
                tableData.Add(new object[results.FieldCount]);
                results.GetValues(tableData[i]);
                i++;
            }
            return tableData;
        }

        /// <summary>
        /// checks if reader has rows to read
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>has rows</returns>
        public virtual bool hasRows(DbDataReader reader = null)
        {
            if(reader== null)
            {
                reader = this.lastQuery;
            }
            return reader.HasRows;
        }

        /// <summary>
        /// reads the next row in the provided reader
        /// </summary>
        /// <param name="querysData"></param>
        /// <returns>object[] of the row </returns>
        public virtual object[] readNext(DbDataReader querysData) //using querydata given
        {
            object[] rowData = new object[querysData.FieldCount];
            if (querysData.Read())
            { 
                querysData.GetValues(rowData);
                return rowData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// reads the next row in the reader form the last query
        /// </summary>
        /// <returns>object[] of the row </returns>
        public virtual object[] readNext() //overload using last query
        {
            object[] rowData = new object[lastQuery.FieldCount];
            if (lastQuery.Read())
            {
                lastQuery.GetValues(rowData);
                return rowData;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// gets last id inserted
        /// </summary>
        /// <returns>lastId</returns>
        public abstract long lastID();

        /// <summary>
        /// deconstur to close the database when refreance to class
        /// </summary>
        ~dbAccess()
        {
            if (!(lastQuery == null || lastQuery.IsClosed))
            {
               lastQuery.Dispose();
            }
            if (connection != null)
            {
                connection.Close();
            }
            }


    }
    public abstract class Parameters
    {
        private Dictionary<string,DbParameter> values;
        public Parameters()
        {
            this.values = new Dictionary<string, DbParameter>();
        }
        public Parameters(string name, object value)
        {
            this.values = new Dictionary<string, DbParameter>();
            this.add(name, value);
        }

        public abstract bool add(string name, object value);
        public abstract bool edit(string name, object value);
        public virtual  DbParameter[] getParameters()
        {
                DbParameter[] array = new DbParameter[this.values.Count];
                this.values.Values.CopyTo(array, 0);
                return array;

        }
    }


}
