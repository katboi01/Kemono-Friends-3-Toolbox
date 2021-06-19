using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static KF3Toolbox.KF3Parse;

namespace KF3Toolbox.GUIHere
{
    public partial class LoadedFriendUI : Form
    {
        KF3Parse p;
        CharaData friend;
        Image noData;
        List<Image> imageList = new List<Image>();

        public LoadedFriendUI(KF3Parse p, CharaData friend, string title, Image image)
        {
            InitializeComponent();
            this.p = p;
            this.friend = friend;
            this.Text = title;
            Bitmap bm = (Bitmap)image;
            this.Icon = Icon.FromHandle(bm.GetHicon());
            noData = Bitmap.FromFile(Directory.GetCurrentDirectory() + @"/Data/icon_chara_error.png");
            UpdateFriendCostumes();
            InitializeFriendStats();
        }

        private void InitializeFriendStats()
        {
            levelTrackBar.Maximum = (friend.rankHigh == 6) ? 99 : 80;
            levelTrackBar.Value = 1;
            wrTrackBar.Maximum = (friend.promotePresetDatas[4].promoteStepDatetime != "1917860400000") ? 5 : 4;
            wrTrackBar.Value = 0;
            starsTrackBar.Minimum = friend.rankLow;
            starsTrackBar.Maximum = friend.rankHigh;
            starsTrackBar.Value = friend.rankLow;
            starsTrackBar_Scroll(null, null);
        }

        private void UpdateFriendStats()
        {
            float starBoost = 1 + (starsTrackBar.Value - 1) * 0.02f;
            int hp, atk, def;
            int hpCosBonus = 0, atkCosBonus = 0, defCosBonus = 0;
            foreach (CharaClothesData clothe in friend.clothesDatas)
            {
                if (starsTrackBar.Value < clothe.getRank) { continue; }
                hpCosBonus += clothe.hpBonus;
                atkCosBonus += clothe.atkBonus;
                defCosBonus += clothe.defBonus;
            }

            hp = StolenStuff.CalcParamInternal(levelTrackBar.Value, 99, friend.paramAlphaBase.hpParamLv1, friend.paramAlphaBase.hpParamLv99, friend.paramAlphaBase.hpParamLvMiddle, friend.paramAlphaBase.hpLvMiddleNum);
            hp = (int)Math.Ceiling((hp + friend.GetPromoteStat(wrTrackBar.Value, "hp")) * starBoost) + hpCosBonus;
            atk = StolenStuff.CalcParamInternal(levelTrackBar.Value, 99, friend.paramAlphaBase.atkParamLv1, friend.paramAlphaBase.atkParamLv99, friend.paramAlphaBase.atkParamLvMiddle, friend.paramAlphaBase.atkLvMiddleNum);
            atk = (int)Math.Ceiling((atk + friend.GetPromoteStat(wrTrackBar.Value, "atk")) * starBoost) + atkCosBonus;
            def = StolenStuff.CalcParamInternal(levelTrackBar.Value, 99, friend.paramAlphaBase.defParamLv1, friend.paramAlphaBase.defParamLv99, friend.paramAlphaBase.defParamLvMiddle, friend.paramAlphaBase.defLvMiddleNum);
            def = (int)Math.Ceiling((def + friend.GetPromoteStat(wrTrackBar.Value, "def")) * starBoost) + defCosBonus;
            hpLabel.Text = "Hp: " + hp;
            atkLabel.Text = "Atk: " + atk;
            defLabel.Text = "Def: " + def;
                /* Old method
                + (Math.Ceiling((KF3Parse.Interpolate(
                friend.paramAlphaBase.hpParamLvMiddle,
                friend.paramAlphaBase.hpParamLv99,
                levelTrackBar.Value - friend.paramAlphaBase.hpLvMiddleNum,
                99 - friend.paramAlphaBase.hpLvMiddleNum)
                + friend.GetPromoteStat(wrTrackBar.Value, "hp")) * starBoost)
                + hpCosBonus);
                */
            statusLabel.Text = $"Status: {KF3Parse.CalcStatus(hp, atk, def)}";
        }

        public void UpdateFriendCostumes()
        {
            tabControl1.TabPages.Clear();
            int i = 0;
            foreach (CharaClothesData clothesData in friend.clothesDatas)
            {
                tabControl1.TabPages.Add(clothesData.name);
                Bitmap bmp;

                if (!File.Exists(KF3Shared.SharedSettings.guiPath + $"icon_dressup_{ clothesData.id}.png"))
                {
                    try
                    {
                        string link = $"https://sandstar.site/static/kf3_db/Texture2D/icon_dressup/icon_dressup_{clothesData.id}.png";
                        System.Net.WebRequest request = System.Net.WebRequest.Create(link);
                        System.Net.WebResponse resp = request.GetResponse();
                        using (Stream respStream = resp.GetResponseStream())
                        {
                            bmp = new Bitmap(respStream);
                            bmp.Save(KF3Shared.SharedSettings.guiPath + $"icon_dressup_{ clothesData.id}.png");
                        }
                    }
                    catch
                    {
                        bmp = new Bitmap(noData);
                    }
                }
                else
                {
                    using (Stream input = File.OpenRead(KF3Shared.SharedSettings.guiPath + $"icon_dressup_{ clothesData.id}.png"))
                    {
                        bmp = new Bitmap(input);
                    }
                }

                tabControl1.TabPages[i].BackgroundImage = bmp;
                tabControl1.TabPages[i].BackgroundImageLayout = ImageLayout.Center;
                i++;
            }
        }

        private void LoadedFriendUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            noData.Dispose();
            foreach (Image img in imageList)
            {
                img.Dispose();
            }
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.BackgroundImage != null) page.BackgroundImage.Dispose();
            }
            imageList.Clear();
            tabControl1.Dispose();
            this.Dispose();
        }

        private void levelTrackBar_Scroll(object sender, EventArgs e)
        {
            levelLabel.Text = $"Level {levelTrackBar.Value}";
            UpdateFriendStats();
        }

        private void wrTrackBar_Scroll(object sender, EventArgs e)
        {
            wrLabel.Text = $"WR {wrTrackBar.Value}";
            UpdateFriendStats();
        }

        private void starsTrackBar_Scroll(object sender, EventArgs e)
        {
            starsLabel.Text = "";
            for(int i = 0; i<starsTrackBar.Value-1; i++)
            {
                starsLabel.Text += "* ";
            }
            starsLabel.Text += "*";
            UpdateFriendStats();
        }
    }
}
