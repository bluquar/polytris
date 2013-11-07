namespace Polytris
{
    partial class Polytris
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
            this.components = new System.ComponentModel.Container();
            this.GameTick = new System.Windows.Forms.Timer(this.components);
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GoButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.NextPieceBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextPieceBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GameTick
            // 
            this.GameTick.Interval = 20;
            this.GameTick.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // Canvas
            // 
            this.Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.MaximumSize = new System.Drawing.Size(584, 911);
            this.Canvas.MinimumSize = new System.Drawing.Size(584, 511);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(584, 711);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(737, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(39, 20);
            this.textBox1.TabIndex = 1;
            // 
            // GoButton
            // 
            this.GoButton.Location = new System.Drawing.Point(782, 9);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(40, 23);
            this.GoButton.TabIndex = 2;
            this.GoButton.Text = "Add";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(590, 9);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(141, 173);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(738, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "> Remove";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(591, 189);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(231, 39);
            this.button2.TabIndex = 6;
            this.button2.Text = "Go!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(591, 234);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(231, 39);
            this.PlayButton.TabIndex = 7;
            this.PlayButton.Text = "Play!";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(591, 276);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 427);
            this.vScrollBar1.TabIndex = 8;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // NextPieceBox
            // 
            this.NextPieceBox.Location = new System.Drawing.Point(652, 316);
            this.NextPieceBox.Name = "NextPieceBox";
            this.NextPieceBox.Size = new System.Drawing.Size(170, 112);
            this.NextPieceBox.TabIndex = 9;
            this.NextPieceBox.TabStop = false;
            this.NextPieceBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Polytris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 712);
            this.Controls.Add(this.NextPieceBox);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.GoButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Canvas);
            this.MaximumSize = new System.Drawing.Size(850, 950);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 550);
            this.Name = "Polytris";
            this.Text = "Polytris";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextPieceBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer GameTick;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.PictureBox NextPieceBox;
    }
}

