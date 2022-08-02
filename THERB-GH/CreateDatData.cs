using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Rhino.Geometry.Intersect;
using Newtonsoft.Json;
using Model;
using Utils;

namespace THERBgh
{
    public class CreateDatData
    {
        public static string CreateADat(Therb therb)
        {

            List<Room> roomList = therb.rooms;
            string aDat = "*hourly data od forced room air ventilation \r\n";

            roomList.ForEach(room =>
            {
                aDat += "into room" + Converter.FillEmpty(room.id, 3) + "from " + Converter.FillEmpty(0, 3)
                    + "=>  \r\n"
                    + Converter.FillEmpty("from outdoor=0", 25)
                    + Converter.FillEmpty("- ", 6)
                    + string.Join("", Enumerable.Repeat(Converter.FillEmpty(room.volume / 2, 7, 1), 12)) + "\r\n" //12回繰り返して呼ぶようにしたい
                    + Converter.FillEmpty("quantity (m3/h)", 25)
                    + Converter.FillEmpty("- ", 6) + "\r\n"
                    + Converter.FillEmpty("(-1.:natural vent.)", 25)
                    + string.Join("", Enumerable.Repeat(Converter.FillEmpty(room.volume / 2, 7, 1), 12)) + "\r\n"; //12回繰り返して呼ぶようにしたい
            });

            return aDat;
        }

        public static string CreateBDat(Therb therb)
        {

            string bDat = "";

            List<Room> roomList = therb.rooms;
            List<Face> faceList = therb.faces;
            List<Window> windowList = therb.windows;
            List<Overhang> overhangList = therb.overhangs;


            //TODO:doorの入力UIも作る
            List<Face> doorList = new List<Face>();
            List<Face> shadingList = new List<Face>();
            List<Face> wingList = new List<Face>();

            if (roomList == null) throw new NullReferenceException("ルームリストが読み込めませんでした。");

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
                }
                else if (face.elementType == "interiorwall" && face.unique)
                {
                    interiorWalls.Add(face);
                }
                else if (face.elementType == "exteriorroof")
                {
                    exteriorRoofs.Add(face);
                }
                else if (face.elementType == "interiorroof" || face.elementType == "interiorfloor")
                {
                    if (face.unique)
                    {
                        interiorRoofFloors.Add(face);
                    }

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
            bDat += Converter.FillEmpty(roomList.Count, 4)
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
                bDat += Converter.FillEmpty("Room " + room.id.ToString(), 12)
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
                bDat += Converter.FillEmpty("Ex-wall " + exWall.partId.ToString(), 13)
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
                bDat += Converter.FillEmpty("Window " + window.id.ToString(), 13)
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
                bDat += Converter.FillEmpty("In-wall " + inWall.partId.ToString(), 13)
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
                + " \r\n     in-door No. " + doorIds + " \r\n";
            });

