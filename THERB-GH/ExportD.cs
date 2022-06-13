using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
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
    public class ExportD : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExportD()
          : base("exportD", "exportD",
              "export d.dat",
              "THERB-GH", "Modelling")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Rooms", "Rooms", "Room class", GH_ParamAccess.list);
            pManager.AddGenericParameter("Faces", "Faces", "Face class", GH_ParamAccess.list);
            pManager.AddGenericParameter("Windows", "Windows", "Window class", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("d_dat", "d_dat", "d.dat file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Room> roomList = new List<Room>();
            List<Face> faceList = new List<Face>();
            List<Face> windowList = new List<Face>();
            string dDat = "";

            DA.GetDataList(0, roomList);
            DA.GetDataList(1, faceList);
            DA.GetDataList(2, windowList);

            //TODO:door,shadingの入力UIも作る
            List<Face> doorList = new List<Face>();
            List<Face> shadingList = new List<Face>();
            List<Face> wingList = new List<Face>();

            if (roomList == null) return;

            List<Face> exteriorWalls = new List<Face>();
            List<Face> interiorWalls = new List<Face>();
            List<Face> exteriorRoofs = new List<Face>();
            List<Face> interiorRoofs = new List<Face>();
            List<Face> exteriorFloors = new List<Face>();
            List<Face> interiorFloors = new List<Face>();
            List<Face> groundFloors = new List<Face>();
            

            //faceを分類
            faceList.ForEach(face =>
            {
                if (face.elementType == "exteriorwall")
                {
                    exteriorWalls.Add(face);
                } else if (face.elementType == "interiorwall")
                {
                    interiorWalls.Add(face);
                } else if (face.elementType == "exteriorroof")
                {
                    exteriorRoofs.Add(face);
                } else if (face.elementType == "interiorroof")
                {
                    interiorRoofs.Add(face);
                }
                else if (face.elementType == "exteriorfloor")
                {
                    exteriorFloors.Add(face);
                }
                else if (face.elementType == "interiorfloor")
                {
                    interiorFloors.Add(face);
                }
                else if (face.elementType == "groundfloor")
                {
                    groundFloors.Add(face);
                }
            });

            //1行目 部屋、壁の数をカウント
            dDat += fillEmpty(roomList.Count,4)
                + fillEmpty(exteriorWalls.Count, 4)
                + fillEmpty(windowList.Count, 4)
                + fillEmpty(interiorWalls.Count, 4)
                + fillEmpty(doorList.Count, 4)
                + fillEmpty(interiorFloors.Count+ exteriorFloors.Count + interiorRoofs.Count, 4)
                + fillEmpty(exteriorRoofs.Count, 4)
                + fillEmpty(groundFloors.Count, 4)
                + fillEmpty(shadingList.Count, 4)
                + fillEmpty(wingList.Count, 4) + " \r\n";

            //室の情報を抽出

            //外壁の情報を抽出
            exteriorWalls.ForEach(exWall =>
            {
                dDat += fillEmpty("Ex-wall " + exWall.id.ToString(), 13);
                //TODO:minPtとmaxPtの座標を書き込み

            });
            

            DA.SetData("d_dat", dDat);
        }

        //TODO:Utilsモジュールにうつす
        private string fillEmpty(int input, int totalLength)
        {
            string inputStr = input.ToString();
            int inputLength = inputStr.Length;
            string emptyString = new string(' ', totalLength - inputLength);

            return emptyString + inputStr;
        }

        private string fillEmpty(string input, int totalLength)
        {
            int inputLength = input.Length;
            string emptyString = new string(' ', totalLength - inputLength);

            return emptyString + input;
        }

        private string fillEmpty(double input, int totalLength, int digit)
        {
            string zeros = new String('0', digit);
            string format = "{0:0." + zeros + "}";
            string inputValue = string.Format(format, input);
            string emptyString = new string(' ', totalLength - inputValue.Length);

            return emptyString + inputValue;
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
            get { return new Guid("d4bfc155-8356-460a-b788-ac7c01f63e91"); }
        }
    }
}
