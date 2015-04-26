using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EWJ.EOrdering.ViewModel.Diamond;
using EWJ.EOrdering.DA.Diamond;
using EWJ.EOrdering.Common;

namespace EWJ.EOrdering.BL.Diamond
{
    public class DiamondBL:BaseBL
    {
        private DiamondDA diamondDa;

        public DiamondBL()
        {
 
        }

        /// <summary>
        /// 查询diamond
        /// </summary>
        /// <param name="queryItem"></param>
        /// <param name="localization"></param>
        /// <returns></returns>
        public static IList<DiamondItem> DiamondsQuery(DiamondQueryVM diaomondQuery, string localization)
        {
            try
            {
                DiamondDA diamondDa = new DiamondDA();
                return diamondDa.GetDiamondList(diaomondQuery.DiamondQueryItem,diaomondQuery.ScendingList, localization).ToList<DiamondItem>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 调配diamond
        /// </summary>
        /// <param name="assignVM"></param>
        /// <returns></returns>
        public IList<AssignResultItem> DiamondsAssign(DiamondAssignVM assignVM)
        {
            try {
                 var groupByPairIDList=from a in assignVM.AssignList
                          where !string.IsNullOrEmpty(a.PairID)
                          group a by a.PairID into g
                          where g.Count()==1
                          select new {
                          doubleNum=g.Count()
                          };
                if(groupByPairIDList.Count()>0)
                {
                    return null;
                }
                else
                {
                    DiamondDA diamondDa = new DiamondDA();
                    DataTable assignTb = CommonBL.GetDataTableFromEntities<AssignItem>(assignVM.AssignList);
                    return diamondDa.DiamondsAssign(assignTb).ToList<AssignResultItem>();            
                }           
            }
            catch (Exception ex)
            {
                throw ex;
            }
 
        }

    }
}
