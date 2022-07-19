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

        public Window(Surface geometry) : base(geometry)
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
}
