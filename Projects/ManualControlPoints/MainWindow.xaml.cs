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
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace ManualControlPoints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public Patient p;
        public Course c;
        public ExternalPlanSetup ps;
        //List<Tuple<string,List<cpInfo>>> fields = new List<Tuple<string,List<cpInfo>>>();
        int fieldnum = 0;
        int cp_num = 0;
        List<fieldInfo> fields = new List<fieldInfo>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getCP_btn_Click(object sender, RoutedEventArgs e)
        {
            //loop through the ffields
            foreach (Beam b in ps.Beams)
            {
                //List<cpInfo> cps = new List<cpInfo>();
                fields.Add(new fieldInfo
                {
                    FieldId = b.Id,
                    cpInfos = new List<cpInfo>(),
                    ebmp = new ExternalBeamMachineParameters(b.TreatmentUnit.Id, b.EnergyModeDisplayName,
                    b.DoseRate, b.Technique.Id, null),
                    collAngle = b.ControlPoints.First().CollimatorAngle,
                    gantry = b.ControlPoints.First().GantryAngle,
                    couch = b.ControlPoints.First().PatientSupportAngle,
                    isocenter = b.IsocenterPosition,
                    bp = b.GetEditableParameters(),
                    MU = b.Meterset,
                });
                ControlPointCollection cpc = b.ControlPoints;
                //loop through control pointss and add to cps
                for (int cp = 0; cp < cpc.Count(); cp++)
                {
                    //create a new instance of cpc
                    fields.Last().cpInfos.Add(new cpInfo
                    {
                        cpId = cp,
                        cpDetails = new List<cpDetail>(),
                        meterSet = cpc[cp].MetersetWeight,
                    });
                    float[,] leaf_positions = cpc[cp].LeafPositions;
                    for (int i = 0; i < leaf_positions.GetLength(1); i++)
                    {
                        fields.Last().cpInfos.Last().cpDetails.Add(new cpDetail
                        {
                            leaffNum = i,
                            leafA = leaf_positions[0, i],//+10, //replace 10mm with leaf shifts
                            leafB = leaf_positions[1, i]// +10 //replace 10mm with leaf shifts.
                        });
                    }
                }
            }
            //make sure you got sosme controlpoints.
            if (fields.Count() != 0)
            {
                fieldnum = 0; cp_num = 0;
                //cp_dg.Items.Clear();
                cp_dg.ItemsSource = fields.First().cpInfos.First().cpDetails;
                cp_dg.Items.Refresh();
                fieldId.Content = fields.First().FieldId;
                cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                    fields.First().cpInfos.First().cpId, fields.First().cpInfos.First().meterSet);
            }
        }
        public class fieldInfo
        {
            public string FieldId { get; set; }
            public List<cpInfo> cpInfos { get; set; }
            //these parameters are just to be copied to the new fields
            public ExternalBeamMachineParameters ebmp { get; set; }
            public double collAngle { get; set; }
            public double gantry { get; set; }
            public double couch { get; set; }
            public VVector isocenter { get; set; }
            public BeamParameters bp { get; set; }
            public MetersetValue MU { get; set; }
        }
        public class cpInfo
        {
            public int cpId { get; set; }
            public double meterSet { get; set; }
            public List<cpDetail> cpDetails { get; set; }
        }
        public class cpDetail
        {
            public int leaffNum { get; set; }
            public float leafA { get; set; }
            public float leafB { get; set; }
        }
        private void prev_btn_Click(object sender, RoutedEventArgs e)
        {
            if (fields.Count() != 0)
            {
                fieldnum--;
                if (fieldnum < 0)
                {
                    fieldnum = fields.Count() - 1;
                }
                //cp_dg.Items.Clear();
                cp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
                cp_dg.Items.Refresh();
                fieldId.Content = fields.ElementAt(fieldnum).FieldId;
                cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
            }
        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
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
                cp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
                cp_dg.Items.Refresh();
                fieldId.Content = fields.ElementAt(fieldnum).FieldId;
                cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                    fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
            }
        }

        private void prevCP_btn_Click(object sender, RoutedEventArgs e)
        {
            cp_num--;
            if (cp_num < 0)
            {
                cp_num = fields.ElementAt(fieldnum).cpInfos.Count() - 1;
            }
            //fieldnum = fields.Count() - 1;
            //cp_dg.Items.Clear();
            cp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
            cp_dg.Items.Refresh();
            fieldId.Content = fields.ElementAt(fieldnum).FieldId;
            cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
        }

        private void nextCP_btn_Click(object sender, RoutedEventArgs e)
        {
            cp_num++;
            if (cp_num == fields.ElementAt(fieldnum).cpInfos.Count())
            {
                cp_num = 0;
            }
            //fieldnum = fields.Count() - 1;
            //cp_dg.Items.Clear();
            cp_dg.ItemsSource = fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpDetails;
            cp_dg.Items.Refresh();
            fieldId.Content = fields.ElementAt(fieldnum).FieldId;
            cpId.Content = String.Format("CP: {0}; MeterSet:{1}",
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).cpId,
                fields.ElementAt(fieldnum).cpInfos.ElementAt(cp_num).meterSet);
        }

        private void newPlan_btn_Click(object sender, RoutedEventArgs e)
        {
            Course c2 = p.AddCourse();
            c2.Id = course_txt.Text;
            ExternalPlanSetup ps2 = c2.AddExternalPlanSetup(ps.StructureSet);
            //doses should all be the same.
            //ps2.DosePerFraction = ps.DosePerFraction; //read only
            //ps2.TotalDose = ps.TotalDose;//read only
            ps2.SetPrescription(
                (int)ps.NumberOfFractions,
                ps.DosePerFraction,
                ps.TreatmentPercentage);
            ps2.PlanNormalizationValue = ps.PlanNormalizationValue;
            //ps2.TreatmentPercentage = ps.TreatmentPercentage;//read only
            
            //ps2.AddMLCBeam()
            ps2.Id = plan_txt.Text;
            List<KeyValuePair<string, MetersetValue>> mu_list = new List<KeyValuePair<string, MetersetValue>>();
            foreach (fieldInfo fi in fields)
            {
                Beam b2 = ps2.AddSlidingWindowBeam(fi.ebmp, fi.cpInfos.Select(x => x.meterSet),
                    fi.collAngle, fi.gantry, fi.couch, fi.isocenter);
                int cploc = 0;
                BeamParameters beamp = fi.bp;
                //b2.ApplyParameters(new BeamParameters(ControlPoint cp))
                //int cploc = 0;
                //foreach (cpInfo cpi in fi.cpInfos)
                double MU_old = 0;
                foreach(ControlPointParameters cpp in beamp.ControlPoints)
                {
                    float[,] leafPos = new float[2, 60];
                    int leafloc = 0;
                    cpInfo cpi = fi.cpInfos[cploc];
                    //get first MU point
                    if(cploc == 0) { MU_old = cpp.MetersetWeight; }
                    else
                    {
                        //interpolate halfway (just take an average for now.
                        //cpp.MetersetWeight = (mu_old + 
                        //meterset weight is read only. I must find the leaf position at the calculated meterset location.
                    }
                    /*foreach (cpDetail cpd in cpi.cpDetails)
                    {
                        leafPos[0, leafloc] = cpd.leafA;
                        leafPos[1, leafloc] = cpd.leafB;
                        leafloc++;
                    }*/
                    //start with the first leaf position, and then interoplate all the rest.
                    float leaf_oldA = 0;
                    float leaf_oldB = 0;
                    for (int i = 0; i<cpi.cpDetails.Count(); i++)
                    {
                        
                        if(i == 0)
                        {
                            leafPos[0, i] = cpi.cpDetails[i].leafA;
                            leafPos[1, i] = cpi.cpDetails[i + 1].leafB;
                            leaf_oldA = leafPos[0, i];
                            leaf_oldB = leafPos[1, i];
                            //mU_old = cpi.meterSet[i];
                        }
                        else
                        {
                            //let the interpolation begin.
                            //first the MU
                        }
                    }
                    //beamp.SetAllLeafPositions(leafPos);
                    //ControlPointParameters cpp =  beamp.ControlPoints[cploc]
                    cpp.LeafPositions = leafPos;
                    b2.ApplyParameters(beamp);
                    cploc++;
                }
                //calculate the dose for each of the fields.
                mu_list.Add(new KeyValuePair<string, MetersetValue>(b2.Id, fi.MU));
            }
            ps2.CalculateDoseWithPresetValues(mu_list);
            MessageBox.Show("plan_txt created successfully.");
        }
    }
}
