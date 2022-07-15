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

        public static string FillEmpty(int input, int totalLength)
        {
            string inputStr = input.ToString();
            int inputLength = inputStr.Length;
            string emptyString = new string(' ', totalLength - inputLength);

            return emptyString + inputStr;
        }

        public static string FillEmpty(string input, int totalLength)
        {
            int inputLength = input.Length;
            string emptyString = new string(' ', totalLength - inputLength);

            return emptyString + input;
        }

        public static string FillEmpty(double input, int totalLength, int digit)
        {
            string zeros = new String('0', digit);
            string format = "{0:0." + zeros + "}";
            string inputValue = string.Format(format, input);
            string emptyString = new string(' ', totalLength - inputValue.Length);

            return emptyString + inputValue;
        }
    }
    
}