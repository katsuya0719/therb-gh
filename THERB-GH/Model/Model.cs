using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.ObjectModel;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoundaryCondition
    {
        exterior,
        interior,
        ground
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurfaceType
    {
        Wall,
        Roof,
        Ceiling,
        Floor
    }

    public class BaseGeo{
        public Guid guid;
        public int id;
        public string displayName;
        public Point3d minPt;
        public Point3d maxPt;

        public Point3d getMinCoord(BrepVertexList vertices)
        {
            //(0,0,0)をoriginにすると、マイナスの座標を含むケースでうまくいかない
            //TODO:this logic should be elaborated more
            Point3d origin = new Point3d(-1000, -1000, -1000);
            int i = 0;
            int minId = 0;
            double minValue = 1000000;
            foreach (BrepVertex vertex in vertices)
            {
                Point3d checkPt = vertex.Location;
                double distance = origin.DistanceTo(checkPt);
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

        public Point3d getMaxCoord(BrepVertexList vertices)
        {
            Point3d origin = new Point3d(-1000, -1000, -1000);
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
        /*
        public static int _totalCount;

        static BaseGeo()
        {
            _totalCount = 0;
        }
        public BaseGeo()
        {
            _totalCount += 1;
            id = _totalCount;
        }
        */
    }

    public class Therb
    {
        public List<Room> rooms;
        public List<Face> faces;
        public List<Window> windows;
        public List<Overhang> overhangs;
        //shadingとかものちのちつけていく

        public Therb(List<Room> rooms, List<Face> faces, List<Window> windows, List<Overhang> overhangs)
        {
            this.rooms = rooms;
            this.faces = faces;
            this.windows = windows;
            this.overhangs = overhangs;
        }
    }

    public class Room : BaseGeo
    {
        public Brep geometry;
        public Point3d centroid;
        private List<Face> _faceList;
        public static int _totalRooms;
        public BrepVertexList vertices;
        public List<Face> sWalls;
        public List<Face> wWalls;
        public List<Face> nWalls;
        public List<Face> eWalls;
        public List<Face> floors;
        public List<Face> roofs;
        public double volume;
        static Room()
        {
            _totalRooms = 0;
        }

        public static void InitTotalRoom()
        {
            _totalRooms = 0;
        }

        public Room(Brep geometry)
        {
            guid = Guid.NewGuid();
            _totalRooms += 1;
            id = _totalRooms;
            //displayName = _totalRooms;
            _faceList = new List<Face>();
            this.geometry = geometry;
            BrepVertexList vertices = geometry.Vertices;
            this.vertices = vertices;
            minPt = getMinCoord(vertices);
            maxPt = getMaxCoord(vertices);
            sWalls = new List<Face>();
            wWalls = new List<Face>();
            nWalls = new List<Face>();
            eWalls = new List<Face>();
            floors = new List<Face>();
            roofs = new List<Face>();
            VolumeMassProperties vmp = VolumeMassProperties.Compute(geometry);
            centroid = vmp.Centroid;
            //brepのボリュームを計算する
            volume = geometry.GetVolume();
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
                    case Direction.S:
                        sWalls.Add(face);
                        break;
                    case Direction.N:
                        nWalls.Add(face);
                        break;
                    case Direction.W:
                        wWalls.Add(face);
                        break;
                    case Direction.E:
                        eWalls.Add(face);
                        break;
                }
            }
            else if (face.face == "roof")
            {
                roofs.Add(face);
            }
            else if (face.face == "floor")
            {
                floors.Add(face);
            }
        }

        public List<int> getDirectionList()
        {
            List<int> directionList = new List<int>();
            directionList.Add(sWalls.Count);
            directionList.Add(wWalls.Count);
            directionList.Add(nWalls.Count);
            directionList.Add(eWalls.Count);
            directionList.Add(floors.Count);
            directionList.Add(roofs.Count);
            int total = sWalls.Count + wWalls.Count + nWalls.Count + eWalls.Count + floors.Count + roofs.Count;
            directionList.Add(total);

            return directionList;//[S,W,N,E,F,CR,total]
        }

        public List<Face> getFaces()
        {
            return _faceList;
        }

        //public static string generateDisplayName{
        //}
        
    }
    
    public class BaseFace : BaseGeo
    {
        public int parentId;
        public Surface geometry;
        public int partId;
        public Vector3d normal;
        public Point3d centerPt;
        public double tiltAngle;
        public BrepVertexList vertices;
        public double area;
        public int constructionId; //とりあえず外壁1,内壁2,床(室内）5,屋根4,床（地面）5, 窓6　THERBのelement Idに相当/鈴木君からもらったデータにとりあえずそろえる
        public Overhang overhang;
        public int overhangId; //overhangは一つしか指定できない

        public BaseFace(Surface geometry)
        {
            this.geometry = geometry;
            //parentId = parent.id; parentの設定方法がFaceとFenestrationで違う
            AreaMassProperties areaMp = AreaMassProperties.Compute(geometry);
            centerPt = areaMp.Centroid;
            area = areaMp.Area;
            //normal = tempNormal; 今のところ、normalというattributeがない
            BrepVertexList vertices = geometry.ToBrep().Vertices;
            this.vertices = vertices;

            minPt = getMinCoord(vertices);
            maxPt = getMaxCoord(vertices);

        }
        public void addOverhangs(Overhang overhang)
        {
            this.overhang = overhang;
            overhangId = overhang.id;
        }
    }

    public enum FaceKeys
    {
        bc,
        elementType,
        direction,
    }

    public enum Direction
    {
        N,
        S,
        W,
        E,
        F,
        CR
    }
    public class Face : BaseFace
    {
        //public int partId;
        //public faceType face{get; private set;}
        public string face { get; private set; }
        public BoundaryCondition bc { get; set; }
        public string elementType { get; set; }
        public Room parent;
        public Vector3d tempNormal;
        public Direction direction;
        public SurfaceType surfaceType;
        public int adjacencyRoomId; //隣接しているRoomのId 外気に接している場合には0
        public List<Window> windows { get; private set; }
        public List<int> windowIds;
        public static int _totalFaces;
        public static int _totalExWalls;
        public static int _totalInWalls;
        public static int _totalFlrCeilings;
        public static int _totalRoofs;
        public static int _totalGrounds;
        public Face adjacencyFace;

        static Face()
        {
            _totalFaces = 0;
            _totalExWalls = 0;
            _totalInWalls = 0;
            _totalFlrCeilings = 0;
            _totalRoofs = 0;
            _totalGrounds = 0;
        }

        public static void InitTotalFace()
        {
            _totalFaces = 0;
            _totalExWalls = 0;
            _totalInWalls = 0;
            _totalFlrCeilings = 0;
            _totalRoofs = 0;
            _totalGrounds = 0;
        }

        public Face(Room parent, Surface geometry, Vector3d normal, Vector3d tempNormal):base(geometry)
        {
            guid = Guid.NewGuid();
            _totalFaces += 1;
            id = _totalFaces;
            //全体を通してのidとelementごとのidをつける
            this.parent = parent;
            parentId = parent.id;
            //方角、床、天井を判別するためにはtempNormalを使う
            face = getFaceType(tempNormal);
            direction = defineDirection(tempNormal);

            this.normal = tempNormal;

            //TODO: this logic has to be elaborated
            if (face == "wall")
            {
                tiltAngle = 90;
            }
            else
            {
                tiltAngle = 0;
            }
            windows = new List<Window>();
            windowIds = new List<int>();
        }

        /*
        public bool filterByKey<T>(T key,string value)
        {
            if (this.key == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */
        public bool filterByBc(BoundaryCondition bc)
        {
            if (this.bc == bc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //TODO: 他のFaceKeysに関しても適用できるよう汎用性を持たせたい（上みたいな感じ）
        public bool filterByDirection(Direction direction)
        {
            if (this.direction == direction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool filterBySurfaceType(SurfaceType surfT)
        {
            if (this.surfaceType == surfT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Direction defineDirection(Vector3d normal)
        {
            if (face == "floor")
            {
                return Direction.F;
            }
            else if (face == "roof")
            {
                return Direction.CR;
            }

            Vector3d northAngle = new Vector3d(0, 1, 0);
            Vector3d westAngle = new Vector3d(-1, 0, 0);

            double angle1 = (180 / Math.PI) * (Vector3d.VectorAngle(normal, northAngle));
            double angle2 = (180 / Math.PI) * (Vector3d.VectorAngle(normal, westAngle));

            //Print("{0}:{1}", normal, angle1);

            if (angle1 < 45)
            {
                return Direction.N;
            }
            else if (angle1 == 45 && angle2 == 45)
            {
                return Direction.N;
            }
            else if (angle1 > 135)
            {
                return Direction.S;
            }
            else if (angle1 == 135 && angle2 == 135)
            {
                return Direction.S;
            }
            else if (angle2 <= 45)
            {
                return Direction.W;
            }
            else
            {
                return Direction.E;
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
            windows.Add(window);
            windowIds.Add(window.id);
        }

        public void setElementType()
        {
            /*
            string firstPhrase = "";
            if (bc == "outdoor")
            {
                firstPhrase = "exterior";
            }
            else
            {
                firstPhrase = bc;
            }
            */
            elementType = bc.ToString() + face;
            //partIdをアサインする
            switch (elementType)
            {
                case "exteriorwall":
                    _totalExWalls += 1;
                    partId = _totalExWalls;
                    break;
                case "interiorwall":
                    _totalInWalls += 1;
                    partId = _totalInWalls;
                    break;
                case "interiorroof":
                    _totalFlrCeilings += 1;
                    partId = _totalFlrCeilings;
                    break;
                case "interiorfloor":
                    _totalFlrCeilings += 1;
                    partId = _totalFlrCeilings;
                    break;
                case "exteriorroof":
                    _totalRoofs += 1;
                    partId = _totalRoofs;
                    break;
                case "groundfloor":
                    _totalGrounds += 1;
                    partId = _totalGrounds;
                    break;
            }
        }

        public void setConstructionId()
        {
            //Print("{0}", elementType);
            switch (elementType)
            {
                case "exteriorwall":
                    constructionId = 1;
                    break;
                case "interiorwall":
                    constructionId = 2;
                    break;
                case "interiorroof":
                    constructionId = 3;
                    break;
                case "interiorfloor":
                    constructionId = 3;
                    break;
                case "exteriorroof":
                    constructionId = 4;
                    break;
                case "groundroof":
                    constructionId = 5;
                    break;
                case "groundfloor":
                    constructionId = 5;
                    break;
            }
        }
    }

    public class Window : BaseFace
    {
        public Face parent;
        public static int _totalWindows;
        
        static Window()
        {
            _totalWindows = 0;
        }

        public static void InitTotalWindow()
        {
            _totalWindows = 0;
        }

            public Window(Surface geometry):base(geometry)
        {
            guid = Guid.NewGuid();
            _totalWindows += 1;
            id = _totalWindows;
            this.constructionId = 6;
        }

        public void addParent(Face parent)
        {
            this.parent = parent;
            parentId = parent.partId;//注意)part idを使っている
            tiltAngle = parent.tiltAngle;
        }
    }

    //overhangはwallに紐づく
    public class Overhang:BaseFace
    {
        public Face parentFace; //overhangの場合は、parentFace,parentWindowどちらも持つ
        public Window parentWindow;
        public int parentWindowId;
        public static int _totalOverhangs;

        static Overhang()
        {
            _totalOverhangs = 0;
        }

        public Overhang(Surface geometry) : base(geometry)
        {
            guid = Guid.NewGuid();
            _totalOverhangs += 1;
            id = _totalOverhangs;
            this.constructionId = 1; //とりあえず外壁に揃えて1とする
        }

        public void addParentFace(Face parentFace)
        {
            this.parentFace = parentFace;
            parentId = parentFace.partId;
            tiltAngle = parentFace.tiltAngle;
        }

        public void addParentWindow(Window parentWindow)
        {
            this.parentWindow = parentWindow;
            parentWindowId = parentWindow.id;
        }
    }
}