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
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using ManualControlPoints.Models;

namespace ManualControlPoints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public double no_norm = 100;
        public double val=0;
        public Patient p;
        public Course c;
        public ExternalPlanSetup ps;
        //List<Tuple<string,List<cpInfo>>> fields = new List<Tuple<string,List<cpInfo>>>();
        public int fieldnum = 0;
        public int cp_num = 0;
        public List<FieldInfo> fields = new List<FieldInfo>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getCP_btn_Click(object sender, RoutedEventArgs e)
        {
            //loop through the ffields
            foreach (Beam b in ps.Beams)
                //for (int ll = 0; ll < ps.Beams.Count(); ll++)
            {
                //Beam b = ps.Beams[ll];
                //List<cpInfo> cps = new List<cpInfo>();
                fields.Add(new FieldInfo
                {
                    FieldId = b.Id,
                    cpInfos = new List<cpInfo>(),
                    Ebmp = new ExternalBeamMachineParameters(b.TreatmentUnit.Id, b.EnergyModeDisplayName,
                    b.DoseRate, b.Technique.Id, null),
                    collAngle = b.ControlPoints.First().CollimatorAngle,
                    gantry = b.ControlPoints.First().GantryAngle,
                    couch = b.ControlPoints.First().PatientSupportAngle,
                    isocenter = b.IsocenterPosition,
                    bp = b.GetEditableParameters(),
                    MU = b.Meterset,
                    gantry_direction = b.GantryDirection,
                    gantry_stop = b.GantryDirection == 0 ? b.ControlPoints.First().GantryAngle :
                    b.ControlPoints.Last().GantryAngle,

                });
                if (b.Applicator != null) { fields.Last().applicator = b.Applicator; }
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
                            //1 is a and 0 is b.
                            leafA = leaf_positions[1, i],//+10, //replace 10mm with leaf shifts
                            leafB = leaf_positions[0, i]// +10 //replace 10mm with leaf shifts.
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
            Course c2 = null;
            if (p.Courses.Where(x => x.Id == course_txt.Text).Count() == 0)
            {
                c2 = p.AddCourse();
                c2.Id = course_txt.Text;
                

            }
            else { c2 = p.Courses.First(x => x.Id == course_txt.Text); }
            ExternalPlanSetup ps2 = c2.AddExternalPlanSetup(ps.StructureSet);
            //doses should all be the same.t 
            //ps2.DosePerFraction = ps.DosePerFraction; //read only
            //ps2.TotalDose = ps.TotalDose;//read only
            ps2.SetPrescription(
                (int)ps.NumberOfFractions,
                ps.DosePerFraction,
                ps.TreatmentPercentage);
            //I've chnaged this down below. Currently, the calculation will take place with preset monitor units
            //making it the same as the plan its copied from but then I scale the normaliztation factor by 1.3% because the discover is not in the beam.
            ps2.PlanNormalizationValue = val*ps.PlanNormalizationValue;

            //val = (double)Convert.ToDouble(Input.Text);
            bool valid = double.TryParse(Input.Text.ToString(), out val);
            ps2.PlanNormalizationValue = val + no_norm;
            //ps2.TreatmentPercentage = ps.TreatmentPercentage;//read only

            //ps2.AddMLCBeam()
            ps2.Id = plan_txt.Text;
            List<KeyValuePair<string, MetersetValue>> mu_list = new List<KeyValuePair<string, MetersetValue>>();
            /*foreach (FieldInfo fi in fields)########################*/
            for (int t = 0; t < fields.Count(); t++)
            {
                FieldInfo fi = fields[t];
                Beam b2;
                if (fi.gantry_direction == 0)
                {
                    b2 = ps2.AddSlidingWindowBeam(fi.Ebmp, fi.cpInfos.Select(x => x.meterSet),
                        fi.collAngle, fi.gantry, fi.couch, fi.isocenter);
                }
                else
                {
                    b2 = ps2.AddVMATBeam(fi.Ebmp, fi.cpInfos.Select(x => x.meterSet), fi.collAngle,
                        fi.gantry, fi.gantry_stop, fi.gantry_direction, fi.couch, fi.isocenter);
                }
                int cploc = 0;
                //if (fi.applicator != null) { b2.Applicator = fi.applicator; }
                BeamParameters beamp = fi.bp;
                //b2.ApplyParameters(new BeamParameters(ControlPoint cp))
                //int cploc = 0;
                //foreach (cpInfo cpi in fi.cpInfos)
                //double MU_old = 0;
                foreach (ControlPointParameters cpp in beamp.ControlPoints)
                    //for(int xx=0; xx< beamp.ControlPoints.Count(); xx++)
                {  /* ControlPointParameters cpp= beamp.ControlPoints[xx];*/
                    float[,] leafPos = new float[2, 60];
                    int leafloc = 0;
                    double x1 = cpp.JawPositions.X1;
                    double x2 = cpp.JawPositions.X2;
                    cpInfo cpi = fi.cpInfos[cploc];
                    
                    //foreach (cpDetail cpd in cpi.cpDetails)#####################
                         for(int dd=0; dd< cpi.cpDetails.Count(); dd++) 
                    {     cpDetail cpd= cpi.cpDetails[dd]; 
                        //sometimes the errors show that the difference will overlap the leaves.
                        //here we check for the overla[p and if there is n overlap, leaf B just gets set to 0.1 less than the leaf A position.
                        //thus ignoring the deviation fort that leaf pair.
                        if (cpd.leafB + Convert.ToSingle(cpd.deviationB) > cpd.leafA + Convert.ToSingle(cpd.deviationA))
                        {
                            leafPos[1, leafloc] = cpd.leafA + (float)cpd.deviationA;
                            leafPos[0, leafloc] = leafPos[1, leafloc] - (float)0.1;
                        }
                        else
                        {
                            /*if (cpd.leafA + (float)cpd.deviationA < x1)
                            {

                                leafPos[1, leafloc] = (float)x1 + (float)0.5;
                                leafPos[0, leafloc] = (float)x1;
                            }
                            else if (cpd.leafA + (float)cpd.deviationA > x2)
                            {
                                leafPos[1, leafloc] = (float)x2;
                                if(cpd.leafB + (float)cpd.deviationB > x2)
                                {
                                    leafPos[0, leafloc] = (float)x2 - (float)0.5;
                                }
                                else
                                {
                                    leafPos[0, leafloc] = cpd.leafB + (float)cpd.deviationB;
                                }
                            }
                            else
                            {
                                leafPos[1, leafloc] = cpd.leafA + Convert.ToSingle(cpd.deviationA);
                                leafPos[0, leafloc] = cpd.leafB + (float)cpd.deviationB;
                            }*/
                            leafPos[1, leafloc] = cpd.leafA + (float)cpd.deviationA;
                            leafPos[0, leafloc] = cpd.leafB + (float)cpd.deviationB;

                            //leafPos[0, leafloc] = cpd.leafA + Convert.ToSingle(cpd.deviationA);
                            //leafPos[1, leafloc] = cpd.leafB + Convert.ToSingle(cpd.deviationB);
                            }

                            leafloc++;
                        }
                        ////start with the first leaf position, and then interoplate all the rest.
                        //float leaf_oldA = 0;
                        //float leaf_oldB = 0;
                        //for (int i = 0; i < cpi.cpDetails.Count(); i++)
                        //{

                        //    if (i == 0)
                        //    {
                        //        leafPos[0, i] = cpi.cpDetails[i].leafA;
                        //        leafPos[1, i] = cpi.cpDetails[i + 1].leafB;
                        //        leaf_oldA = leafPos[0, i];
                        //        leaf_oldB = leafPos[1, i];
                        //        //mU_old = cpi.meterSet[i];
                        //    }
                        //    else
                        //    {
                        //        //let the interpolation begin.
                        //        //first the MU
                        //    }
                        //}
                        //beamp.SetAllLeafPositions(leafPos);
                        //ControlPointParameters cpp =  beamp.ControlPoints[cploc]
                        cpp.LeafPositions = leafPos;
                    //double check to see if this has to be applied every time. VMAT code is taking a long time.


                    //********************************** 
                    b2.ApplyParameters(beamp);
                    //**********************************



                    cploc++;
                    }
               
                //calculate the dose for each of the fields.
                mu_list.Add(new KeyValuePair<string, MetersetValue>(b2.Id, fi.MU));
                }
           
            ps2.CalculateDoseWithPresetValues(mu_list);
            //ps2.PlanNormalizationMethod = ps.PlanNormalizationMethod;\
            //need to renormalize by 1.3% in order to take into account the Discover that we cannot add to the newly calculated plan.
            //ps2.PlanNormalizationValue = val * ps2.PlanNormalizationValue;
            //val = (double)Convert.ToDouble(Input.Text);
            if (double.TryParse(Input.Text.ToString(), out val))
            { ps2.PlanNormalizationValue = val + no_norm; }
            MessageBox.Show($"{plan_txt.Text} created successfully.");
            }

            private void getDev_btn_Click(object sender, RoutedEventArgs e)
            {
                //first alert the user to the number of fields that have been detecteed. 
                //two messages can be displayed.
                int field_count = fields.Count();
                if (field_count == 0)
                {
                    MessageBox.Show("No fields currently found, Pleasee grab the plan before searching for the deviations");
                }
                else
                {
                    //MessageBox.Show($"Detected {field_count} fields. Grab all the deviations associated with these fields.");
                    var devWindow = new DeviationFind();
                    devWindow.field_list = fields;
                    devWindow.Show();

                }
            }
        }
    }
