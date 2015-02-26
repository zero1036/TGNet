using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TG.Example
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public delegate void Delegate1();
        public delegate void Delegate2(DataTable dt);
        public void buttonFind2_Click(object sender, EventArgs e)
        {
            Delegate1 d1 = new Delegate1(Find);
            d1.BeginInvoke(new AsyncCallback(AsyncCallback1), d1);
        }
        public void AsyncCallback1(IAsyncResult iAsyncResult)
        {
            Delegate1 d1 = (Delegate1)iAsyncResult.AsyncState;
            d1.EndInvoke(iAsyncResult);
        }
        public void Find()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("age", typeof(int));


            for (int i = 1; i <= 10000000; i++)
            {
                AddRow(dt, string.Format("张{0}", i), i);
            }

            //AddRow(dt, "张三", 19);
            //AddRow(dt, "李四", 18);
            this.Invoke(new Delegate2(Bind2), new object[] { dt });
        }
        public void AddRow(DataTable dt, string name, int age)
        {
            DataRow dr = dt.NewRow();
            dr["name"] = name;
            dr["age"] = age;
            dt.Rows.Add(dr);
        }
        public void Bind2(DataTable dt)
        {
            this.dataGridView1.DataSource = dt;
        }


    }
}
