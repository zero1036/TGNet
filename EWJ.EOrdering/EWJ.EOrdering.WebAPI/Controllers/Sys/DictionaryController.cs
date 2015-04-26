using EWJ.EOrdering.BL.Sys;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Sys;
using EWJ.EOrdering.WebAPI.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWJ.EOrdering.WebAPI.Controllers.Sys
{
    public class DictionaryController : BaseCtrl
    {
        [HttpPost]
        public HttpResponseMessage GetByCode([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DictionaryModel model = new DictionaryModel();
                model = new DictionaryBL().GetByID(param.DicCode);
                response = Request.CreateResponse<DictionaryModel>(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetListByParentCode([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                IList<DictionaryVM> list = new List<DictionaryVM>();
                list = new DictionaryBL().GetList(param.ParentCode, false, SysCommon.CurrentUser.Localization);
                response = Request.CreateResponse<IList<DictionaryVM>>(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetAllListByParentCode([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                IList<DictionaryModel> list = new List<DictionaryModel>();
                list = new DictionaryBL().GetList(param.ParentCode);
                response = Request.CreateResponse<IList<DictionaryModel>>(HttpStatusCode.OK, list);

            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [HttpPost]
        public HttpResponseMessage Add([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                bool result = new DictionaryBL().Add(param.Model, SysCommon.CurrentUser.Id, DateTime.Now);
                if (result)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [HttpPost]
        public HttpResponseMessage Delete([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                bool result = new DictionaryBL().Delete(param.DicCode);
                if (result)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [HttpPost]
        public HttpResponseMessage Update([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                bool result = new DictionaryBL().Update(param.Model, SysCommon.CurrentUser.Id, DateTime.Now);
                if (result)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [HttpPost]
        public HttpResponseMessage GetNextCode([FromBody]DictonaryParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string result = new DictionaryBL().GetNextCode(param.ParentCode);
                response = Request.CreateResponse<string>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


    }
}
