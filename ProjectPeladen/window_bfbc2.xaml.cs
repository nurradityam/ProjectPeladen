using System;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace ProjectPeladen
{
    public partial class Bfbc2_window : Window
    {
        string binFolderPath;
        string movFolderPath;
        string cfgFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BFBC2";

        public Bfbc2_window()
        {
            InitializeComponent();

            // load saved launcher config
            Bfbc2_Path_Textbox.Text = Properties.Settings.Default.BFBC2_LastGameDir;
            Bfbc2_MasterServerIP_Textbox.Text = Properties.Settings.Default.BFBC2_MasterServerIP;
            Bfbc2_SkipIntro_CheckBox.IsChecked = Properties.Settings.Default.BFBC2_SkipIntro;
            Bfbc2_Resolusi_ComboBox.SelectedIndex = Properties.Settings.Default.BFBC2_Resolution;
            Bfbc2_Fov_ComboBox.SelectedIndex = Properties.Settings.Default.BFBC2_FOV;
            Bfbc2_ModeKentang_CheckBox.IsChecked = Properties.Settings.Default.BFBC2_ModeKentang;

            if (Bfbc2_Path_Textbox.Text != null)
                Bfbc2_isEnabledControl(false);
            else
                Bfbc2_isEnabledControl(true);

            Bfbc2_ModeKentang_Control();
        }

#region BFBC2 WINDOW EVENT
        private void Bfbc2_Path_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Pilih lokasi file \"BFBC2Game.exe\"";
            ofd.Filter = "|BFBC2Game.exe";

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    Bfbc2_Path_Textbox.Text = ofd.FileName.Remove(ofd.FileName.Length - 13); // hasil path seko dialog box bakal di kurangi 7 char seko mburi, ben entuk dir path

                    // update GUI control
                    Bfbc2_isEnabledControl(true);
                    Bfbc2_ModeKentang_Control();

                    break;

                case System.Windows.Forms.DialogResult.Cancel:
                    break;
            }
        }

        private void Bfbc2_ModeKentang_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Bfbc2_Resolusi_ComboBox.IsEnabled = true;
            Bfbc2_Fov_ComboBox.IsEnabled = false;
        }

        private void Bfbc2_ModeKentang_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Bfbc2_Resolusi_ComboBox.IsEnabled = false;
            Bfbc2_Fov_ComboBox.IsEnabled = true;
        }

        private void Bfbc2_SkipIntro_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Bfbc2_SkipIntro();
        }

        private void Bfbc2_SkipIntro_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Bfbc2_SkipIntro();
        }

        private void Bfbc2_Launch_Click(object sender, RoutedEventArgs e)
        {
            FileIniDataParser Ini = new FileIniDataParser();

            // save config
            Properties.Settings.Default.BFBC2_LastGameDir       = Bfbc2_Path_Textbox.Text;
            Properties.Settings.Default.BFBC2_MasterServerIP    = Bfbc2_MasterServerIP_Textbox.Text;
            Properties.Settings.Default.BFBC2_SkipIntro         = Convert.ToBoolean(Bfbc2_SkipIntro_CheckBox.IsChecked);
            Properties.Settings.Default.BFBC2_ModeKentang       = Convert.ToBoolean(Bfbc2_ModeKentang_CheckBox.IsChecked);
            Properties.Settings.Default.BFBC2_FOV               = Bfbc2_Fov_ComboBox.SelectedIndex;
            Properties.Settings.Default.BFBC2_Resolution        = Bfbc2_Resolusi_ComboBox.SelectedIndex;
            Properties.Settings.Default.Save();

            // set server ip
            IniData serverConfig = Ini.ReadFile(binFolderPath + "bfbc2.ini");
            serverConfig["info"]["host"]            = Bfbc2_MasterServerIP_Textbox.Text;
            serverConfig["client"]["reroute_http"]  = "1";
            Ini.WriteFile(binFolderPath + "bfbc2.ini", serverConfig);

            // mode kentang
            if (Bfbc2_ModeKentang_CheckBox.IsChecked == true)
            {
                string pilihanResolusiWidth = "800";
                string pilihanResolusiHeight = "600";

                if (!File.Exists(cfgFolderPath + @"\settings.ini"))
                {
                    Directory.CreateDirectory(cfgFolderPath);

                    var createConfig = File.Create(cfgFolderPath + @"\settings.ini");
                    createConfig.Close();
                }
                IniData config = Ini.ReadFile(cfgFolderPath + @"\settings.ini");

                switch (Bfbc2_Resolusi_ComboBox.Text)
                {
                    case "800x600":
                        pilihanResolusiWidth    = "800";
                        pilihanResolusiHeight   = "600";

                        break;

                    case "1280x720":
                        pilihanResolusiWidth    = "1280";
                        pilihanResolusiHeight   = "720";

                        break;

                    case "1366x768":
                        pilihanResolusiWidth    = "1366";
                        pilihanResolusiHeight   = "768";

                        break;
                }

                if (Convert.ToInt16(Bfbc2_Fov_ComboBox.Text) != 55)
                    config["Graphics"]["Fov"] = Bfbc2_Fov_ComboBox.Text;
                else
                    config["Graphics"]["Fov"] = "55";

                config["WindowSettings"]["Width"]       = pilihanResolusiWidth;
                config["WindowSettings"]["Height"]      = pilihanResolusiHeight;
                config["WindowSettings"]["Fullscreen"]  = "true";
                config["WindowSettings"]["VSync"]       = "false";
                config["Sound"]["Quality"]              = "low";
                config["Graphics"]["Effects"]           = "low";
                config["Graphics"]["Vehicles"]          = "low";
                config["Graphics"]["Overgrowth"]        = "low";
                config["Graphics"]["Undergrowth"]       = "low";
                config["Graphics"]["StaticObjects"]     = "low";
                config["Graphics"]["Terrain"]           = "low";
                config["Graphics"]["Shadows"]           = "low";
                config["Graphics"]["Bloom"]             = "false";
                config["Graphics"]["HSAO"]              = "false";
                config["Graphics"]["MSAA"]              = "0";
                config["Graphics"]["Water"]             = "low";
                config["Graphics"]["MainQuality"]       = "custom";
                config["Graphics"]["Texture"]           = "low";
                config["Graphics"]["DxVersion"]         = "9";
                config["Graphics"]["Aniso"]             = "0";
                config["Graphics"]["Detail"]            = "low";
                config["Graphics"]["RenderAheadLimit"]  = "0";

                Ini.WriteFile(cfgFolderPath + @"\settings.ini", config);
            }

            Function.RunExe(dir: binFolderPath, filename: "BFBC2Game.exe", param:"");
            this.Close();
        }
