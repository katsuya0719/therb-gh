using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Collections;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Model
{
    public enum BoundaryCondition
    {
        Outdoors,
        Surface,
        Ground
    }
    public enum SurfaceType
    {
        Wall,
        Roof,
        Ceiling,
        Floor
    }

    public class Zone
    {
        public string name;
        public int multiplier;
        
        
    }

    public class BaseBaues
    {
        public string name;
        public string constructionName;
        public List<double> vertices;
    }

    public class SurfaceDetailed : BaseBaues
    {
        public SurfaceType surfaceType;
        public int surfaceTypeId; //そのsurface typeのid
        public string boundaryCondition;
        public string boundaryConditionObject;
        public List<FenestrationDetailed> windows;
        public static int _totalWalls;
        public static int _totalRoofs;
        public static int _totalCeilings;
        public static int _totalFloors;

        static SurfaceDetailed()
        {
            _totalWalls = 0;
            _totalRoofs = 0;
            _totalCeilings = 0;
            _totalFloors = 0;
        }

        public SurfaceDetailed(string name, Face face)
        {
            this.vertices = face.bauesVertices;
            switch (face.face)
            {
                case "wall":
                    this.surfaceType = SurfaceType.Wall;
                    _totalWalls += 1;
                    surfaceTypeId = _totalWalls;
                    break;
                case "floor":
                    this.surfaceType = SurfaceType.Floor;
                    _totalFloors += 1;
                    surfaceTypeId = _totalFloors;
                    break;
                case "roof":
                    if (face.elementType == "exteriorroof")
                    {
                        this.surfaceType = SurfaceType.Roof;
                        _totalRoofs += 1;
                        surfaceTypeId = _totalRoofs;
                        break;
                    }
                    else
                    {
                        this.surfaceType = SurfaceType.Ceiling;
                        _totalCeilings += 1;
                        surfaceTypeId = _totalCeilings;
                        break;
                    }
            }
            this.name = "room" + face.parent.id + "_" + this.surfaceType.ToString() + surfaceTypeId.ToString();
        }
    }

    public class FenestrationDetailed : BaseBaues
    {

    }

    public class BaseGeo{
        public Guid guid;
        public int id;
        public string displayName;
    }
    public class Room : BaseGeo
    {
        public Brep geometry;
        public Point3d centroid;
        private List<Face> _faceList;
        public static int _totalRooms;
        public BrepVertexList vertices;
        public Point3d minPt;
        public Point3d maxPt;
        public List<Face> sWalls;
        public List<Face> wWalls;
        public List<Face> nWalls;
        public List<Face> eWalls;
        public List<Face> floors;
        public List<Face> roofs;

        static Room()
        {
            _totalRooms = 0;
        }

        public Room(Brep geometry)
        {
            this.guid = Guid.NewGuid();
            _totalRooms += 1;
            this.id = _totalRooms;
            //this.displayName = _totalRooms;
            this._faceList = new List<Face>();
            this.geometry = geometry;
            BrepVertexList vertices = geometry.Vertices;
            this.vertices = vertices;
            this.minPt = getMinCoord(vertices);
            this.maxPt = getMaxCoord(vertices);
            this.sWalls = new List<Face>();
            this.wWalls = new List<Face>();
            this.nWalls = new List<Face>();
            this.eWalls = new List<Face>();
            this.floors = new List<Face>();
            this.roofs = new List<Face>();
            VolumeMassProperties vmp = VolumeMassProperties.Compute(geometry);
            this.centroid = vmp.Centroid;
            //TODO:どのタイミングで
        }

        public void addFace(Face face)
        {
            _faceList.Add(face);
        }

        public void groupChildFaces(Face face)
        {
            if (face.face == "wall")
            {
                switch (face.direction)
                {
                    case "S":
                        this.sWalls.Add(face);
                        break;
                    case "N":
                        this.nWalls.Add(face);
                        break;
                    case "W":
                        this.wWalls.Add(face);
                        break;
                    case "E":
                        this.eWalls.Add(face);
                        break;
                }
            }
            else if (face.face == "roof")
            {
                this.roofs.Add(face);
            }
            else if (face.face == "floor")
            {
                this.floors.Add(face);
            }
        }

        public List<int> getDirectionList()
        {
            List<int> directionList = new List<int>();
            directionList.Add(this.sWalls.Count);
            directionList.Add(this.wWalls.Count);
            directionList.Add(this.nWalls.Count);
            directionList.Add(this.eWalls.Count);
            directionList.Add(this.floors.Count);
            directionList.Add(this.roofs.Count);
            int total = this.sWalls.Count + this.wWalls.Count + this.nWalls.Count + this.eWalls.Count + this.floors.Count + this.roofs.Count;
            directionList.Add(total);

            return directionList;//[S,W,N,E,F,CR,total]
        }

        public List<Face> getFaces()
        {
            return _faceList;
        }

        //public static string generateDisplayName{
        //}
        private Point3d getMinCoord(BrepVertexList vertices)
        {
            Point3d origin = new Point3d(0, 0, 0);
            int i = 0;
            int minId = 0;
            double minValue = 1000;
            foreach (BrepVertex vertex in vertices)
            {

                Point3d checkPt = vertex.Location;
                double distance = origin.DistanceTo(checkPt);
                //Print("{0}:{1}", i, distance);
                if (distance < minValue)
                {
                    minId = i;
                    minValue = distance;
                }
                i += 1;
            }
            //Print("{0}:{1}", minId, minValue);

            return vertices[minId].Location;
        }

        private Point3d getMaxCoord(BrepVertexList vertices)
        {
            Point3d origin = new Point3d(0, 0, 0);
            int i = 0;
            int maxId = 0;
            double maxValue = -1000;
            foreach (BrepVertex vertex in vertices)
            {

                Point3d checkPt = vertex.Location;
                double distance = origin.DistanceTo(checkPt);
                //Print("{0}:{1}", i, distance);
                if (distance > maxValue)
                {
                    maxId = i;
                    maxValue = distance;
                }
                i += 1;
            }
            //Print("{0}:{1}", minId, minValue);

            return vertices[maxId].Location;
        }
    }

    public class Face : BaseGeo
    {
        public int partId;
        //public faceType face{get; private set;}
        public string face { get; private set; }
        //public boundaryCondition bc{get; set;}
        public string bc { get; set; }
        public string elementType { get; set; }
        public int constructionId; //とりあえず外壁1,内壁2,床(室内）5,屋根4,床（地面）5, 窓6　THERBのelement Idに相当/鈴木君からもらったデータにとりあえずそろえる
        public Room parent;
        public int parentId;
        public Surface geometry { get; private set; }
        public BrepVertexList vertices;
        public Vector3d normal { get; private set; }
        public Vector3d tempNormal;
        public Point3d centerPt;
        public double tiltAngle;
        public double area;
        public string direction;
        public int adjacencyRoomId; //隣接しているRoomのId 外気に接している場合には0
        public List<Window> windows { get; private set; }
        public List<int> windowIds;
        public static int _totalFaces;
        public static int _totalExWalls;
        public static int _totalInWalls;
        public static int _totalFlrCeilings;
        public static int _totalRoofs;
        public static int _totalGrounds;
        //BAUES Analysis用の属性
        public BoundaryCondition bauesBC;
        public int adjacencyFaceId;
        public List<double> bauesVertices;

        static Face()
        {
            _totalFaces = 0;
            _totalExWalls = 0;
            _totalInWalls = 0;
            _totalFlrCeilings = 0;
            _totalRoofs = 0;
            _totalGrounds = 0;
        }

        public Face(Room parent, Surface geometry, Vector3d normal, Vector3d tempNormal)
        {
            this.guid = Guid.NewGuid();
            _totalFaces += 1;
            this.id = _totalFaces;
            //全体を通してのidとelementごとのidをつける
            this.geometry = geometry;
            this.parent = parent;
            this.parentId = parent.id;
            AreaMassProperties areaMp = AreaMassProperties.Compute(geometry);
            this.centerPt = areaMp.Centroid;
            this.area = areaMp.Area;
            this.normal = tempNormal;
            BrepVertexList vertices = geometry.ToBrep().Vertices;
            this.vertices = vertices;
            this.bauesVertices = getBauesVertices();


            //方角、床、天井を判別するためにはtempNormalを使う
            this.face = getFaceType(tempNormal);
            this.direction = defineDirection(tempNormal);

            //TODO: this logic has to be elaborated
            if (this.face == "wall")
            {
                this.tiltAngle = 90;
            }
            else
            {
                this.tiltAngle = 0;
            }
            this.windows = new List<Window>();
            this.windowIds = new List<int>();
        }
        private List<double> getBauesVertices()
        {
            List<double> bauesVertices = new List<double>();
            foreach (BrepVertex vertex in this.vertices)
            {
                Point3d pt = vertex.Location;
                bauesVertices.Add(pt.X);
                bauesVertices.Add(pt.Y);
                bauesVertices.Add(pt.Z);
            }
            return bauesVertices;
        }


        private string defineDirection(Vector3d normal)
        {
            if (this.face == "floor")
            {
                return "F";
            }
            else if (this.face == "roof")
            {
                return "CR";
            }

            Vector3d northAngle = new Vector3d(0, 1, 0);
            Vector3d westAngle = new Vector3d(-1, 0, 0);

            double angle1 = (180 / Math.PI) * (Vector3d.VectorAngle(normal, northAngle));
            double angle2 = (180 / Math.PI) * (Vector3d.VectorAngle(normal, westAngle));

            //Print("{0}:{1}", normal, angle1);

            if (angle1 < 45)
            {
                return "N";
            }
            else if (angle1 == 45 && angle2 == 45)
            {
                return "N";
            }
            else if (angle1 > 135)
            {
                return "S";
            }
            else if (angle1 == 135 && angle2 == 135)
            {
                return "S";
            }
            else if (angle2 <= 45)
            {
                return "W";
            }
            else
            {
                return "E";
            }
        }

        public string getFaceType(Vector3d normal)
        {

            //better to add filter logic for internal wall
            double Z = normal.Z;
            int roofAngle = 10;
            if (Z > Math.Cos((Math.PI / 180) * roofAngle))
            {
                //return faceType.roof;
                return "roof";
            }
            else if (Z < -Math.Cos((Math.PI / 180) * 85))
            {
                //return faceType.floor;
                return "floor";
            }
            else
            {
                //string direction = defineDirection(normal);
                //Print("{0}:{1}", normal, direction);
                //return faceType.wall;
                return "wall";
            }
        }

        public void addWindows(Window window)
        {
            this.windows.Add(window);
            this.windowIds.Add(window.id);
        }

        public void setElementType()
        {
            string firstPhrase = "";
            if (this.bc == "outdoor")
            {
                firstPhrase = "exterior";
            }
            else
            {
                firstPhrase = this.bc;
            }
            this.elementType = firstPhrase + this.face;
            //partIdをアサインする
            switch (this.elementType)
            {
                case "exteriorwall":
                    _totalExWalls += 1;
                    this.partId = _totalExWalls;
                    break;
                case "interiorwall":
                    _totalInWalls += 1;
                    this.partId = _totalInWalls;
                    break;
                case "interiorroof":
                    _totalFlrCeilings += 1;
                    this.partId = _totalFlrCeilings;
                    break;
                case "interiorfloor":
                    _totalFlrCeilings += 1;
                    this.partId = _totalFlrCeilings;
                    break;
                case "exteriorroof":
                    _totalRoofs += 1;
                    this.partId = _totalRoofs;
                    break;
                case "groundfloor":
                    _totalGrounds += 1;
                    this.partId = _totalGrounds;
                    break;
            }
        }

        public void setConstructionId()
        {
            //Print("{0}", this.elementType);
            switch (this.elementType)
            {
                case "exteriorwall":
                    this.constructionId = 1;
                    break;
                case "interiorwall":
                    this.constructionId = 2;
                    break;
                case "interiorroof":
                    this.constructionId = 3;
                    break;
                case "interiorfloor":
                    this.constructionId = 3;
                    break;
                case "exteriorroof":
                    this.constructionId = 4;
                    break;
                case "groundroof":
                    this.constructionId = 5;
                    break;
                case "groundfloor":
                    this.constructionId = 5;
                    break;
            }
        }
    }

    public class Window : BaseGeo
    {
        public int parentId { get; set; }
        public Face parent;
        public Surface geometry { get; private set; }
        public Vector3d normal { get; private set; }
        public Point3d centerPt;
        public double tiltAngle;
        public int constructionId; //とりあえず外壁1,内壁2,床(室内）3,屋根4,床（地面）5, 窓6　THERBのelement Idに相当
        public double area;
        public static int _totalWindows;

        static Window()
        {
            _totalWindows = 0;
        }

        public Window(Surface geometry)
        {
            this.guid = Guid.NewGuid();
            _totalWindows += 1;
            this.id = _totalWindows;
            this.geometry = geometry;
            AreaMassProperties areaMp = AreaMassProperties.Compute(geometry);
            this.centerPt = areaMp.Centroid;
            this.area = areaMp.Area;
            this.constructionId = 6;
        }
    }
}