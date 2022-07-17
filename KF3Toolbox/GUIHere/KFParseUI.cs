using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using static KF3Parse;

public partial class KFParseUI : Form
{
    public KF3Parse p = new KF3Parse();
    ImageList characterPortraits = new ImageList();
    Image noData;
    public KFParseUI()
    {
        InitializeComponent();
    }

    private void KFParseUI_Load(object sender, EventArgs e)
    {
        p.ParseKf3(false, true);
        noData = Bitmap.FromFile(Directory.GetCurrentDirectory() + @"/Data/icon_chara_error.png");
        LoadFriends();
    }

    private void LoadFriends()
    {
        friendsListView.Columns.Add("image", 64, HorizontalAlignment.Left);
        friendsListView.Columns.Add("id", 32, HorizontalAlignment.Left);
        friendsListView.Columns.Add("name", 256, HorizontalAlignment.Left);
        friendsListView.Items.Clear();
        characterPortraits.Images.Clear();
        characterPortraits.ImageSize = new Size(64, 64);
        characterPortraits.ColorDepth = ColorDepth.Depth32Bit;

        foreach (CharaData friend in p.CharaDatas)
        {

            if (File.Exists(SharedSettings.guiPath + @"\Character Images\mini\icon_chara_mini_" + friend.id.ToString().PadLeft(4, '0') + ".png"))
            {
                characterPortraits.Images.Add(Bitmap.FromFile(SharedSettings.guiPath + @"\Character Images\mini\icon_chara_mini_" + friend.id.ToString().PadLeft(4, '0') + ".png"));
            }
            else characterPortraits.Images.Add(noData);

            /*
            string link = $"https://sandstar.site/static/kf3_db/Texture2D/icon_chara/chara/icon_chara_{friend.id.ToString().PadLeft(4, '0')}.png";
            System.Net.WebRequest request = System.Net.WebRequest.Create(link);
            System.Net.WebResponse resp = request.GetResponse();
            System.IO.Stream respStream = resp.GetResponseStream();
            Bitmap bmp = new Bitmap(respStream);
            respStream.Dispose();
            characterPortraits.Images.Add(bmp);
            */

            ListViewItem lvi = new ListViewItem();
            lvi.SubItems.Add(friend.id.ToString());
            if (string.IsNullOrEmpty(friend.nameEn))
            {
                lvi.SubItems.Add(friend.name);
            }
            else if (!string.IsNullOrEmpty(friend.nickname))
            {
                lvi.SubItems.Add(friend.nameEn + " " + friend.nickname);
            }
            else lvi.SubItems.Add(friend.nameEn);

            lvi.ImageIndex = characterPortraits.Images.Count - 1;
            friendsListView.Items.Add(lvi);
        }
        friendsListView.SmallImageList = characterPortraits;
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
        ListViewItem item = friendsListView.SelectedItems[0];
        LoadedFriendUI friendUI = new LoadedFriendUI(p, p.FriendFromNumber(item.SubItems[1].Text), item.SubItems[2].Text, characterPortraits.Images[item.ImageIndex]);
        friendUI.Show();
    }
}
