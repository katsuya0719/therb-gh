using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Intersect;
using Newtonsoft.Json;
using Model;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace THERBgh
{
    public class SplitGeos : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SplitGeos()
          : base("splitGeos", "split geometries",
              "Description",
              "THERB-GH", "Modelling")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("geos", "geometries", "list of geometries", GH_ParamAccess.list);
            pManager.AddNumberParameter("tol", "tolerance", "tolerance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("splitted", "splitted", "splitted geometries", GH_ParamAccess.list);
            pManager.AddGenericParameter("errors", "errors", "error geometries", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //var geos = new Brep();
            List<Brep> geos = new List<Brep>();

            //DA.GetData("geos", ref geos);
            //tolとwindowsは任意のパラメータとしたい
            double tol = 0.1;
            DA.GetDataList(0, geos);
            DA.GetData(1, ref tol);

            List<Brep> splitGeos;
            splitGeos = SplitGeometry(geos, tol);

            List<Surface> successSrfs = new List<Surface>();
            List<Surface> failSrfs = new List<Surface>();

            //Roomに対する処理
            foreach (Brep geo in splitGeos)
            {
                //TODO: SurfaceとFaceの違い理解
                BrepFaceList srfs = geo.Faces;
                foreach (var brepface in srfs)
                {
                    Brep brep = brepface.DuplicateFace(false);
                    Surface srf;
                    if (brep.IsSurface)
                    {
                        //srf = brepface.ToNurbsSurface();
                        successSrfs.Add(brepface);
                    }
                    else
                    {
                        var edges = brep.Edges;
                        if (4 < edges.Count)
                        {
                            failSrfs.Add(brepface);
                        }
                        if (edges.Count < 2) throw new Exception("Edgeの分割に失敗しました。");
                        //srf = Brep.CreateEdgeSurface(edges).Faces[0].ToNurbsSurface();
                    }
                }
            }
            DA.SetDataList("splitted",successSrfs);
            DA.SetDataList("errors", failSrfs);
        }

        //private List<Brep> divideLshape(Brep brep)
        //{

        //}

        private List<Brep> SplitGeometry(List<Brep> breps, double tol)
        {
            if (breps.Count == 1)
            {
                //if (!breps[0].IsSolid) throw new Exception("開かれたBrepが入れられました");
                return breps;
            }
            List<Brep> splitGeos = new List<Brep>();
            for (int i = 0; i < breps.Count; i = i + 1)
            {
                //if (!breps[i].IsSolid) throw new Exception("開かれたBrepが入れられました");

                List<Brep> cutterBreps = breps.FindAll(geo => geo != breps[i]);
                Brep[] splitGeo = breps[i].Split(cutterBreps, tol);
                Brep jointedBrep = Brep.JoinBreps(splitGeo, tol)[0];

                if (!jointedBrep.IsSolid) throw new Exception("BrepのJointが失敗しました。");

                splitGeos.Add(jointedBrep);
            }
            return splitGeos;
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
            get { return new Guid("65a3866f-68c8-4976-ba05-06f8d8dde4a0"); }
        }
    }
}
