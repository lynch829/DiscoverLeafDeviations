using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilDICOM;
using EvilDICOM.RT;
using EvilDICOM.Core;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using EvilDICOM.Core.Element;
using EvilDICOM.Core.IO;
using EvilDICOM.Core.Helpers;
using EvilDICOM.Core.Interfaces;

namespace DicomPlanCreation
{
    public class FieldInfos
    {
        internal List<IDICOMElement> collimator;

        public string FieldId { get; set; }
        public List<cpInfo> cpInfos { get; set; }
        //public List<devInfo> devInfos { get; set; }
        //these parameters are just to be copied to the new fields
        //public ExternalBeamMachineParameters Ebmp { get; set; }

        //need to know if it is a VMAT field.
        //public GantryDirection gantry_direction { get; set; }
        public double collAngle { get; set; }
        public double gantry { get; set; }
        public double couch { get; set; }
        //public VVector isocenter { get; set; }
        //public BeamParameters bp { get; set; }
        //public MetersetValue MU { get; set; }
        //public Applicator applicator { get; set; }
        public double gantry_stop { get; internal set; }
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
        public double deviationA { get; set; }
        public double deviationB { get; set; }
    }
}
