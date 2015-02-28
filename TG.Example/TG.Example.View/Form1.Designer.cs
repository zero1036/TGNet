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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 311);
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
    }
}

