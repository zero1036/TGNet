using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Utility.DBCommon.dao;
using System.Data;


namespace UnitTest.DBCommon
{
    [TestClass]
    public class AOPTest
    {
        [TestMethod]
        public void AOPConnectTest()
        {
            DataConsolidationBL dc = TransactionAOP.newInstance(typeof(DataConsolidationBL)) as DataConsolidationBL;
            dc.test();
        }
    }

    public class DataConsolidationBL
    {
        private ADOTemplate template = null;
        public DataConsolidationBL()
        {
            template = new ADOTemplate();
        }



        public virtual DataTable test()
        {
            DataTable dt = template.Query("select * from DataConsolidation;", null, null);
            return dt;
        }

    }
}
