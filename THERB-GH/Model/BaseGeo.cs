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
    public class BaseGeo
    {
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
}
