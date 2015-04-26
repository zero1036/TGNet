using EWJ.EOrdering.DA.Sys;
using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.BL.Sys
{
    public class DictionaryBL : BaseBL
    {
        public DictionaryModel GetByID(string code)
        {
            DictionaryModel model = new DictionaryModel();
            code = string.IsNullOrEmpty(code) ? string.Empty : code;
            DataSet ds = new DictionaryDA().Get(code);
            model = ds.ToList<DictionaryModel>().FirstOrDefault();
            return model;
        }

        public IList<DictionaryModel> GetList(string pCode)
        {
            IList<DictionaryModel> list = new List<DictionaryModel>();

            pCode = string.IsNullOrEmpty(pCode) ? string.Empty : pCode;
            DataSet ds = new DictionaryDA().GetByParent(pCode);
            list = ds.ToList<DictionaryModel>();

            return list;
        }

        public IList<DictionaryVM> GetList(string pCode, bool? isDisable, string localization)
        {
            IList<DictionaryVM> list = new List<DictionaryVM>();

            pCode = string.IsNullOrEmpty(pCode) ? string.Empty : pCode;
            DataSet ds = new DictionaryDA().GetByParent(pCode, isDisable, localization);
            list = ds.ToList<DictionaryVM>();

            return list;
        }

        public bool Add(DictionaryModel model, Guid userID, DateTime dt)
        {
            int iCount = new DictionaryDA().Add(model.DicCode, model.EN_Name, model.CHS_Name, model.CHT_Name
                                                , model.ParentCode, model.Remark, model.Index
                                                , model.IsDisable, userID, dt
                                                , model.Extend1, model.Extend2, model.Extend3, model.Extend4);
            return iCount > 0 ? true : false;
        }

        public bool Delete(string code)
        {
            int iCount = new DictionaryDA().Delete(code);
            return iCount > 0 ? true : false;
        }

        public bool Update(DictionaryModel model, Guid userID, DateTime dt)
        {
            if (model == null || string.IsNullOrEmpty(model.DicCode))
            {
                return false;
            }
            bool result = new DictionaryDA().Update(model.DicCode, model.EN_Name, model.CHS_Name, model.CHT_Name
                                                , model.ParentCode, model.Remark, model.Index
                                                , model.IsDisable, userID, DateTime.Now
                                                , model.Extend1, model.Extend2, model.Extend3, model.Extend4);

            return result;
        }

        public string GetNextCode(string pCode)
        {
            string result = "001";
            pCode = string.IsNullOrEmpty(pCode) ? string.Empty : pCode;
            object obj = new DictionaryDA().GetMaxCode(pCode);
            if (obj == null)
            {
                return result;
            }

            if (string.IsNullOrEmpty(pCode))
            {
                result = (int.Parse(obj.ToString().Substring(0, 3)) + 1).ToString("000");
            }
            else
            {
                string strTemp = obj.ToString();
                strTemp = string.IsNullOrEmpty(strTemp) ? "000" : strTemp;
                result = pCode + (int.Parse(strTemp.Substring(strTemp.Length - 3)) + 1).ToString("000");
            }

            return result;
        }
    }
}
