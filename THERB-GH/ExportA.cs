﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Rhino.Geometry.Intersect;
using Newtonsoft.Json;
using Model;
using Utils;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace THERBgh
{
    public class ExportA : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExportA()
          : base("exportA", "exportA",
              "export a.dat",
              "THERB-GH", "Simulation")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Therb", "therb", "THERB class", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("a_dat", "a_dat", "a.dat file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Therb therb = null;
            DA.GetData(0, ref therb);

            if (therb == null) return;

            DA.SetData("a_dat", CreateDatData.CreateADat(therb));
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
<<<<<<< HEAD:THERB-GH/ReadProperty.cs
            get { return new Guid("3701b3dc-c4b6-4d44-b859-fe3f581efb8c"); }
=======
            get { return new Guid("4d940719-485a-4f3f-96ba-58a40632dbd4"); }
>>>>>>> 5062e72d74991fdd999e66d6d9a0d4de289a4ace:THERB-GH/ExportA.cs
        }
    }
}
