using System.ComponentModel;

namespace Chat_UDP
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.b1 = new System.Windows.Forms.Button();
            this.x2 = new System.Windows.Forms.Label();
            this.x1 = new System.Windows.Forms.Label();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // b1
            // 
            this.b1.Location = new System.Drawing.Point(208, 604);
            this.b1.Margin = new System.Windows.Forms.Padding(4);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(160, 54);
            this.b1.TabIndex = 11;
            this.b1.Text = "Send";
            this.b1.UseVisualStyleBackColor = true;
            this.b1.Click += new System.EventHandler(this.b1_Click);
            // 
            // x2
            // 
            this.x2.AutoSize = true;
            this.x2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.x2.Location = new System.Drawing.Point(80, 523);
            this.x2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.x2.Name = "x2";
            this.x2.Size = new System.Drawing.Size(161, 18);
            this.x2.TabIndex = 10;
            this.x2.Text = "Enter Your Message";
            // 
            // x1
            // 
            this.x1.AutoSize = true;
            this.x1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.x1.Location = new System.Drawing.Point(153, 22);
            this.x1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.x1.Name = "x1";
            this.x1.Size = new System.Drawing.Size(215, 44);
            this.x1.TabIndex = 9;
            this.x1.Text = "WhatsUDP";
            // 
            // txt2
            // 
            this.txt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt2.Location = new System.Drawing.Point(80, 543);
            this.txt2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt2.Multiline = true;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(401, 34);
            this.txt2.TabIndex = 8;
            // 
            // txt1
            // 
            this.txt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt1.Location = new System.Drawing.Point(80, 68);
            this.txt1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt1.Multiline = true;
            this.txt1.Name = "txt1";
            this.txt1.ReadOnly = true;
            this.txt1.Size = new System.Drawing.Size(401, 418);
            this.txt1.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 681);
            this.Controls.Add(this.b1);
            this.Controls.Add(this.x2);
            this.Controls.Add(this.x1);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.txt1);
            this.Name = "Form2";
            this.Text = "Person2";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button b1;
        private System.Windows.Forms.Label x2;
        private System.Windows.Forms.TextBox txt2;

        private System.Windows.Forms.Label x1;

        private System.Windows.Forms.TextBox txt1;

        #endregion
    }
}