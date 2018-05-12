using System.Windows;
using System.Windows.Forms;

/// <summary>
/// TODO: reg add SN aoe3, homecity card unlocked template
/// </summary>

namespace ProjectPeladen
{
    public partial class aoe3_window : Window
    {
        Function f = new Function();
        OpenFileDialog ofd = new OpenFileDialog();

        public aoe3_window()
        {
            InitializeComponent();

            aoe3_Path_Textbox.Text = Function.GetIniValue(section: "aoe3", key: "gameDir");
        }

        #region aoe3 WINDOW EVENT
        private void aoe3_Path_Click(object sender, RoutedEventArgs e)
        {
            ofd.Title = "Pilih lokasi file \"age3y.exe\"";
            ofd.Filter = "|age3y.exe";

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    aoe3_Path_Textbox.Text = ofd.FileName.Remove(ofd.FileName.Length - 9); // hasil path seko dialog box bakal di kurangi X char ko mburi

                    aoe3_isEnabledControl(true);
                    break;

                case System.Windows.Forms.DialogResult.Cancel: // ben ora crash pas user klik selain OK
                    break;
            }
        }

        private void aoe3_Launch_Click(object sender, RoutedEventArgs e)
        {
            Function.SetIniValue(section: "aoe3", key: "gameDir", value: aoe3_Path_Textbox.Text);

            Function.RunExe(dir: aoe3_Path_Textbox.Text, filename: "age3y.exe", param: null);

            this.Close();
        }
        #endregion

#region aoe3 WINDOW METHOD
        private void aoe3_isEnabledControl(bool status)
        {
            aoe3_Launch_Button.IsEnabled = status;
        }
#endregion
    }
}
