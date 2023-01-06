namespace Lottery539
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.lvShow = new System.Windows.Forms.ListView();
            this.lvStatistics = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Font = new System.Drawing.Font("新細明體", 8F);
            this.btnUpdate.Location = new System.Drawing.Point(918, 536);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUpdate.Size = new System.Drawing.Size(65, 26);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnQuery.Location = new System.Drawing.Point(1021, 552);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnQuery.Size = new System.Drawing.Size(92, 32);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "查詢";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // lvShow
            // 
            this.lvShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvShow.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvShow.HideSelection = false;
            this.lvShow.Location = new System.Drawing.Point(0, 0);
            this.lvShow.Name = "lvShow";
            this.lvShow.Size = new System.Drawing.Size(1125, 363);
            this.lvShow.TabIndex = 16;
            this.lvShow.UseCompatibleStateImageBehavior = false;
            // 
            // lvStatistics
            // 
            this.lvStatistics.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvStatistics.Font = new System.Drawing.Font("新細明體", 12F);
            this.lvStatistics.HideSelection = false;
            this.lvStatistics.Location = new System.Drawing.Point(0, 363);
            this.lvStatistics.Name = "lvStatistics";
            this.lvStatistics.Size = new System.Drawing.Size(869, 264);
            this.lvStatistics.TabIndex = 17;
            this.lvStatistics.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 627);
            this.Controls.Add(this.lvStatistics);
            this.Controls.Add(this.lvShow);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnUpdate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ListView lvShow;
        private System.Windows.Forms.ListView lvStatistics;
    }
}