            //床・天井の情報を抽出
            interiorRoofFloors.ForEach(inMat =>
            {
                bDat += Converter.FillEmpty("flr&cling " + inMat.partId.ToString(), 13)
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
                bDat += Converter.FillEmpty("Roof " + roof.partId.ToString(), 13)
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
                bDat += Converter.FillEmpty("Ground " + floor.partId.ToString(), 13)
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

            return bDat;
        }

        public static string CreateRDat(Therb therb)
        {

            List<Room> roomList = therb.rooms;
            List<Face> faceList = therb.faces;
            List<Window> windowList = therb.windows;

            string rDat = "";

            if (roomList == null) throw new NullReferenceException("ルームリストが読み込めませんでした。");

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




            roomList.ForEach(room =>
            {
                List<int> directions = room.getDirectionList();
                //idList.Add(room.id);
                rDat += Converter.FillEmpty(room.id, 5)
                + Converter.FillEmpty(directions[0], 5)
                + Converter.FillEmpty(directions[1], 5)
                + Converter.FillEmpty(directions[2], 5)
                + Converter.FillEmpty(directions[3], 5)
                + Converter.FillEmpty(directions[4], 5)
                + Converter.FillEmpty(directions[5], 5)
                + Converter.FillEmpty(directions[6], 5) + " \r\n";
            });


            //Faceデータに対する処理
            roomList.ForEach(room =>
            {
                int id = 1;
                faceList.ForEach(face =>
                {
                    if (room.id == face.parentId)
                    {
                        rDat += Converter.FillEmpty(room.id, 5)
                        + Converter.FillEmpty(id, 5)
                        + Converter.FillEmpty(directionDict[face.direction.ToString()], 5)
                        + Converter.FillEmpty(directionCount[face.direction.ToString()], 5)
                        + Converter.FillEmpty(face.constructionId, 5)
                        + Converter.FillEmpty(face.partId, 5)
                        + Converter.FillEmpty(face.adjacencyRoomId, 5) + " \r\n";

                        directionCount[face.direction.ToString()] += 1;
                        id += 1;

                        //TODO:窓に関する処理を追加
                        
                        face.windows.ForEach(window =>
                        {
                            rDat += Converter.FillEmpty(room.id, 5)
                            + Converter.FillEmpty(id, 5)
                            + Converter.FillEmpty(directionDict[face.direction.ToString()], 5)
                            + Converter.FillEmpty(directionCount[face.direction.ToString()], 5)
                            + Converter.FillEmpty(6, 5)
                            + Converter.FillEmpty(window.id, 5)
                            + Converter.FillEmpty(face.adjacencyRoomId, 5) + " \r\n";

                            id += 1;
                        });
                    }


                });
            });

            return rDat;
        }

        public static string CreateWDat()
        {
            //TODO: this mock should be dynamic
            string mock =
                @"{
                ""data"": [
                    {
                        ""categories"": ""interiorWall"",
                        ""description"": ""plywood"",
                        ""id"": 1,
                        ""materials"": [
                            {
                                ""conductivity"": 0.111,
                                ""density"": 550.0,
                                ""description"": ""from Therb"",
                                ""id"": 1,
                                ""name"": ""plywood"",
                                ""specificHeat"": 1880.0
                            }
                        ],
                        ""thickness"": [50],
                        ""name"": ""plywood interior wall"",
                    }
                ],
            }";

            //読み込んだAPIのresponseをパースする
            Mock source = JsonConvert.DeserializeObject<Mock>(mock);

            List<Construction> constructions = source.data;

            //w.datデータを構成していく  
            string wDat = "";

            var elementIdDict = new Dictionary<string, int>(){
                {"exteriorWall",1},
                {"interiorWall",2},
                {"floorCeiling",3},
                {"roof",4},
                {"groundFloor",5},
                {"window",6},
            };

            var classificationDict = new Dictionary<string, int>(){
                {"exteriorWall",1},
                {"interiorWall",1},
                {"floorCeiling",1},
                {"roof",1},
                {"groundFloor",1},
                {"window",6},
            };

            constructions.ForEach(construction =>
            {
                int numMaterials = construction.materials.Count;

                wDat += Converter.FillEmpty(construction.id, 3)
                + Converter.FillEmpty(elementIdDict[construction.categories], 2)
                + " 0.70 0.90 0.70 0.90 0.000e-09 0.000e-10"
                + Converter.FillEmpty(numMaterials, 3) + " \r\n";

                //2行目入力 classification
                
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(classificationDict[construction.categories], 10);
                });
                wDat += FillMultipleZeros(13-numMaterials,10,0);
                wDat += " \r\n";

                //3行目入力 分割数
                //TODO: 厚みとかによってdynamicに分割数が変わるロジック
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(1, 10);
                });
                wDat += FillMultipleZeros(13 - numMaterials, 10,0);
                wDat += " \r\n";

                //4行目入力 厚み
                construction.thickness.ForEach(thickness =>
                {
                    wDat += Converter.FillEmpty(thickness, 10, 3);
                });
                wDat += FillMultipleZeros(13 - numMaterials, 10,3);
                wDat += " \r\n";

                //5行目　熱伝導率
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(material.conductivity, 10,3);
                });
                wDat += FillMultipleZeros(13 - numMaterials, 10, 3);
                wDat += " \r\n";

                //6行目　比熱
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(material.specificHeat, 10, 1);
                });
                wDat += FillMultipleZeros(13 - numMaterials, 10, 1);
                wDat += " \r\n";

                //7行目　密度
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(material.density, 10, 1);
                });
                wDat += FillMultipleZeros(13 - numMaterials, 10, 1);
                wDat += " \r\n";

                //8行目　水分伝導率
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(0, 10);
                });
                wDat += " \r\n";

                //9行目　水分容量
                construction.materials.ForEach(material =>
                {
                    wDat += Converter.FillEmpty(0, 10);
                });
                wDat += " \r\n";

            });

            return wDat;
        }

        private static string FillMultipleZeros(int repeatNum,int totalLength,int digit)
        {
            string result = "";
            for (int i = 0; i < repeatNum; i++)
            {
                result += Converter.FillEmpty(0, totalLength,digit);
            }
            return result;
        }

        private static string OutputWindowIds(Face face)
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
    }
    public class Mock
    {
        public List<Construction> data;
    }

    public class Construction
    {
        public int id;
        public string categories;
        public List<Material> materials;
        public List<Double> thickness;
        
    }

    public class Material
    {
        public int id;
        public string name;
        public double conductivity;
        public double density;
        public double specificHeat;
    }
}


