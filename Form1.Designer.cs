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
            this.cbLotteryType = new System.Windows.Forms.ComboBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lvNumResult = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("新細明體", 10F);
            this.btnUpdate.Location = new System.Drawing.Point(1267, 647);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUpdate.Size = new System.Drawing.Size(59, 26);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnQuery.Location = new System.Drawing.Point(1332, 680);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnQuery.Size = new System.Drawing.Size(123, 38);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "查詢";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lvShow
            // 
            this.lvShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvShow.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvShow.HideSelection = false;
            this.lvShow.Location = new System.Drawing.Point(0, 0);
            this.lvShow.Name = "lvShow";
            this.lvShow.Size = new System.Drawing.Size(1489, 501);
            this.lvShow.TabIndex = 16;
            this.lvShow.UseCompatibleStateImageBehavior = false;
            // 
            // lvStatistics
            // 
            this.lvStatistics.Font = new System.Drawing.Font("新細明體", 12F);
            this.lvStatistics.HideSelection = false;
            this.lvStatistics.Location = new System.Drawing.Point(0, 625);
            this.lvStatistics.Name = "lvStatistics";
            this.lvStatistics.Size = new System.Drawing.Size(1261, 92);
            this.lvStatistics.TabIndex = 17;
            this.lvStatistics.UseCompatibleStateImageBehavior = false;
            // 
            // cbLotteryType
            // 
            this.cbLotteryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLotteryType.Font = new System.Drawing.Font("新細明體", 14F);
            this.cbLotteryType.FormattingEnabled = true;
            this.cbLotteryType.Location = new System.Drawing.Point(1332, 511);
            this.cbLotteryType.Margin = new System.Windows.Forms.Padding(3, 3, 30, 30);
            this.cbLotteryType.Name = "cbLotteryType";
            this.cbLotteryType.Size = new System.Drawing.Size(123, 27);
            this.cbLotteryType.TabIndex = 18;
            this.cbLotteryType.SelectedIndexChanged += new System.EventHandler(this.cbLotteryType_SelectedIndexChanged);
            // 
            // tbMax
            // 
            this.tbMax.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbMax.Location = new System.Drawing.Point(1332, 568);
            this.tbMax.MaxLength = 2;
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(123, 30);
            this.tbMax.TabIndex = 19;
            this.tbMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMax_KeyPress);
            // 
            // tbMin
            // 
            this.tbMin.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbMin.Location = new System.Drawing.Point(1332, 612);
            this.tbMin.MaxLength = 2;
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(123, 30);
            this.tbMin.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 14F);
            this.label3.Location = new System.Drawing.Point(1274, 615);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 19);
            this.label3.TabIndex = 23;
            this.label3.Text = "下限:";
            // 
            // lvNumResult
            // 
            this.lvNumResult.Font = new System.Drawing.Font("新細明體", 14F);
            this.lvNumResult.HideSelection = false;
            this.lvNumResult.Location = new System.Drawing.Point(0, 510);
            this.lvNumResult.Name = "lvNumResult";
            this.lvNumResult.Size = new System.Drawing.Size(1261, 109);
            this.lvNumResult.TabIndex = 24;
            this.lvNumResult.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(1274, 514);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 19);
            this.label1.TabIndex = 21;
            this.label1.Text = "期數:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(1274, 571);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 19);
            this.label2.TabIndex = 22;
            this.label2.Text = "上限:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 719);
            this.Controls.Add(this.lvNumResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbMin);
            this.Controls.Add(this.tbMax);
            this.Controls.Add(this.cbLotteryType);
            this.Controls.Add(this.lvStatistics);
            this.Controls.Add(this.lvShow);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnUpdate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ListView lvShow;
        private System.Windows.Forms.ListView lvStatistics;
        private System.Windows.Forms.ComboBox cbLotteryType;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvNumResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

