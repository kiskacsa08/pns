using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Pns.Globals;

namespace Pns
{
    public class BaseQuantity
    {
        #region Members
        private defaults.MU_Base_Quantities m_id;
        private string m_quantity;
        private string m_name;
        private string m_symbol;
        private bool m_isextensive;
        #endregion

        #region Constructors
        public BaseQuantity()
        {
            m_id = defaults.MU_Base_Quantities.b_undefined;
            m_quantity = "";
            m_name = "";
            m_symbol = "";
            m_isextensive = false;
        }
        public BaseQuantity(defaults.MU_Base_Quantities t_id, string t_quantity, string t_name, string t_symbol, bool t_isextensive)
        {
            m_id = t_id;
            m_quantity = t_quantity;
            m_name = t_name;
            m_symbol = t_symbol;
            m_isextensive = t_isextensive;
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public int id { get { return (int)m_id; } set { m_id = (defaults.MU_Base_Quantities)value; } }
        [XmlAttribute()]
        public string quantity { get { return m_quantity; } set { m_quantity = value; } }
        [XmlAttribute()]
        public string name { get { return m_name; } set { m_name = value; } }
        [XmlAttribute()]
        public string symbol { get { return m_symbol; } set { m_symbol = value; } }
        [XmlAttribute()]
        public bool isextensive { get { return m_isextensive; } set { m_isextensive = value; } }
        #endregion
    }

    public class BaseQuantities
    {
        #region Members
        private defaults.MU_Groups m_group_id;
        private List<BaseQuantity> m_bqs;
        #endregion

        #region Constructors
        public BaseQuantities()
        {
            m_group_id = defaults.MU_Groups.base_group;
            m_bqs = new List<BaseQuantity>();
        }
        #endregion

        #region Member functions
        public void Add(BaseQuantity item) { m_bqs.Add(item); }
        public BaseQuantity FindQuantity(int t_id)
        {
            foreach (BaseQuantity item in m_bqs) if (item.id == t_id) return item;
            throw new Exception(def_MU_ex.Ex_base_quantity_not_found);
        }
        public string GetSymbol(int t_id) { return FindQuantity(t_id).symbol; }
        public bool GetIsExtensive(int t_id) { return FindQuantity(t_id).isextensive; }
        #endregion

        #region Properties
        [XmlAttribute("group_id")]
        public int id { get { return (int)m_group_id; } set { m_group_id = (defaults.MU_Groups)value; } }
        [XmlElement("base_quantity")]
        public List<BaseQuantity> base_quantities { get { return m_bqs; } set { m_bqs = value; } }
        #endregion
    }

    public class Derivation
    {
        #region Members
        private defaults.MU_Groups m_quantity_group_id;
        private int m_quantity_id;
        private double m_factor;
        private int m_exponent;
        #endregion

        #region Constructors
        public Derivation()
        {
            m_quantity_group_id = defaults.MU_Groups.undefined;
            m_quantity_id = 0;
            m_factor = 0.0;
            m_exponent = 0;
        }
        public Derivation(defaults.MU_Groups t_quantity_group_id, int t_quantity_id, double t_factor, int t_exponent)
        {
            m_quantity_group_id = t_quantity_group_id;
            m_quantity_id = t_quantity_id;
            m_factor = t_factor;
            m_exponent = t_exponent;
        }
        public Derivation(Derivation d)
        {
            m_quantity_group_id = d.m_quantity_group_id;
            m_quantity_id = d.m_quantity_id;
            m_factor = d.m_factor;
            m_exponent = d.m_exponent;
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public int group_id { get { return (int)m_quantity_group_id; } set { m_quantity_group_id = (defaults.MU_Groups)value; } }
        [XmlAttribute()]
        public int id { get { return m_quantity_id; } set { m_quantity_id = value; } }
        [XmlAttribute()]
        public double factor { get { return m_factor; } set { m_factor = value; } }
        [XmlAttribute()]
        public int exponent { get { return m_exponent; } set { m_exponent = value; } }
        #endregion
    }

    public class DerivationList : List<Derivation>
    {
        #region Constructors
        public DerivationList() { }
        public DerivationList(DerivationList list) { foreach (Derivation element in list) Add(new Derivation(element)); }
        #endregion
    }

