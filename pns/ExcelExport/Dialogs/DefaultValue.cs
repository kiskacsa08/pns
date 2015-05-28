using System;
using System.ComponentModel;
using System.Windows.Forms;
using DynamicPropertyGrid;
using Pns.Xml_Serialization.PnsDefaults;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Xml_Serialization.PnsGUI.Dialogs.DefValues;
using Pns.Globals;

namespace Pns.Dialogs
{
    public partial class DefaultValue : Form
    {
        #region Members
        private bool m_cancel;
        #endregion

        #region Constructors
        public DefaultValue()
        {
            InitializeComponent();
            Text = def_DefaultValuesDlg.dialog_text;
            buttonUpdate.Text = def_DefaultValuesDlg.button_update_text;
            buttonCancel.Text = def_DefaultValuesDlg.button_cancel_text;
            buttonDefaults.Text = def_DefaultValuesDlg.button_default_text;
            propertyGridDefaultValues.SelectedObject = DefaultMUsAndValues.DefaultValues;
            propertyGridDefaultValues.ExpandAllGridItems();
            DefaultMUsAndValues.DefaultValues.Backup();
            DefaultMUsAndValues.DefaultValues.Refresh();
            m_cancel = true;
        }
        #endregion

        #region Event handlers
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            DefaultValuesProperties t_defvalprop = (DefaultValuesProperties)propertyGridDefaultValues.SelectedObject;
            t_defvalprop.UpdateFields();
            if (t_defvalprop.Validate())
            {
                m_cancel = false;
                Close();
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void buttonDefaults_Click(object sender, EventArgs e)
        {
            ((DefaultValuesProperties)propertyGridDefaultValues.SelectedObject).Reset();
            propertyGridDefaultValues.Refresh();
        }
        private void DefaultValue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancel) ((DefaultValuesProperties)propertyGridDefaultValues.SelectedObject).Restore();
        }
        private void propertyGridDefaultValues_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DefaultValuesProperties t_defvalprop = (DefaultValuesProperties)propertyGridDefaultValues.SelectedObject;
            t_defvalprop.UpdateFields();
            t_defvalprop.Refresh();
            propertyGridDefaultValues.Refresh();
        }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class ParamPair : CustomClass
    {
        #region Members
        protected double f_par1;
        protected double f_par2;
        protected double f_b_par1;
        protected double f_b_par2;
        #endregion

        #region Member functions
        public void Backup()
        {
            f_b_par1 = f_par1;
            f_b_par2 = f_par2;
        }
        public void Restore()
        {
            f_par1 = f_b_par1;
            f_par2 = f_b_par2;
        }
        public override string ToString()
        {
            return f_par1.ToString() + ", " + f_par2.ToString();
        }
        #endregion
    }

    public class oCostParams : ParamPair
    {
        #region Constructors
        public oCostParams(double t_fix, double t_prop)
        {
            f_par1 = t_fix != def_Values.d_NperA ? t_fix : def_Values.o_fix;
            f_par2 = t_prop != def_Values.d_NperA ? t_prop : def_Values.o_prop;
            AddProperties(); 
        }
        #endregion

        #region Member functions
        public void Reset()
        {
            f_par1 = def_Values.o_fix;
            f_par2 = def_Values.o_prop;
            Clear();
            AddProperties();
        }
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_ofix, fixed_charge, def_DefaultValuesPropGrid.D_ofix, "", fixed_charge.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_oprop, prop_constant, def_DefaultValuesPropGrid.D_oprop, "", prop_constant.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) fixed_charge = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) prop_constant = (string)((CustomProp)List[1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = fixed_charge;
            if (List.Count > 1) ((CustomProp)List[1]).Value = prop_constant;
        }
        public void ToXML(Problem xml_tag)
        {
            xml_tag.fixedCharge = f_par1;
            xml_tag.propConstant = f_par2;
        }
        public void ToXML(XMLDefaults xml_tag)
        {
            xml_tag.fixedCharge = f_par1;
            xml_tag.propConstant = f_par2;
        }
        public override string ToString() 
        {
            return "fix:"+fixed_charge + "; prop.:" + prop_constant;
        }
        #endregion

        #region Properties
        public string fixed_charge
        {
            get { return d_fixed_charge.ToString() + " " + DefaultMUsAndValues.MUs.DefaultCostMU.ToString(); }
            set 
            { 
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != def_Values.d_NperA) f_par1 = t_value;
                else f_par1 = def_Values.o_fix;
            }
        }
        public double d_fixed_charge { get { return f_par1; } }
        public string prop_constant
        {
            get { return d_prop_constant.ToString() + " " + DefaultMUsAndValues.MUs.DefaultCostMU.ToString(); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != def_Values.d_NperA) f_par2 = t_value;
                else f_par2 = def_Values.o_prop;
            }
        }
        public double d_prop_constant { get { return f_par2; } }
        #endregion
    }

    public class iCostParams : ParamPair
    {
        #region Constructors
        public iCostParams(double t_fix, double t_prop)
        {
            f_par1 = t_fix != def_Values.d_NperA ? t_fix : def_Values.i_fix;
            f_par2 = t_prop != def_Values.d_NperA ? t_prop : def_Values.i_prop;
            AddProperties(); 
        }
        #endregion

        #region Member functions
        public void Reset()
        {
            f_par1 = def_Values.i_fix;
            f_par2 = def_Values.i_prop;
            Clear();
            AddProperties();
        }
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_ifix, fixed_charge, def_DefaultValuesPropGrid.D_ifix, "", fixed_charge.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_iprop, prop_constant, def_DefaultValuesPropGrid.D_iprop, "", prop_constant.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) fixed_charge = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) prop_constant = (string)((CustomProp)List[1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = fixed_charge;
            if (List.Count > 1) ((CustomProp)List[1]).Value = prop_constant;
        }
        public void ToXML(Problem xml_tag)
        {
            xml_tag.iFixedCharge = f_par1;
            xml_tag.iPropConstant = f_par2;
        }
        public void ToXML(XMLDefaults xml_tag)
        {
            xml_tag.iFixedCharge = f_par1;
            xml_tag.iPropConstant = f_par2;
        }
        public override string ToString()
        {
            return "fix:" + fixed_charge + "; prop.:" + prop_constant;
        }
        #endregion

        #region Properties
        public string fixed_charge
        {
            get { return d_fixed_charge.ToString() + " " + DefaultMUsAndValues.MUs.DefaultCurrencyMU.ToString(); }
            set 
            { 
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != def_Values.d_NperA) f_par1 = t_value;
                else f_par1 = def_Values.i_fix;
            }
        }
        public double d_fixed_charge { get { return f_par1; } }
        public string prop_constant
        {
            get { return d_prop_constant.ToString() + " " + DefaultMUsAndValues.MUs.DefaultCurrencyMU.ToString(); }
            set 
            { 
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != def_Values.d_NperA) f_par2 = t_value;
                else f_par2 = def_Values.i_prop;
            }
        }
        public double d_prop_constant { get { return f_par2; } }
        #endregion
    }

    public class CapacityParams : ParamPair
    {
        #region Constructors
        public CapacityParams(double t_lb, double t_ub)
        {
            f_par1 = t_lb != def_Values.d_NperA ? t_lb : def_Values.lower_bound;
            f_par2 = t_ub;
            AddProperties(); 
        }
        #endregion

        #region Member functions
        public void Reset()
        {
            f_par1 = def_Values.lower_bound;
            f_par2 = def_Values.d_NperA;
            Clear();
            AddProperties();
        }
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_lb, lower_bound, def_DefaultValuesPropGrid.D_lb, "", lower_bound.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_ub, upper_bound, def_DefaultValuesPropGrid.D_ub, "", upper_bound.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) lower_bound = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) upper_bound = (string)((CustomProp)List[1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = lower_bound;
            if (List.Count > 1) ((CustomProp)List[1]).Value = upper_bound;
        }
        public bool Validate()
        {
            if (d_lower_bound > d_upper_bound)
            {
                MessageBox.Show(Msg_lb_ub);
                return false;
            }
            return true;
        }
        public void ToXML(Problem xml_tag)
        {
            xml_tag.lowerBound = f_par1;
            xml_tag.upperBound = f_par2;
        }
        public void ToXML(XMLDefaults xml_tag)
        {
            xml_tag.lowerBound = f_par1;
            xml_tag.upperBound = f_par2;
        }
        public override string ToString()
        {
            return "lb.:" + lower_bound + "; ub.:" + upper_bound;
        }
        #endregion

        #region Properties
        public string lower_bound 
        { 
            get { return d_lower_bound.ToString(); } 
            set 
            { 
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != def_Values.d_NperA) f_par1 = t_value;
                else f_par1 = def_Values.lower_bound;
            }
        }
        public double d_lower_bound { get { return f_par1; } }
        public string upper_bound
        {
            get { return d_upper_bound.ToString(); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != DefaultMUsAndValues.DefaultValues.d_solver_ub) f_par2 = t_value;
                else f_par2 = def_Values.d_NperA;
            }
        }
        public double d_upper_bound
        {
            get
            {
                if (f_par2 != def_Values.d_NperA) return f_par2;
                if (DefaultMUsAndValues.DefaultValues != null) return DefaultMUsAndValues.DefaultValues.d_solver_ub;
                return def_Values.solver_upper_limit;
            }
        }
        static public string Msg_lb_ub { get { return def_DefaultValuesPropGrid.DN_lb + def_PnsStudio.Msg_must_be_less_or_equal_than + def_DefaultValuesPropGrid.DN_ub; } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class DefaultValuesProperties : CustomClass
    {
        #region Members
        private double f_minimum_flow;
        private double f_maximum_flow;
        private double f_price;
        private double f_flowrate;
        private double f_solver_ub;
        private int f_max_solutions;
        private oCostParams f_ocost;
        private iCostParams f_icost;
        private CapacityParams f_cap_constr;
        private double f_b_minimum_flow;
        private double f_b_maximum_flow;
        private double f_b_price;
        private double f_b_flowrate;
        private double f_b_solver_ub;
        private int f_b_max_solutions;
        #endregion

        #region Constructors
        public DefaultValuesProperties(XMLDefaults defaults)
        {
            f_minimum_flow = defaults.minimumFlow != def_Values.d_NperA ? defaults.minimumFlow : def_Values.minimum_flow;
            f_maximum_flow = defaults.maximumFlow;
            f_price = defaults.price != def_Values.d_NperA ? defaults.price : def_Values.price;
            f_flowrate = defaults.flowrate != def_Values.d_NperA ? defaults.flowrate : def_Values.io_flowrate;
            f_solver_ub = defaults.solverUpperLimit != def_Values.d_NperA ? defaults.solverUpperLimit : def_Values.solver_upper_limit;
            f_max_solutions = defaults.maxSolutions != def_Values.d_NperA ? defaults.maxSolutions : def_Values.max_solutions;
            f_ocost = new oCostParams(defaults.fixedCharge, defaults.propConstant);
            f_icost = new iCostParams(defaults.iFixedCharge, defaults.iPropConstant);
            f_cap_constr = new CapacityParams(defaults.lowerBound, defaults.upperBound);
            AddProperties();
        }
        public DefaultValuesProperties(Problem defaults)
        {
            f_minimum_flow = defaults.minimumFlow != def_Values.d_NperA ? defaults.minimumFlow : def_Values.minimum_flow;
            f_maximum_flow = defaults.maximumFlow;
            f_price = defaults.price != def_Values.d_NperA ? defaults.price : def_Values.price;
            f_flowrate = defaults.flowrate != def_Values.d_NperA ? defaults.flowrate : def_Values.io_flowrate;
            f_solver_ub = defaults.solverUpperLimit != def_Values.d_NperA ? defaults.solverUpperLimit : def_Values.solver_upper_limit;
            f_ocost = new oCostParams(defaults.fixedCharge, defaults.propConstant);
            f_icost = new iCostParams(defaults.iFixedCharge, defaults.iPropConstant);
            f_cap_constr = new CapacityParams(defaults.lowerBound, defaults.upperBound);
            AddProperties();
        }
        #endregion

        #region Member functions
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_minimum_flow, minimum_flow, def_DefaultValuesPropGrid.D_minimum_flow,
                def_DefaultValuesPropGrid.Cat_material, minimum_flow.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_maximum_flow, maximum_flow, def_DefaultValuesPropGrid.D_maximum_flow,
                def_DefaultValuesPropGrid.Cat_material, maximum_flow.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_price, price, def_DefaultValuesPropGrid.D_price,
                def_DefaultValuesPropGrid.Cat_material, price.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_flow_rate, flowrate, def_DefaultValuesPropGrid.D_flow_rate,
                def_DefaultValuesPropGrid.Cat_op, flowrate.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_ocost, f_ocost, def_DefaultValuesPropGrid.D_ocost,
                def_DefaultValuesPropGrid.Cat_op, f_ocost.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_icost, f_icost, def_DefaultValuesPropGrid.D_icost,
                def_DefaultValuesPropGrid.Cat_op, f_icost.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_cap_constr, f_cap_constr, def_DefaultValuesPropGrid.D_cap_constr,
                def_DefaultValuesPropGrid.Cat_op, f_cap_constr.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_solver_ub, solver_ub, def_DefaultValuesPropGrid.D_solver_ub,
                def_DefaultValuesPropGrid.Cat_solver, solver_ub.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_DefaultValuesPropGrid.DN_max_solutions, max_solutions, def_DefaultValuesPropGrid.D_max_solutions,
                def_DefaultValuesPropGrid.Cat_solver, max_solutions.GetType(), false, true));
        }
        public void Reset()
        {
            f_minimum_flow = def_Values.minimum_flow;
            f_maximum_flow = def_Values.d_NperA;
            f_price = def_Values.price;
            f_flowrate = def_Values.io_flowrate;
            f_solver_ub = def_Values.solver_upper_limit;
            f_max_solutions = def_Values.max_solutions;
            f_ocost.Reset();
            f_icost.Reset();
            f_cap_constr.Reset();
            Clear();
            AddProperties();
        }
        public void UpdateFields()
        {
            if (List.Count > 0) minimum_flow = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) maximum_flow = (string)((CustomProp)List[1]).Value;
            if (List.Count > 2) price = (string)((CustomProp)List[2]).Value;
            if (List.Count > 3) flowrate = (string)((CustomProp)List[3]).Value;
            f_ocost.UpdateFields();
            f_icost.UpdateFields();
            f_cap_constr.UpdateFields();
            if (List.Count > 7) solver_ub = (string)((CustomProp)List[7]).Value;
            if (List.Count > 8) max_solutions = (string)((CustomProp)List[8]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = minimum_flow;
            if (List.Count > 1) ((CustomProp)List[1]).Value = maximum_flow;
            if (List.Count > 2) ((CustomProp)List[2]).Value = price;
            if (List.Count > 3) ((CustomProp)List[3]).Value = flowrate;
            f_ocost.Refresh();
            f_icost.Refresh();
            f_cap_constr.Refresh();
            if (List.Count > 7) ((CustomProp)List[7]).Value = solver_ub;
            if (List.Count > 8) ((CustomProp)List[8]).Value = max_solutions;
        }
        public void Backup()
        {
            f_b_minimum_flow = f_minimum_flow;
            f_b_maximum_flow = f_maximum_flow;
            f_b_price = f_price;
            f_b_flowrate = f_flowrate;
            f_b_solver_ub = f_solver_ub;
            f_b_max_solutions = f_max_solutions;
            f_ocost.Backup();
            f_icost.Backup();
            f_cap_constr.Backup();
        }
        public void Restore()
        {
            f_minimum_flow = f_b_minimum_flow;
            f_maximum_flow = f_b_maximum_flow;
            f_price = f_b_price;
            f_flowrate = f_b_flowrate;
            f_solver_ub = f_b_solver_ub;
            f_max_solutions = f_b_max_solutions;
            f_ocost.Restore();
            f_icost.Restore();
            f_cap_constr.Restore();
        }
        public bool Validate()
        {
            if (d_minimum_flow > d_maximum_flow)
            {
                MessageBox.Show(Msg_minflow_maxflow);
                return false;
            }
            return f_cap_constr.Validate();
        }
        public void ToXML(Problem xml_tag)
        {
            xml_tag.minimumFlow = f_minimum_flow;
            xml_tag.maximumFlow = f_maximum_flow;
            xml_tag.price = f_price;
            xml_tag.flowrate = f_flowrate;
            f_ocost.ToXML(xml_tag);
            f_icost.ToXML(xml_tag);
            f_cap_constr.ToXML(xml_tag);
            xml_tag.solverUpperLimit = f_solver_ub;
            xml_tag.maxSolutions = f_max_solutions;
        }
        public void ToXML(XMLDefaults xml_tag)
        {
            xml_tag.minimumFlow = f_minimum_flow;
            xml_tag.maximumFlow = f_maximum_flow;
            xml_tag.price = f_price;
            xml_tag.flowrate = f_flowrate;
            f_ocost.ToXML(xml_tag);
            f_icost.ToXML(xml_tag);
            f_cap_constr.ToXML(xml_tag);
            xml_tag.solverUpperLimit = f_solver_ub;
            xml_tag.maxSolutions = f_max_solutions;
        }
        #endregion

        #region Properties
        public string minimum_flow
        {
            get { return d_minimum_flow + " (e.g.: " + DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU).DefaultFlowMU + ")"; }
            set
            {
                double t_value = Converters.ToDouble(0, value, d_solver_ub);
                if (t_value != def_Values.d_NperA) f_minimum_flow = t_value;
                else f_minimum_flow = def_Values.minimum_flow;
            }
        }
        public double d_minimum_flow { get { return f_minimum_flow; } }
        public string maximum_flow
        {
            get { return d_maximum_flow + " (e.g.: " + DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU).DefaultFlowMU + ")"; }
            set
            {
                double t_value = Converters.ToDouble(0, value, d_solver_ub);
                if (t_value != d_solver_ub) f_maximum_flow = t_value;
                else f_maximum_flow = def_Values.d_NperA;
            }
        }
        public double d_maximum_flow
        {
            get
            {
                if (f_maximum_flow != def_Values.d_NperA) return f_maximum_flow;
                return d_solver_ub;
            }
        }
        public string price
        {
            get { return d_price + " (e.g.: " + DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU).DefaultPriceMU + ")"; }
            set
            {
                double t_value = Converters.ToDouble(-d_solver_ub, value, d_solver_ub);
                if (t_value != def_Values.d_NperA) f_price = t_value;
                else f_price = def_Values.price;
            }
        }
        public double d_price { get { return f_price; } }
        public string flowrate
        {
            get { return d_flowrate + " (e.g.: " + DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU).DefaultFlowMU + ")"; }
            set
            {
                double t_value = Converters.ToDouble(0, value, d_solver_ub);
                if (t_value != def_Values.d_NperA) f_flowrate = t_value;
                else f_flowrate = def_Values.io_flowrate;
            }
        }
        public double d_flowrate { get { return f_flowrate; } }
        public string solver_ub
        {
            get { return d_solver_ub.ToString(); }
            set
            {
                double t_value = Converters.ToDouble(0, value);
                if (t_value != def_Values.d_NperA) f_solver_ub = t_value;
                else f_solver_ub = def_Values.solver_upper_limit;
            }
        }
        public double d_solver_ub { get { return f_solver_ub; } }
        public string max_solutions
        {
            get { return i_max_solutions.ToString(); }
            set
            {
                int t_value = (int)Converters.ToDouble(1, value);
                if (t_value != def_Values.d_NperA) f_max_solutions = t_value;
                else f_max_solutions = def_Values.max_solutions;
            }
        }
        public int i_max_solutions { get { return f_max_solutions; } set { f_max_solutions = value; } }
        public oCostParams OperatingCost { get { return f_ocost; } }
        public iCostParams InvestmentCost { get { return f_icost; } }
        public CapacityParams CapacityConstraints { get { return f_cap_constr; } }
        static public string Msg_minflow_maxflow { get { return def_DefaultValuesPropGrid.DN_minimum_flow + def_PnsStudio.Msg_must_be_less_or_equal_than + def_DefaultValuesPropGrid.DN_maximum_flow; } }
        #endregion
    }
}
