using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using EG.Utility.DBCommon.dao;
using EG.Business.Common;
using log4net;
using System.Reflection;

namespace EG.UnitTest
{
    public class ExcelParser
    {

        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IDbCommand command;
        private int rowCount;
        private int colCount;
        private Range range;

        private string type;
        private string dbName;
        private string tableName;
        private string[] colNames;

        private string insertSql;
        private string deleteSql;

        public void Parse(string fileName)
        {
            Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;

            ADOTemplate template = new ADOTemplate();

            xlApp = new ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            for (int iSheet = 1; iSheet <= xlWorkBook.Worksheets.Count; iSheet++)
            {
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(iSheet);
                range = xlWorkSheet.UsedRange;

                rowCount = range.Rows.Count;
                if (rowCount < 2)
                {
                    continue;
                }

                colCount = range.Columns.Count;

                this.ReadConfig();

                this.ReadColumns();

                IDbTransaction Transaction = null;
                using (IDbConnection connection = DBUtil.GetConnection(dbName))
                {
                    try
                    {
                        connection.Open();

                        Transaction = connection.BeginTransaction();

                        new TransactionContext(connection, Transaction, ConfigCache.GetDBType(dbName));

                        for (int iRow = 2; iRow <= rowCount; iRow++)
                        {
                            Hashtable data = this.ReadDataRow(iRow);

                            string op = (string)data["$op"];

                            if ("s".Equals(op))
                            {
                                template.Execute((string)data["$sql"], null, null);
                            }
                            else if ("i".Equals(op))
                            {
                                template.Execute(this.insertSql, data);
                            }
                            else if ("d".Equals(op))
                            {
                                template.Execute(this.deleteSql, data);
                            }
                        }

                        Transaction.Commit();

                    }
                    catch (Exception e)
                    {
                        if (Transaction != null)
                        {
                            Transaction.Rollback();
                        }
                        throw e;
                    }
                    finally
                    {
                        if (connection != null && ConnectionState.Open == connection.State)
                        {
                            connection.Close();
                        }
                    }
                }

                ReleaseObject(xlWorkSheet);
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
        }


        private object GetValue(int iRow, int iCol)
        {
            return (range.Cells[iRow, iCol] as Range).Value2;
        }

        private string GetString(int iRow, int iCol)
        {
            return (string)(range.Cells[iRow, iCol] as Range).Value2;
        }

        private void ReadConfig()
        {
            this.type = GetString(1, 2);
            this.dbName = GetString(1, 4);
            this.tableName = GetString(1, 6);
        }



        private void ReadColumns()
        {
            this.colNames = new string[this.colCount + 2];

            for (int iCol = 2; iCol <= colCount; iCol++)
            {
                this.colNames[iCol] = this.GetString(2, iCol);
            }

            this.GenerateInsertSQL();
            this.GenerateDeleteSQL();
        }

        private void GenerateInsertSQL()
        {
            string valuesSql = "";

            insertSql = "insert into " + this.tableName + "(";

            for (int iCol = 2; iCol <= colCount; iCol++)
            {
                string colName = this.colNames[iCol];

                if (colName == null)
                {
                    break;
                }

                if (iCol != 2)
                {
                    insertSql += " ,";
                    valuesSql += " ,";
                }

                insertSql += colName;
                valuesSql += "[" + colName + "]";
            }
            insertSql += " )values(" + valuesSql + ")";

            log.Debug("insert sql: " + insertSql);
        }

        private void GenerateDeleteSQL()
        {

            deleteSql = "delete from " + this.tableName + " where";

            for (int iCol = 2; iCol <= colCount; iCol++)
            {
                string colName = this.colNames[iCol];

                if (colName == null)
                {
                    break;
                }

                deleteSql += "{ " + colName + "=[" + colName + "] and \n}";
            }

            deleteSql += "1=1";// 如果没有and语句，则出错！即必需有条件删除数据！


            log.Debug("delete sql: " + deleteSql);
        }

        private Hashtable ReadDataRow(int iCurRow)
        {
            Hashtable table = new Hashtable();

            string op = this.GetString(iCurRow, 1);

            table.Add("$op", op);

            if ("s".Equals(op))
            {
                table.Add("$sql", this.GetString(iCurRow, 2));
                return table;
            }

            for (int iCol = 2; iCol <= colCount; iCol++)
            {
                string colName = this.colNames[iCol];

                if (colName == null)
                {
                    break;
                }

                table.Add(colName, this.GetValue(iCurRow, iCol));
            }

            return table;
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
