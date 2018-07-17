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
using EvilDICOM;
using EvilDICOM.RT;
using EvilDICOM.Core;
using System.IO;
using Microsoft.Win32;
using EvilDICOM.Core.Element;
using EvilDICOM.Core.IO;
using EvilDICOM.Core.Helpers;

namespace DicomPlanCreation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getcpp_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".dcm";
            string filename = "";
             if (ofd.ShowDialog() == true)
                 {
                filename = ofd.FileName;
                 }
            var dcm = DICOMObject.Read(filename);

        }

        private void getDevn_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void prev_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void next_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void prevCP_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextCP_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewPlan_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