    public class DerivedUnit
    {
        #region Members
        private int m_id;
        private string m_name;
        private string m_symbol;
        private DerivationList m_derivations;
        #endregion

        #region Constructors
        public DerivedUnit()
        {
            m_id = 0;
            m_name = "";
            m_symbol = "";
            m_derivations = null;
        }
        public DerivedUnit(int t_id, string t_name, string t_symbol)
        {
            m_id = t_id;
            m_name = t_name;
            m_symbol = t_symbol;
            m_derivations = null;
        }
        #endregion

        #region Member functions
        public void AddDerivationItem(defaults.MU_Groups t_quantity_group_id, int t_quantity_id, double t_factor, int t_exponent)
        {
            if (m_derivations == null) m_derivations = new DerivationList();
            m_derivations.Add(new Derivation(t_quantity_group_id, t_quantity_id, t_factor, t_exponent));
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public int id { get { return m_id; } set { m_id = value; } }
        [XmlAttribute()]
        public string name { get { return m_name; } set { m_name = value; } }
        [XmlAttribute()]
        public string symbol { get { return m_symbol; } set { m_symbol = value; } }
        [XmlArrayItem("item")]
        public DerivationList derivation { get { return m_derivations; } set { m_derivations = value; } }
        #endregion
    }

    public class DerivedQuantity
    {
        #region Members
        private defaults.MU_Groups m_id;
        private string m_quantity;
        private List<DerivedUnit> m_units;
        #endregion

        #region Constructors
        public DerivedQuantity()
        {
            m_id = defaults.MU_Groups.undefined;
            m_quantity = "";
            m_units = null;
        }
        public DerivedQuantity(defaults.MU_Groups t_id, string t_quantity)
        {
            m_id = t_id;
            m_quantity = t_quantity;
            m_units = null;
        }
        #endregion

        #region Member functions
        public DerivedUnit AddUnit(int t_id, string t_name, string t_symbol)
        {
            if (m_units == null) m_units = new List<DerivedUnit>();
            DerivedUnit t_du;
            m_units.Add(t_du = new DerivedUnit(t_id, t_name, t_symbol));
            return t_du;
        }
        public DerivedUnit FindUnit(int t_id)
        {
            foreach (DerivedUnit item in m_units) if (item.id == t_id) return item;
            throw new Exception(def_MU_ex.Ex_derived_unit_not_found);
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public int group_id { get { return (int)m_id; } set { m_id = (defaults.MU_Groups)value; } }
        [XmlAttribute()]
        public string quantity { get { return m_quantity; } set { m_quantity = value; } }
        [XmlArrayItem("unit")]
        public List<DerivedUnit> units { get { return m_units; } set { m_units = value; } }
        #endregion
    }

    [XmlRoot("pns_mu")]
    public class MU_XML
    {
        #region Members
        private BaseQuantities m_basequantities;
        private List<DerivedQuantity> m_derivedquantities;
        #endregion

