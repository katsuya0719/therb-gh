using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Utils
{
    public class Converter
    {

        public string FillEmpty(int input, int totalLength)
        {
            string inputStr = input.ToString();
            int inputLength = inputStr.Length;
            string emptyString = new string(' ', totalLength - inputLength);

            return emptyString + inputStr;
        }
    }
    
}