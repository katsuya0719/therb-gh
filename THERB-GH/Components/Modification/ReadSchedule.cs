using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Model;
using System.Windows.Forms;
using Rhino.Collections;
using System.Reflection;
using System.Net;
using Newtonsoft.Json;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace THERBgh
{
    public class ReadSchedule : GH_Component
    {
        const string SCHEDULE_URL = "https://stingray-app-vgak2.ondigitalocean.app/schedules";
        //const string SCHEDULE_URL = "http://localhost:5000/schedules";//データを入れるまでの一次的措置

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ReadSchedule()
          : base("ReadSchedule", "ReadSchedule",
              "read schedule data",
              "THERB-GH", "IO")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Run", "Run", "Read schedule json data", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Schedules", "Schedules", "schedule list", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var flag = false;
            DA.GetData("Run", ref flag);
            if (!flag) return;

            //jsonデータを読み込む（データフォーマットはnotionを参照）
            var wc = new WebClient();

            string text = wc.DownloadString(SCHEDULE_URL);

            List<Schedule> schedules = JsonConvert.DeserializeObject<ResSchedule>(text).data;

            DA.SetDataList("Schedules", schedules);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("db3606c2-7523-4984-95cc-e5d55466043e"); }
        }
    }
}
