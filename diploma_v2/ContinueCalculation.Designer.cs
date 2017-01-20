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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContinueCalculation));
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
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Solve);
            this.panel1.Controls.Add(this.DFP);
            this.panel1.Controls.Add(this.Gradient);
            this.panel1.Controls.Add(this.Newton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.Tag = "Продолжить...";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Solve
            // 
            resources.ApplyResources(this.Solve, "Solve");
            this.Solve.Name = "Solve";
            this.Solve.UseVisualStyleBackColor = true;
            this.Solve.Click += new System.EventHandler(this.Solve_Click);
            // 
            // DFP
            // 
            resources.ApplyResources(this.DFP, "DFP");
            this.DFP.Name = "DFP";
            this.DFP.TabStop = true;
            this.DFP.UseVisualStyleBackColor = true;
            // 
            // Gradient
            // 
            resources.ApplyResources(this.Gradient, "Gradient");
            this.Gradient.Name = "Gradient";
            this.Gradient.TabStop = true;
            this.Gradient.UseVisualStyleBackColor = true;
            // 
            // Newton
            // 
            resources.ApplyResources(this.Newton, "Newton");
            this.Newton.Checked = true;
            this.Newton.Name = "Newton";
            this.Newton.TabStop = true;
            this.Newton.UseVisualStyleBackColor = true;
            // 
            // mustBeSaved
            // 
            resources.ApplyResources(this.mustBeSaved, "mustBeSaved");
            this.mustBeSaved.Checked = true;
            this.mustBeSaved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mustBeSaved.Name = "mustBeSaved";
            this.mustBeSaved.UseVisualStyleBackColor = true;
            // 
            // ContinueCalculation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mustBeSaved);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "ContinueCalculation";
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