using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace PNSDraw
{
    /// <summary>
    ///    Az osztály tulajdonságinak a sorrendjét beállító rendező soztály\n
    /// </summary>
    /**Converter, melynek segítségével be lehet állítani,\n
    *  hogy a propertygridbe (osztály property-jeit megjelenítő ablak)\n
    *  milyen sorrendbe jelnjenek meg a tulajdonságok.
    * <p></p>
    * - Talált kód - www.codeprojekt.com
    * */
    public class PropertySorter : ExpandableObjectConverter
    {
        string oldString;

        #region Methods

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(ObjectProperty))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is ObjectProperty)
            {

                ObjectProperty so = (ObjectProperty)value;
                oldString = so.ToString();
                return so.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            ObjectProperty op;
            //TODO: oldStringből kiszedni Splittel az értékeket, és megvizsgálni, hogy az újban 3 space-van-e, ha nem, akkor az oldString alapján létrehozott op-ot adom vissza
            if ((((string)value).Split(' ').Length - 1) == 2)
            {
                string[] oldValues = ((string)value).Split(' ');
                op = new ObjectProperty(oldValues[0] + " ");
                op.Value = double.Parse(oldValues[1]);
                op.MU = oldValues[2];
                return op;
            }
            else
            {
                string[] oldValues = oldString.Split(' ');
                op = new ObjectProperty(oldValues[0] + " ");
                op.Value = double.Parse(oldValues[1]);
                op.MU = oldValues[2];
                return op;
            }
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //
            // This override returns a list of properties in order
            //
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            ArrayList orderedProperties = new ArrayList();
            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];
                if (attribute != null)
                {
                    //
                    // If the attribute is found, then create an pair object to hold it
                    //
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    //
                    // If no order attribute is specifed then given it an order of 0
                    //
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }
            //
            // Perform the actual order using the value PropertyOrderPair classes
            // implementation of IComparable to sort
            //
            orderedProperties.Sort();
            //
            // Build a string list of the ordered names
            //
            ArrayList propertyNames = new ArrayList();
            foreach (PropertyOrderPair pop in orderedProperties)
            {
                propertyNames.Add(pop.Name);
            }
            //
            // Pass in the ordered list for the PropertyDescriptorCollection to sort by
            //
            return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
        }
        #endregion
    }

    /// <summary>
    ///    A property gridbe való megjelenítést segítő osztály\n
    /// </summary>
    #region Helper Class - PropertyOrderAttribute
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        //
        // Simple attribute to allow the order of a property to be specified
        //
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get
            {
                return _order;
            }
        }
    }
    #endregion

    /// <summary>
    ///    A property gridbe való megjelenítést segítő osztály\n
    /// </summary>
    #region Helper Class - PropertyOrderPair
    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public PropertyOrderPair(string name, int order)
        {
            _order = order;
            _name = name;
        }

        public int CompareTo(object obj)
        {
            //
            // Sort the pair objects by ordering by order value
            // Equal values get the same rank
            //
            int otherOrder = ((PropertyOrderPair)obj)._order;
            if (otherOrder == _order)
            {
                //
                // If order not specified, sort by name
                //
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
            {
                return -1;
            }
            return 1;
        }
    }
    #endregion

}
