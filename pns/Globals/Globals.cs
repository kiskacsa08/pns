using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using DynamicPropertyGrid;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns
{
    internal class CustomExpandableConverter : ExpandableObjectConverter
    {
        #region Member functions
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value is IOMaterials) return ((IOMaterials)value).Count == 0 ? "-" : "("+((IOMaterials)value).Count.ToString()+")";
                if (value is oCostParams) return "";
                if (value is iCostParams) return "";
                if (value is CapacityParams) return "";
                if (value is IOMaterial) return "";
                if (value is OperatingCost) return "";
                if (value is InvestmentCost) return "";
                if (value is OverallCost) return "";
                if (value is Bounds) return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }

    [TypeConverter(typeof(DropDownList))]
    public class MU
    {
        #region Members
        private int f_category_id;
        private int f_id;
        #endregion

        #region Constructors
        public MU(int t_category_id, int t_id)
        {
            f_category_id = t_category_id;
            f_id = t_id;
        }
        #endregion

        #region Member functions
        public bool IsEquivalent(int t_category_id, int t_id) { return f_category_id == t_category_id && f_id == t_id; }
        public XMLMU ToXMLMU()
        {
            XMLMU xmlmu = new XMLMU();
            xmlmu.group_id = f_category_id;
            xmlmu.item_id = f_id;
            return xmlmu;
        }
        public override string ToString() { return DefaultMUsAndValues.GetSymbol(f_category_id, f_id); }
        #endregion

        #region Properties
        public int CategoryID { get { return f_category_id; } }
        public int ItemID { get { return f_id; } }
        #endregion
    }

    public class MUs : List<MU>
    {
        #region Constructors
        public MUs(int t_category_id) { DefaultMUsAndValues.GetMUs(t_category_id, this); }
        #endregion

        #region Member functions
        public MU Find(int t_category_id, int t_id) { return Find(t_category_id, t_id, false); }
        public MU Find(int t_category_id, int t_id, bool t_null_allowed)
        {
            foreach (MU mu in this) if (mu.IsEquivalent(t_category_id, t_id)) return mu;
            if (t_null_allowed) return null;
            throw new Exception(def_MU_ex.Ex_MU_not_found);
        }
        #endregion
    }

    [TypeConverter(typeof(DropDownList))]
    public partial class FractionMU
    {
        #region Members
        private MU f_numerator_mu;
        private MU f_denominator_mu;
        #endregion

        #region Constructors
        public FractionMU(MU t_numerator_mu, MU t_denominator_mu)
        {
            f_numerator_mu = t_numerator_mu;
            f_denominator_mu = t_denominator_mu;
        }
        public FractionMU(XMLFlowMU flowmu)
        {
            f_numerator_mu = DefaultMUsAndValues.MUs.FindMU(flowmu.quantity_mu);
            f_denominator_mu = DefaultMUsAndValues.MUs.FindMU(flowmu.time_mu);
        }
        public FractionMU(XMLPriceMU pricemu)
        {
            f_numerator_mu = DefaultMUsAndValues.MUs.FindMU(pricemu.currency_mu);
            f_denominator_mu = DefaultMUsAndValues.MUs.FindMU(pricemu.quantity_mu);
        }
        #endregion

        #region Member functions
        public XMLFlowMU ToXMLFlowMU()
        {
            XMLFlowMU xmlflowmu = new XMLFlowMU();
            xmlflowmu.quantity_mu = f_numerator_mu.ToXMLMU();
            xmlflowmu.time_mu = f_denominator_mu.ToXMLMU();
            return xmlflowmu;
        }
        public XMLPriceMU ToXMLPriceMU()
        {
            XMLPriceMU xmlpricemu = new XMLPriceMU();
            xmlpricemu.currency_mu = f_numerator_mu.ToXMLMU();
            xmlpricemu.quantity_mu = f_denominator_mu.ToXMLMU();
            return xmlpricemu;
        }
        public XMLCostMU ToXMLCostMU()
        {
            XMLCostMU xmlcostmu = new XMLCostMU();
            xmlcostmu.currency_mu = f_numerator_mu.ToXMLMU();
            xmlcostmu.time_mu = f_denominator_mu.ToXMLMU();
            return xmlcostmu;
        }
        public override string ToString()
        {
            return f_numerator_mu.ToString() + "/" + f_denominator_mu.ToString();
        }
        public bool IsEquivalent(MU t_numerator, MU t_denominator) { return f_numerator_mu == t_numerator && f_denominator_mu == t_denominator; }
        #endregion

        #region Properties
        public MU Numerator { get { return f_numerator_mu; } }
        public MU Denominator { get { return f_denominator_mu; } }
        #endregion
    }

    public class FractionMUs : List<FractionMU>
    {
        #region Constructors
        public FractionMUs(MUs t_numerators, MUs t_denominators)
        {
            foreach (MU t_numerator in t_numerators)
            {
                foreach (MU t_denominator in t_denominators)
                {
                    Add(new FractionMU(t_numerator, t_denominator));
                }
            }
        }
        #endregion

        #region Member functions
        public FractionMU Find(MU t_numerator, MU t_denominator, bool t_null_allowed)
        {
            foreach (FractionMU item in this) if (item.IsEquivalent(t_numerator, t_denominator)) return item;
            if (t_null_allowed) return null;
            throw new Exception(def_MU_ex.Ex_FMU_not_found);
        }
        public FractionMU Find(MU t_numerator, MU t_denominator) { return Find(t_numerator, t_denominator, false); }
        public FractionMU Find(FractionMU mu, bool t_null_allowed) { return Find(mu.Numerator, mu.Denominator, t_null_allowed); }
        public FractionMU Find(FractionMU mu) { return Find(mu, false); }
        #endregion
    }

    [TypeConverter(typeof(DropDownList))]
    public class MUCategory
    {
        #region Members
        private int f_category_id;
        private MUs f_mus;
        private MU f_def_mu;
        private FractionMUs f_flow_mus;
        private FractionMUs f_price_mus;
        #endregion

        #region Constructors
        public MUCategory(int t_category_id)
        {
            f_category_id = t_category_id;
            f_mus = new MUs(f_category_id);
            f_def_mu = f_mus[0];
            f_flow_mus = null;
            f_price_mus = null;
        }
        #endregion

        #region Member functions
        public override string ToString() { return DefaultMUsAndValues.GetCategory(f_category_id); }
        public MU FindMU(int t_category_id, int t_id) { return f_mus.Find(t_category_id, t_id); }
        #endregion

        #region Properties
        public string Category { get { return DefaultMUsAndValues.GetCategory(f_category_id); } }
        public int CategoryID { get { return f_category_id; } }
        public MUs MUList { get { return f_mus; } }
        public MU DefaultMU { get { return f_def_mu; } set { f_def_mu = value; } }
        public FractionMUs FlowMUs
        {
            get
            {
                if (f_flow_mus == null) f_flow_mus = new FractionMUs(MUList, DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultTimeMU).MUList);
                return f_flow_mus;
            }
        }
        public FractionMUs PriceMUs
        {
            get
            {
                if (f_price_mus == null) f_price_mus = new FractionMUs(DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultCurrencyMU).MUList, MUList);
                return f_price_mus;
            }
        }
        public FractionMU DefaultFlowMU
        {
            get
            {
                return FlowMUs.Find(DefaultMU, DefaultMUsAndValues.MUs.DefaultTimeMU);
            }
        }
        public FractionMU DefaultPriceMU
        {
            get
            {
                return PriceMUs.Find(DefaultMUsAndValues.MUs.DefaultCurrencyMU, DefaultMU);
            }
        }
        #endregion
    }

    public class MUCategories : List<MUCategory>
    {
        #region Constructors
        public MUCategories() { DefaultMUsAndValues.GetCategories(this); }
        #endregion

        #region Member functions
        public MUCategory Find(int t_category_id, bool t_null_allowed)
        {
            foreach (MUCategory category in this) if (category.CategoryID == t_category_id) return category;
            if (t_null_allowed) return null;
            throw new Exception(def_MU_ex.Ex_category_not_found);
        }
        public MUCategory Find(int t_category_id) { return Find(t_category_id, false); }
        public MU FindMU(int t_group_id, int t_item_id)
        {
            if (t_group_id == (int)defaults.MU_Groups.base_group)
            {
                foreach (MUCategory t_category in this)
                {
                    foreach (MU t_mu in t_category.MUList)
                    {
                        if (t_mu.IsEquivalent(t_group_id, t_item_id)) return t_mu;
                    }
                }
                throw new Exception(def_MU_ex.Ex_unit_not_found);
            }
            else return Find(t_group_id).FindMU(t_group_id, t_item_id);
        }
        public MU FindMU(XMLMU mu) { return FindMU(mu.group_id, mu.item_id); }
        public MUCategory GetDerivedCategoryFromMU(int t_group_id, int t_item_id) { return GetDerivedCategoryFromMU(t_group_id, t_item_id, false); }
        public MUCategory GetDerivedCategoryFromMU(int t_group_id, int t_item_id, bool t_null_allowed)
        {
            foreach (MUCategory t_category in this)
            {
                foreach (MU t_mu in t_category.MUList)
                {
                    if (t_mu.IsEquivalent(t_group_id, t_item_id)) return t_category;
                }
            }
            if (t_null_allowed) return null;
            throw new Exception(def_MU_ex.Ex_derived_category_not_found);
        }
        public MUCategory GetDerivedCategoryFromMU(XMLMU mu) { return GetDerivedCategoryFromMU(mu.group_id, mu.item_id); }
        public MUCategory GetDerivedCategoryFromMU(MU mu) { return GetDerivedCategoryFromMU(mu.CategoryID, mu.ItemID); }
        public MUCategory GetDerivedCategoryFromMU(XMLMU mu, bool t_null_allowed) { return GetDerivedCategoryFromMU(mu.group_id, mu.item_id, t_null_allowed); }
        public MUCategory GetDerivedCategoryFromMU(MU mu, bool t_null_allowed) { return GetDerivedCategoryFromMU(mu.CategoryID, mu.ItemID, t_null_allowed); }
        #endregion
    }

    public class DropDownList : TypeConverter
    {
        #region Member functions
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.ComponentType == typeof(MeasurementUnitsProperties))
            {
                if (context.PropertyDescriptor.PropertyType == typeof(MU))
                {
                    return new StandardValuesCollection((MUs)(((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList));
                }
                else if (context.PropertyDescriptor.PropertyType == typeof(MUCategory))
                {
                    return new StandardValuesCollection((MUCategories)(((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList));
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(MaterialProperties))
            {
                if (context.PropertyDescriptor.PropertyType == typeof(MUCategory))
                {
                    return new StandardValuesCollection((MUCategories)(((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList));
                }
                else if (context.PropertyDescriptor.PropertyType == typeof(FractionMU))
                {
                    return new StandardValuesCollection((FractionMUs)(((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList));
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(IOMaterial))
            {
                return new StandardValuesCollection(((IOMaterial)context.Instance).MatProp.material_category.FlowMUs);
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(OperatingCost))
            {
                return new StandardValuesCollection(DefaultMUsAndValues.MUs.CostMUs);
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(InvestmentCost))
            {
                return new StandardValuesCollection(DefaultMUsAndValues.MUs.CurrencyMUs);
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(OverallCost))
            {
                return new StandardValuesCollection(DefaultMUsAndValues.MUs.CostMUs);
            }
            return new StandardValuesCollection(null);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (context.PropertyDescriptor.ComponentType == typeof(MeasurementUnitsProperties))
            {
                if (context.PropertyDescriptor.PropertyType == typeof(MU))
                {
                    foreach (MU item in (MUs)((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList)
                    {
                        if (item.ToString() == value.ToString()) return item;
                    }
                }
                else if (context.PropertyDescriptor.PropertyType == typeof(MUCategory))
                {
                    foreach (MUCategory item in (MUCategories)((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList)
                    {
                        if (item.ToString() == value.ToString()) return item;
                    }
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(MaterialProperties))
            {
                if (context.PropertyDescriptor.PropertyType == typeof(MUCategory))
                {
                    foreach (MUCategory item in (MUCategories)((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList)
                    {
                        if (item.ToString() == value.ToString()) return item;
                    }
                }
                else if (context.PropertyDescriptor.PropertyType == typeof(FractionMU))
                {
                    foreach (FractionMU item in (FractionMUs)((CustomPropertyDescriptor)context.PropertyDescriptor).SelectList)
                    {
                        if (item.ToString() == value.ToString()) return item;
                    }
                }
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(IOMaterial))
            {
                foreach (FractionMU item in ((IOMaterial)context.Instance).MatProp.material_category.FlowMUs) if (item.ToString() == value.ToString()) return item;
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(OperatingCost))
            {
                foreach (FractionMU item in DefaultMUsAndValues.MUs.CostMUs) if (item.ToString() == value.ToString()) return item;
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(InvestmentCost))
            {
                foreach (MU item in DefaultMUsAndValues.MUs.CurrencyMUs) if (item.ToString() == value.ToString()) return item;
            }
            else if (context.PropertyDescriptor.ComponentType == typeof(OverallCost))
            {
                foreach (FractionMU item in DefaultMUsAndValues.MUs.CostMUs) if (item.ToString() == value.ToString()) return item;
            }
            return base.ConvertFrom(context, culture, value);
        }
        #endregion
    }
    public class DescriptionStr
    {
        string m_description;
        public DescriptionStr(string t_description) { m_description = t_description; }
        public override string ToString() { return description; }
        public string description { get { return m_description; } set { m_description = value; } }
    }
}
