﻿namespace Lottery539
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOpenDialog = new System.Windows.Forms.Button();
            this.tbLotteryType = new System.Windows.Forms.TextBox();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.lvNumResult = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.lvStatistics = new System.Windows.Forms.ListView();
            this.lvShow = new System.Windows.Forms.ListView();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbMaxDate = new System.Windows.Forms.Label();
            this.btnOpenDialog2 = new System.Windows.Forms.Button();
            this.lvNextIssue = new System.Windows.Forms.ListView();
            this.lbCount = new System.Windows.Forms.Label();
            this.lbAllCount = new System.Windows.Forms.Label();
            this.lvBenchmark = new System.Windows.Forms.ListView();
            this.btnQueryAll = new System.Windows.Forms.Button();
            this.tbPeriod2 = new System.Windows.Forms.TextBox();
            this.dtEnd2 = new System.Windows.Forms.DateTimePicker();
            this.dtStart2 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.btnQuery2 = new System.Windows.Forms.Button();
            this.btnUpdate2 = new System.Windows.Forms.Button();
            this.lvDetail2 = new System.Windows.Forms.ListView();
            this.lvResult2 = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 13F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1457, 757);
            this.tabControl1.TabIndex = 28;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1449, 726);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "冷熱表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOpenDialog);
            this.panel1.Controls.Add(this.tbLotteryType);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Controls.Add(this.dtStart);
            this.panel1.Controls.Add(this.lvNumResult);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbMin);
            this.panel1.Controls.Add(this.tbMax);
            this.panel1.Controls.Add(this.lvStatistics);
            this.panel1.Controls.Add(this.lvShow);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1443, 720);
            this.panel1.TabIndex = 0;
            // 
            // btnOpenDialog
            // 
            this.btnOpenDialog.Font = new System.Drawing.Font("新細明體", 10F);
            this.btnOpenDialog.Location = new System.Drawing.Point(1168, 686);
            this.btnOpenDialog.Name = "btnOpenDialog";
            this.btnOpenDialog.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnOpenDialog.Size = new System.Drawing.Size(111, 26);
            this.btnOpenDialog.TabIndex = 41;
            this.btnOpenDialog.Text = "手動添加期號";
            this.btnOpenDialog.UseVisualStyleBackColor = true;
            this.btnOpenDialog.Click += new System.EventHandler(this.btnOpenDialog_Click);
            // 
            // tbLotteryType
            // 
            this.tbLotteryType.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbLotteryType.Location = new System.Drawing.Point(1222, 508);
            this.tbLotteryType.MaxLength = 2;
            this.tbLotteryType.Name = "tbLotteryType";
            this.tbLotteryType.ReadOnly = true;
            this.tbLotteryType.Size = new System.Drawing.Size(212, 30);
            this.tbLotteryType.TabIndex = 40;
            // 
            // dtEnd
            // 
            this.dtEnd.CalendarFont = new System.Drawing.Font("新細明體", 14F);
            this.dtEnd.Font = new System.Drawing.Font("新細明體", 12F);
            this.dtEnd.Location = new System.Drawing.Point(1299, 467);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(135, 27);
            this.dtEnd.TabIndex = 39;
            // 
            // dtStart
            // 
            this.dtStart.CalendarFont = new System.Drawing.Font("新細明體", 14F);
            this.dtStart.Font = new System.Drawing.Font("新細明體", 12F);
            this.dtStart.Location = new System.Drawing.Point(1157, 467);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(136, 27);
            this.dtStart.TabIndex = 38;
            // 
            // lvNumResult
            // 
            this.lvNumResult.Font = new System.Drawing.Font("新細明體", 14F);
            this.lvNumResult.HideSelection = false;
            this.lvNumResult.Location = new System.Drawing.Point(-3, 467);
            this.lvNumResult.Name = "lvNumResult";
            this.lvNumResult.Size = new System.Drawing.Size(1144, 137);
            this.lvNumResult.TabIndex = 37;
            this.lvNumResult.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 14F);
            this.label3.Location = new System.Drawing.Point(1164, 613);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 19);
            this.label3.TabIndex = 36;
            this.label3.Text = "下限:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(1164, 569);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 19);
            this.label2.TabIndex = 35;
            this.label2.Text = "上限:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(1164, 512);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 19);
            this.label1.TabIndex = 34;
            this.label1.Text = "期數:";
            // 
            // tbMin
            // 
            this.tbMin.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbMin.Location = new System.Drawing.Point(1222, 610);
            this.tbMin.MaxLength = 2;
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(212, 30);
            this.tbMin.TabIndex = 33;
            // 
            // tbMax
            // 
            this.tbMax.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbMax.Location = new System.Drawing.Point(1222, 566);
            this.tbMax.MaxLength = 2;
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(212, 30);
            this.tbMax.TabIndex = 32;
            // 
            // lvStatistics
            // 
            this.lvStatistics.Font = new System.Drawing.Font("新細明體", 12F);
            this.lvStatistics.HideSelection = false;
            this.lvStatistics.Location = new System.Drawing.Point(-3, 610);
            this.lvStatistics.Name = "lvStatistics";
            this.lvStatistics.Size = new System.Drawing.Size(1144, 92);
            this.lvStatistics.TabIndex = 31;
            this.lvStatistics.UseCompatibleStateImageBehavior = false;
            // 
            // lvShow
            // 
            this.lvShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvShow.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvShow.HideSelection = false;
            this.lvShow.Location = new System.Drawing.Point(0, 0);
            this.lvShow.Name = "lvShow";
            this.lvShow.Size = new System.Drawing.Size(1443, 463);
            this.lvShow.TabIndex = 30;
            this.lvShow.UseCompatibleStateImageBehavior = false;
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnQuery.Location = new System.Drawing.Point(1311, 654);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnQuery.Size = new System.Drawing.Size(123, 63);
            this.btnQuery.TabIndex = 29;
            this.btnQuery.Text = "查詢";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("新細明體", 10F);
            this.btnUpdate.Location = new System.Drawing.Point(1168, 654);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUpdate.Size = new System.Drawing.Size(111, 26);
            this.btnUpdate.TabIndex = 28;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbMaxDate);
            this.tabPage2.Controls.Add(this.btnOpenDialog2);
            this.tabPage2.Controls.Add(this.lvNextIssue);
            this.tabPage2.Controls.Add(this.lbCount);
            this.tabPage2.Controls.Add(this.lbAllCount);
            this.tabPage2.Controls.Add(this.lvBenchmark);
            this.tabPage2.Controls.Add(this.btnQueryAll);
            this.tabPage2.Controls.Add(this.tbPeriod2);
            this.tabPage2.Controls.Add(this.dtEnd2);
            this.tabPage2.Controls.Add(this.dtStart2);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnQuery2);
            this.tabPage2.Controls.Add(this.btnUpdate2);
            this.tabPage2.Controls.Add(this.lvDetail2);
            this.tabPage2.Controls.Add(this.lvResult2);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1449, 726);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "開獎紀錄回測";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbMaxDate
            // 
            this.lbMaxDate.AutoSize = true;
            this.lbMaxDate.Font = new System.Drawing.Font("新細明體", 14F);
            this.lbMaxDate.Location = new System.Drawing.Point(340, 3);
            this.lbMaxDate.Name = "lbMaxDate";
            this.lbMaxDate.Size = new System.Drawing.Size(0, 19);
            this.lbMaxDate.TabIndex = 60;
            // 
            // btnOpenDialog2
            // 
            this.btnOpenDialog2.Font = new System.Drawing.Font("新細明體", 10F);
            this.btnOpenDialog2.Location = new System.Drawing.Point(1169, 632);
            this.btnOpenDialog2.Name = "btnOpenDialog2";
            this.btnOpenDialog2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnOpenDialog2.Size = new System.Drawing.Size(121, 26);
            this.btnOpenDialog2.TabIndex = 58;
            this.btnOpenDialog2.Text = "手動添加期號";
            this.btnOpenDialog2.UseVisualStyleBackColor = true;
            this.btnOpenDialog2.Click += new System.EventHandler(this.btnOpenDialog2_Click);
            // 
            // lvNextIssue
            // 
            this.lvNextIssue.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvNextIssue.HideSelection = false;
            this.lvNextIssue.Location = new System.Drawing.Point(769, 0);
            this.lvNextIssue.Name = "lvNextIssue";
            this.lvNextIssue.Size = new System.Drawing.Size(672, 55);
            this.lvNextIssue.TabIndex = 57;
            this.lvNextIssue.UseCompatibleStateImageBehavior = false;
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Font = new System.Drawing.Font("新細明體", 14F);
            this.lbCount.Location = new System.Drawing.Point(201, 3);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(0, 19);
            this.lbCount.TabIndex = 56;
            // 
            // lbAllCount
            // 
            this.lbAllCount.AutoSize = true;
            this.lbAllCount.Font = new System.Drawing.Font("新細明體", 14F);
            this.lbAllCount.Location = new System.Drawing.Point(-1, 3);
            this.lbAllCount.Name = "lbAllCount";
            this.lbAllCount.Size = new System.Drawing.Size(0, 19);
            this.lbAllCount.TabIndex = 55;
            // 
            // lvBenchmark
            // 
            this.lvBenchmark.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvBenchmark.HideSelection = false;
            this.lvBenchmark.Location = new System.Drawing.Point(771, 61);
            this.lvBenchmark.Name = "lvBenchmark";
            this.lvBenchmark.Size = new System.Drawing.Size(675, 408);
            this.lvBenchmark.TabIndex = 54;
            this.lvBenchmark.UseCompatibleStateImageBehavior = false;
            // 
            // btnQueryAll
            // 
            this.btnQueryAll.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnQueryAll.Location = new System.Drawing.Point(1320, 663);
            this.btnQueryAll.Name = "btnQueryAll";
            this.btnQueryAll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnQueryAll.Size = new System.Drawing.Size(123, 57);
            this.btnQueryAll.TabIndex = 53;
            this.btnQueryAll.Text = "查詢全部";
            this.btnQueryAll.UseVisualStyleBackColor = true;
            this.btnQueryAll.Click += new System.EventHandler(this.btnQueryAll_Click);
            // 
            // tbPeriod2
            // 
            this.tbPeriod2.Font = new System.Drawing.Font("新細明體", 14F);
            this.tbPeriod2.Location = new System.Drawing.Point(1223, 539);
            this.tbPeriod2.MaxLength = 2;
            this.tbPeriod2.Name = "tbPeriod2";
            this.tbPeriod2.Size = new System.Drawing.Size(123, 30);
            this.tbPeriod2.TabIndex = 50;
            // 
            // dtEnd2
            // 
            this.dtEnd2.CalendarFont = new System.Drawing.Font("新細明體", 14F);
            this.dtEnd2.Font = new System.Drawing.Font("新細明體", 12F);
            this.dtEnd2.Location = new System.Drawing.Point(1314, 492);
            this.dtEnd2.Name = "dtEnd2";
            this.dtEnd2.Size = new System.Drawing.Size(135, 27);
            this.dtEnd2.TabIndex = 49;
            this.dtEnd2.ValueChanged += new System.EventHandler(this.dtEnd2_ValueChanged);
            // 
            // dtStart2
            // 
            this.dtStart2.CalendarFont = new System.Drawing.Font("新細明體", 14F);
            this.dtStart2.Font = new System.Drawing.Font("新細明體", 12F);
            this.dtStart2.Location = new System.Drawing.Point(1169, 492);
            this.dtStart2.Name = "dtStart2";
            this.dtStart2.Size = new System.Drawing.Size(136, 27);
            this.dtStart2.TabIndex = 48;
            this.dtStart2.ValueChanged += new System.EventHandler(this.dtStart2_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 14F);
            this.label6.Location = new System.Drawing.Point(1165, 542);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 19);
            this.label6.TabIndex = 45;
            this.label6.Text = "期數:";
            // 
            // btnQuery2
            // 
            this.btnQuery2.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnQuery2.Location = new System.Drawing.Point(1320, 588);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnQuery2.Size = new System.Drawing.Size(123, 59);
            this.btnQuery2.TabIndex = 42;
            this.btnQuery2.Text = "查詢";
            this.btnQuery2.UseVisualStyleBackColor = true;
            this.btnQuery2.Click += new System.EventHandler(this.btnQuery2_Click);
            // 
            // btnUpdate2
            // 
            this.btnUpdate2.Font = new System.Drawing.Font("新細明體", 10F);
            this.btnUpdate2.Location = new System.Drawing.Point(1169, 588);
            this.btnUpdate2.Name = "btnUpdate2";
            this.btnUpdate2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUpdate2.Size = new System.Drawing.Size(121, 26);
            this.btnUpdate2.TabIndex = 41;
            this.btnUpdate2.Text = "更新";
            this.btnUpdate2.UseVisualStyleBackColor = true;
            this.btnUpdate2.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lvDetail2
            // 
            this.lvDetail2.Font = new System.Drawing.Font("新細明體", 14F);
            this.lvDetail2.HideSelection = false;
            this.lvDetail2.Location = new System.Drawing.Point(3, 475);
            this.lvDetail2.Name = "lvDetail2";
            this.lvDetail2.Size = new System.Drawing.Size(1152, 248);
            this.lvDetail2.TabIndex = 32;
            this.lvDetail2.UseCompatibleStateImageBehavior = false;
            // 
            // lvResult2
            // 
            this.lvResult2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lvResult2.HideSelection = false;
            this.lvResult2.Location = new System.Drawing.Point(0, 39);
            this.lvResult2.Name = "lvResult2";
            this.lvResult2.Size = new System.Drawing.Size(765, 430);
            this.lvResult2.TabIndex = 31;
            this.lvResult2.UseCompatibleStateImageBehavior = false;
            this.lvResult2.SelectedIndexChanged += new System.EventHandler(this.lvResult2_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1457, 757);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "539";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbLotteryType;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.ListView lvNumResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.ListView lvStatistics;
        private System.Windows.Forms.ListView lvShow;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView lvResult2;
        private System.Windows.Forms.TextBox tbPeriod2;
        private System.Windows.Forms.DateTimePicker dtEnd2;
        private System.Windows.Forms.DateTimePicker dtStart2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnQuery2;
        private System.Windows.Forms.Button btnUpdate2;
        private System.Windows.Forms.ListView lvDetail2;
        private System.Windows.Forms.Button btnQueryAll;
        private System.Windows.Forms.ListView lvBenchmark;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Label lbAllCount;
        private System.Windows.Forms.ListView lvNextIssue;
        private System.Windows.Forms.Button btnOpenDialog;
        private System.Windows.Forms.Button btnOpenDialog2;
        private System.Windows.Forms.Label lbMaxDate;
    }
}

