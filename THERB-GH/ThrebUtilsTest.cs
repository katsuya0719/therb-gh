using System;
using Utils;
using THERBgh;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Rhino.Geometry.Intersect;
using Newtonsoft.Json;
using Model;

namespace THERBgh
{
    public static class ThrebUtilsTest
    {
        public static void ExBoxTest()
        {
            var boxes = new List<Box>();
            boxes.Add(new Box(
                basePlane: Plane.WorldXY,
                xSize: new Interval(0, 4000),
                ySize: new Interval(0, 8000),
                zSize: new Interval(0, 8000)));
            boxes.Add(new Box(
                basePlane: Plane.WorldXY,
                xSize: new Interval(4000, 8000),
                ySize: new Interval(0, 8000),
                zSize: new Interval(0, 3000)));
            boxes.Add(new Box(
                basePlane: Plane.WorldXY,
                xSize: new Interval(4000, 8000),
                ySize: new Interval(0, 4000),
                zSize: new Interval(3000, 6000)));
            boxes.Add(new Box(
                basePlane: Plane.WorldXY,
                xSize: new Interval(4000, 8000),
                ySize: new Interval(4000, 8000),
                zSize: new Interval(3000, 6000)));
            var boxTest = boxes[0].ToBrep();
            //var exBoxes = ExBox.SplitGeometry(boxes, 0.1);
            /*foreach (var exBox in exBoxes)
            {
                Debug.WriteLine(exBox.BoxSurfaces.Count);
            }*/
        }
    }
}
