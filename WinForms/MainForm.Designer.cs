﻿namespace WinForms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.startrecord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startrecord
            // 
            this.startrecord.Location = new System.Drawing.Point(69, 28);
            this.startrecord.Name = "startrecord";
            this.startrecord.Size = new System.Drawing.Size(75, 23);
            this.startrecord.TabIndex = 0;
            this.startrecord.Text = "开始识别";
            this.startrecord.UseVisualStyleBackColor = true;
            this.startrecord.Click += new System.EventHandler(this.startrecord_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 84);
            this.Controls.Add(this.startrecord);
            this.Name = "MainForm";
            this.Text = "语音辅助识别";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startrecord;
    }
}

