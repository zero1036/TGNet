using EWJ.EOrdering.ViewModel.ModelVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWJ.EOrdering.DA.Model;
using EWJ.EOrdering.DA.ModelOptions;
using System.Data;
namespace EWJ.EOrdering.BL.ModelBL
{
    public class SCModel:BaseBL
    {
        public SCModelsVM Getlist(ModelSearchVM v)
        {
            SCModelDA da = new SCModelDA();
            DataSet ds = da.GetModels(v.Localization, v.OrderType, v.BeginNo, v.PageNo, v.EWJNo, v.ModelTypeCode, v.CaiZhiCode, v.ColorCode, v.KelaWeightCode);
            return GetModels(ds);
        }

        private SCModelsVM GetModels(DataSet ds)
        {
            DataTable dt;
            dt = ds.Tables[0];
            SCModelsVM sc = new SCModelsVM();
            sc.RecNum = int.Parse(dt.Rows[0][0].ToString());
            sc.SCModels = new List<SCModelVM>();
            dt = ds.Tables[1];
            SCModelVM model;
            foreach (DataRow dr in dt.Rows)
            {
                model = new SCModelVM();
                model.ModelID = dr["ModelID"].ToString();
                model.ModelTypeCode = dr["ModelTypeID"].ToString();
                model.ModelTypeString = dr["ModelTypeStr"].ToString();
                model.EWJNo = dr["EWJNo"].ToString();
                model.FromDate = DateTime.Parse(dr["FromDate"].ToString()).ToShortDateString();
                model.ImagePath = EWJ.EOrdering.Common.ConfigHelper.DEFAULT_ImageFolder + dr["ImagePath"].ToString();
                model.Leixiao = DateTime.Parse(dr["FromDate"].ToString()).AddMonths(3) < DateTime.Now ? dr["SaleQuantity"].ToString() : "新品";
                sc.SCModels.Add(model);
            }
            return sc;
        }

       

    }
}
