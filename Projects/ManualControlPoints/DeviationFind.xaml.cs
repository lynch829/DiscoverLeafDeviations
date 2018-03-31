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
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using ManualControlPoints.Models;
using Microsoft.Win32;
using System.IO;

namespace ManualControlPoints
{
    /// <summary>
    /// Interaction logic for DeviationFind.xaml
    /// </summary>
    public partial class DeviationFind : Window
    {
        public List<FieldInfo> field_list = new List<FieldInfo>();
        public DeviationFind()
        {
            InitializeComponent();
        }

        private void loadDev_btn_Click(object sender, RoutedEventArgs e)
        {
            //check that all the textboxes are not null or empty
            foreach (StackPanel sp in field_files.Children)
            {
                if (sp.Children.OfType<Button>().Count() != 0)
                {
                    if (String.IsNullOrEmpty(sp.Children.OfType<TextBox>().First().Text))
                    {
                        MessageBox.Show($"Missing file for field {sp.Children.OfType<TextBlock>().First().Text}");
                        return;
                    }
                    else
                    {
                        //first load up the file with the name of the file from the textbox.
                        string filename = sp.Children.OfType<TextBox>().First().Text;
                        //each row of the file is a control point. 
                        int cpnum = 0;
                        foreach (string line in File.ReadAllLines(filename))
                        {
                            //each column is a leaf pair. delimited by tabs.
                            //the file reads b first (x1) and then a next (x2).
                            int colCount = 0;
                            foreach (string leafDev in line.Split('\t'))
                            {
                                //now find that control point associated with that number
                                if (colCount < 60)
                                {
                                    //this is bank b
                                    field_list.First(x => x.FieldId == sp.Children.OfType<TextBlock>().First().Text)
                                        .cpInfos.First(x => x.cpId == cpnum).cpDetails.First(x => x.leaffNum == colCount).deviationB =
                                        leafDev == "NaN" ? 0 : Convert.ToDouble(leafDev);
                                    //you will just add these to the mlc positions. Since they are both in IEC61217 scale.
                                }
                                else
                                {
                                    //this is bank a.
                                    field_list.First(x => x.FieldId == sp.Children.OfType<TextBlock>().First().Text)
                                        .cpInfos.First(x => x.cpId == cpnum).cpDetails.First(x => x.leaffNum == colCount - 60).deviationA =
                                        leafDev == "NaN" ? 0 : Convert.ToDouble(leafDev);
                                }
                                colCount++;
                            }
                            cpnum++;
                        }
                    }
                }
            }
            //push the field_list back into the MainWindow class.
            var mwindow = new MainWindow();
            mwindow.fields = field_list;
            //set the itemssource of the 
            //cp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
            //cp_dg.Items.Refresh();
            mwindow.cp_dg.ItemsSource = field_list.ElementAt(mwindow.fieldnum).cpInfos.ElementAt(mwindow.cp_num).cpDetails;
            mwindow.cp_dg.Items.Refresh();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //List<FieldInfo> fields_list = ManualControlPoints.fields;; 
            StackPanel sp1 = new StackPanel();
            sp1.Orientation = Orientation.Horizontal;
            TextBlock tb = new TextBlock();
            tb.Text = "Field ID";
            tb.FontSize = 18; tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.Margin = new Thickness(5);
            TextBlock tb2 = new TextBlock();
            tb.Text = "Browse to file";
            tb2.FontSize = 18; tb.HorizontalAlignment = HorizontalAlignment.Right;
            tb2.Margin = new Thickness(5);
            sp1.Children.Add(tb); sp1.Children.Add(tb2);
            field_files.Children.Add(sp1);
            foreach (FieldInfo field in field_list)
            {
                //create a textblock and a box to find the file and another textbox to put the file name.
                StackPanel sp2 = new StackPanel();
                sp2.Orientation = Orientation.Horizontal;
                TextBlock tb1 = new TextBlock();
                tb1.Text = field.FieldId;
                tb1.Margin = new Thickness(10, 10, 0, 0);
                tb1.HorizontalAlignment = HorizontalAlignment.Left;
                sp2.Children.Add(tb1);
                //button to bbrowse to file.
                Button btn = new Button();
                btn.Name = tb1.Text.Replace(" ", "") + "_btn";
                btn.Content = "...";
                btn.Width = 50;
                btn.Height = 30;
                btn.Margin = new Thickness(30, 10, 0, 0);
                btn.Click += Btn_Click;
                sp2.Children.Add(btn);
                //textboxial
                TextBox tbx = new TextBox();
                tbx.HorizontalAlignment = HorizontalAlignment.Right;
                tbx.Width = 150; tbx.Height = 30;
                tbx.Margin = new Thickness(0, 10, 10, 0);
                tbx.Name = tb1.Text.Replace(" ", "") + "_txt";
                sp2.Children.Add(tbx);
                field_files.Children.Add(sp2);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files|*.csv|Text Files|*.txt";
            if (ofd.ShowDialog() == true)
            {
                foreach (StackPanel sp in field_files.Children)
                {

                    //just findd the ssender.
                    if (sp.Children.OfType<Button>().Count() != 0)
                    {
                        if (sp.Children.OfType<Button>().First() == sender)
                        {
                            sp.Children.OfType<TextBox>().First().Text = ofd.FileName;
                        }
                    }
                }
            }
        }
    }
}
