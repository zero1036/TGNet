using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Model;
using System.Data;
using System.Collections;
namespace EG.WeChat.Platform.DA.QYMaiListDA
{
    public class QYDepartmentDA:DataBase
    {
         private ADOTemplateX template = new ADOTemplateX();
         public DataTable GetAllDepartmentData()
         {
            return  template.Query("select * from T_QY_Department where DeleteDate is null",null,null,null);
         }

         public DataTable GetDepartmentDataByWxId(string DepartmentWxID)
         {

             return template.Query("select * from T_QY_Department where DeleteDate is null and DepartmentID=@DepartmentID", new string[] {"@DepartmentID"},
                 new string[] {DepartmentWxID });

         }

         public DataTable GetDepartmentDataByPKId(string ID)
         {

             return template.Query("select * from T_QY_Department where DeleteDate is null and ID=@ID", new string[] { "@ID" },
                 new string[] { ID },null);

         }

         public DataTable GetDepartmentDataByParentWxId(string DepartmentWxID)
         {

             return template.Query("select * from T_QY_Department where DeleteDate is null and ParentDepartmentID =@DepartmentWxID",
                 new string[] { "@DepartmentWxID" },
                 new string[] { DepartmentWxID },null);

         }

         public bool AddDepartment(string DepartmentID,string ParentDepartmentID,string Name,string wcOrder,string CreateBy)
         {
             try
             {
                 string sql = @"insert into T_QY_Department(DepartmentID,ParentDepartmentID,Name,wcOrder,CreateBy) 
                            values(@DepartmentID,@ParentDepartmentID,@Name,@wcOrder,@CreateBy)";
                 return template.Execute(sql, new string[] { "@DepartmentID", "@ParentDepartmentID", "@Name", "@wcOrder", "@CreateBy" },
                                       new string[] { DepartmentID, ParentDepartmentID, Name, wcOrder, CreateBy }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY insert T_QY_Department error:", e);
             }
             return false;
         }

         public bool DeleteDepartmentByPkID(string ID, string DeleteBy)
         {
             try
             {
                 string sql = @"update T_QY_Department set DeleteBy=@DeleteBy,DeleteDate=GETDATE() where ID=@ID";
                 return template.Execute(sql, new string[] { "@DeleteBy", "@ID" },
                                       new string[] { DeleteBy, ID }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY delete T_QY_Department by pkid error:", e);
             }
             return false;
         }

         public bool DeleteDepartmentByWXID(string DepartmentID, string DeleteBy)
         {
             try
             {
                 string sql = @"update T_QY_Department set DeleteBy=@DeleteBy,DeleteDate=GETDATE() where DepartmentID =@DepartmentID";
                 return template.Execute(sql, new string[] { "@DeleteBy", "@DepartmentID" },
                                       new string[] { DeleteBy, DepartmentID }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY delete T_QY_Department by wxid error:", e);
             }
             return false;
         }

         public bool DeleteDepartmentByWxID(string DepartmentID, string DeleteBy)
         {
             try
             {
                 string sql = @"update T_QY_Department set DeleteBy=@DeleteBy,DeleteDate=GETDATE() where DepartmentID =@DepartmentID";
                 return template.Execute(sql, new string[] { "@DeleteBy", "@DepartmentID" },
                                       new string[] { DeleteBy, DepartmentID }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY delete T_QY_Department error:", e);
             }
             return false;
         }

         public bool UpdateDepartmentByPkID(string ID, string ParentDepartmentID, string Name, string wcOrder, string UpdateBy)
         {
             try
             {
                 string sql = @"update T_QY_Department set ParentDepartmentID=@ParentDepartmentID,Name=@Name,wcOrder=@wcOrder,
                                UpdateDate=GETDATE(),UpdateBy=@UpdateBy where ID=@ID";
                 return template.Execute(sql, new string[] { "@ParentDepartmentID", "@Name", "@wcOrder","@UpdateBy","@ID" },
                                       new string[] { ParentDepartmentID, Name, wcOrder, UpdateBy, ID }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY delete T_QY_Department error:", e);
             }
             return false;
         }

         public bool UpdateDepartmentByWxID(string DepartmentID, string ParentDepartmentID, string Name, string wcOrder, string UpdateBy)
         {
             try
             {
                 string sql = @"update T_QY_Department set ParentDepartmentID=@ParentDepartmentID,Name=@Name,wcOrder=@wcOrder,
                                UpdateDate=GETDATE(),UpdateBy=@UpdateBy where DepartmentID=@DepartmentID";
                 return template.Execute(sql, new string[] { "@ParentDepartmentID", "@Name", "@wcOrder", "@UpdateBy", "@DepartmentID" },
                                       new string[] { ParentDepartmentID, Name, wcOrder, UpdateBy, DepartmentID }, null) == 1;
             }
             catch (Exception e)
             {
                 Logger.Log4Net.Error("QY delete T_QY_Department error:", e);
             }
             return false;
         }

         

    }
}
