using EWJ.EOrdering.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EWJ.EOrdering.DA.ModelOptions
{
    public class OptionsDA : BaseDA
    {
        private string GetALLOptionsStr = @"   select * from view_DictionaryLocalization where Localization=@Localization and isnull(ParentCode,'')<>''  and IsDisable is null ";
        public DataSet GetALLOptions(string Localization)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Localization", Localization));
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, GetALLOptionsStr,paramList.ToArray());

            return ds;
        }

    }
}
