using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.WeChat.Model
{

    /// <summary>
    /// 用户信息返回类型
    /// </summary>
    public class LoginInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<AccessRight> AccessRight { get; set; }
    }


    /// <summary>
    /// 结果返回类型
    /// </summary>
    public class ResultM
    {
        int _Affected = 0;

        public bool IsSuccess
        {
            get { return _Affected > 0; }
        }

        public int Affected
        {
            get { return _Affected; }
            set { _Affected = value; }
        }

        public string Message { get; set; }
    }


    /// <summary>
    /// 结果集返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultM<T>
    {
        bool _IsSuccess = true;

        public List<T> EntityList { get; set; }

        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public string Message { get; set; }
    }


    /// <summary>
    /// 分页列表返回类型
    /// </summary>
    public class PagingM
    {
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private int _totalCount = 0;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; ;}
            set { _pageIndex = value; }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (_totalCount > 0)
                    return  Convert.ToInt32(Math.Ceiling((_totalCount + 0.0) / _pageSize));
                else
                    return 1;
            }
        }

        /// <summary>
        /// 数据集
        /// </summary>
        public DataTable DataTable { get; set; }
    }


}
