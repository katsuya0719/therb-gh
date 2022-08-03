using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.ObjectModel;

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
        public bool unique;
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

        public Face(Room parent, Surface geometry, Vector3d normal, Vector3d tempNormal) : base(geometry)
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
            this.unique = true;

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

        public void setPartId()
        {
            if (unique)
            {
                switch (elementType)
                {
                    case "exteriorwall":
                        partId = _totalExWalls;
                        break;
                    case "interiorwall":
                        partId = _totalInWalls;
                        break;
                    case "interiorroof":
                        partId = _totalFlrCeilings;
                        break;
                    case "interiorfloor":
                        partId = _totalFlrCeilings;
                        break;
                    case "exteriorroof":
                        partId = _totalRoofs;
                        break;
                    case "groundfloor":
                        partId = _totalGrounds;
                        break;
                }
            }
        }

        public void setElementType()
        {
            if (!unique) return;

            elementType = bc.ToString() + face;

            switch (elementType)
            {
                case "exteriorwall":
                    _totalExWalls += 1;
                    break;
                case "interiorwall":
                    _totalInWalls += 1;
                    break;
                case "interiorroof":
                    _totalFlrCeilings += 1;
                    break;
                case "interiorfloor":
                    _totalFlrCeilings += 1;
                    break;
                case "exteriorroof":
                    _totalRoofs += 1;
                    break;
                case "groundfloor":
                    _totalGrounds += 1;
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
}
