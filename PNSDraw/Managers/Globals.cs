using System;
using System.Collections.Generic;
using System.Text;

namespace PNSDraw
{
    public static class Globals
    {
        public static int MaterialSize = 100;
        public static int EdgeNodeSize = 40;
        public static int TemporaryEdgeNodeSize = 20;
        public static int OperatingUnitWidth = 750;
        public static int OperatingUnitHeight = 100;
        public static int GridSize = 300;

        public static int DefaultFontSize = 12;

        public static int DefaultLineHeight = 120;

        public static int LineSize = 10;

        public static Dictionary<string, string> DefaultParameters = new Dictionary<string,string>();


        public static void ResetDefaults()
        {
            DefaultFontSize = 12;
            GridSize = 300;
        }

        public static class MaterialTypes
        {
            public const int Raw = 0;
            public const int Intermediate = 1;
            public const int Product = 2;
        }

        public static class PrintViewSettings
        {
            public static bool ShowMaterialText = true;
            public static bool ShowOperatingUnitText = true;
            public static bool ShowEdgeText = true;
            public static bool ShowComments = true;
            public static bool ShowParameters = true;
        }


        public static class SolutionSettings
        {

            public enum ValueStyle { None = 0, Original=1, Calculated=2}

            public class StructureItem
            {

                public System.Drawing.Color Color = System.Drawing.Color.Black;

                public ValueStyle EdgeText = ValueStyle.Calculated;
                public ValueStyle MaterialText = ValueStyle.Original;
                public ValueStyle OperatingUnitText = ValueStyle.Original;
            }

            public static StructureItem IncludedItem = new StructureItem();

            public static StructureItem ExcludedItem = new StructureItem();

            static SolutionSettings()
            {
                
                ExcludedItem.Color = System.Drawing.Color.LightGray;
                ExcludedItem.EdgeText = ValueStyle.None;
                 
            }

        }

        static Globals()
        {
            DefaultParameters.Add("price", "0");
            DefaultParameters.Add("payoutperiod", "10");
            DefaultParameters.Add("workinghour", "8000");
        }
    }
}
