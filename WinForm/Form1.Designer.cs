namespace WinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pb = new PictureBox();
            btn = new Button();
            flp = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pb).BeginInit();
            SuspendLayout();
            // 
            // pb
            // 
            pb.Location = new Point(12, 12);
            pb.Name = "pb";
            pb.Size = new Size(408, 409);
            pb.TabIndex = 0;
            pb.TabStop = false;
            // 
            // btn
            // 
            btn.Location = new Point(681, 378);
            btn.Name = "btn";
            btn.Size = new Size(75, 23);
            btn.TabIndex = 1;
            btn.Text = "Run Model";
            btn.UseVisualStyleBackColor = true;
            btn.Click += btn_Click;
            // 
            // flp
            // 
            flp.Location = new Point(451, 12);
            flp.Name = "flp";
            flp.Size = new Size(200, 409);
            flp.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(flp);
            Controls.Add(btn);
            Controls.Add(pb);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pb).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pb;
        private Button btn;
        private FlowLayoutPanel flp;
    }
}
