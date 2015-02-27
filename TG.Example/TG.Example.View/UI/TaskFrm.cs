using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*****************************************************
* 目的：Task Example
* 创建人：林子聪
* 创建时间：
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TG.Example.View
{
    public partial class TaskFrm : Form
    {
        private TaskExampleClass pTestClass;
        /// <summary>
        /// 
        /// </summary>
        public TaskFrm()
        {
            InitializeComponent();
            pTestClass = new TaskExampleClass();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnStart_Click(object sender, EventArgs e)
        {
            long sta = DateTime.Now.Ticks;

            #region Parallel分区实例
            //分区for循环
            //pTestClass.Parallel_For_Local_Test();
            //pTestClass.Parallel_For_Local_Test2();
            //单线for循环
            //pTestClass.For_Local_Test();
            #endregion

            #region Task and Task.Factory
            //pTestClass.TaskFunB();
            //pTestClass.TaskFunC();
            //pTestClass.TaskFunD();
            //pTestClass.TaskFunE();
            //pTestClass.TaskFunF();
            pTestClass.TaskFunG();
            #endregion

            long ennd = DateTime.Now.Ticks;
            System.Diagnostics.Debug.WriteLine((ennd - sta) / 10000);
        }
    }
}
