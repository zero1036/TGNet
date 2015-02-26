using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;

namespace EG.Utility.AppCommon
{
    public class ExcelHelper
    {
        public static System.Data.DataTable GetDataTableFromCSV(string csvFilePath)
        {
            return GetDataTableFromCSV(csvFilePath, ',', Encoding.Default);
        }
        public static System.Data.DataTable GetDataTableFromCSV(string csvFilePath, Encoding charset)
        {
            return GetDataTableFromCSV(csvFilePath, ',', charset);
        }
        public static System.Data.DataTable GetDataTableFromCSV(string csvFilePath, char splitChar, Encoding charset)
        {
            System.Data.DataTable resultDT = new System.Data.DataTable();
            StreamReader reader = new StreamReader(csvFilePath, charset);
            int po = 0;
            while (reader.Peek() > 0)
            {
                string str = reader.ReadLine();
                string[] split = str.Split(splitChar);

                DataRow dr = resultDT.NewRow();
                for (int i = 0; i < split.Length; i++)
                {

                    if (po == 0) resultDT.Columns.Add(split[i].Trim().Replace("\"", ""));
                    else dr[i] = split[i].Trim();
                }
                if (po != 0)
                    resultDT.Rows.Add(dr);

                po++;
            }

            return resultDT;
        }
    }
}
