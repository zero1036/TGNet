using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TS.Base
{
    public partial class Form1 : Form
    {
        protected Form1ChildClass m_Form1ChildClass;
        protected LogInfo<Control, RichTextBox> m_log;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_log = new LogInfo<Control, RichTextBox>(this, this.richTextBox1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            m_Form1ChildClass = new Form1ChildClass();
            //同步调用
            //m_Form1ChildClass.m_delegate1 = m_log.WriteOutputInfo;
            //m_Form1ChildClass.function();

            //异步调用
            m_Form1ChildClass.function_Asyn();
            m_Form1ChildClass.m_delegate1.BeginInvoke("tgor", new AsyncCallback(CallBack), null);
        }
        //回调
        public void CallBack(IAsyncResult iAsyncResult)
        {
            m_log.WriteOutputInfo(m_Form1ChildClass.m_delegate1.EndInvoke(iAsyncResult));
            System.Diagnostics.Debug.WriteLine("完成异步调用");
        }


    }

    public class LogInfo<T, K>
    {
        private T m_objBase;
        private K m_textBox;

        public static LogInfo<T, K> Instance = new LogInfo<T, K>();

        public LogInfo()
        {

        }
        public LogInfo(T objBase, K textBox1)
        {
            m_objBase = objBase;
            m_textBox = textBox1;
        }

        protected delegate string SetTextCallback(string str);
        public string WriteOutputInfo(string str)
        {
            Control ctlText = (m_textBox as Control);
            Control ctlBase = (m_objBase as Control);

            if (ctlText.InvokeRequired)
            {
                SetTextCallback dback = new SetTextCallback(WriteOutputInfo);

                if (ctlBase is Control)
                {
                    ctlBase.Invoke(dback, new object[] { str });
                }
            }
            else
            {
                ctlText.Text = str;
            }
            return string.Empty;
        }
    }

    public class Form1ChildClass
    {
        public delegate string delegateMethod1(string str);
        public delegateMethod1 m_delegate1;

        public Form1ChildClass()
        {

        }
        public void function()
        {
            //直接同步调用情况，持久执行，主界面会有卡顿情况
            m_delegate1(delegateMethod1_Function("tgor"));
        }

        public void function_Asyn()
        {
            //异步调用情况，持久执行，主界面不会有卡顿情况
            m_delegate1 = new delegateMethod1(delegateMethod1_Function);
        }

        //实际处理函数
        public string delegateMethod1_Function(string str)
        {
            Thread.Sleep(1000);
            return str + " OK";
        }



    }


}
