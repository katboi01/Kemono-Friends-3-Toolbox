
partial class KFParseUI
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
        this.friendsListView = new System.Windows.Forms.ListView();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.friendsPage = new System.Windows.Forms.TabPage();
        this.photosPage = new System.Windows.Forms.TabPage();
        this.itemsPage = new System.Windows.Forms.TabPage();
        this.tabControl1.SuspendLayout();
        this.friendsPage.SuspendLayout();
        this.SuspendLayout();
        // 
        // friendsListView
        // 
        this.friendsListView.FullRowSelect = true;
        this.friendsListView.GridLines = true;
        this.friendsListView.HideSelection = false;
        this.friendsListView.Location = new System.Drawing.Point(6, 43);
        this.friendsListView.Name = "friendsListView";
        this.friendsListView.Size = new System.Drawing.Size(360, 281);
        this.friendsListView.TabIndex = 2;
        this.friendsListView.UseCompatibleStateImageBehavior = false;
        this.friendsListView.View = System.Windows.Forms.View.Details;
        this.friendsListView.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
        // 
        // tabControl1
        // 
        this.tabControl1.Controls.Add(this.friendsPage);
        this.tabControl1.Controls.Add(this.photosPage);
        this.tabControl1.Controls.Add(this.itemsPage);
        this.tabControl1.Location = new System.Drawing.Point(12, 12);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(385, 371);
        this.tabControl1.TabIndex = 4;
        // 
        // friendsPage
        // 
        this.friendsPage.Controls.Add(this.friendsListView);
        this.friendsPage.Location = new System.Drawing.Point(4, 22);
        this.friendsPage.Name = "friendsPage";
        this.friendsPage.Padding = new System.Windows.Forms.Padding(3);
        this.friendsPage.Size = new System.Drawing.Size(377, 345);
        this.friendsPage.TabIndex = 0;
        this.friendsPage.Text = "Friends";
        this.friendsPage.UseVisualStyleBackColor = true;
        // 
        // photosPage
        // 
        this.photosPage.Location = new System.Drawing.Point(4, 22);
        this.photosPage.Name = "photosPage";
        this.photosPage.Padding = new System.Windows.Forms.Padding(3);
        this.photosPage.Size = new System.Drawing.Size(377, 345);
        this.photosPage.TabIndex = 1;
        this.photosPage.Text = "Photos";
        this.photosPage.UseVisualStyleBackColor = true;
        // 
        // itemsPage
        // 
        this.itemsPage.Location = new System.Drawing.Point(4, 22);
        this.itemsPage.Name = "itemsPage";
        this.itemsPage.Size = new System.Drawing.Size(377, 345);
        this.itemsPage.TabIndex = 2;
        this.itemsPage.Text = "Items";
        this.itemsPage.UseVisualStyleBackColor = true;
        // 
        // KFParseUI
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.tabControl1);
        this.Name = "KFParseUI";
        this.Text = "KFParseUI";
        this.Load += new System.EventHandler(this.KFParseUI_Load);
        this.tabControl1.ResumeLayout(false);
        this.friendsPage.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.ListView friendsListView;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage friendsPage;
    private System.Windows.Forms.TabPage photosPage;
    private System.Windows.Forms.TabPage itemsPage;
}