using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.8.4.3")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
[assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context, System.Windows.Window window/*, ScriptEnvironment environment*/)
        {
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            //access the patient
            Patient p = context.Patient;
            p.BeginModifications();
            //access the course and plan.
            Course c = context.Course;
            ExternalPlanSetup ps = context.ExternalPlanSetup;
            var mainWindow = new ManualControlPoints.MainWindow();
            //set window properties.
            window.Width = mainWindow.Width + 10;
            window.Height = mainWindow.Height + 5;
            window.Content = mainWindow;
            //send data to mainwindow.
            mainWindow.p = p;
            mainWindow.c = c;
            mainWindow.ps = ps;
        }
    }
}
