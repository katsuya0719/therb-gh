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
}
