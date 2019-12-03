using System;
using System.Windows;

namespace ProjectPeladen
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // populate comboBox
            Main_SelectGame_ComboBox.Items.Add("Battlefield 2");
            Main_SelectGame_ComboBox.Items.Add("Battlefield Bad Company 2");
            Main_SelectGame_ComboBox.Items.Add("Company of Heroes");

            // load last selected game from config
            Main_SelectGame_ComboBox.SelectedIndex = Properties.Settings.Default.Menu_LastSelectedGame;
        }

        public void Main_GameSelect_Click(object sender, RoutedEventArgs e)
        {
            // save current config
            Properties.Settings.Default.Menu_LastSelectedGame = Main_SelectGame_ComboBox.SelectedIndex;
            Properties.Settings.Default.Save();

            switch (Main_SelectGame_ComboBox.Text)
            {
                case "Battlefield 2":
                    Battlefield2_window Battlefield2_window = new Battlefield2_window();
                    Battlefield2_window.Show();
                    this.Close();

                    break;

                case "Battlefield Bad Company 2":
                    Bfbc2_window Bfbc2_window = new Bfbc2_window();
                    Bfbc2_window.Show();
                    this.Close();

                    break;

                case "Company of Heroes":
                    CoH_window CoH_window = new CoH_window();
                    CoH_window.Show();
                    this.Close();

                    break;

                case "ArmA Gold":
                    ArmAgold_window ArmAgold_window = new ArmAgold_window();
                    ArmAgold_window.Show();
                    this.Close();

                    break;

                case "Age of Empires III":
                    aoe3_window aoe3_window = new aoe3_window();
                    aoe3_window.Show();
                    this.Close();

                    break;
            }
        }

        public void Main_dikaPackageInstall_Button_Click(object sender, RoutedEventArgs e)
        {
            window_dikaPackage dp = new window_dikaPackage();
            dp.Show();
        }
    }
}