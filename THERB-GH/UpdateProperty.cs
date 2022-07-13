using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Model;
using System.Windows.Forms;
using Rhino.Collections;
using System.Reflection;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace THERBgh
{
    public class UpdateProperty : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public UpdateProperty()
          : base("UpdateProperty", "UpdateProperty",
              "update properties",
              "THERB-GH", "Modelling")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Face", "Face", "Face list", GH_ParamAccess.list);
            pManager.AddIntegerParameter("property", "property", "property to update", GH_ParamAccess.item);
            var propertyInput = (Param_Integer)pManager[1];
            propertyInput.AddNamedValue("concrete_exteriorWall", (int)BoundaryCondition.exterior);
            propertyInput.AddNamedValue("concrete_exteriorFloor", (int)BoundaryCondition.interior);
            propertyInput.AddNamedValue("None", -1);
            pManager.AddIntegerParameter("value", "value", "value for assigning to property ", GH_ParamAccess.item);
            var valueInput = (Param_Integer)pManager[2];
            valueInput.AddNamedValue("1", 1);
            valueInput.AddNamedValue("2", 2);
            valueInput.AddNamedValue("None", -1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //TODO: RegisterOutputParamsをdynamicにしたい
            pManager.AddGenericParameter("Face", "Face", "updated Face list", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Face> faceList = new List<Face>();
            int propertyInt = -1;
            int value = -1;

            DA.GetDataList(0, faceList);
            DA.GetData(1, ref propertyInt);
            DA.GetData(2, ref value);
            if (propertyInt < -1 | 2 < propertyInt)
            {
                throw new Exception("propertyに範囲外の数字が入れられました。");
            }
            if (value < -1 | 2 < value)
            {
                throw new Exception("valueに範囲外の数字が入れられました。");
            }

            if (value == -1) return;
            
            foreach (var face in faceList)
            {
                face.constructionId = value;
            }

            DA.SetDataList("Face", faceList);
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
            get { return new Guid("41C9E254-FA7B-4750-A1C9-094938D1DAF2"); }
        }
    }
}