        #region Constructors
        public MU_XML() { }
        public MU_XML(bool a)
        {
            m_basequantities = new BaseQuantities();
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_mass, "mass", "kilogram", "kg", true));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_amount_of_substance, "amount of substance", "mole", "mol", true));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_time, "time", "second", "s", false));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_currency, "currency", "forint", "HUF", true));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_lenght, "length", "meter", "m", true));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_electric_current, "electric current", "ampere", "A", true));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_thermodynamic_temperature, "thermodinamic temperature", "kelvin", "K", false));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_luminous_intensity, "luminous intensity", "candela", "cd", false));
            m_basequantities.Add(new BaseQuantity(defaults.MU_Base_Quantities.b_capacity, "capacity", "capacity", "unit", true));
            m_derivedquantities = new List<DerivedQuantity>();
            DerivedQuantity t_dq;
            DerivedUnit t_du;

            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_mass, "mass"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Mass.tonne, "tonne", "t");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_mass, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Mass.gram, "gram", "g");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_mass, 0.001, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_volume, "volume"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Volume.cubic_meter, "cubic meter", "m³");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1, 3);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Volume.cubic_decimeter, "cubic decimeter", "dm³");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.decimeter, 1, 3);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Volume.cubic_centimeter, "cubic centimeter", "cm³");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.centimeter, 1, 3);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_amount_of_substance, "amount of substance"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Amount_of_substance.millimole, "millimole", "mmol");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_amount_of_substance, 0.001, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Amount_of_substance.kilomole, "kilomole", "kmol");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_amount_of_substance, 1000, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_energy, "energy, work, quantity of heat"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.joule, "joule", "J");
            t_du.AddDerivationItem(defaults.MU_Groups.d_force, (int)defaults.MU_Groups_Force.newton, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.kilojoule, "kilojoule", "kJ");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.joule, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.megajoule, "megajoule", "MJ");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.kilojoule, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.gigajoule, "gigajoule", "GJ");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.megajoule, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.terajoule, "terajoule", "TJ");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.gigajoule, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.watthour, "watthour", "Wh");
            t_du.AddDerivationItem(defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.watt, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.kilowatthour, "kilowatthour", "kWh");
            t_du.AddDerivationItem(defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.kilowatt, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.megawatthour, "megawatthour", "MWh");
            t_du.AddDerivationItem(defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.megawatt, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.gigawatthour, "gigawatthour", "GWh");
            t_du.AddDerivationItem(defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.gigawatt, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Energy.terawatthour, "terawatthour", "TWh");
            t_du.AddDerivationItem(defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.terawatt, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_time, "time"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Time.minute, "minute", "min");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 60, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Time.hour, "hour", "h");
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.minute, 60, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Time.year, "year", "yr");
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 8760, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Time.payout_period, "payout period (year)", "payout period");
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.year, 5, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_currency, "currency"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Currency.euro, "euro", "€");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_currency, 300, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Currency.dollar, "dollar", "$");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_currency, 200, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_lenght, "length"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Length.kilometer, "kilometer", "km");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Length.decimeter, "decimeter", "dm");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 0.1, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Length.centimeter, "centimeter", "cm");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 0.01, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Length.millimeter, "millimeter", "mm");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 0.001, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Length.coll, "coll", "\"");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 0.0254, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_electric_current, "electric current"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Electric_current.milliamper, "milliampere", "mA");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_electric_current, 0.001, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Electric_current.kiloamper, "kiloampere", "kA");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_electric_current, 1000, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_area, "area"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Area.square_meter, "square meter", "m²");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1, 2);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Area.square_kilometer, "square kilometer", "km²");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.kilometer, 1, 2);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Area.square_centimeter, "square centimeter", "cm²");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.centimeter, 1, 2);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Area.hectar, "hectar", "ha");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 100, 2);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_speed, "speed"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Speed.meter_per_second, "meter per second", "m/s");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Speed.kilometer_per_hour, "kilometer per hour", "km/h");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.kilometer, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Speed.miles_per_hour, "miles per hour", "miles/h");
            t_du.AddDerivationItem(defaults.MU_Groups.d_lenght, (int)defaults.MU_Groups_Length.kilometer, 1.6, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.hour, 1, -1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_acceleration, "acceleration"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Acceleration.meter_per_second_squared, "meter per second squared", "m/s²");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -2);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_density, "mass density"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Mass_density.kilogram_per_cubic_meter, "kilogram per cubic meter", "kg/m³");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_mass, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_volume, (int)defaults.MU_Groups_Volume.cubic_meter, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Mass_density.tonne_per_cubic_meter, "tonne per cubic meter", "t/m³");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Groups_Mass.tonne, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_volume, (int)defaults.MU_Groups_Volume.cubic_meter, 1, -1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_concentration, "concentration"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Concentration.mole_per_cubic_meter, "mole per cubic meter", "mol/m³");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_amount_of_substance, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_volume, (int)defaults.MU_Groups_Volume.cubic_meter, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Concentration.mole_per_cubic_decimeter, "mole per cubic decimeter", "mol/dm³");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_amount_of_substance, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_volume, (int)defaults.MU_Groups_Volume.cubic_decimeter, 1, -1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_force, "force"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Force.newton, "newton", "N");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_mass, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_acceleration, (int)defaults.MU_Groups_Acceleration.meter_per_second_squared, 1, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_pressure, "pressure"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Pressure.pascal, "pascal", "Pa");
            t_du.AddDerivationItem(defaults.MU_Groups.d_force, (int)defaults.MU_Groups_Force.newton, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.d_area, (int)defaults.MU_Groups_Area.square_meter, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Pressure.kilopascal, "kilopascal", "kPa");
            t_du.AddDerivationItem(defaults.MU_Groups.d_pressure, (int)defaults.MU_Groups_Pressure.pascal, 1000, 1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Pressure.megapascal, "megapascal", "MPa");
            t_du.AddDerivationItem(defaults.MU_Groups.d_pressure, (int)defaults.MU_Groups_Pressure.kilopascal, 1000, 1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_power, "power"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Power.watt, "watt", "W");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.joule, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Power.kilowatt, "kilowatt", "kW");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.kilojoule, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Power.megawatt, "megawatt", "MW");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.megajoule, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Power.gigawatt, "gigawatt", "GW");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.gigajoule, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Power.terawatt, "terawatt", "TW");
            t_du.AddDerivationItem(defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.terajoule, 1, 1);
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_time, 1, -1);
            m_derivedquantities.Add(t_dq = new DerivedQuantity(defaults.MU_Groups.d_capacity, "capacity"));
            t_du = t_dq.AddUnit((int)defaults.MU_Groups_Capacity.capacity, "capacity", "unit");
            t_du.AddDerivationItem(defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_capacity, 1, 1);
        }
        #endregion

        #region Member functions
        private DerivedQuantity FindDerivedQuantity(int t_id)
        {
            foreach (DerivedQuantity item in m_derivedquantities) if (item.group_id == t_id) return item;
            throw new Exception(def_MU_ex.Ex_derived_group_not_found);
        }
        private bool simplify(Derivation result, Derivation item, bool add)
        {
            if (result.group_id == item.group_id && result.id == item.id)
            {
                if (add) result.exponent += item.exponent;
                else result.exponent -= item.exponent;
                return true;
            }
            return false;
        }
        private bool convert_to_base(ref double factor, DerivationList list)
        {
            int i = -1;
            while (++i < list.Count)
            {
                factor *= System.Math.Pow(list[i].factor, list[i].exponent);
                if (list[i].group_id != (int)defaults.MU_Groups.base_group)
                {
                    DerivationList t_list = get_list(list[i]);
                    list.RemoveAt(i);
                    convert_to_base(ref factor, t_list);
                    foreach (Derivation item in t_list)
                    {
                        int j = -1;
                        while (++j < list.Count && !simplify(list[j], item, true)) ;
                        if (j == list.Count)
                        {
                            item.factor = 1;
                            list.Add(new Derivation(item));
                        }
                        else if (list[j].exponent == 0) list.RemoveAt(j);
                    }
                    i--;
                }
            }
            return true;
        }
        private DerivationList get_list(MU mu) { return get_list(mu.CategoryID, mu.ItemID, 1); }
        private DerivationList get_list(Derivation item) { return get_list(item.group_id, item.id, item.exponent); }
        private DerivationList get_list(int group_id, int id, int exponent)
        {
            DerivationList list;
            if (group_id == (int)defaults.MU_Groups.base_group)
            {
                m_basequantities.FindQuantity(id);
                list = new DerivationList();
                list.Add(new Derivation(defaults.MU_Groups.base_group, id, 1.0, exponent));
            }
            else
            {
                list = new DerivationList(FindDerivedQuantity(group_id).FindUnit(id).derivation);
                foreach (Derivation item in list) item.exponent *= exponent;

            }
            return list;
        }
        private bool check_dimensions(DerivationList from, DerivationList to)
        {
            foreach (Derivation item in to)
            {
                int i = -1;
                while (++i < from.Count && !simplify(from[i], item, false)) ;
                if (i == from.Count) from.Add(new Derivation(item));
                else if (from[i].exponent == 0) from.RemoveAt(i);
            }
            return from.Count == 0;
        }
        private double convert(MU from, MU to)
        {
            DerivationList from_list = get_list(from);
            DerivationList to_list = get_list(to);
            double from_factor = 1;
            double to_factor = 1;
            convert_to_base(ref from_factor, from_list);
            convert_to_base(ref to_factor, to_list);
            if (check_dimensions(from_list, to_list)) return from_factor / to_factor;
            else return def_Values.d_NperA;
        }
        public double convert(MU from, MU to, double t_working_hours_per_year, double t_payout_period)
        {
            double t_working_hours_per_year_backup = WorkingHoursPerYear;
            double t_payout_period_backup = PayoutPeriod;
            WorkingHoursPerYear = t_working_hours_per_year;
            PayoutPeriod = t_payout_period;
            double t_result = convert(from, to);
            WorkingHoursPerYear = t_working_hours_per_year_backup;
            PayoutPeriod = t_payout_period_backup;
            return t_result;
        }
        public double convert(FractionMU from, FractionMU to, double t_working_hours_per_year, double t_payout_period)
        {
            double t_working_hours_per_year_backup = WorkingHoursPerYear;
            double t_payout_period_backup = PayoutPeriod;
            WorkingHoursPerYear = t_working_hours_per_year;
            PayoutPeriod = t_payout_period;
            double t_numerator = convert(from.Numerator, to.Numerator);
            double t_denominator = convert(from.Denominator, to.Denominator);
            WorkingHoursPerYear = t_working_hours_per_year_backup;
            PayoutPeriod = t_payout_period_backup;
            if (t_denominator != def_Values.d_NperA && t_denominator != 0 && t_numerator != def_Values.d_NperA) return t_numerator / t_denominator;
            return def_Values.d_NperA;
        }
        public string GetSymbol(int category_id, int id)
        {
            if (category_id == (int)defaults.MU_Groups.base_group) return base_quantities.GetSymbol(id);
            else return FindDerivedQuantity(category_id).FindUnit(id).symbol;
        }
        public string GetCategory(int id) { return FindDerivedQuantity(id).quantity; }
        public void GetCategories(MUCategories categories)
        {
            foreach (DerivedQuantity item in m_derivedquantities)
            {
                MUCategory t_category = new MUCategory(item.group_id);
                if (IsExtensive(t_category.MUList[0]) || t_category.CategoryID == (int)defaults.MU_Groups.d_time) categories.Add(t_category);
            }
        }
        public void GetMUs(int t_category_id, MUs t_mus)
        {
            DerivedQuantity t_group = FindDerivedQuantity(t_category_id);
            DerivationList derivations = t_group.units[0].derivation;
            if (derivations.Count == 1 && derivations[0].group_id == (int)defaults.MU_Groups.base_group && derivations[0].exponent == 1)
            {
                t_mus.Add(new MU((int)defaults.MU_Groups.base_group, derivations[0].id));
            }
            foreach (DerivedUnit unit in t_group.units) t_mus.Add(new MU(t_category_id, unit.id));
        }
        public bool IsExtensive(MU mu)
        {
            DerivationList list = get_list(mu);
            double factor = 1;
            convert_to_base(ref factor, list);
            bool t_is_extensive_numerator_item = false;
            bool t_is_extensive_denominator_item = false;
            foreach (Derivation item in list)
            {
                if (base_quantities.GetIsExtensive(item.id))
                {
                    if (item.exponent > 0) t_is_extensive_numerator_item = true;
                    else if (item.exponent < 0) t_is_extensive_denominator_item = true;
                }
            }
            return t_is_extensive_numerator_item && !t_is_extensive_denominator_item;
        }
        #endregion

        #region Properties
        public BaseQuantities base_quantities { get { return m_basequantities; } set { m_basequantities = value; } }
        [XmlArrayItem("derived_quantity")]
        public List<DerivedQuantity> derived_quantities { get { return m_derivedquantities; } set { m_derivedquantities = value; } }
        private double WorkingHoursPerYear
        {
            get { return FindDerivedQuantity((int)defaults.MU_Groups.d_time).FindUnit((int)defaults.MU_Groups_Time.year).derivation[0].factor; }
            set { FindDerivedQuantity((int)defaults.MU_Groups.d_time).FindUnit((int)defaults.MU_Groups_Time.year).derivation[0].factor = value; }
        }
        private double PayoutPeriod
        {
            get { return FindDerivedQuantity((int)defaults.MU_Groups.d_time).FindUnit((int)defaults.MU_Groups_Time.payout_period).derivation[0].factor; }
            set { FindDerivedQuantity((int)defaults.MU_Groups.d_time).FindUnit((int)defaults.MU_Groups_Time.payout_period).derivation[0].factor = value; }
        }
        #endregion
    }
}
