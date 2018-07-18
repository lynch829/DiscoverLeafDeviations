﻿using System;
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
using System.Reflection;
using EvilDICOM.Core.Interfaces;

namespace DicomPlanCreation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int fieldnum = 0;
        public int cp_num = 0;
        public List<FieldInfos> fields = new List<FieldInfos>();
        private List<IDICOMElement> collimator;

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

            var sel = dcm.GetSelector();
            foreach (var field in sel.BeamSequence.Items)
            {
                fields.Add(new FieldInfos
                {
                    FieldId = field.FindFirst(TagHelper.PatientID.ToString();
                collimator = field.FindAll(new Tag[] { TagHelper.ControlPointSequence, TagHelper.BeamLimitingDeviceAngle });
                var gantry = field.FindAll(new Tag[] { TagHelper.ControlPointSequence, TagHelper.GantryAngle }).First();
                var field_selector = field.GetSelector();
                int control_point_number = 0;
                foreach (var cp in field_selector.ControlPointSequence.Data_)
                {
                    var cp_selector = cp.GetSelector();
                    int leaf_number = 0;
                    int leaf_row = 0;
                    float[,] leaf_positions = new float[2, 60];

                    foreach (var leaf_pos in cp_selector.LeafJawPositions_.Last().DData_)
                    {
                        //leaf p[ositions
                        leaf_positions[leaf_row, leaf_number % 60] = (float)Convert.ToDouble(leaf_pos);
                        leaf_number++;
                        if (leaf_number == 59) { leaf_row = 1; }
                    }

                    control_point_number++;
                }

            });
        }






        private void getDevn_btn_Click(object sender, RoutedEventArgs e)
        {
            int field_count = fields.Count();
            if (field_count == 0)
            {
                MessageBox.Show("No fields currently found, Pleasee grab the plan before searching for the deviations");
            }
            else
            {
                //MessageBox.Show($"Detected {field_count} fields. Grab all the deviations associated with these fields.");
                var devWindow = new Window1();
                devWindow.field_list = fields;
                devWindow.Show();

            }

        }

        private void prev_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (fields.Count() != 0)
            {
                fieldnum--;
                if (fieldnum < 0)
                {
                    fieldnum = fields.Count() - 1;
                }
                //cp_dg.Items.Clear();
                cpp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
                cpp_dg.Items.Refresh();
                fieldID.Content = fields.ElementAt(fieldnum).FieldId;
                cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
            }
        }

        private void next_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (fields.Count() != 0)
            {
                fieldnum++;
                if (fieldnum == fields.Count())
                {
                    fieldnum = 0;
                }
                //fieldnum = fields.Count() - 1;
                //cp_dg.Items.Clear();
                cpp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
                cpp_dg.Items.Refresh();
                fieldID.Content = fields.ElementAt(fieldnum).FieldId;
                cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
            }
        }

    

        private void prevCP_Btn_Click(object sender, RoutedEventArgs e)
        {
            cp_num--;
            if (cp_num < 0)
            {
                cp_num = fields.ElementAt(fieldnum).cpInfos.Count() - 1;
            }
            //fieldnum = fields.Count() - 1;
            //cp_dg.Items.Clear();
            cpp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
            cpp_dg.Items.Refresh();
            fieldID.Content = fields.ElementAt(fieldnum).FieldId;
            cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
        }

        private void nextCP_Btn_Click(object sender, RoutedEventArgs e)
        {
            cp_num++;
            if (cp_num == fields.ElementAt(fieldnum).cpInfos.Count())
            {
                cp_num = 0;
            }
            //fieldnum = fields.Count() - 1;
            //cp_dg.Items.Clear();
            cpp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
            cpp_dg.Items.Refresh();
            fieldID.Content = fields.ElementAt(fieldnum).FieldId;
            cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
        }

        private void NewPlan_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

