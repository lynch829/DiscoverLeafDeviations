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

namespace DicomPlanCreation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        internal List<FieldInfos> field_list;

        public Window1()
        {
            InitializeComponent();
        }

        private void loadDev_btn_Click(object sender, RoutedEventArgs e)
        {
            //check that all the textboxes are not null or empty
            foreach (StackPanel sp in field_files.Children)
            //for(kk=0; kk<fields_files.Children; kk++)
            {      /* Stackpanel sp= fields_files.Children[kk];*/
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
                        //for (int ss=0; ss< File.ReadAllLines(filename).Count(); ss++)############
                        {  /*string line= File.ReadAllLines(filename)[ss];#############*/
                            //each column is a leaf pair. delimited by tabs.
                            //the file reads b first (x1) and then a next (x2).
                            int colCount = 0;
                            foreach (string leafDev in line.Split('\t'))
                            //for (int ff=0; ff<line.Split('\t').Count(); ff++)#################
                            {
                                //string leafDev= line.Split('\t')[ff];#################
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
            mwindow.cpp_dg.ItemsSource = field_list.ElementAt(mwindow.fieldnum).cpInfos.ElementAt(mwindow.cp_num).cpDetails;

            mwindow.cpp_dg.Items.Refresh();
            this.Close();

        }
    }
}
