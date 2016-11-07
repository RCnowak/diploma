namespace diploma_v2
{
    partial class ContinueCalculation
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.Solve = new System.Windows.Forms.Button();
            this.DFP = new System.Windows.Forms.RadioButton();
            this.Gradient = new System.Windows.Forms.RadioButton();
            this.Newton = new System.Windows.Forms.RadioButton();
            this.mustBeSaved = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Результат вычислений";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 42);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(323, 198);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(128, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 37);
            this.button1.TabIndex = 2;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Solve);
            this.panel1.Controls.Add(this.DFP);
            this.panel1.Controls.Add(this.Gradient);
            this.panel1.Controls.Add(this.Newton);
            this.panel1.Location = new System.Drawing.Point(16, 278);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 142);
            this.panel1.TabIndex = 3;
            this.panel1.Tag = "Продолжить...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Продолжить, используя метод";
            // 
            // Solve
            // 
            this.Solve.Location = new System.Drawing.Point(228, 67);
            this.Solve.Name = "Solve";
            this.Solve.Size = new System.Drawing.Size(75, 23);
            this.Solve.TabIndex = 3;
            this.Solve.Text = "Решить";
            this.Solve.UseVisualStyleBackColor = true;
            this.Solve.Click += new System.EventHandler(this.Solve_Click);
            // 
            // DFP
            // 
            this.DFP.AutoSize = true;
            this.DFP.Location = new System.Drawing.Point(18, 111);
            this.DFP.Name = "DFP";
            this.DFP.Size = new System.Drawing.Size(81, 17);
            this.DFP.TabIndex = 2;
            this.DFP.TabStop = true;
            this.DFP.Text = "Метод DFP";
            this.DFP.UseVisualStyleBackColor = true;
            // 
            // Gradient
            // 
            this.Gradient.AutoSize = true;
            this.Gradient.Location = new System.Drawing.Point(18, 73);
            this.Gradient.Name = "Gradient";
            this.Gradient.Size = new System.Drawing.Size(167, 17);
            this.Gradient.TabIndex = 1;
            this.Gradient.TabStop = true;
            this.Gradient.Text = "Метод градиентного спуска";
            this.Gradient.UseVisualStyleBackColor = true;
            // 
            // Newton
            // 
            this.Newton.AutoSize = true;
            this.Newton.Checked = true;
            this.Newton.Location = new System.Drawing.Point(18, 35);
            this.Newton.Name = "Newton";
            this.Newton.Size = new System.Drawing.Size(105, 17);
            this.Newton.TabIndex = 0;
            this.Newton.TabStop = true;
            this.Newton.Text = "Метод Ньютона";
            this.Newton.UseVisualStyleBackColor = true;
            // 
            // mustBeSaved
            // 
            this.mustBeSaved.AutoSize = true;
            this.mustBeSaved.Checked = true;
            this.mustBeSaved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mustBeSaved.Location = new System.Drawing.Point(16, 246);
            this.mustBeSaved.Name = "mustBeSaved";
            this.mustBeSaved.Size = new System.Drawing.Size(141, 17);
            this.mustBeSaved.TabIndex = 4;
            this.mustBeSaved.Text = "Сохранить результаты";
            this.mustBeSaved.UseVisualStyleBackColor = true;
            // 
            // ContinueCalculation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 492);
            this.Controls.Add(this.mustBeSaved);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "ContinueCalculation";
            this.Text = "Выберете действие";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Solve;
        private System.Windows.Forms.RadioButton DFP;
        private System.Windows.Forms.RadioButton Gradient;
        private System.Windows.Forms.RadioButton Newton;
        private System.Windows.Forms.CheckBox mustBeSaved;
    }
}