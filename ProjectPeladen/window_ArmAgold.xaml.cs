using Microsoft.Win32;
using System.Windows;

namespace ProjectPeladen
{
    public partial class ArmAgold_window : Window
    {
        //Function f = new Function();
        System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

        public ArmAgold_window()
        {
            InitializeComponent();

            ArmAgold_Path_Textbox.Text = Function.GetIniValue(section: "ArmAgold", key: "gameDir");
        }

        #region ArmAgold WINDOW EVENT
        private void ArmAgold_Path_Click(object sender, RoutedEventArgs e)
        {
            ofd.Title = "Pilih lokasi file \"arma.exe\"";
            ofd.Filter = "|arma.exe";

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    ArmAgold_Path_Textbox.Text = ofd.FileName.Remove(ofd.FileName.Length - 8); // hasil path seko dialog box bakal di kurangi X char ko mburi

                    ArmAgold_isEnabledControl(true);
                    break;

                case System.Windows.Forms.DialogResult.Cancel: // ben ora crash pas user klik selain OK
                    break;
            }
        }

        private void ArmAgold_Launch_Click(object sender, RoutedEventArgs e)
        {
            Function.SetIniValue(section: "ArmAgold", key: "gameDir", value: ArmAgold_Path_Textbox.Text);

            Function.WriteRegKey(@"SOFTWARE\Bohemia Interactive Studio\ArmA", "MAIN", Function.GetIniValue(section: "ArmAgold", key: "gameDir")); /// set gamdir for game
            // REINSTALL ARMA, REG ASLINE ILANG   Function.WriteRegKey(@"SOFTWARE\Bohemia Interactive Studio\BattlEye", "MAIN", Function.GetIniValue(section: "ArmAgold", key: "gameDir")); /// set gamdir for AntiCheat "BattleEye"

            /// spesial format reg_binary, kapan2 pindah neng Function wae
            byte[] dataRegBinary = new byte[] { 0x73, 0xf0, 0x45, 0x8b, 0xd4, 0x9d, 0x40, 0x6c, 0xb9, 0xaa, 0x86, 0xaf, 0xc8, 0xba, 0x85 };
            RegistryKey rkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Bohemia Interactive Studio\ArmA");
            rkey.SetValue("KEY", dataRegBinary);

            Function.RunExe(dir: ArmAgold_Path_Textbox.Text, filename: "arma.exe", param: "-mod = dbe1");

            this.Close();
        }
        #endregion

#region ArmAgold WINDOW METHOD
        private void ArmAgold_isEnabledControl(bool status)
        {
            ArmAgold_Launch_Button.IsEnabled = status;
        }
#endregion
    }
}