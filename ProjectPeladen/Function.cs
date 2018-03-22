using System;
using System.Diagnostics;
using IniParser;
using IniParser.Model;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace ProjectPeladen
{
    public class Function
    {
        public static void InitConfig()
        {
            FileIniDataParser Ini = new FileIniDataParser();
            /// cek exist ProjectPeladen_config.ini, nek gk ketemu bakal gawe blank
            if (File.Exists(System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\ProjectPeladen_config.ini") == false)
            {
                Directory.CreateDirectory(System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\");
                File.Create(System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\ProjectPeladen_config.ini").Close();
            }
        }

        public static void RunExe(String dir, String filename, String param, bool cpu0 = false)
        {
            Process RunExe = new Process(); //gawe instance
            RunExe.StartInfo.FileName = dir + filename;
            RunExe.StartInfo.WorkingDirectory = dir;
            RunExe.StartInfo.Arguments = param;
            RunExe.Start();

            if (cpu0 == true)
            {
                RunExe.ProcessorAffinity = (System.IntPtr)1;
            }

            RunExe.WaitForExit();
        }

        public static string GetIniValue(string section, string key)
        {
            FileIniDataParser Ini = new FileIniDataParser();

            IniData parsedData = Ini.ReadFile(System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\ProjectPeladen_config.ini");
            string readData = parsedData[section][key];

            return readData;
        }

        public static void SetIniValue(string section, string key, object value)
        {
            FileIniDataParser Ini = new FileIniDataParser();
            IniData parsedData = Ini.ReadFile(System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\ProjectPeladen_config.ini");

            parsedData[section][key] = value.ToString(); // SECTION,KEY = VALUE
            Ini.WriteFile((System.Environment.GetEnvironmentVariable("APPDATA") + @"\ProjectPeladen\ProjectPeladen_config.ini"), parsedData);
        }

        public static void WriteRegKey(string subkey, string key, object data) // @"SOFTWARE\blablabla, "", "telo1223"
        {
            /// nek pengen key (default), nganggo ""
            /// kabeh SUBKEY bakal ngarah ke LOCAL MACHINE 32 bit,dadine nek app di run neng 64bit, engko bakal read/write seko subkey WOW6432Node
            
            RegistryKey rkey = Registry.LocalMachine.CreateSubKey(subkey);
            rkey.SetValue(key, data.ToString());
        }

        public static void FileReplaceString(string file, string oldString, string newString)
        {
            string text = File.ReadAllText(file);
            text = text.Replace(oldString, newString);
            File.WriteAllText(file, text);
        }

        public static void ExtractEmbeddedResourceText(string resource, string extractTo)
        {
            // https://stackoverflow.com/questions/24584040/extract-embedded-resources-in-c-sharp
            // sementara dingo berbasis resource text, not binary
            System.IO.Stream fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ProjectPeladen.EmbeddedResources."+resource);

            DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(extractTo)); /// gawe folder sek
            string scriptContents = new StreamReader(fs).ReadToEnd();
            File.WriteAllText(extractTo, scriptContents);
        }




        /// <summary>
        /// ref: http://navwin.com/Topics/ExtractEmbeddedResource/ExtractEmbeddedResource.aspx
        /// </summary>
        /// <param name="targetAssembly"></param>
        /// <param name="resourceName"></param>
        /// <param name="filepath"></param>
        public static void WriteResourceToFile(Assembly targetAssembly, string resourceName, string filepath)

        {

            using (Stream s = targetAssembly.GetManifestResourceStream(targetAssembly.GetName().Name + "." + resourceName))

            {

                if (s == null)

                {

                    throw new Exception("Cannot find embedded resource '" + resourceName + "'");

                }

                byte[] buffer = new byte[s.Length];

                s.Read(buffer, 0, buffer.Length);

                using (BinaryWriter sw = new BinaryWriter(File.Open(filepath, FileMode.Create)))

                {

                    sw.Write(buffer);

                }

            }

        }

        public void SetAppConfig()
        {
            //System.Configuration.ConfigurationSettings.AppSettings.Add();
        }
    }
}