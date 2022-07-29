using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ExportB : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExportB()
          : base("exportB", "exportB",
              "export b.dat",
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
            pManager.AddTextParameter("b_dat", "b_dat", "b.dat file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string dDat = "";

            Therb therb = null;
            DA.GetData(0, ref therb);

            List<Room> roomList = therb.rooms;
            List<Face> faceList = therb.faces;
            List<Window> windowList = therb.windows;
            List<Overhang> overhangList = therb.overhangs;


            //TODO:doorの入力UIも作る
            List<Face> doorList = new List<Face>();
            List<Face> shadingList = new List<Face>();
            List<Face> wingList = new List<Face>();

            if (roomList == null) return;

            List<Face> exteriorWalls = new List<Face>();
            List<Face> interiorWalls = new List<Face>();
            List<Face> exteriorRoofs = new List<Face>();
            List<Face> interiorRoofFloors = new List<Face>();
            List<Face> exteriorFloors = new List<Face>();
            //List<Face> interiorFloors = new List<Face>();
            List<Face> groundFloors = new List<Face>();
            

            //faceを分類
            faceList.ForEach(face =>
            {
                if (face.elementType == "exteriorwall")
                {
                    exteriorWalls.Add(face);
                } else if (face.elementType == "interiorwall" && face.unique)
                {
                    interiorWalls.Add(face);
                } else if (face.elementType == "exteriorroof")
                {
                    exteriorRoofs.Add(face);
                } else if (face.elementType == "interiorroof" || face.elementType == "interiorfloor")
                {
                    interiorRoofFloors.Add(face);
                }
                else if (face.elementType == "exteriorfloor")
                {
                    exteriorFloors.Add(face);
                }
                else if (face.elementType == "groundfloor")
                {
                    groundFloors.Add(face);
                }
            });

            //TODO:exteriorFloorの扱いがどうなるかを確認

            //1行目 部屋、壁の数をカウント
            dDat += Converter.FillEmpty(roomList.Count,4)
                + Converter.FillEmpty(exteriorWalls.Count, 4)
                + Converter.FillEmpty(windowList.Count, 4)
                + Converter.FillEmpty(interiorWalls.Count, 4)
                + Converter.FillEmpty(doorList.Count, 4)
                + Converter.FillEmpty(interiorRoofFloors.Count, 4)
                + Converter.FillEmpty(exteriorRoofs.Count, 4)
                + Converter.FillEmpty(groundFloors.Count, 4)
                + Converter.FillEmpty(shadingList.Count, 4)
                + Converter.FillEmpty(wingList.Count, 4) + " \r\n";

            //室の情報を抽出
            roomList.ForEach(room =>
            {
                dDat += Converter.FillEmpty("Room " + room.id.ToString(), 12)
                + Converter.FillEmpty(room.minPt.X, 9, 3)
                + Converter.FillEmpty(room.minPt.Y, 8, 3)
                + Converter.FillEmpty(room.minPt.Z, 8, 3)
                + Converter.FillEmpty(room.maxPt.X, 8, 3)
                + Converter.FillEmpty(room.maxPt.Y, 8, 3)
                + Converter.FillEmpty(room.maxPt.Z, 8, 3)
                + Converter.FillEmpty(room.volume, 10, 3)
                 + "      18.800   16.7000 \r\n";
                //volumeを抽出する必要
            });

            //外壁の情報を抽出
            exteriorWalls.ForEach(exWall =>
            {
                dDat += Converter.FillEmpty("Ex-wall " + exWall.partId.ToString(), 13)
                + Converter.FillEmpty(exWall.minPt.X, 8, 3)
                + Converter.FillEmpty(exWall.minPt.Y, 8, 3)
                + Converter.FillEmpty(exWall.minPt.Z, 8, 3)
                + Converter.FillEmpty(exWall.maxPt.X, 8, 3)
                + Converter.FillEmpty(exWall.maxPt.Y, 8, 3)
                + Converter.FillEmpty(exWall.maxPt.Z, 8, 3)
                + Converter.FillEmpty(exWall.tiltAngle, 10, 3)
                + Converter.FillEmpty(exWall.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(exWall.constructionId, 5)
                + "  overhang No.     0      wing1 No.    0   wing2 No.    0 \r\n      window No. "
                + OutputWindowIds(exWall)
                //TODO:windowIdsの処理を入れ込む必要
                + "\r\n";
            });

            //窓の情報を抽出
            windowList.ForEach(window =>
            {
                dDat += Converter.FillEmpty("Window " + window.id.ToString(), 13)
                + Converter.FillEmpty(window.minPt.X, 8, 3)
                + Converter.FillEmpty(window.minPt.Y, 8, 3)
                + Converter.FillEmpty(window.minPt.Z, 8, 3)
                + Converter.FillEmpty(window.maxPt.X, 8, 3)
                + Converter.FillEmpty(window.maxPt.Y, 8, 3)
                + Converter.FillEmpty(window.maxPt.Z, 8, 3)
                + Converter.FillEmpty(window.tiltAngle, 10, 3)
                + Converter.FillEmpty(window.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(window.constructionId, 5)
                + "  overhang No.     0      wing1 No.    0   wing2 No.    0\r\n";
            });

            string doorIds = "   0   0   0   0   0   0   0   0   0   0   0";
            //内壁の情報を抽出
            interiorWalls.ForEach(inWall =>
            {
                dDat += Converter.FillEmpty("In-wall " + inWall.partId.ToString(), 13)
                + Converter.FillEmpty(inWall.minPt.X, 8, 3)
                + Converter.FillEmpty(inWall.minPt.Y, 8, 3)
                + Converter.FillEmpty(inWall.minPt.Z, 8, 3)
                + Converter.FillEmpty(inWall.maxPt.X, 8, 3)
                + Converter.FillEmpty(inWall.maxPt.Y, 8, 3)
                + Converter.FillEmpty(inWall.maxPt.Z, 8, 3)
                + Converter.FillEmpty(inWall.tiltAngle, 10, 3)
                + Converter.FillEmpty(inWall.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(inWall.constructionId, 5)
                +" \r\n     in-door No. " + doorIds + " \r\n";
            });

            //床・天井の情報を抽出
            interiorRoofFloors.ForEach(inMat =>
            {
                dDat += Converter.FillEmpty("flr&cling " + inMat.partId.ToString(), 13)
                + Converter.FillEmpty(inMat.minPt.X, 8, 3)
                + Converter.FillEmpty(inMat.minPt.Y, 8, 3)
                + Converter.FillEmpty(inMat.minPt.Z, 8, 3)
                + Converter.FillEmpty(inMat.maxPt.X, 8, 3)
                + Converter.FillEmpty(inMat.maxPt.Y, 8, 3)
                + Converter.FillEmpty(inMat.maxPt.Z, 8, 3)
                + Converter.FillEmpty(inMat.tiltAngle, 10, 3)
                + Converter.FillEmpty(inMat.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(inMat.constructionId, 5) + "\r\n";
            });

            exteriorRoofs.ForEach(roof =>
            {
                dDat += Converter.FillEmpty("Roof " + roof.partId.ToString(), 13)
                + Converter.FillEmpty(roof.minPt.X, 8, 3)
                + Converter.FillEmpty(roof.minPt.Y, 8, 3)
                + Converter.FillEmpty(roof.minPt.Z, 8, 3)
                + Converter.FillEmpty(roof.maxPt.X, 8, 3)
                + Converter.FillEmpty(roof.maxPt.Y, 8, 3)
                + Converter.FillEmpty(roof.maxPt.Z, 8, 3)
                + Converter.FillEmpty(roof.tiltAngle, 10, 3)
                + Converter.FillEmpty(roof.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(roof.constructionId, 5) + "\r\n";
            });

            groundFloors.ForEach(floor =>
            {
                dDat += Converter.FillEmpty("Ground " + floor.partId.ToString(), 13)
                + Converter.FillEmpty(floor.minPt.X, 8, 3)
                + Converter.FillEmpty(floor.minPt.Y, 8, 3)
                + Converter.FillEmpty(floor.minPt.Z, 8, 3)
                + Converter.FillEmpty(floor.maxPt.X, 8, 3)
                + Converter.FillEmpty(floor.maxPt.Y, 8, 3)
                + Converter.FillEmpty(floor.maxPt.Z, 8, 3)
                + Converter.FillEmpty(floor.tiltAngle, 10, 3)
                + Converter.FillEmpty(floor.area, 12, 4)
                + "    0\r\n  structure No. "
                + Converter.FillEmpty(floor.constructionId, 5) + "\r\n";
            });


            DA.SetData("b_dat", dDat);
        }

        //TODO:Utilsモジュールにうつす
        private string OutputWindowIds(Face face)
        {
            const int MAX_IMPUT_ID_COUNT = 11;

            string windowIdStrs = "";
            for (int windowIdIndex = 0; windowIdIndex < MAX_IMPUT_ID_COUNT; windowIdIndex++)
            {
                if (windowIdIndex < face.windowIds.Count)
                {
                    windowIdStrs += string.Format("{0,4}", face.windowIds[windowIdIndex]);
                }
                else
                {
                    windowIdStrs += Converter.FillEmpty("0", 4);
                }
            }
            return windowIdStrs;
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
