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
    public class ExportR : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExportR()
          : base("exportR", "exportR",
              "export r.dat",
              "THERB-GH", "Modelling")
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
            pManager.AddTextParameter("r_dat", "r_dat", "r.dat file", GH_ParamAccess.item);
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

            List<Room> roomList = therb.rooms;
            List<Face> faceList = therb.faces;
            List<Window> windowList = therb.windows;

            string rDat = "";

            if (roomList == null) return;

            var directionDict = new Dictionary<string, int>(){
                {"S",1},
                {"W",2},
                {"N",3},
                {"E",4},
                {"F",5},
                {"CR",6},
            };

            var directionCount = new Dictionary<string, int>(){
                {"S",1},
                {"W",1},
                {"N",1},
                {"E",1},
                {"F",1},
                {"CR",1},
            };



            //TODO: 齋藤君に修正願い
            roomList.ForEach(room =>
            {
                List<int> directions = room.getDirectionList();
                //idList.Add(room.id);
                rDat += fillEmpty(room.id, 5)
                + fillEmpty(directions[0], 5)
                + fillEmpty(directions[1], 5)
                + fillEmpty(directions[2], 5)
                + fillEmpty(directions[3], 5)
                + fillEmpty(directions[4], 5)
                + fillEmpty(directions[5], 5)
                + fillEmpty(directions[6], 5) + " \r\n";
            });

            int id = 1;
            //Faceデータに対する処理
            roomList.ForEach(room =>
            {
                faceList.ForEach(face =>
                {
                    if (room.id == face.parentId)
                    {
                        rDat += fillEmpty(room.id, 5)
                        + fillEmpty(id, 5)
                        + fillEmpty(directionDict[face.direction.ToString()], 5)
                        + fillEmpty(directionCount[face.direction.ToString()], 5)
                        + fillEmpty(face.constructionId, 5)
                        + fillEmpty(face.partId, 5)
                        + fillEmpty(face.adjacencyRoomId, 5) + " \r\n";

                        directionCount[face.direction.ToString()] += 1;
                        id += 1;

                        //TODO:窓に関する処理を追加
                        face.windows.ForEach(window =>
                        {
                            rDat += fillEmpty(room.id, 5)
                            + fillEmpty(id, 5)
                            + fillEmpty(directionDict[face.direction.ToString()], 5)
                            + fillEmpty(directionCount[face.direction.ToString()], 5)
                            + fillEmpty(6, 5)
                            + fillEmpty(window.partId, 5)
                            + fillEmpty(face.adjacencyRoomId, 5) + " \r\n";

                            id += 1;
                        });
                    }


                });
            });
            
            DA.SetData("r_dat", rDat);
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
            get { return new Guid("ac7ba740-49bd-45a4-8c84-8d9a69081c38"); }
        }
    }
}
