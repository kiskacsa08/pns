using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial
{
    public class def_mat_prop
    {
        #region Members
        static public string cat1 = "Material properties";

        static public string DN_name = "Name";
        static public string D_name = "Name of the material. It must be unique in the problem definition.";

        static public string DN_typename = "Type";
        static public string D_typename = "Material type.";
        static public string V_typename_r = "raw material";
        static public string V_typename_i = "intermediate";
        static public string V_typename_p = "product";

        static public string DN_quantity = "Quantity type";
        static public string D_quantity = "Type of material quantity.";

        static public string DN_raw_min = "Required flow";
        static public string D_raw_min = "";
        static public string DN_raw_minmu = "Required flow Mu";
        static public string D_raw_minmu = "Measurement unit of required flow.";

        static public string DN_raw_max = "Maximum available flow";
        static public string D_raw_max = "";
        static public string DN_raw_maxmu = "Maximum flow Mu";
        static public string D_raw_maxmu = "Measurement unit of maximum flow.";

        static public string DN_intermediate_min = "Required flow";
        static public string D_intermediate_min = "";
        static public string DN_intermediate_minmu = "Flow Mu";
        static public string D_intermediate_minmu = "Measurement unit of flow.";

        static public string DN_intermediate_max = "Maximum available flow";
        static public string D_intermediate_max = "";
        static public string DN_intermediate_maxmu = "Maximum flow Mu";
        static public string D_intermediate_maxmu = "Measurement unit of maximum flow.";

        static public string DN_product_min = "Required flow";
        static public string D_product_min = "";
        static public string DN_product_minmu = "Required flow Mu";
        static public string D_product_minmu = "Measurement unit of required flow.";

        static public string DN_product_max = "Maximum flow";
        static public string D_product_max = "";
        static public string DN_product_maxmu = "Maximum flow Mu";
        static public string D_product_maxmu = "Measurement unit of maximum flow.";

        static public string DN_price = "Price";
        static public string D_price = "";
        static public string DN_pricemu = "Price Mu";
        static public string D_pricemu = "Measurement unit of price.";

        static public string DN_description = "Description";
        static public string D_description = "Description";
        #endregion

        #region Properties
        public string Category { get { return cat1; } set { cat1 = value; } }
        public PropertyGridItemXMLTag Name
        {
            get { return new PropertyGridItemXMLTag(DN_name, D_name); }
            set
            {
                DN_name = value.DisplayName;
                D_name = value.Description;
            }
        }
        public PropertyGridItemXMLTag Type
        {
            get { return new PropertyGridItemXMLTag(DN_typename, D_typename, V_typename_r, V_typename_i, V_typename_p); }
            set
            {
                DN_typename = value.DisplayName;
                D_typename = value.Description;
                V_typename_r = value.Value1;
                V_typename_i = value.Value2;
                V_typename_p = value.Value3;
            }
        }
        public PropertyGridItemXMLTag Quantity
        {
            get { return new PropertyGridItemXMLTag(DN_quantity, D_quantity); }
            set
            {
                DN_quantity = value.DisplayName;
                D_quantity = value.Description;
            }
        }
        public PropertyGridItemXMLTag RawMin
        {
            get { return new PropertyGridItemXMLTag(DN_raw_min, D_raw_min); }
            set
            {
                DN_raw_min = value.DisplayName;
                D_raw_min = value.Description;
            }
        }
        public PropertyGridItemXMLTag RawMinMU
        {
            get { return new PropertyGridItemXMLTag(DN_raw_minmu, D_raw_minmu); }
            set
            {
                DN_raw_minmu = value.DisplayName;
                D_raw_minmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag RawMax
        {
            get { return new PropertyGridItemXMLTag(DN_raw_max, D_raw_max); }
            set
            {
                DN_raw_max = value.DisplayName;
                D_raw_max = value.Description;
            }
        }
        public PropertyGridItemXMLTag RawMaxMU
        {
            get { return new PropertyGridItemXMLTag(DN_raw_maxmu, D_raw_maxmu); }
            set
            {
                DN_raw_maxmu = value.DisplayName;
                D_raw_maxmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag IntermediateMin
        {
            get { return new PropertyGridItemXMLTag(DN_intermediate_min, D_intermediate_min); }
            set
            {
                DN_intermediate_min = value.DisplayName;
                D_intermediate_min = value.Description;
            }
        }
        public PropertyGridItemXMLTag IntermediateMinMU
        {
            get { return new PropertyGridItemXMLTag(DN_intermediate_minmu, D_intermediate_minmu); }
            set
            {
                DN_intermediate_minmu = value.DisplayName;
                D_intermediate_minmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag IntermediateMax
        {
            get { return new PropertyGridItemXMLTag(DN_intermediate_max, D_intermediate_max); }
            set
            {
                DN_intermediate_max = value.DisplayName;
                D_intermediate_max = value.Description;
            }
        }
        public PropertyGridItemXMLTag IntermediateMaxMU
        {
            get { return new PropertyGridItemXMLTag(DN_intermediate_maxmu, D_intermediate_maxmu); }
            set
            {
                DN_intermediate_maxmu = value.DisplayName;
                D_intermediate_maxmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag ProductMin
        {
            get { return new PropertyGridItemXMLTag(DN_product_min, D_product_min); }
            set
            {
                DN_product_min = value.DisplayName;
                D_product_min = value.Description;
            }
        }
        public PropertyGridItemXMLTag ProductMinMU
        {
            get { return new PropertyGridItemXMLTag(DN_product_minmu, D_product_minmu); }
            set
            {
                DN_product_minmu = value.DisplayName;
                D_product_minmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag ProductMax
        {
            get { return new PropertyGridItemXMLTag(DN_product_max, D_product_max); }
            set
            {
                DN_product_max = value.DisplayName;
                D_product_max = value.Description;
            }
        }
        public PropertyGridItemXMLTag ProductMaxMU
        {
            get { return new PropertyGridItemXMLTag(DN_product_maxmu, D_product_maxmu); }
            set
            {
                DN_product_maxmu = value.DisplayName;
                D_product_maxmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag Price
        {
            get { return new PropertyGridItemXMLTag(DN_price, D_price); }
            set
            {
                DN_price = value.DisplayName;
                D_price = value.Description;
            }
        }
        public PropertyGridItemXMLTag PriceMU
        {
            get { return new PropertyGridItemXMLTag(DN_pricemu, D_pricemu); }
            set
            {
                DN_pricemu = value.DisplayName;
                D_pricemu = value.Description;
            }
        }
        public PropertyGridItemXMLTag Description
        {
            get { return new PropertyGridItemXMLTag(DN_description, D_description); }
            set
            {
                DN_description = value.DisplayName;
                D_description = value.Description;
            }
        }
        #endregion
    }
}
