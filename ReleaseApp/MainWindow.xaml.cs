using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace ReleaseApp
{

  
    public partial class MainWindow : Window
    {
        List<CheckBox> checkBoxList;
        SortedDictionary<string, string> market;
        SortedDictionary<string, string> mode;
        Dictionary<string, string> BrandtoSoft;
        List<string> marketIndex;
        public int counter2 = 0;

        public MainWindow()
        {
            InitializeComponent();
            updateLabels();
            verifyInstalledBrands();
            initializeElements();
            bindMarketDictionary();
            bindlogmode();
            string path = Directory.GetCurrentDirectory();
            //MessageBox.Show( path);
            imgSonic.Source = new BitmapImage(new Uri($"{path}/sonic2.png", UriKind.Absolute));
            imgOticon.Source = new BitmapImage(new Uri($"{path}/oticon2.png", UriKind.Absolute));
            imgBernafon.Source = new BitmapImage(new Uri($"{path}/bernafon2.png", UriKind.Absolute));
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnLogMode.IsEnabled = false;
            btnHattori.IsEnabled = false;
            btnuninstal.IsEnabled = false;
            btnDeletelogs.IsEnabled = false;
            btnFS.IsEnabled = false;

        }
        
        void bindMarketDictionary()
        {
            market = new SortedDictionary<string, string>
            {
                { "Australia (AU)", "AU"},
                { "Denmark (DK)", "DK"},
                { "Germany (DE)", "DE"},
                { "United Kingdom (UK)", "UK"},
                { "United States (US)", "US"},
                { "Canada (CA)", "CA"},
                { "Spain (ES)", "ES"},
                { "New Zeland (NZ)", "NZ"},
                { "Switzerland (CH)", "CH"},
                { "Finland (FI)", "FI"},
                { "France (FR)", "FR"},
                { "Italy (IT)", "IT"},
                { "Japan (JP)", "JP"},
                { "Korea (KR)", "KR"},
                { "Norway (NO)", "NO"},
                { "Nederland (NL)", "NL"},
                { "Brazil (BR)", "BR"},
                { "Poland (PL)", "PL"},
                { "Portugal (PT)", "PT"},
                { "Sweden (SE)", "SE"},
                { "Singapore (SG)", "SG"},
                { "PRC China (CN)", "CN"},
                { "South Africa (ZA)", "ZA"},
                { "", "NA"}
            };

            marketIndex = new List<string>()
            {
                {"NA"},
                {"AU"},
                {"BR"},
                {"CA"},
                {"DK"},
                {"FI"},
                {"FR"},
                {"DE"},
                {"IT"},
                {"JP"},
                {"KR"},
                {"NL"},
                {"NZ"},
                {"NO"},
                {"PL"},
                {"PT"},
                {"CN"},
                {"SG"},
                {"ZA"},
                {"ES"},
                {"SE"},
                {"CH"},
                {"UK"},
                {"US"},
                {"he"}
            };

            BrandtoSoft = new Dictionary<string, string>()
            {
                {"Oticon", "Genie"},
                {"Bernafon", "Oasis"},
                {"Sonic", "ExpressFit"}
            };

            cmbMarket.ItemsSource = market;
            cmbMarket.DisplayMemberPath = "Key";
            cmbMarket.SelectedValuePath = "Value";        
        }


        void deleteLogs(string brand_name)
        {

           string DirectoryName = $"C:/ProgramData/{brand_name}/{BrandtoSoft[brand_name]}/Logfiles/";


            System.IO.DirectoryInfo di = new DirectoryInfo(DirectoryName);
            try
            {

                string tempName;
                foreach (FileInfo file in di.GetFiles())
                {

                    tempName = $"{di.ToString()}/{file.Name.ToString()}";
                    File.SetAttributes(tempName, FileAttributes.Normal);
                    File.Delete($"{di.ToString()}/{file.Name.ToString()}");
                }
              
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Cos sie zepsulo");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Cos sie mocno zepsulo");
            }

        }


        void bindlogmode()
        {
            mode = new SortedDictionary<string, string>
            {
                { "All", "All"},
                { "Debug", "DEBUG"},
                { "Error", "ERROR"}
            };

            marketIndex = new List<string>()
            {
                {"All"},
                {"DEBUG"},
                {"ERROR"}
            };

            cmbLogMode.ItemsSource = mode;
            cmbLogMode.DisplayMemberPath = "Key";
            cmbLogMode.SelectedValuePath = "Value";
        }


        void handleSelectedMarket()
        {
            int counter = 0;
            bool show = false;
            string[] markets = new string[3];
            foreach (CheckBox checkbox in checkBoxList)
            {      
                if ((bool)checkbox.IsChecked)
                {
                    markets[counter] = getData($"C:/ProgramData/{checkbox.Name}/Common/ManufacturerInfo.XML");
                    counter++;
                }
            }
            int a;
            foreach (CheckBox checkbox in checkBoxList)
            {      
                if ((bool)checkbox.IsChecked && counter == 1)
                {
                    a = marketIndex.IndexOf(getData($"C:/ProgramData/{checkbox.Name}/Common/ManufacturerInfo.XML"));
                    cmbMarket.SelectedIndex = marketIndex.IndexOf(getData($"C:/ProgramData/{checkbox.Name}/Common/ManufacturerInfo.XML"));
                }
                else if (counter > 1)
                {
                    for (int i=0; i< counter; ++i)
                    {
                        if (markets[i] == markets[counter-1])
                        {
                            show = true;
                        }
                        else
                        {
                            show = false;
                            break;
                        }
                    }
                    if (show)
                    {
                        cmbMarket.SelectedIndex = marketIndex.IndexOf(getData($"C:/ProgramData/{checkbox.Name}/Common/ManufacturerInfo.XML"));
                    }
                    else
                    {
                        cmbMarket.SelectedIndex = 0;
                    }
                }
                else if (counter == 0)
                {
                    cmbMarket.SelectedIndex = 0;
                }          
            }
        }

        void initializeElements()
        {
            checkBoxList = new List<CheckBox>()
            {
                Oticon,
                Bernafon,
                Sonic
            };

            string[] sources = 
            {
                "C:/ProgramData/Oticon/Common/ManufacturerInfo.XML",
                "C:/ProgramData/Bernafon/Common/ManufacturerInfo.XML",
                "C:/ ProgramData/Sonic/Common/ManufacturerInfo.XML"
            };
        }

        String getData(string name)
        {
            String line = String.Empty;
            int counter = 0;
            try
            {
                using (StreamReader sr = new StreamReader(name))
                {
                    while (counter != 4)
                    {
                        line = sr.ReadLine();
                        counter++;
                    }
                    if (line[15] == 'e')
                    {
                        return "Defukt";
                    }
                    return $"{line[14]}{line[15]}";
                }
            } 
            catch(FileNotFoundException)
            {
                return "";
            }
            catch(DirectoryNotFoundException)
            {
                return "";
            }
            catch(NullReferenceException)
            {
                return "";
            }
        }

        bool checkBoxes()
        {
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked)
                {
                    return true;
                }
            }
            return false;
        }

    

        void updateLabels()
        {
            lblG.Foreground = new SolidColorBrush(Colors.Black);
            lblO.Foreground = new SolidColorBrush(Colors.Black);
            lblE.Foreground = new SolidColorBrush(Colors.Black);
            lblG.Content = getData("C:/ProgramData/Oticon/Common/ManufacturerInfo.XML");
            lblO.Content = getData("C:/ProgramData/Bernafon/Common/ManufacturerInfo.XML");
            lblE.Content = getData("C:/ProgramData/Sonic/Common/ManufacturerInfo.XML");
        }

        void changeMarket(string source)
        {
            string[] oldFile;
            int counter = 0;

            try
            {
                oldFile = File.ReadAllLines(source);
                using (StreamWriter sw = new StreamWriter(source))
                {
                    foreach (var line in oldFile)
                    {
                        if (counter == 3)
                        {
                            sw.WriteLine($"  <MarketName>{cmbMarket.SelectedValue}</MarketName>");
                        }
                        else
                        {
                            sw.WriteLine(line);
                        }
                        counter++;
                    }
                }
            }     
            catch(FileNotFoundException)
            {

            }
            catch(DirectoryNotFoundException)
            {

            }
            catch(NullReferenceException)
            {

            }
        }



        bool changeLog_Mode(string source)
        {
            string[] oldFile;
            int counter = 0;
            bool message=false;
            try
            {
                oldFile = File.ReadAllLines(source);
                using (StreamWriter sw = new StreamWriter(source))
                {
                    foreach (var line in oldFile)
                    {
                        if (counter == 23) //tryb logow
                        {
                            sw.WriteLine($"      <level value=\"{cmbLogMode.SelectedValue}\"/>");
                            
                        }


                        if (counter == 34) //rozmiar plikow
                        {
                            if (cmbLogMode.SelectedValue.ToString() == "ERROR")
                            {
                                sw.WriteLine($"      <maximumFileSize value=\"{5}MB\"/>");
                              
                            }
                            if (cmbLogMode.SelectedValue.ToString() == "DEBUG")
                            {
                                sw.WriteLine($"      <maximumFileSize value=\"{10}MB\"/>");
                               
                            }
                            if (cmbLogMode.SelectedValue.ToString() == "ALL")
                            {
                                sw.WriteLine($"      <maximumFileSize value=\"{20}MB\"/>");
                                
                            }

                        }

                        if (counter == 37) //ilosc plikow
                        {
                            if (cmbLogMode.SelectedValue.ToString() == "ERROR")
                            {
                                sw.WriteLine($"      <maxSizeRollBackups value=\"{5}\"/>");
                            }
                            if (cmbLogMode.SelectedValue.ToString() == "DEBUG")
                            {
                                sw.WriteLine($"      <maxSizeRollBackups value=\"{10}\"/>");
                            }
                            if (cmbLogMode.SelectedValue.ToString() == "ALL")
                            {
                                sw.WriteLine($"      <maxSizeRollBackups value=\"{20}\"/>");
                            }

                        }
                        
                        if (counter != 23 && counter != 37 && counter != 34)
                        {
                            sw.WriteLine(line);
                        }
                        
                        counter++;
                    }
                }
                
                message = true;
                return message;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File Not Found");
                return message;
            }
            catch (DirectoryNotFoundException)
            {
                return message;
            }
            catch (NullReferenceException)
            {
                return message;
            }
        }


        bool verifyInstanceOfExec(string name)
        {
            foreach (CheckBox checkbox in checkBoxList)
            {
                if (checkbox.Name == name)
                {
                    
                    if ( File.Exists($"C:/Program Files (x86)/{name}/{BrandtoSoft[checkbox.Name]}/{BrandtoSoft[checkbox.Name]}2/{BrandtoSoft[checkbox.Name]}.exe"))
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;

        }
        void verifyInstalledBrands()
        {
            if (!Directory.Exists("C:/ProgramData/Oticon"))
            {
                Oticon.IsEnabled = false;
                lblG.Foreground = new SolidColorBrush(Colors.Red);
                lblG.Content = "FS not installed";
                Oticon.IsChecked = false;
            }
            if (!Directory.Exists("C:/ProgramData/Bernafon"))
            {
                Bernafon.IsEnabled = false;
                lblO.Foreground = new SolidColorBrush(Colors.Red);
                lblO.Content = "FS not installed";
                Bernafon.IsChecked = false;
            }
            if (!Directory.Exists("C:/ProgramData/Sonic"))
            {
                Sonic.IsEnabled = false;
                lblE.Foreground = new SolidColorBrush(Colors.Red);
                lblE.Content = "FS not installed";
                Sonic.IsChecked = false;
            }
        }

        void deleteTrash(string DirectoryName)
        {

            string tempName;
            System.IO.DirectoryInfo di = new DirectoryInfo(DirectoryName);
            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    tempName = $"{di.ToString()}/{file.Name.ToString()}";
                    File.SetAttributes(tempName, FileAttributes.Normal);
                    File.Delete($"{di.ToString()}/{file.Name.ToString()}");
                }
                foreach (DirectoryInfo directory in di.GetDirectories())
                {
                    deleteTrash(di.ToString()+"/"+directory.ToString());
                    Directory.Delete(di.ToString() + "/" + directory.ToString());
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Cos sie zepsulo");
            }
            catch(DirectoryNotFoundException)
            {
                MessageBox.Show("Cos sie mocno zepsulo");
            }
        }

        bool checkRunningProcess(string name)
        {
            Process[] proc = Process.GetProcessesByName(name);
            Process[] localAll = Process.GetProcesses();

            foreach (Process item in localAll)
            {
                string tmop = item.ProcessName;
                if (tmop == name)
                {
                    return false;
                }
            }
            return true;
        }
        

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool message = false;
            string[] marki = { "Genie","Oasis","EXPRESSfit" };
            int count3 = 0;
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked)
                {
                    if (checkRunningProcess(marki[count3]))
                    {
                        changeMarket($"C:/ProgramData/{checkbox.Name}/Common/ManufacturerInfo.XML");
                    }
                    else
                    {
                        message = true;
                    }
                }
                count3++;
            }
            if (message)
            {
                MessageBox.Show("Close fitting software", "Brand", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            updateLabels();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool fined = false;
            if ((bool)Oticon.IsChecked && checkRunningProcess("Genie") && !verifyInstanceOfExec("Oticon"))
            {
                deleteTrash("C:/ProgramData/Oticon");
                deleteTrash("C:/Program Files (x86)/Oticon");
                Directory.Delete("C:/ProgramData/Oticon");
                Directory.Delete("C:/Program Files (x86)/Oticon");
                MessageBox.Show("Trash deleted successfully!", "deleteTrash", MessageBoxButton.OK, MessageBoxImage.Information);
                fined = true;
            }
            if ((bool)Bernafon.IsChecked && checkRunningProcess("Oasis") && !verifyInstanceOfExec("Bernafon"))
            {
                deleteTrash("C:/ProgramData/Bernafon");
                deleteTrash("C:/Program Files (x86)/Bernafon");
                Directory.Delete("C:/ProgramData/Bernafon");
                Directory.Delete("C:/Program Files (x86)/Bernafon");
                MessageBox.Show("Trash deleted successfully!", "deleteTrash", MessageBoxButton.OK, MessageBoxImage.Information);
                fined = true;
            }
            if ((bool)Sonic.IsChecked && checkRunningProcess("Expressfit") && !verifyInstanceOfExec("Sonic"))
            {
                deleteTrash("C:/ProgramData/Sonic");
                deleteTrash("C:/Program Files (x86)/Sonic");
                Directory.Delete("C:/ProgramData/Sonic");
                Directory.Delete("C:/Program Files (x86)/Sonic");
                MessageBox.Show("Trash deleted successfully!", "deleteTrash", MessageBoxButton.OK, MessageBoxImage.Information);
                fined = true;
            }
            if (!fined)
            {
                MessageBox.Show("Delete FS", "Brand", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
            verifyInstalledBrands();
            updateLabels();
        }

        private void btnFS_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked && File.Exists($"C:/Program Files (x86)/{checkbox.Name}/{BrandtoSoft[checkbox.Name]}/{BrandtoSoft[checkbox.Name]}2/{BrandtoSoft[checkbox.Name]}.exe") && checkRunningProcess(BrandtoSoft[checkbox.Name]))
                {
                 Process.Start($"C:/Program Files (x86)/{checkbox.Name}/{BrandtoSoft[checkbox.Name]}/{BrandtoSoft[checkbox.Name]}2/{BrandtoSoft[checkbox.Name]}.exe");
                }
            }
            updateLabels();
            verifyInstalledBrands();
        }

        private void btnHattori_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked && File.Exists($"C:/Program Files (x86)/{checkbox.Name}/FirmwareUpdater/FirmwareUpdater.exe"))
                {
                    Process.Start(($"C:/Program Files (x86)/{checkbox.Name}/FirmwareUpdater/FirmwareUpdater.exe"));
                }
            }
            if ((bool)Oticon.IsChecked)
            {
                if (File.Exists("C:/Program Files (x86)/Oticon/FirmwareUpdater/FirmwareUpdater/FirmwareUpdater.exe"))
                {
                    Process.Start("C:/Program Files (x86)/Oticon/FirmwareUpdater/FirmwareUpdater/FirmwareUpdater.exe");
                }
            }

            updateLabels();
            verifyInstalledBrands();
        }

        private void btnuninstal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Soon ...");
        }

        private void Brand_Unchecked(object sender, RoutedEventArgs e)
        {
            handleSelectedMarket();
            if (!checkBoxes())
            {
                btnHattori.IsEnabled = false;
                btnFS.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                btnLogMode.IsEnabled = false;
                btnDeletelogs.IsEnabled = false;
            }
        }

        private void Brand_Checked(object sender, RoutedEventArgs e)
        {
            handleSelectedMarket();
            btnHattori.IsEnabled = true;
            btnFS.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnLogMode.IsEnabled = true;
            btnDeletelogs.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            if (checkBoxes())
            {
                foreach (CheckBox checkbox in checkBoxList)
                {
                    int tmp = counter2 % 2;
                    if (tmp == 0) {
                        if (checkbox.IsEnabled == true)
                        {
                            checkbox.IsChecked = true;
                        }

                    }
                    else
                    {
                        if (checkbox.IsEnabled == true)
                        {
                            checkbox.IsChecked = false;
                        }
                    }
                  
                }
            }
            else
            {
                foreach (CheckBox checkbox in checkBoxList)
                {
                    if (checkbox.IsEnabled == true)
                    {
                        checkbox.IsChecked = true;
                    }
                }
            }


            counter2++;
        }

        private void cmbMarket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnChange_mode_log(object sender, RoutedEventArgs e)
        {
            bool message = false;
            bool changed = false;
            string[] marki = { "Genie", "Oasis", "EXPRESSfit" };
            int count3 = 0;
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked)
                {
                    if (checkRunningProcess(marki[count3]))
                    {
                        changed = changeLog_Mode($"C:/Program Files (x86)/{checkbox.Name}/{marki[count3]}/{marki[count3]}{"2"}/Configure.log4net"); 
                    }
                    else
                    {
                        message = true;
                    }
                }
                count3++;
            }
            if (message)
            {
                MessageBox.Show("Close fitting software", "Brand", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (changed && !message)
            {
                MessageBox.Show("Updated");
            }
            if(!changed && !message)
            {
                MessageBox.Show("Error, check file");
            }
            updateLabels();
        }

        private void btnDelete_logs(object sender, RoutedEventArgs e)
        {
            bool message = false;
            bool message2 = false;
            bool deleted = false;
            foreach (CheckBox checkbox in checkBoxList)
            {
                if ((bool)checkbox.IsChecked) //analiza => jeden zaznaczony dwa nie 
                {
                    if (checkRunningProcess(BrandtoSoft[checkbox.Name]))
                    {
                       deleteLogs(checkbox.Name.ToString());
                       deleted = true;
                    }
                    else
                    {
                        message = true;
                    }
                    
                }
                else
                {
                    message2 = true;
                    
                }
            }
            if (message2 && !deleted)
            {
                MessageBox.Show("Select fitting software", "Brand", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (message)
            {
                MessageBox.Show("Close fitting software", "Brand", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            updateLabels();
            verifyInstalledBrands();
        }
    }

}
