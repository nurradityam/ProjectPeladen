using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ProjectPeladen
{
    public partial class CoH_window : Window
    {
        // instance here
        OpenFileDialog ofd = new OpenFileDialog();

        public CoH_window()
        {
            InitializeComponent();

            // load last config
            CoH_Path_Textbox.Text = Function.GetIniValue(section: "CoH", key: "gameDir");
            CoH_PlayerName_Textbox.Text = Function.GetIniValue(section: "CoH", key: "playerName");
            CoH_ModeKentang_CheckBox.IsChecked = Convert.ToBoolean(Function.GetIniValue(section: "CoH", key: "modeKentang"));
            CoH_Resolusi_ComboBox.SelectedIndex = Convert.ToInt16(Function.GetIniValue(section: "CoH", key: "modeKentangResolusi"));

            // kontrol GUI, nek gamedir durung defined, kabeh opsi bakal disabled
            if (CoH_Path_Textbox.Text == "")
            {
                CoH_isEnabledControl(false);
                CoH_Resolusi_ComboBoxIsEnabledControl(false);
            }
            else
            {
                CoH_Mod_ComboBoxIteminitialize(); // nek gamedir wes defined, engko listing mod_combobox bakal mlaku
            }
        }

        #region COH WINDOW EVENT
        private void CoH_Path_Click(object sender, RoutedEventArgs e)
        {
            ofd.Title = "Pilih lokasi file \"RelicCOH.exe\"";
            ofd.Filter = "|RelicCOH.exe";

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    CoH_Path_Textbox.Text = ofd.FileName.Remove(ofd.FileName.Length - 12); // hasil path seko dialog box bakal di kurangi X char ko mburi

                    CoH_isEnabledControl(true);
                    CoH_Mod_ComboBoxIteminitialize();
                    break;

                case System.Windows.Forms.DialogResult.Cancel: // ben ora crash pas user klik selain OK
                    break;
            }
        }

        private void CoH_Launch_Click(object sender, RoutedEventArgs e)
        {
            //save used config
            Function.SetIniValue(section: "CoH", key: "gameDir", value: CoH_Path_Textbox.Text);
            Function.SetIniValue(section: "CoH", key: "mode",value: CoH_Mod_ComboBox.Text);
            Function.SetIniValue(section: "CoH", key: "modeKentang", value: Convert.ToString(CoH_ModeKentang_CheckBox.IsChecked));
            Function.SetIniValue(section: "CoH", key: "modeKentangResolusi", value: Convert.ToString(CoH_Resolusi_ComboBox.SelectedIndex));

            string param = null;
            string exe = null;

            #region MODE KENTANG
            if (CoH_ModeKentang_CheckBox.IsChecked == true)
            {
                Function.ExtractEmbeddedResourceText("CoH.templatePotato_configuration.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\configuration.lua"));

                string pilihanResolusiWidth = "800";
                string pilihanResolusiHeight= "600";

                switch (CoH_Resolusi_ComboBox.Text)
                {
                    case "800x600":
                        pilihanResolusiWidth = "800";
                        pilihanResolusiHeight = "600";
                        break;

                    case "1280x720":
                        pilihanResolusiWidth = "1280";
                        pilihanResolusiHeight = "720";
                        break;

                    case "1366x768":
                        pilihanResolusiWidth = "1366";
                        pilihanResolusiHeight = "768";
                        break;
                }

                Function.FileReplaceString((System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\configuration.lua"), "template_width", pilihanResolusiWidth);
                Function.FileReplaceString((System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\configuration.lua"), "template_height", pilihanResolusiHeight);
            }
            #endregion

            #region PLAYERNAME CHANGER
            if (CoH_PlayerName_Textbox.Text != null)
            {
                Function.SetIniValue(section: "CoH", key: "COMPUTERNAMEasli", value: Environment.GetEnvironmentVariable("COMPUTERNAME"));
                Function.SetIniValue(section: "CoH", key: "playerName", value: CoH_PlayerName_Textbox.Text);

                Function.WriteRegKey(subkey: @"SYSTEM\ControlSet001\Control\ComputerName\ActiveComputerName", key: "ComputerName", data: CoH_PlayerName_Textbox.Text);
            }
            #endregion

            switch (CoH_Mod_ComboBox.Text)
            {
                case "Company of Heroes":
                    /// ngeoverwrite campaignstate.lua ke savegame user
                    Function.ExtractEmbeddedResourceText("CoH.campaignLuaUnlock.COH_campaignstate.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\Savegames\RelicCOH\Campaigns\coh\campaignstate.lua"));
                    Function.ExtractEmbeddedResourceText("CoH.campaignLuaUnlock.CXP1_campaignstate.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\Savegames\RelicCOH\Campaigns\cxp1\campaignstate.lua"));
                    Function.ExtractEmbeddedResourceText("CoH.campaignLuaUnlock.CXP2_campaignstate.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\Savegames\RelicCOH\Campaigns\cxp2\campaignstate.lua"));
                    Function.ExtractEmbeddedResourceText("CoH.campaignLuaUnlock.DLC1_campaignstate.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\Savegames\RelicCOH\Campaigns\dlc1\campaignstate.lua"));

                    /// ngeoverwrite playercfg.lua, dingo ngapusi last fps(ben dadi ijo ping e)
                    Function.ExtractEmbeddedResourceText("CoH.playercfg.lua", (System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\playercfg.lua"));

                    exe = "RelicCOH.exe";
                    param = "-nomovies";
                    break;

                case "Joint Operations":
                    exe = "jointops.exe";
                    param = null;
                    break;

                case "Modern Combat":
                    exe = "RelicCOH.exe";
                    param = "-dev -mod ModernCombat -nomovies";
                    break;

                case "Eastern Front":
                    exe = "RelicCOH.exe";
                    param = "-dev -mod EF_beta -nomovies";
                    break;

                case "The Great War 1918":
                    exe = "RelicCOH.exe";
                    param = "-mod tgw -nomovies";
                    break;

                case "Europe at War":
                    exe = "RelicCOH.exe";
                    param = "-dev -mod Europe_At_War -nomovies";
                    break;

                case "The Far East":
                    exe = "RelicCOH.exe";
                    param = "-dev -mod Far_East_Mod -nomovies";
                    break;
            }

            if (CoH_PlayerName_Textbox.Text != "")
            {
                Function.RunExe(dir: CoH_Path_Textbox.Text, filename: exe, param: param);
                Function.WriteRegKey(subkey: @"SYSTEM\ControlSet001\Control\ComputerName\ActiveComputerName", key: "ComputerName", data: Function.GetIniValue(section: "CoH", key: "COMPUTERNAMEasli"));
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Nama Player harus diisi.","ProjectPeladen - Error");
            }
        }

        private void CoH_ModeKentang_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CoH_Resolusi_ComboBoxIsEnabledControl(true);
        }

        private void CoH_ModeKentang_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CoH_Resolusi_ComboBoxIsEnabledControl(false);
        }
        #endregion

        #region COH WINDOW METHOD
        private void CoH_isEnabledControl(bool status)
        {
            CoH_Launch_Button.IsEnabled = status;
            CoH_Mod_ComboBox.IsEnabled = status;
            CoH_PlayerName_Textbox.IsEnabled = status;
            CoH_ModeKentang_CheckBox.IsEnabled = status;
        }

        private void CoH_Resolusi_ComboBoxIsEnabledControl(bool status)
        {
            CoH_Resolusi_ComboBox.IsEnabled = status;
            CoH_Resolusi_Label.IsEnabled = status;
        }

        private void CoH_Mod_ComboBoxIteminitialize()
        {
            CoH_Mod_ComboBox.Items.Add("Company of Heroes");

            if (Directory.Exists(CoH_Path_Textbox.Text + "ModernCombat"))
            {
                CoH_Mod_ComboBox.Items.Add("Modern Combat");
            }

            if (Directory.Exists(CoH_Path_Textbox.Text + "JointOps"))
            {
                CoH_Mod_ComboBox.Items.Add("Joint Operations");
            }

            if (Directory.Exists(CoH_Path_Textbox.Text + "EF_beta"))
            {
                CoH_Mod_ComboBox.Items.Add("Eastern Front");
            }

            if (Directory.Exists(CoH_Path_Textbox.Text + "tgw"))
            {
                CoH_Mod_ComboBox.Items.Add("The Great War 1918");
            }

            if (Directory.Exists(CoH_Path_Textbox.Text + "Europe_At_War"))
            {
                CoH_Mod_ComboBox.Items.Add("Europe at War");
            }

            if (Directory.Exists(CoH_Path_Textbox.Text + "Far_East_Mod"))
            {
                CoH_Mod_ComboBox.Items.Add("The Far East");
            }

            ///decide nek durung ono seng kesave bakal nganggo default,nek ono kesave yo nganggo iku
            if (Function.GetIniValue(section: "CoH", key: "mode") == null)
            {
                CoH_Mod_ComboBox.Text = "Company of Heroes";
            }
            else
            {
                CoH_Mod_ComboBox.Text = Function.GetIniValue(section: "CoH", key: "mode");
            }
        }
        #endregion

        #region UNUSED CODES
        /*
        private void CoH_ClearCache_Button_Click(object sender, RoutedEventArgs e) /// UNUSED, marai njaluk SN eneh
        {
            // exception handler, nek folder target raono diabaikan
            bool folderCacheLogDitemukan = true; // penentu folder target ono po ra
            try
            {
                Directory.Delete((System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\Cache"), true);
                Directory.Delete((System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\My Games\Company of Heroes\LogFiles"), true);
            }

            catch (System.IO.DirectoryNotFoundException)
            {
                folderCacheLogDitemukan = false; // folder target berarti gk ono
            }

            finally
            {
                if (folderCacheLogDitemukan == true)
                {
                    System.Windows.MessageBox.Show("Folder cache & logs berhasil dihapus.");
                }
                else
                {
                    System.Windows.MessageBox.Show("Aduh, folder cache & logs tidak ditemukan");
                }
            }
        }
        */
        #endregion
    }
}
