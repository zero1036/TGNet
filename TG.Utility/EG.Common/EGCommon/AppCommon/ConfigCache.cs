
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using EG.Utility.Log;
using log4net;
using System.Xml;
using EG.Utility.AppCommon;
using EG.Utility.DBCommon.dao;

namespace EG.Business.Common
{
    public class ConfigCache
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DataSet DC = new DataSet();
        private static DataRowCollection AppConfigRows = null;
        private static DataTable dtAppconfig = new DataTable("AppConfig");

        /// <summary>
        /// dc add datatable --by edgar 2013-10-23
        /// </summary>
        /// <param name="dt">data table</param>
        public static void AddTable(DataTable dt)
        {
            if (DC.Tables.Contains(dt.TableName))
            {
                DC.Tables.Remove(dt.TableName);
            }
            DC.Tables.Add(dt);
        }

        /// <summary>
        /// load appconfig --by edgar 2013-10-22
        /// </summary>
        /// <param name="encryptions">decrypt encryption info</param>
        /// <returns>appconfig.appsettings all content</returns>
        public static DataTable LoadAppConfig(string[] encryptions)
        {
            log.Info("App Config Loading...");
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                d.Add(key, ConfigurationManager.AppSettings.Get(key));
            }
            return LoadAppConfig(encryptions, d);
        }

        /// <summary>
        /// load config from files -- by edgar 2013-12-5
        /// </summary>
        /// <param name="encryptions">decrypt encryption info</param>
        /// <param name="d">the config datas</param>
        /// <returns>appconfig.appsettings all content</returns>
        private static DataTable LoadAppConfig(string[] encryptions, Dictionary<string, string> d)
        {
            dtAppconfig.Columns.Add("key", typeof(string));
            dtAppconfig.Columns.Add("value", typeof(string));
            dtAppconfig.PrimaryKey = new DataColumn[] { dtAppconfig.Columns["key"] };

            foreach (KeyValuePair<string, string> kv in d)
            {
                DataRow dr = dtAppconfig.NewRow();
                dr["key"] = kv.Key;
                dr["value"] = kv.Value;
                dtAppconfig.Rows.Add(dr);
            }


            if (encryptions != null && encryptions.Length != 0)
            {
                DataRow encrypedRecord = dtAppconfig.Rows.Find("UserKey");
                if (encrypedRecord != null)
                {
                    encrypedRecord["value"] = DecryptClientKey(encrypedRecord["value"] as string);
                    string UserKey = encrypedRecord["value"].ToString();

                    foreach (string encryptionNodeName in encryptions)
                    {
                        encrypedRecord = dtAppconfig.Rows.Find(encryptionNodeName);
                        if (encrypedRecord == null)
                        {
                            throw new Exception("No such encryption node:" + encryptionNodeName);
                        }
                        else
                        {
                            if (encrypedRecord["value"] == null)
                            {
                                log.Warn("encryption node " + encryptionNodeName + " value is blank");
                            }
                            else
                            {
#if NET35
                                if (dtAppconfig.Rows.Contains("EncryptType") &&
                                    dtAppconfig.Rows.Find("EncryptType")["value"].ToString().Equals("35"))
                                { 
                                    encrypedRecord["value"] = DecryptInformation(true, encrypedRecord["value"] as string, UserKey); 
                                }
                                else
                                { 
                                    encrypedRecord["value"] = DecryptInformation(encrypedRecord["value"] as string, UserKey); 
                                }

#endif
#if NET40
                                encrypedRecord["value"] = DecryptInformation(encrypedRecord["value"] as string, UserKey);
#endif

                            }
                        }
                    }
                }
            }
            AddTable(dtAppconfig);
            AppConfigRows = dtAppconfig.Rows;

            log.Info("App Conifg Loaded");

            return dtAppconfig;
        }

        /// <summary>
        /// load config from files -- by edgar 2013-12-5
        /// </summary>
        /// <param name="encryptions">decrypt encryption info</param>
        /// <param name="fileNames">files name e.g."app.xml", "app.config" Ps:the file type is xml</param>
        /// <returns>appconfig.appsettings all content</returns>
        public static DataTable LoadAppConfig(string[] encryptions, string[] fileNames)
        {
            log.Info("App Config Loading...");
            Dictionary<string, string> d = new Dictionary<string, string>();
            XmlDocument doc = null;
            foreach (string fileName in fileNames)
            {
                log.Info(fileName + "Loading...");
                doc = new XmlDocument();
                string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
                doc.Load(strFileName);
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                foreach (XmlNode node in nodes)
                {
                    d.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
            return LoadAppConfig(encryptions, d);
        }


        public static void Load(CacheConfiguration config)
        {
            log.Info("TableConfiguration Loading...");

            DataSet ds = LoadDataSet(config);

            SetDC(ds);
        }

        public static void Load(TableConfiguration pTableConfig)
        {
            var config = new CacheConfiguration();
            config.tables.Add(pTableConfig);
            Load(config);
        }

        public static DataSet LoadDataSet(CacheConfiguration pConfig)
        {
            IList<TableConfiguration> tables = pConfig.tables;
            ADOTemplate template = new ADOTemplate();
            DataSet ds = new DataSet();
            foreach (TableConfiguration table in tables)
            {
                DataTable dataTable = template.Query(table.Sql, null, null, table.DBName);
                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["id"] };

                DataTable dt = dataTable.Copy();
                dt.TableName = table.TableName;
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static void SetDC(DataSet pDataSet)
        {
            if (pDataSet == null || pDataSet.Tables.Count <= 0)
                return;

            lock (DC)
            {
                foreach (DataTable table in pDataSet.Tables)
                {
                    AddTable(table.Copy());
                }
            }

        }


        /// <summary>
        /// get appconfig.appsettings content --by edgar 2013-10-22
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAppConfig()
        {
            return dtAppconfig;
        }

        /// <summary>
        /// get appconfig's value --by edgar 2013-10-24 
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>value</returns>
        public static string GetAppConfig(string key)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("Get App Config:" + key);
            }

            if (dtAppconfig == null)
            {
                throw new Exception("App Config has not been initialize");
            }
            else
            {
                DataRow row = dtAppconfig.Rows.Find(key);
                if (row == null)
                {
                    return null;
                }
                else
                {
                    return row["value"] as string;
                }
            }
        }

        /// <summary>
        /// Get Long Value From App Config Settings
        /// </summary>
        /// <param name="key">the key to find Setting</param>
        /// <returns>Config Value As Long</returns>
        public static long GetLongAppConfig(string key)
        {
            return long.Parse(GetAppConfig(key));
        }

        /// <summary>
        /// Get Int Value From App Config Settings
        /// </summary>
        /// <param name="key">the key to find Setting</param>
        /// <returns>Config Int As Int</returns>
        public static int GetIntAppConfig(string key)
        {
            return int.Parse(GetAppConfig(key));
        }

        /// <summary>
        /// Get Bool Value From App Config Settings
        /// if setting is Y/TRUE/YES/1, then return true, otherwise, return false.
        /// </summary>
        /// <param name="key">the key to find Setting</param>
        /// <returns>Config Bool As bool</returns>
        public static bool GetBoolAppConfig(string key)
        {
            string value = GetAppConfig(key);

            if (value == null)
            {
                return false;
            }

            value = value.ToLower();

            if ("y".Equals(value)
                || "true".Equals(value)
                || "yes".Equals(value)
                || "1".Equals(value))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Get Double Value From App Config Settings
        /// </summary>
        /// <param name="key">the key to find Setting</param>
        /// <returns>Config Value As Double</returns>
        public static double GetDoubleAppConfig(string key)
        {
            return double.Parse(GetAppConfig(key));
        }

        /// <summary>
        /// Get DateTime Value From App Config Settings
        /// </summary>
        /// <param name="key">the key to find Setting</param>
        /// <returns>Config Value As DateTime</returns>
        public static DateTime GetDateTimeAppConfig(string key)
        {
            return DateTime.Parse(GetAppConfig(key));
        }

        /// <summary>
        /// get appconfig db connect string--by edgar 2013-10-23
        /// 
        /// connect string ref: http://wenku.baidu.com/view/0c2a1734ee06eff9aef807ea.html
        /// 
        /// </summary>
        /// <param name="DBName">e.g. if has many db to connect,and wannt to get "Bullion" db connection str now,
        /// DBName should be "Bullion",the appconfig is "Bullion_DB_IP、Bullion_DB_PSW"...and so on
        /// if dbname is blank,can be null or ""</param>
        /// <returns>db connect string</returns>
        public static string GetDBConnectStr(string DBName)
        {
            short dbType = GetDBType(DBName);

            DBName += (string.IsNullOrEmpty(DBName)) ? "" : "_";

            string user = dtAppconfig.Rows.Find(DBName + "DB_USER")["value"].ToString();
            string schema = dtAppconfig.Rows.Find(DBName + "DB_DATABASE")["value"].ToString();
            string IP = dtAppconfig.Rows.Find(DBName + "DB_IP")["value"].ToString();
            string port = dtAppconfig.Rows.Find(DBName + "DB_PORT")["value"] as string;
            //IP += port.Equals("") ? null : ":" + port;
            string psw = dtAppconfig.Rows.Find(DBName + "DB_PSW")["value"].ToString();

            StringBuilder connectStr = new StringBuilder();

            if (dbType == ADOTemplate.DB_TYPE_SQLSERVER)//sql server
            {
                connectStr.Append("User ID=");
                connectStr.Append(user);
                connectStr.Append(";initial catalog=");
                connectStr.Append(schema);
                connectStr.Append(";Data Source=");
                connectStr.Append(IP);
                if (port != null && !port.Equals(""))
                {
                    connectStr.Append(",").Append(port);
                }
                connectStr.Append(";Password=");
                connectStr.Append(psw);
                connectStr.Append(";");
            }
            else if (dbType == ADOTemplate.DB_TYPE_ORACLE)//oracle
            {
                connectStr.Append("Data Source=");
                connectStr.Append(IP);
                if (port != null && !port.Equals(""))
                {
                    connectStr.Append(":").Append(port);
                }
                connectStr.Append("/");
                connectStr.Append(schema);
                connectStr.Append(";user id=");
                connectStr.Append(user);
                connectStr.Append(";password=");
                connectStr.Append(psw);
                connectStr.Append(";");
            }
            else if (dbType == ADOTemplate.DB_TYPE_MYSQL)//mysql 
            {
                // server=localhost;User Id=root;database=hknd;Character Set=latin1
                connectStr.Append("server=").Append(IP);
                if (port != null && !port.Equals(""))
                {
                    connectStr.Append(";port=");
                    connectStr.Append(port);
                }
                connectStr.Append(";database=");
                connectStr.Append(schema);
                connectStr.Append(";User Id=");
                connectStr.Append(user);
                connectStr.Append(";password=");
                connectStr.Append(psw);

                string charSet = dtAppconfig.Rows.Find(DBName + "CHARACTER_SET")["value"] as string;

                if (!string.IsNullOrEmpty(charSet))
                {
                    connectStr.Append(";Character Set=");
                    connectStr.Append(charSet);
                }
                connectStr.Append(";");
            }

            return connectStr.ToString();
        }

        /// <summary>
        /// Get the DB_TYPE from app.config by dbName
        /// </summary>
        /// <param name="DBName">the name of db</param>
        /// <returns></returns>
        public static short GetDBType(string DBName)
        {
            DBName += (string.IsNullOrEmpty(DBName)) ? "" : "_";

            var drType = dtAppconfig.Rows.Find(DBName + "DB_TYPE");
            if (drType != null)
                return short.Parse(drType["value"] as string);
            else
                return short.Parse(dtAppconfig.Rows.Find("DB_TYPE")["value"] as string);
        }

        /// <summary>
        /// decrypt client key --by edgar 2013-10-21
        /// </summary>
        /// <param name="encryptedClientKey">user key</param>
        /// <returns>decyption</returns>
        static string DecryptClientKey(string encryptedClientKey)
        {
            string programKey = "SecretK1";
            string DecryptedClientKey;
#if NET40
            DecryptedClientKey = new Security().Decrypt(encryptedClientKey, programKey);
#endif
#if NET35
            if (dtAppconfig.Rows.Contains("EncryptType") &&
                dtAppconfig.Rows.Find("EncryptType")["value"].ToString().Equals("35"))
            {
                DecryptedClientKey = Emperor.UtilityLib.CyberUtils.Decrypt("Aes", 256, encryptedClientKey, programKey);
            }
            else { DecryptedClientKey = new Security().Decrypt(encryptedClientKey, programKey); }
#endif

            return DecryptedClientKey;
        }

        /// <summary>
        /// decrypt information --by edgar 2013-10-21
        /// </summary>
        /// <param name="pIsEncrypted"></param>
        /// <param name="pEncryptedString">encryption information</param>
        /// <param name="pClientKey">user key</param>
        /// <returns>decyption information</returns>
        static string DecryptInformation(bool pIsEncrypted, string pEncryptedString, string pClientKey)
        {
            if (!pIsEncrypted)
                return pEncryptedString;
            else
            {
                try
                {
                    string programKey = "SecretK1";
                    string combinedKey = programKey + pClientKey;
                    return Emperor.UtilityLib.CyberUtils.Decrypt("Aes", 256, pEncryptedString, combinedKey);
                }
                catch (Exception ex)
                {
                    //Log.Log_ERROR(typeof(DataConsolidationBL), ex);
                    Log.Log_ERROR(typeof(ConfigCache),
                        new Exception("Error: Cannot Decrypt Information by ClientKey."));
                    throw ex;
                }
            }
        }

        /// <summary>
        /// net40 时使用 --edgar
        /// </summary>
        /// <param name="strToDecrypt"></param>
        /// <returns></returns>
        static string DecryptInformation(string strToDecrypt, string DecryptKey)
        {
            return new Security().Decrypt(strToDecrypt, DecryptKey);
        }

        public static DataRow Get(string _tableName, object _key)
        {
            return DC.Tables[_tableName].Rows.Find(_key);
        }

        /// <summary>
        /// Get the first row with special filter
        /// </summary>
        /// <param name="columns">filter conditions</param>
        /// <returns>the first row</returns>
        public static DataRow GetFirstDataRow(string tableName, Dictionary<string, object> columns)
        {
            IEnumerable<DataRow> dr = GetDataRow(tableName, columns);
            return dr == null ? null : dr.FirstOrDefault();
        }

        /// <summary>
        /// Get the rows with special filter
        /// </summary>
        /// <param name="columns">filter conditions</param>
        /// <returns>the list of rows</returns>
        public static IEnumerable<DataRow> GetDataRow(string tableName, Dictionary<string, object> columns)
        {
            DataTable dt = DC.Tables[tableName];

            if (dt == null)
                return null;

            //verfy if the table contain the column
            bool isContain = true;
            foreach (var para in columns.Keys)
            {
                if (!dt.Columns.Contains(para))
                    isContain = false;
            }

            if (isContain)
            {
                //filter data with conditions
                var linq = from row in dt.AsEnumerable() select row;
                foreach (var column in columns)
                {
                    int i = linq.Count();
                    DataRow a = linq.FirstOrDefault();
                    linq = linq.Where(r => r[column.Key].ToString() == column.Value.ToString());//r.Field<string>(column.Key)
                }

                return linq;
            }
            else
            {
                return null;
            }
        }

        public static long GetLong(string _tableName, string _colName, object _key)
        {
            return Convert.ToInt32(GetObject(_tableName, _colName, _key));
        }

        public static double GetDouble(string _tableName, string _colName, object _key)
        {
            return Convert.ToDouble(GetObject(_tableName, _colName, _key));
        }

        public static DateTime GetDateTime(string _tableName, string _colName, object _key)
        {
            return Convert.ToDateTime(GetObject(_tableName, _colName, _key));
        }

        public static object GetObject(string _tableName, string _colName, object _key)
        {
            DataRow dr = DC.Tables[_tableName].Rows.Find(_key);
            if (dr == null)
            {
                return null;
            }

            return dr[_colName];
        }

        public static void SetDbName(string pDbName)
        {
            if (dtAppconfig != null)
            {
                if (dtAppconfig.Rows.Contains("DB_DATABASE"))
                {
                    dtAppconfig.Rows.Find("DB_DATABASE")["value"] = pDbName;
                }
                else
                {
                    DataRow dr = dtAppconfig.NewRow();
                    dr["key"] = "DB_DATABASE";
                    dr["value"] = pDbName;
                    dtAppconfig.Rows.Add(dr);
                }
            }
        }
    }



    public class CacheConfiguration
    {
        public IList<TableConfiguration> tables = new List<TableConfiguration>();

        public void Add(string _dbName, string _tableName, string _sql, string _params)
        {
            TableConfiguration table = new TableConfiguration();

            table.DBName = _dbName;
            table.TableName = _tableName;
            table.Sql = _sql;

            tables.Add(table);
        }

        public void Clear()
        {
            tables.Clear();
        }
    }


    public class TableConfiguration
    {
        public string DBName { get; set; }

        public string TableName { get; set; }

        public string Sql { get; set; }

    }
}