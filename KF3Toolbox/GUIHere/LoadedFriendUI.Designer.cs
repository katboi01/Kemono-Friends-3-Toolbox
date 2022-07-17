
partial class LoadedFriendUI
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadedFriendUI));
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.cosDefault = new System.Windows.Forms.TabPage();
        this.cosSpecial = new System.Windows.Forms.TabPage();
        this.cosSport = new System.Windows.Forms.TabPage();
        this.cosPark = new System.Windows.Forms.TabPage();
        this.cosMaid = new System.Windows.Forms.TabPage();
        this.levelLabel = new System.Windows.Forms.Label();
        this.levelTrackBar = new System.Windows.Forms.TrackBar();
        this.wrTrackBar = new System.Windows.Forms.TrackBar();
        this.wrLabel = new System.Windows.Forms.Label();
        this.tabControl2 = new System.Windows.Forms.TabControl();
        this.statsTabPage = new System.Windows.Forms.TabPage();
        this.infoTabPage = new System.Windows.Forms.TabPage();
        this.hpLabel = new System.Windows.Forms.Label();
        this.atkLabel = new System.Windows.Forms.Label();
        this.defLabel = new System.Windows.Forms.Label();
        this.statusLabel = new System.Windows.Forms.Label();
        this.starsTrackBar = new System.Windows.Forms.TrackBar();
        this.starsLabel = new System.Windows.Forms.Label();
        this.tabControl1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.levelTrackBar)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wrTrackBar)).BeginInit();
        this.tabControl2.SuspendLayout();
        this.statsTabPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.starsTrackBar)).BeginInit();
        this.SuspendLayout();
        // 
        // tabControl1
        // 
        this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
        this.tabControl1.Controls.Add(this.cosDefault);
        this.tabControl1.Controls.Add(this.cosSpecial);
        this.tabControl1.Controls.Add(this.cosSport);
        this.tabControl1.Controls.Add(this.cosPark);
        this.tabControl1.Controls.Add(this.cosMaid);
        this.tabControl1.ItemSize = new System.Drawing.Size(10, 20);
        this.tabControl1.Location = new System.Drawing.Point(12, 12);
        this.tabControl1.Multiline = true;
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(189, 564);
        this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
        this.tabControl1.TabIndex = 0;
        // 
        // cosDefault
        // 
        this.cosDefault.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cosDefault.BackgroundImage")));
        this.cosDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.cosDefault.Location = new System.Drawing.Point(4, 47);
        this.cosDefault.Name = "cosDefault";
        this.cosDefault.Padding = new System.Windows.Forms.Padding(3);
        this.cosDefault.Size = new System.Drawing.Size(181, 513);
        this.cosDefault.TabIndex = 0;
        this.cosDefault.Text = "default";
        this.cosDefault.UseVisualStyleBackColor = true;
        // 
        // cosSpecial
        // 
        this.cosSpecial.Location = new System.Drawing.Point(4, 47);
        this.cosSpecial.Name = "cosSpecial";
        this.cosSpecial.Size = new System.Drawing.Size(181, 513);
        this.cosSpecial.TabIndex = 4;
        this.cosSpecial.Text = "special";
        this.cosSpecial.UseVisualStyleBackColor = true;
        // 
        // cosSport
        // 
        this.cosSport.Location = new System.Drawing.Point(4, 47);
        this.cosSport.Name = "cosSport";
        this.cosSport.Padding = new System.Windows.Forms.Padding(3);
        this.cosSport.Size = new System.Drawing.Size(181, 513);
        this.cosSport.TabIndex = 1;
        this.cosSport.Text = "tracksuit";
        this.cosSport.UseVisualStyleBackColor = true;
        // 
        // cosPark
        // 
        this.cosPark.Location = new System.Drawing.Point(4, 47);
        this.cosPark.Name = "cosPark";
        this.cosPark.Size = new System.Drawing.Size(181, 513);
        this.cosPark.TabIndex = 2;
        this.cosPark.Text = "park staff";
        this.cosPark.UseVisualStyleBackColor = true;
        // 
        // cosMaid
        // 
        this.cosMaid.Location = new System.Drawing.Point(4, 47);
        this.cosMaid.Name = "cosMaid";
        this.cosMaid.Size = new System.Drawing.Size(181, 513);
        this.cosMaid.TabIndex = 3;
        this.cosMaid.Text = "maid";
        this.cosMaid.UseVisualStyleBackColor = true;
        // 
        // levelLabel
        // 
        this.levelLabel.AutoSize = true;
        this.levelLabel.Location = new System.Drawing.Point(84, 104);
        this.levelLabel.Name = "levelLabel";
        this.levelLabel.Size = new System.Drawing.Size(48, 13);
        this.levelLabel.TabIndex = 1;
        this.levelLabel.Text = "Level 01";
        // 
        // levelTrackBar
        // 
        this.levelTrackBar.Location = new System.Drawing.Point(17, 120);
        this.levelTrackBar.Maximum = 90;
        this.levelTrackBar.Minimum = 1;
        this.levelTrackBar.Name = "levelTrackBar";
        this.levelTrackBar.Size = new System.Drawing.Size(168, 45);
        this.levelTrackBar.TabIndex = 2;
        this.levelTrackBar.TickFrequency = 5;
        this.levelTrackBar.Value = 1;
        this.levelTrackBar.Scroll += new System.EventHandler(this.levelTrackBar_Scroll);
        // 
        // wrTrackBar
        // 
        this.wrTrackBar.Location = new System.Drawing.Point(228, 120);
        this.wrTrackBar.Maximum = 4;
        this.wrTrackBar.Name = "wrTrackBar";
        this.wrTrackBar.Size = new System.Drawing.Size(104, 45);
        this.wrTrackBar.TabIndex = 3;
        this.wrTrackBar.Scroll += new System.EventHandler(this.wrTrackBar_Scroll);
        // 
        // wrLabel
        // 
        this.wrLabel.AutoSize = true;
        this.wrLabel.Location = new System.Drawing.Point(265, 104);
        this.wrLabel.Name = "wrLabel";
        this.wrLabel.Size = new System.Drawing.Size(35, 13);
        this.wrLabel.TabIndex = 4;
        this.wrLabel.Text = "WR 0";
        // 
        // tabControl2
        // 
        this.tabControl2.Controls.Add(this.statsTabPage);
        this.tabControl2.Controls.Add(this.infoTabPage);
        this.tabControl2.Location = new System.Drawing.Point(241, 59);
        this.tabControl2.Name = "tabControl2";
        this.tabControl2.SelectedIndex = 0;
        this.tabControl2.Size = new System.Drawing.Size(375, 405);
        this.tabControl2.TabIndex = 5;
        // 
        // statsTabPage
        // 
        this.statsTabPage.Controls.Add(this.starsLabel);
        this.statsTabPage.Controls.Add(this.starsTrackBar);
        this.statsTabPage.Controls.Add(this.statusLabel);
        this.statsTabPage.Controls.Add(this.defLabel);
        this.statsTabPage.Controls.Add(this.atkLabel);
        this.statsTabPage.Controls.Add(this.hpLabel);
        this.statsTabPage.Controls.Add(this.levelLabel);
        this.statsTabPage.Controls.Add(this.wrLabel);
        this.statsTabPage.Controls.Add(this.wrTrackBar);
        this.statsTabPage.Controls.Add(this.levelTrackBar);
        this.statsTabPage.Location = new System.Drawing.Point(4, 22);
        this.statsTabPage.Name = "statsTabPage";
        this.statsTabPage.Padding = new System.Windows.Forms.Padding(3);
        this.statsTabPage.Size = new System.Drawing.Size(367, 379);
        this.statsTabPage.TabIndex = 0;
        this.statsTabPage.Text = "Stats";
        this.statsTabPage.UseVisualStyleBackColor = true;
        // 
        // infoTabPage
        // 
        this.infoTabPage.Location = new System.Drawing.Point(4, 22);
        this.infoTabPage.Name = "infoTabPage";
        this.infoTabPage.Padding = new System.Windows.Forms.Padding(3);
        this.infoTabPage.Size = new System.Drawing.Size(330, 337);
        this.infoTabPage.TabIndex = 1;
        this.infoTabPage.Text = "Info";
        this.infoTabPage.UseVisualStyleBackColor = true;
        // 
        // hpLabel
        // 
        this.hpLabel.AutoSize = true;
        this.hpLabel.Location = new System.Drawing.Point(12, 200);
        this.hpLabel.Name = "hpLabel";
        this.hpLabel.Size = new System.Drawing.Size(24, 13);
        this.hpLabel.TabIndex = 5;
        this.hpLabel.Text = "Hp:";
        // 
        // atkLabel
        // 
        this.atkLabel.AutoSize = true;
        this.atkLabel.Location = new System.Drawing.Point(12, 227);
        this.atkLabel.Name = "atkLabel";
        this.atkLabel.Size = new System.Drawing.Size(26, 13);
        this.atkLabel.TabIndex = 6;
        this.atkLabel.Text = "Atk:";
        // 
        // defLabel
        // 
        this.defLabel.AutoSize = true;
        this.defLabel.Location = new System.Drawing.Point(12, 253);
        this.defLabel.Name = "defLabel";
        this.defLabel.Size = new System.Drawing.Size(27, 13);
        this.defLabel.TabIndex = 7;
        this.defLabel.Text = "Def:";
        // 
        // statusLabel
        // 
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new System.Drawing.Point(12, 174);
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new System.Drawing.Size(40, 13);
        this.statusLabel.TabIndex = 8;
        this.statusLabel.Text = "Status:";
        // 
        // starsTrackBar
        // 
        this.starsTrackBar.Location = new System.Drawing.Point(100, 46);
        this.starsTrackBar.Maximum = 6;
        this.starsTrackBar.Minimum = 1;
        this.starsTrackBar.Name = "starsTrackBar";
        this.starsTrackBar.Size = new System.Drawing.Size(168, 45);
        this.starsTrackBar.TabIndex = 9;
        this.starsTrackBar.Value = 1;
        this.starsTrackBar.Scroll += new System.EventHandler(this.starsTrackBar_Scroll);
        // 
        // starsLabel
        // 
        this.starsLabel.AutoSize = true;
        this.starsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.starsLabel.Location = new System.Drawing.Point(147, 19);
        this.starsLabel.Name = "starsLabel";
        this.starsLabel.Size = new System.Drawing.Size(77, 24);
        this.starsLabel.TabIndex = 10;
        this.starsLabel.Text = "* * * * * *";
        // 
        // LoadedFriendUI
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(684, 638);
        this.Controls.Add(this.tabControl2);
        this.Controls.Add(this.tabControl1);
        this.Name = "LoadedFriendUI";
        this.Text = "LoadedFriendUI";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadedFriendUI_FormClosing);
        this.tabControl1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.levelTrackBar)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wrTrackBar)).EndInit();
        this.tabControl2.ResumeLayout(false);
        this.statsTabPage.ResumeLayout(false);
        this.statsTabPage.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.starsTrackBar)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage cosDefault;
    private System.Windows.Forms.TabPage cosSport;
    private System.Windows.Forms.TabPage cosPark;
    private System.Windows.Forms.TabPage cosMaid;
    private System.Windows.Forms.TabPage cosSpecial;
    private System.Windows.Forms.Label levelLabel;
    private System.Windows.Forms.TrackBar levelTrackBar;
    private System.Windows.Forms.TrackBar wrTrackBar;
    private System.Windows.Forms.Label wrLabel;
    private System.Windows.Forms.TabControl tabControl2;
    private System.Windows.Forms.TabPage statsTabPage;
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.Label defLabel;
    private System.Windows.Forms.Label atkLabel;
    private System.Windows.Forms.Label hpLabel;
    private System.Windows.Forms.TabPage infoTabPage;
    private System.Windows.Forms.Label starsLabel;
    private System.Windows.Forms.TrackBar starsTrackBar;
}