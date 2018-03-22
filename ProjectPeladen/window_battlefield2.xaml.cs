using System;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Windows.Controls;

namespace ProjectPeladen
{
    public partial class Battlefield2_window : Window
    {
        OpenFileDialog ofd = new OpenFileDialog();

        public Battlefield2_window()
        {
            InitializeComponent();

            // GET last config
            Battlefield2_Path_Textbox.Text = Function.GetIniValue(section: "BF2", key: "gameDir");
            Battlefield2_SN_ComboBox.Text = Function.GetIniValue(section: "BF2", key: "SNselector");
            Battlefield2_AffinityCPU0_CheckBox.IsChecked = Convert.ToBoolean(Function.GetIniValue(section: "BF2", key: "setAffinity"));

            // Initialize
            Battlefield2_Mod_ComboBoxIteminitialize(); // check for available mod
            Battlefield2_GameEnvInitialize(); // put every shit todo

            if (Battlefield2_Path_Textbox.Text == null)
            {
                Battlefield2_isEnabledControl(false);
            }
        }

#region BATTLEFIELD2 WINDOW EVENT
        private void Battlefield2_Path_Click(object sender, RoutedEventArgs e)
        {
            ofd.Title = "Pilih lokasi file \"BF2.exe\"";
            ofd.Filter = "|BF2.exe";

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    Battlefield2_Path_Textbox.Text = ofd.FileName.Remove(ofd.FileName.Length - 7); // hasil path seko dialog box bakal di kurangi 7 char seko mburi, ben entuk dir path

                    // UPDATE menu control
                    Battlefield2_isEnabledControl(true);
                    Battlefield2_Mod_ComboBoxIteminitialize();
                    Battlefield2_Mod_ComboBox.SelectedIndex = 0;
                    Battlefield2_SN_ComboBox.SelectedIndex = 0;
                    break;

                case System.Windows.Forms.DialogResult.Cancel:
                    break;
            }
        }

        private void Battlefield2_PlayerNameApply_Click(object sender, RoutedEventArgs e)
        {
            Function.ExtractEmbeddedResourceText("BF2.Profile.con", System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\Battlefield 2\Profiles\0001\Profile.con");

            string newPlayerName = Battlefield2_PlayerName_Textbox.Text;
            Function.FileReplaceString(System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\Battlefield 2\Profiles\0001\Profile.con", "TEMPLATE", newPlayerName);

            System.Windows.MessageBox.Show("TODO: add exception handling\n mohon cek manual", "Sepertinya Sukses", MessageBoxButton.OK);
        }

        private void Battlefield2_Launch_Click(object sender, RoutedEventArgs e)
        {
            // SET last config
            Function.SetIniValue(section: "BF2", key: "gameDir", value: Battlefield2_Path_Textbox.Text);
            Function.SetIniValue(section: "BF2", key: "mode", value: Battlefield2_Mod_ComboBox.Text);
            Function.SetIniValue(section: "BF2", key: "SNselector", value: Battlefield2_SN_ComboBox.Text);
            Function.SetIniValue(section: "BF2", key: "setAffinity", value: Battlefield2_AffinityCPU0_CheckBox.IsChecked);

            // DECLARE var
            string ModSelect = "bf2";
            string JoinIPparam = "";

            bool SetAffinity = Battlefield2_AffinityCPU0_CheckBox.IsChecked == true ? true : false;
            if (Battlefield2_JoinIP_Textbox.Text != "")
            {
                string joinIP = Convert.ToString(Battlefield2_JoinIP_Textbox.Text);
                JoinIPparam = "+joinServer " + joinIP;
            }
            switch (Battlefield2_SN_ComboBox.Text)
            {
                case "1":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc","", @"x93920P2A-DZB7-FIGH-TING-4FUN");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x93920CAS-PA2H-FIGH-TING-4FUN");
                    break;

                case "2":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x939253CL-TLQY-BQJH-RSVB-EGUF");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392TJ9R-V2RQ-WWZZ-8RRJ-DRH4");
                    break;

                case "3":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x9392XHPP-PV8Y-FYCW-EQ97-CMYX");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392BUR9-8TZH-EF2H-VYLZ-AKWN");
                    break;

                case "4":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x9392KHLU-N6LQ-8NU8-7SMX-U5X4");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392JFK3-GT44-8YTL-GY7Y-TAAF");
                    break;

