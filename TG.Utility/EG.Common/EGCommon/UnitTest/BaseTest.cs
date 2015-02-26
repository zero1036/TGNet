using EG.Business.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EG.UnitTest
{
    public class BaseTest
    {
        protected string dataPath;

        private TestContext m_testContext;

        public TestContext TestContext
        {
            get { return m_testContext; }
            set { m_testContext = value; }
        }


        [TestInitialize()]
        public void DataInit()
        {
            BeforeDataInit();
            ConfigCache.LoadAppConfig(null);

            string testName = TestContext.TestName;
            FileInfo fileInfo = new FileInfo(dataPath + testName + ".xls");

            if (fileInfo.Exists)
            {
                ExcelParser parser = new ExcelParser();
                parser.Parse(fileInfo.FullName);
            }

            AfterDataInit();
        }
        
        public virtual void BeforeDataInit(){}
        public virtual void AfterDataInit(){}
    }
}
