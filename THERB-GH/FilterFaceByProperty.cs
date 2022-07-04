using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Model;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace THERBgh
{
    public class FilterFaceByProperty : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public FilterFaceByProperty()
          : base("FilterFaceByProperty", "FilterFaceByProperty",
              "Filter face by properties",
              "THERB-GH", "Modelling")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Therb", "Therb", "Therb data", GH_ParamAccess.item);
            pManager.AddTextParameter("bc", "bc", "boundary condition to filter", GH_ParamAccess.item);
            //pManager.AddTextParameter("class", "class", "room or face or window", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //TODO: RegisterOutputParamsをdynamicにしたい
            pManager.AddGenericParameter("trueFace", "trueFace", "true Face list", GH_ParamAccess.list);
            pManager.AddGenericParameter("falseFace", "falseFace", "false Face list", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //List<Face> faceList = new List<Face>();       
            //DA.GetDataList(0, faceList);
            Therb Therb = null;
            DA.GetData(0, ref Therb);

            List<Face> faceList = Therb.faces;

            string bc = "";
            DA.GetData(1, ref bc);
            //keyが正しくなかったらエラーを返すようにする  
            Enum.TryParse(bc, out BoundaryCondition boundaryCondition);

            List<Face> trueFaceList = new List<Face>();
            List<Face> falseFaceList = new List<Face>();

            faceList.ForEach(face =>
            {
                if (face.filterByBc(boundaryCondition)){
                    trueFaceList.Add(face);
                }
                else
                {
                    falseFaceList.Add(face);
                }
            });

            DA.SetDataList("trueFace", trueFaceList);
            DA.SetDataList("falseFace", falseFaceList);
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
            get { return new Guid("8706930f-23f5-48d3-885a-78a128be5948"); }
        }
    }
}