                case "5":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x93927FFP-XVKB-492E-KP4N-3Z8R");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392W5VV-SK7V-RD4Q-N5N6-VL45");
                    break;

                case "6":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x93925JD5-XAP3-DSMA-LBTZ-W7XB");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x939233B3-84S2-KYW4-F26K-NU8R");
                    break;

                case "7":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x9392VDPF-RYJ8-4CCE-WJRW-KL48");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392QMDV-C3Q9-KYCV-HVK3-AVSH");
                    break;

                case "8":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x9392HH4C-VG7Y-5NNH-6TUJ-LDER");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392MF2J-Z9YT-4WKS-93WQ-SQ3X");
                    break;

                case "9":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x93923VE6-P8ND-L8LZ-NQCP-H2Y6");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392ZPPF-VZGP-JBKF-KLFE-CDBU");
                    break;

                case "10":
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\ergc", "", @"x93925F6W-Z264-NVSU-BQBG-HE9H");
                    Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\ergc", "", @"x9392P97X-PPZW-8LUG-QV9U-GV83");
                    break;
            }
            switch (Battlefield2_Mod_ComboBox.Text) // get text displayed on combobox (default bf2)
            {
                case "Battlefield 2":
                    ModSelect = " + modPath mods/bf2";
                    break;
                case "Battlefield 2 Special Forces":
                    ModSelect = " + modPath mods/xpack";
                    break;
                case "Battleship 0.32":
                    ModSelect = " + modPath mods/bb";
                    break;
                case "BF2ALL64 1.0":
                    ModSelect = " + modPath mods/bf2all64";
                    break;
                case "BF2 Weapon Unlock":
                    ModSelect = " +modPath mods/unlocks123";
                    break;
                case "BF2SF Weapon Unlock":
                    ModSelect = " +modPath mods/sf_unlocks";
                    break;
                case "Omnicide Final":
                    ModSelect = " +modPath mods/omnicide";
                    break;
                case "SOW 3.0":
                    ModSelect = " +modPath mods/sow";
                    break;
                case "HARDCORE 2.1.5":
                    ModSelect = " +modPath mods/bf2hc";
                    break;
                case "CQB 1.9":
                    ModSelect = " +modPath mods/cqb";
                    break;
                case "Fallen Times":
                    ModSelect = " +modPath mods/fallen_times";
                    break;

            }

            Function.RunExe(dir: Battlefield2_Path_Textbox.Text, filename: "BF2.exe", param: @"+menu 1 +fullscreen 1 +widescreen 1 +restart +szx 1366 +szy 768 " + JoinIPparam + ModSelect, cpu0: SetAffinity);
            this.Close();
        }
#endregion

#region BATTLEFIELD2 WINDOW METHOD
        private void Battlefield2_isEnabledControl(bool status)
        {
            Battlefield2_Launch_Button.IsEnabled = status;
            Battlefield2_Mod_ComboBox.IsEnabled = status;
            Battlefield2_SN_ComboBox.IsEnabled = status;
            Battlefield2_JoinIP_Textbox.IsEnabled = status;
            Battlefield2_PlayerName_Textbox.IsEnabled = status;
            Battlefield2_PlayerNameApply.IsEnabled = status;
            Battlefield2_AffinityCPU0_CheckBox.IsEnabled = status;
        }

        private void Battlefield2_Mod_ComboBoxIteminitialize()
        {
            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\bf2"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("Battlefield 2");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\xpack"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("Battlefield 2 Special Forces");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\bb"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("Battleship 0.32");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\bf2all64"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("BF2ALL64 1.0");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\unlocks123"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("BF2 Weapon Unlock");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\sf_unlocks"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("BF2SF Weapon Unlock");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\omnicide"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("Omnicide Final");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\sow"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("SOW 3.0");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\bf2hc"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("HARDCORE 2.1.5");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\cqb"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("CQB 1.9");
            }

            if (Directory.Exists(Battlefield2_Path_Textbox.Text + @"mods\fallen_times"))
            {
                Battlefield2_Mod_ComboBox.Items.Add("Fallen Times");
            }

            Battlefield2_Mod_ComboBox.Text = Function.GetIniValue(section: "BF2", key: "mode");
        }

        private void Battlefield2_GameEnvInitialize()
        {
            Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2\wdc", "", "true");
            Function.WriteRegKey(@"SOFTWARE\Electronic Arts\EA Games\Battlefield 2 Special Forces\wdc", "", "true");
        }

        private void Battlefield2_PlayerName_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Battlefield2_PlayerNameApply.IsEnabled = true;
        }
#endregion
    }
}