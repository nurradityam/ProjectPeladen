using System.Windows;
using Newtonsoft.Json.Linq;
using System.IO;
using Ionic.Zip;
using System.ComponentModel;

namespace ProjectPeladen
{
    public partial class window_dikaPackage : Window
    {
        public window_dikaPackage()
        {
            InitializeComponent();
            DikaPackage();
        }

        public void DikaPackage()
        {
            #region BROWSE DIKAPACKAGE FILE DIALOG
            string manifest = null;

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
            {
                Title = "Pilih lokasi file \"*.dikaPackage\"",
                Filter = "*.dikaPackage|*.dikaPackage"
            };

            switch (ofd.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.OK:
                    manifest = ofd.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    this.Close();
                    return;
            }
            #endregion

            #region READ MANIFEST
            JObject manifestFile = JObject.Parse(File.ReadAllText(manifest));

            string game = (string)manifestFile["game"];
            string addonValue = (string)manifestFile["addonValue"];
            string addonKey = (string)manifestFile["addonKey"];
            string extractDir = (string)manifestFile["extractDir"];

            if (extractDir == "getGameDir")
            {
                if (Function.GetIniValue(section: game, key: "gameDir") == null)
                {
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
                    {
                        Description = $"Pilih lokasi folder GameDir {game}"
                    };
                    if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        extractDir = fbd.SelectedPath + "\\"; //nambah slash "\"
                        Function.SetIniValue(section: game, key: "gameDir", value: extractDir); //ben ngeshare karo launcher game
                    }
                }
                else
                {
                    extractDir = Function.GetIniValue(section: game, key: "gameDir");
                }
            }
            #endregion

            #region EXTRACT PACKAGE
            string zipFileName = manifest + "Zip";

            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.ProgressChanged += (o, e) => { progressBar.Value = e.ProgressPercentage; };
            worker.DoWork += (o, e) =>
            {
                using (ZipFile zip = ZipFile.Read(zipFileName))
                {
                    int percentComplete = 0;

                    foreach (ZipEntry file in zip)
                    {
                        file.Extract(extractDir, ExtractExistingFileAction.OverwriteSilently);
                        percentComplete++;
                        worker.ReportProgress(percentComplete);

                        /// butuh invoke soale bedo thread
                        this.Dispatcher.Invoke(() =>
                        {
                            progressBar.Minimum = 0;
                            progressBar.Maximum = zip.Entries.Count;

                            if (progressBar.Value == progressBar.Maximum)
                            {
                                
                                #region REGISTER PACKAGE TO CONFIG
                                if (addonKey != "")
                                {
                                    Function.SetIniValue(game, addonKey, addonValue); /// REGISTER MANIFEST TO CONFIG FILE
                                }
                                #endregion

                                this.Close(); /// ngeclose window_dikapackage
                                MessageBox.Show("Selesai :)", "Extract Selesai");
                            }
                        });
                    }
                }
            };
            worker.RunWorkerAsync();
            #endregion
        }
    }
}