#endregion

#region BFBC2 METHOD
        private void Bfbc2_isEnabledControl(bool status)
        {
            Bfbc2_Launch_Button.IsEnabled = status;
            Bfbc2_MasterServerIP_Textbox.IsEnabled = status;
            Bfbc2_ModeKentang_CheckBox.IsEnabled = status;
            Bfbc2_Resolusi_ComboBox.IsEnabled = status;
            Bfbc2_SkipIntro_CheckBox.IsEnabled = status;
            Bfbc2_Fov_ComboBox.IsEnabled = status;

            binFolderPath = Bfbc2_Path_Textbox.Text;
            movFolderPath = binFolderPath + @"Output\win32\movies";
        }

        private void Bfbc2_ModeKentang_Control()
        {
            if (Bfbc2_ModeKentang_CheckBox.IsChecked == true)
                Bfbc2_Resolusi_ComboBox.SelectedIndex = Properties.Settings.Default.BFBC2_Resolution;
            else
                Bfbc2_Resolusi_ComboBox.IsEnabled = false;
        }

        private void Bfbc2_SkipIntro()
        {
            if (File.Exists(movFolderPath + @"\ea_logo_hd.res") &&
                File.Exists(movFolderPath + @"\dolbydigital.res"))
            {
                File.Move(movFolderPath + @"\ea_logo_hd.res", movFolderPath + @"\SKIP_ea_logo_hd.res");
                File.Move(movFolderPath + @"\dolbydigital.res", movFolderPath + @"\SKIP_dolbydigital.res");
            }
            else if (File.Exists(movFolderPath + @"\SKIP_ea_logo_hd.res") &&
                     File.Exists(movFolderPath + @"\SKIP_dolbydigital.res"))
            {
                File.Move(movFolderPath + @"\SKIP_ea_logo_hd.res", movFolderPath + @"\ea_logo_hd.res");
                File.Move(movFolderPath + @"\SKIP_dolbydigital.res", movFolderPath + @"\dolbydigital.res");
            }
        }
        #endregion
    }
}