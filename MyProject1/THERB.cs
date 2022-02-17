using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Intersect;
using Model;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace MyProject1
{
    public class THERB : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public THERB()
          : base("THERB", "THERB",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("geos", "geometries", "list of geometries", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("windows", "windows", "list of windows", GH_ParamAccess.list);
            pManager.AddBrepParameter("tol", "tolerance", "tolerance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rooms", "Rooms", "Room class", GH_ParamAccess.list);
            pManager.AddGenericParameter("Faces", "Faces", "Face class", GH_ParamAccess.list);
            pManager.AddGenericParameter("Windows", "Windows", "Window class", GH_ParamAccess.list);
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
            List<Surface> windows = new List<Surface>();
            List<Room> roomList = new List<Room>();
            List<Face> faceList = new List<Face>();

            //DA.GetData("geos", ref geos);
            double tol = 0.1;
            DA.GetDataList(0, geos);
            DA.GetDataList(1, windows);
            DA.GetData(2, ref tol);

            //Roomに対する処理
            foreach (Brep geo in geos)
            {
                Room temp = new Room(geo);
                BrepFaceList brepFaces = geo.Faces;
                foreach(Surface srf in brepFaces)
                {
                    Vector3d normal = srf.NormalAt(0.5, 0.5);
                    Vector3d reverseNormal = reviseNormal(srf, temp);
                    Face face = new Face(temp, srf, normal, reverseNormal);
                    faceList.Add(face);
                    temp.addFace(face);
                }
                roomList.Add(temp);
            }

            //FaceのboundaryConditionを計算する処理
            List<Face> faceListBC = solveBoundary(faceList, tol);

            //Roomに属するfaceの属性(wall,ceiling,roof)を集計するロジック
            foreach (Room room in roomList)
            {
                List<Face> faces = room.getFaces();
                foreach (Face face in faces)
                {
                    room.groupChildFaces(face);
                }
            }
            //windowがどのwallの上にあるかどうかを判断するロジック
            List<Window> windowList = windowOnFace(faceListBC, windows);

            //window情報をfaceにaddする
            //refパラメータとか使ってwindowOnFaceに付加するのがいいかも
            List<Face> faceListWindow = new List<Face>();
            foreach (Face face in faceListBC)
            {
                foreach (Window window in windowList)
                {
                    if (window.parentId == face.id)
                    {
                        face.addWindows(window);
                    }
                }
                faceListWindow.Add(face);
            }

            DA.SetData("Rooms", roomList);
            DA.SetData("Faces", faceList);
            DA.SetData("Windows", windowList);
        }


        private List<Window> windowOnFace(List<Face> faceList, List<Surface> windows)
        {
            List<Face> externalFaces = faceList.FindAll(face => face.bc == "outdoor");

            List<Window> windowList = new List<Window>();
            foreach(Surface windowGeo in windows) {
                int parentId = 0;
                double tiltAngle = 0;
                double closestDistance = 10000;
                Window window = new Window(windowGeo);
                foreach(Face exFace in externalFaces)
                {
                    double distance = window.centerPt.DistanceTo(exFace.centerPt);
                    if (distance < closestDistance)
                    {
                        parentId = exFace.id;
                        tiltAngle = exFace.tiltAngle;
                        closestDistance = distance;
                    }
                }

                if (closestDistance < 10000)
                {
                    window.parentId = parentId;
                    window.tiltAngle = tiltAngle;
                }
                else
                {
                    //エラー処理を加える
                }
                windowList.Add(window);
            }
            return windowList;
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
                    testFace.adjacencyFaceId = adjacentFace.partId;
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
            get { return new Guid("871d367b-8760-4573-b161-304cb7d17bf0"); }
        }
    }
}
