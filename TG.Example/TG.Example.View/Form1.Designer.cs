namespace TG.Example.View
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnPLinqExampleClass1 = new System.Windows.Forms.Button();
            this.btnPLinqExampleClass2 = new System.Windows.Forms.Button();
            this.btnPLinqMapreduceEC = new System.Windows.Forms.Button();
            this.btnMysqlQuery = new System.Windows.Forms.Button();
            this.btnMysqlQuery1 = new System.Windows.Forms.Button();
            this.btnMysqlInsert = new System.Windows.Forms.Button();
            this.btnProcedure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(161, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Task Example";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPLinqExampleClass1
            // 
            this.btnPLinqExampleClass1.Location = new System.Drawing.Point(179, 12);
            this.btnPLinqExampleClass1.Name = "btnPLinqExampleClass1";
            this.btnPLinqExampleClass1.Size = new System.Drawing.Size(161, 32);
            this.btnPLinqExampleClass1.TabIndex = 1;
            this.btnPLinqExampleClass1.Text = "PLinqExampleClass1";
            this.btnPLinqExampleClass1.UseVisualStyleBackColor = true;
            this.btnPLinqExampleClass1.Click += new System.EventHandler(this.btnPLinqExampleClass1_Click);
            // 
            // btnPLinqExampleClass2
            // 
            this.btnPLinqExampleClass2.Location = new System.Drawing.Point(346, 12);
            this.btnPLinqExampleClass2.Name = "btnPLinqExampleClass2";
            this.btnPLinqExampleClass2.Size = new System.Drawing.Size(161, 32);
            this.btnPLinqExampleClass2.TabIndex = 2;
            this.btnPLinqExampleClass2.Text = "PLinqExampleClass2";
            this.btnPLinqExampleClass2.UseVisualStyleBackColor = true;
            this.btnPLinqExampleClass2.Click += new System.EventHandler(this.btnPLinqExampleClass2_Click);
            // 
            // btnPLinqMapreduceEC
            // 
            this.btnPLinqMapreduceEC.Location = new System.Drawing.Point(179, 50);
            this.btnPLinqMapreduceEC.Name = "btnPLinqMapreduceEC";
            this.btnPLinqMapreduceEC.Size = new System.Drawing.Size(161, 32);
            this.btnPLinqMapreduceEC.TabIndex = 3;
            this.btnPLinqMapreduceEC.Text = "PLinqMapreduceEC";
            this.btnPLinqMapreduceEC.UseVisualStyleBackColor = true;
            this.btnPLinqMapreduceEC.Click += new System.EventHandler(this.btnPLinqMapreduceEC_Click);
            // 
            // btnMysqlQuery
            // 
            this.btnMysqlQuery.Location = new System.Drawing.Point(179, 88);
            this.btnMysqlQuery.Name = "btnMysqlQuery";
            this.btnMysqlQuery.Size = new System.Drawing.Size(161, 32);
            this.btnMysqlQuery.TabIndex = 4;
            this.btnMysqlQuery.Text = "Mysql关联查询";
            this.btnMysqlQuery.UseVisualStyleBackColor = true;
            this.btnMysqlQuery.Click += new System.EventHandler(this.btnMysqlQuery_Click);
            // 
            // btnMysqlQuery1
            // 
            this.btnMysqlQuery1.Location = new System.Drawing.Point(12, 88);
            this.btnMysqlQuery1.Name = "btnMysqlQuery1";
            this.btnMysqlQuery1.Size = new System.Drawing.Size(161, 32);
            this.btnMysqlQuery1.TabIndex = 5;
            this.btnMysqlQuery1.Text = "Mysql简单查询";
            this.btnMysqlQuery1.UseVisualStyleBackColor = true;
            this.btnMysqlQuery1.Click += new System.EventHandler(this.btnMysqlQuery1_Click);
            // 
            // btnMysqlInsert
            // 
            this.btnMysqlInsert.Location = new System.Drawing.Point(346, 88);
            this.btnMysqlInsert.Name = "btnMysqlInsert";
            this.btnMysqlInsert.Size = new System.Drawing.Size(161, 32);
            this.btnMysqlInsert.TabIndex = 6;
            this.btnMysqlInsert.Text = "Mysql插入";
            this.btnMysqlInsert.UseVisualStyleBackColor = true;
            this.btnMysqlInsert.Click += new System.EventHandler(this.btnMysqlInsert_Click);
            // 
            // btnProcedure
            // 
            this.btnProcedure.Location = new System.Drawing.Point(12, 126);
            this.btnProcedure.Name = "btnProcedure";
            this.btnProcedure.Size = new System.Drawing.Size(161, 32);
            this.btnProcedure.TabIndex = 7;
            this.btnProcedure.Text = "Mysql存储过程";
            this.btnProcedure.UseVisualStyleBackColor = true;
            this.btnProcedure.Click += new System.EventHandler(this.btnProcedure_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 311);
            this.Controls.Add(this.btnProcedure);
            this.Controls.Add(this.btnMysqlInsert);
            this.Controls.Add(this.btnMysqlQuery1);
            this.Controls.Add(this.btnMysqlQuery);
            this.Controls.Add(this.btnPLinqMapreduceEC);
            this.Controls.Add(this.btnPLinqExampleClass2);
            this.Controls.Add(this.btnPLinqExampleClass1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Dotnet Example";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPLinqExampleClass1;
        private System.Windows.Forms.Button btnPLinqExampleClass2;
        private System.Windows.Forms.Button btnPLinqMapreduceEC;
        private System.Windows.Forms.Button btnMysqlQuery;
        private System.Windows.Forms.Button btnMysqlQuery1;
        private System.Windows.Forms.Button btnMysqlInsert;
        private System.Windows.Forms.Button btnProcedure;
    }
}

