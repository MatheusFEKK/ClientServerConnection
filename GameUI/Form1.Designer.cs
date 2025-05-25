namespace GameUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Nickname1 = new System.Windows.Forms.Label();
            this.Nickname2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Nickname2);
            this.panel1.Controls.Add(this.Nickname1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // Nickname1
            // 
            this.Nickname1.AutoSize = true;
            this.Nickname1.Location = new System.Drawing.Point(47, 33);
            this.Nickname1.Name = "Nickname1";
            this.Nickname1.Size = new System.Drawing.Size(35, 13);
            this.Nickname1.TabIndex = 0;
            this.Nickname1.Text = "label1";
            this.Nickname1.Click += new System.EventHandler(this.Nickname1_Click);
            // 
            // Nickname2
            // 
            this.Nickname2.AutoSize = true;
            this.Nickname2.Location = new System.Drawing.Point(704, 33);
            this.Nickname2.Name = "Nickname2";
            this.Nickname2.Size = new System.Drawing.Size(35, 13);
            this.Nickname2.TabIndex = 1;
            this.Nickname2.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Nickname1;
        private System.Windows.Forms.Label Nickname2;
    }
}

