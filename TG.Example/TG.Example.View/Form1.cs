﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*****************************************************
* 目的：主窗
* 创建人：林子聪
* 创建时间：20141124
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TG.Example.View
{
    public partial class Form1 : Form
    {
        //private MysqlTest _MysqlTest;
        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //_MysqlTest = new MysqlTest();
        }
        /// <summary>
        /// Task Example
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            TaskFrm frm = new TaskFrm();
            frm.ShowDialog();
        }

        private void btnPLinqExampleClass1_Click(object sender, EventArgs e)
        {
            PLinqExampleClass pc = new PLinqExampleClass();
            pc.Main();
        }

        private void btnPLinqExampleClass2_Click(object sender, EventArgs e)
        {
            PLinqExampleClass2 pc = new PLinqExampleClass2();
            pc.Main();
        }

        private void btnPLinqMapreduceEC_Click(object sender, EventArgs e)
        {
            PLinqMapreduceEC pc = new PLinqMapreduceEC();
            pc.Main();
        }

        private void btnMysqlQuery_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(_MysqlTest.QueryFunc2());
        }

        private void btnMysqlQuery1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(_MysqlTest.QueryFunc());
        }

        private void btnMysqlInsert_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(_MysqlTest.InsertFunc());
        }

        private void btnProcedure_Click(object sender, EventArgs e)
        {
            //_MysqlTest.ProcedureFunc();
        }

        private void btnInsert2_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(_MysqlTest.InsertFunc2());
        }

        private void btnInsert3_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(_MysqlTest.InsertFunc3());
        }

        private void btnAutofac_Click(object sender, EventArgs e)
        {
            var au = new AutofacExampleClass();
            au.RegisterAssemblyAutofac();
        }

        private void btnReflect_Click(object sender, EventArgs e)
        {
            var au = new AutofacExampleClass();
            au.RegisterAssemblyReflection();
        }

        private void btnRedisRobMoney_Click(object sender, EventArgs e)
        {
            var ru = new RobMoney();
            ru.GoRobMoney();
        }

        private void btnAddHongbao_Click(object sender, EventArgs e)
        {
            var ru = new RobMoney();
            ru.AddMoneyToRedis();
        }

        private void btnCheckConsumed_Click(object sender, EventArgs e)
        {
            var ru = new RobMoney();
            ru.TestConsumedHongbao();
        }

        private void btnRedisHashset_Click(object sender, EventArgs e)
        {
            var ru = new RedisHash();
            ru.hset();
        }

        private void btnHashGet_Click(object sender, EventArgs e)
        {
            var ru = new RedisHash();
            ru.hget();
        }

        private void btnAsyncAndAwait_Click(object sender, EventArgs e)
        {
            var aa = new AsyncAwait();
            aa.GoFunc();
        }

        private void btnRedisStringSet_Click(object sender, EventArgs e)
        {
            var dc = new RedisString();
            dc.StringSet();
        }

        private void btnhmset_Click(object sender, EventArgs e)
        {
            var ru = new RedisHash();
            ru.hmset();
        }

        private void btnSortedSetsRank_Click(object sender, EventArgs e)
        {
            var ru = new RedisSortedSets();
            ru.ZRank();
        }

        private void btnSingleAndStatic_Click(object sender, EventArgs e)
        {
            var ss = new SingleInstanceAndStatic();
            ss.Go();
        }


    }
}
