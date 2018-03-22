using System.Windows;

namespace ProjectPeladen
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Function.InitConfig();
        }

        public void Main_GameSelect_Click(object sender, RoutedEventArgs e)
        {
            Function.InitConfig(); /// inisialisasi program: load last config
            switch (Main_ComboBox.Text)
            {
                case "Battlefield 2":
                    Battlefield2_window Battlefield2_window = new Battlefield2_window();
                    Battlefield2_window.Show();
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

        private void Button_FunctionTest_Click(object sender, RoutedEventArgs e)
        {
            //f.GetPPconfigValue();
        }
    }
}