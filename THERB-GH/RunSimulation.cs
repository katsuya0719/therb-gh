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
    public class RunSimulation : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public RunSimulation()
          : base("THERB", "THERB",
              "Run THERB simulation",
              "THERB-GH", "Simulation")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("bDat", "bDat", "b.dat data", GH_ParamAccess.item);
            pManager.AddTextParameter("rDat", "rDat", "r.dat data", GH_ParamAccess.item);
            pManager.AddTextParameter("name", "name", "simulation case name", GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "run THERB simulation", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("result", "result", "Room class", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //処理1. example/test/THERB_formatの中にあるデータをまるごとc://therb/{name}フォルダにコピー 

            //処理2. inputのb.dat,r.datデータをc://therb/{name}フォルダにb.dat,r.datファイルとして書き込み  

            //処理3. コマンドラインを立ち上げ、therb.exeファイルを呼び出す  

        }


        private List<Window> windowOnFace(List<Face> faceList, List<Surface> windows)
        {
            //TODO:内壁に窓を配置するケースもあるっぽい
            List<Face> externalFaces = faceList.FindAll(face => face.bc == "outdoor");

            List<Window> windowList = new List<Window>();
            foreach(Surface windowGeo in windows) {
                Face parent = externalFaces[0];
                double closestDistance = 10000;
                Window window = new Window(windowGeo);
                foreach(Face exFace in externalFaces)
                {
                    double distance = window.centerPt.DistanceTo(exFace.centerPt);
                    if (distance < closestDistance)
                    {
                        parent = exFace;
                        closestDistance = distance;
                    }
                }

                if (closestDistance < 10000)
                {
                    window.addParent(parent);
                }
                else
                {
                    //エラー処理を加える
                }
                windowList.Add(window);

            }
            return windowList;
        }

        //TODO: refactorしてwindowOnFaceと一緒の関数にする
        private List<Overhang> overhangOnWindow(List<Window> windowList, List<Surface> overhangs)
        {
            List<Overhang> overhangList = new List<Overhang>();
            foreach (Surface overhangGeo in overhangs)
            {
                Window parent = windowList[0];
                double closestDistance = 10000;
                Overhang overhang = new Overhang(overhangGeo);
                foreach (Window window in windowList)
                {
                    double distance = overhang.centerPt.DistanceTo(window.centerPt);
                    if (distance < closestDistance)
                    {
                        parent = window;
                        closestDistance = distance;
                    }
                }

                if (closestDistance < 10000)
                {
                    overhang.addParentWindow(parent);
                    overhang.addParentFace(parent.parent);
                }
                else
                {
                    //エラー処理を加える
                }
                overhangList.Add(overhang);

            }
            return overhangList;
        }

        private List<Face> solveBoundary(List<Face> faceList, double tol)
        {
            List<Face> faceListBC = new List<Face>();
            for (int i=0;i<faceList.Count; i++)
            {
                //テストするsurfaceを選択する
                Face testFace = faceList[i];

                //違うparentを持つfaceを選択する
                List<Face> targetFaces = faceList.FindAll(face => face.parent != testFace.parent);

                List<Surface> targetSurfaces = new List<Surface>();

                foreach (Face face in targetFaces)
                {
                    targetSurfaces.Add(face.geometry);
                };

                Point3d testPt = new Point3d(testFace.centerPt);
                testPt += testFace.normal * -tol / 2;
                Ray3d testRay = new Ray3d(testPt, testFace.normal);

                if (shootIt(testRay, targetSurfaces, tol, 2))
                {
                    testFace.bc = "interior";
                    //どのfaceに接しているのかをチェック
                    //testFace.centerPtから一番近いtargetSurfacesが隣接しているsurfaceだと判断する
                    Face adjacentFace = getClosestFaceFromFace(testFace, targetFaces);
                    testFace.adjacencyRoomId = adjacentFace.parentId;
                    //testFace.adjacencyFaceId = adjacentFace.partId;
                    testFace.adjacencyFace = adjacentFace;
                }
                else
                {
                    //z座標<=0であれば、bc=groundとする
                    if(testFace.centerPt.Z <= tol)
                    {
                        testFace.bc = "ground";
                    }
                    else
                    {
                        testFace.bc = "outdoor";
                    }
                    testFace.adjacencyRoomId = 0;
                }
                testFace.setElementType();
                testFace.setConstructionId();
                faceListBC.Add(testFace);
            }
            return faceListBC;
        }

        private bool shootIt(Ray3d ray, List<Surface> srfs, double tol, int bounce)
        {
            Point3d[] intersectPts = Intersection.RayShoot(ray, srfs, 1);
            if (intersectPts.Length > 0)
            {
                if (ray.Position.DistanceTo(intersectPts[0]) <= tol)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private Face getClosestFaceFromFace(Face originFace, List<Face> faces)
        {
            Face closestFace = null;
            double closestDistance = 10000;
            foreach(Face face in faces)
            {
                double distance = 10000;
                //faceがwallであればwallのみでテストする
                if (originFace.face == "wall" && face.face == "wall")
                {
                    distance = originFace.centerPt.DistanceTo(face.centerPt);
                }else if(originFace.face != "wall" && face.face != "wall"){
                    distance = originFace.centerPt.DistanceTo(face.centerPt);
                }
                if (distance < closestDistance)
                {
                    closestFace = face;
                    closestDistance = distance;
                }
            }
            return closestFace;
        }

        private Vector3d reviseNormal(Surface srf, Room parent)
        {
            //モデルによってはsurfaceのvectorの方向が意図と逆に向いている場合があるのでそれを修正
            Point3d centroid = parent.centroid;
            AreaMassProperties areaMp = AreaMassProperties.Compute(srf);
            Point3d centerPt = areaMp.Centroid;
            Vector3d normal = srf.NormalAt(0.5, 0.5);
            Point3d testPt = centerPt + normal;
            double baseDistance = centroid.DistanceTo(centerPt);
            double testDistance = centroid.DistanceTo(testPt);

            if (baseDistance > testDistance)
            {
                Vector3d revNormal = new Vector3d(-normal.X, -normal.Y, -normal.Z);
                return revNormal;
            }
            else
            {
                return normal;
            }
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
            get { return new Guid("b56ae3c7-9860-4f35-951e-0d6d427f5a2e"); }
        }
    }
}
