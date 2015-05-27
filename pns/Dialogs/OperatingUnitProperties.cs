using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DynamicPropertyGrid;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial;
using Pns.Xml_Serialization.PnsGUI.Dialogs.OpUnit;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Globals;

namespace Pns.Dialogs
{
    public partial class OperatingUnitPanel : UserControl
    {
        #region Members
        private OperatingUnitProperties cmpou;
        #endregion

        #region Constructors
        public OperatingUnitPanel(OperatingUnitProperties ou)
        {
            InitializeComponent();
            cmpou = new OperatingUnitProperties(ou);
            propertyGridOperatingUnit.SelectedObject = ou;
            labelText = ou.currname;
            if (cmpou.isnewitem)
            {
                buttonUpdate.Text = def_ou_panel.UpdateText_new;
                buttonCancel.Text = def_ou_panel.CancelText_new;
                buttonCancel.Enabled = false;
                buttonDelete.Text = def_ou_panel.DeleteText_new;
            }
            else
            {
                buttonUpdate.Text = def_ou_panel.UpdateText;
                buttonCancel.Text = def_ou_panel.CancelText;
                buttonCancel.Enabled = true;
                buttonDelete.Text = def_ou_panel.DeleteText;
            }
            checkBoxAutoConvert.Text = def_ou_panel.AutoConvertText;
            Name = ou.currname;
            events.OperatingUnitPanelChange += new SelectOperatingUnitPanelEventHandler(events_OperatingUnitPanelChange);
            propertyGridOperatingUnit.ExpandAllGridItems();
            checkBoxAutoConvert.Checked = ou.AutoConvert;
        }
        #endregion

        #region Event Handlers
        internal void events_OperatingUnitPanelChange(object sender, EventArgs e)
        {
            if (sender == this)
            {
                labelOperatingUnit.ForeColor = System.Drawing.SystemColors.HighlightText;
                labelOperatingUnit.BackColor = System.Drawing.SystemColors.Highlight;
                PnsStudio.OUTree.Hide();
                PnsStudio.OUTree.SelectedNode = PnsStudio.FindOUNode(cmpou.currname);
                PnsStudio.OUTree.Show();
            }
            else
            {
                labelOperatingUnit.ForeColor = System.Drawing.SystemColors.ControlText;
                labelOperatingUnit.BackColor = System.Drawing.SystemColors.Control;
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            PnsStudio.OUPanelEnsureVisible(this);
            OperatingUnitProperties t_ouprop = (OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject;
            t_ouprop.UpdateFields();
            if (t_ouprop.Validate()) events.OnOUPropChange(this, new OperatingUnitPropertyEventArgs(t_ouprop, defaults.OUPropButtons.update));
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            PnsStudio.OUPanelEnsureVisible(this);
            events.OnOUPropChange(this, new OperatingUnitPropertyEventArgs((OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject, defaults.OUPropButtons.cancel));
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            PnsStudio.OUPanelEnsureVisible(this);
            events.OnOUPropChange(this, new OperatingUnitPropertyEventArgs((OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject, defaults.OUPropButtons.delete));
        }
        private void propertyGridOperatingUnit_Leave(object sender, EventArgs e)
        {
            if (OUPropGrid == sender)
            {
                labelOperatingUnit.ForeColor = System.Drawing.SystemColors.HighlightText;
                labelOperatingUnit.BackColor = System.Drawing.SystemColors.ControlDark;
            }
        }
        private void propertyGridOperatingUnit_Enter(object sender, EventArgs e) { events.OnOperatingUnitPanelChange(this, e); }
        private void OperatingUnitPanel_Click(object sender, EventArgs e) { PnsStudio.OUPanelEnsureVisible(this); }
        private void tableLayoutOperatingUnit_Click(object sender, EventArgs e) { PnsStudio.OUPanelEnsureVisible(this); }
        private void labelOperatingUnit_Click(object sender, EventArgs e) { PnsStudio.OUPanelEnsureVisible(this); }
        private void propertyGridOperatingUnit_Click(object sender, EventArgs e) { PnsStudio.OUPanelEnsureVisible(this); }
        private void propertyGridOperatingUnit_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OperatingUnitProperties t_ouprop = (OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject;
            t_ouprop.UpdateFields();
            t_ouprop.Refresh();
            OUPropGrid.Refresh();
        }
        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
            PnsStudio.OUPanelEnsureVisible(this); 
            ((OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject).AutoConvert = checkBoxAutoConvert.Checked;
        }
        #endregion

        #region Properties
        public bool isModified { get { return cmpou.new_or_modified((OperatingUnitProperties)propertyGridOperatingUnit.SelectedObject); } }
        public string cmp_currname { set { cmpou.currname = value; } }
        public string labelText
        {
            set
            {
                labelOperatingUnit.Text = value + def_ou_panel.title_p1;
                if (cmpou.isnewitem) labelOperatingUnit.Text += def_ou_panel.title_p2;
            }
        }
        public Label OUPropLabel { get { return labelOperatingUnit; } }
        public PropertyGrid OUPropGrid { get { return propertyGridOperatingUnit; } }
        public Button OUPropUpdate { get { return buttonUpdate; } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class IOMaterial : CustomClass
    {
        #region Members
        private string m_name;
        private OperatingUnitProperties m_ou;
        private Layout m_layout;
        private int m_rateposx;
        private int m_rateposy;
        private double m_rate;
        private FractionMU m_ratemu;
        #endregion

        #region Constructors
        public IOMaterial(string t_name, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            m_name = t_name;
            m_layout = null;
            m_rateposx = 0;
            m_rateposy = 0;
            m_ratemu = MatProp.MinMU;
            m_rate = DefaultMUsAndValues.DefaultValues.d_flowrate * RateToDefault;
            AddProperties();
        }
        public IOMaterial(FlowMaterial mat, OperatingUnitProperties t_ou)
        {
            m_name = mat.name;
            m_ou = t_ou;
            m_layout = mat.layout;
            m_rateposx = mat.ratePosX;
            m_rateposy = mat.ratePosY;
            m_rate = mat.rate;
            if (m_rate == def_Values.d_NperA) m_rate = DefaultMUsAndValues.DefaultValues.d_flowrate;
            if (mat.flowrateMU != null)
            {
                MU t_numerator = DefaultMUsAndValues.MUs.FindMU(mat.flowrateMU.quantity_mu);
                MU t_denominator = DefaultMUsAndValues.MUs.FindMU(mat.flowrateMU.time_mu);
                if (MatProp.material_category != DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(t_numerator)) throw new Exception(def_PnsStudio_ex.Ex_iomaterial_type_mismatch);
                m_ratemu = MatProp.material_category.FlowMUs.Find(t_numerator, t_denominator);
            }
            else
            {
                m_ratemu = MatProp.MinMU;
                m_ratemu = DefaultFlowMU;
            }
            AddProperties();
        }
        public IOMaterial(IOMaterial mat, OperatingUnitProperties t_ou)
        {
            m_name = mat.m_name;
            m_ou = t_ou;
            m_layout = mat.m_layout;
            m_rateposx = mat.m_rateposx;
            m_rateposy = mat.m_rateposy;
            m_rate = mat.m_rate;
            m_ratemu = mat.m_ratemu;
            AddProperties();
        }
        #endregion

        #region Operator overloadings
        public static bool operator ==(IOMaterial mat1, IOMaterial mat2)
        {
            if (ReferenceEquals(mat1, null) && ReferenceEquals(mat2, null)) return true;
            if (ReferenceEquals(mat1, null) || ReferenceEquals(mat2, null)) return false;
            if (mat1.m_name != mat2.m_name) return false;
            if (mat1.m_layout != mat2.m_layout) return false;
            if (mat1.m_rateposx != mat2.m_rateposx) return false;
            if (mat1.m_rateposy != mat2.m_rateposy) return false;
            if (mat1.m_rate != mat2.m_rate) return false;
            if (mat1.m_ratemu != mat2.m_ratemu) return false;
            return true;
        }
        public static bool operator !=(IOMaterial mat1, IOMaterial mat2)
        {
            return !(mat1 == mat2);
        }
        public override bool Equals(object obj)
        {
            if (obj is IOMaterial) return this == (IOMaterial)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        private void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_iomat_prop.DN_rate, s_rate, def_iomat_prop.D_rate, "", s_rate.GetType(), false, true));
            Add(new CustomProp(this.GetType(), MatProp.material_category.FlowMUs, def_iomat_prop.DN_mu, RateMU, def_iomat_prop.D_mu, "", RateMU.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) s_rate = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) RateMU = (FractionMU)((CustomProp)List[1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = s_rate;
            if (List.Count > 1)
            {
                ((CustomProp)List[1]).Value = RateMU;
                ((CustomProp)List[1]).SelectList = MatProp.material_category.FlowMUs;
            }
        }
        public void UpdateRateValue()
        {
            FractionMU t_new_flowmu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(m_ratemu.Numerator).DefaultFlowMU;
            MU t_numerator = null;
            MU t_denominator = null;
            foreach (MU item in DefaultMUsAndValues.MUs.OldDefaultMUs)
            {
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(m_ratemu.Numerator)) t_numerator = item;
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(m_ratemu.Denominator)) t_denominator = item;
                if (t_numerator != null && t_denominator != null) break;
            }
            FractionMU t_old_flowmu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(m_ratemu.Numerator).FlowMUs.Find(t_numerator, t_denominator);
            m_rate *= DefaultMUsAndValues.ConvertMU(t_old_flowmu, t_new_flowmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
        }
        public FlowMaterial ToXML()
        {
            FlowMaterial mat = new FlowMaterial();
            mat.layout = m_layout;
            mat.ratePosX = m_rateposx;
            mat.ratePosY = m_rateposy;
            mat.name = m_name;
            mat.rate = m_rate;
            if (m_ratemu != DefaultFlowMU)
            {
                //                mat.flowRateMu = m_ratemu.ToString();
                mat.flowrateMU = m_ratemu.ToXMLFlowMU();
            }
            mat.flowRateMu = m_ratemu.ToString();
            return mat;
        }
        public override string ToString() { return Name + ": " + s_rate; }
        public string GenOutput(double t_size) { return Name + ": " + g_rate * t_size + " " + DefaultFlowMU; }
        public void GenTreeOutput(double t_size, TreeNode t_node) { t_node.Nodes.Add(Name + ": " + g_rate * t_size + " " + DefaultFlowMU); }
        #endregion

        #region Properties
        public OperatingUnitProperties OU { get { return m_ou; } }
        public string s_rate
        {
            get { return l_rate + " " + m_ratemu; }
            set 
            { 
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub / RateToDefault);
                if (t_value != def_Values.d_NperA) m_rate = t_value * RateToDefault;
                else m_rate = def_Values.io_flowrate * RateToDefault;
            }
        }
        public double l_rate { get { return m_rate / RateToDefault; } }
        public double g_rate { get { return m_rate; } set { m_rate = value; } }
        private double RateToDefault { get { return DefaultMUsAndValues.ConvertMU(m_ratemu, DefaultFlowMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public FractionMU RateMU
        {
            get { return m_ratemu; }
            set
            {
                if (!m_ou.AutoConvert) { m_rate *= DefaultMUsAndValues.ConvertMU(value, m_ratemu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); }
                m_ratemu = value;
            }
        }
        public FractionMU DirectRateMU { set { m_ratemu = value; } }
        public MaterialProperties MatProp { get { return PnsStudio.FindMaterial(m_name); } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public FractionMU DefaultFlowMU { get { return DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(m_ratemu.Numerator).DefaultFlowMU; } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class IOMaterials : CustomClass
    {
        #region Constructors
        public IOMaterials() { }
        public IOMaterials(FlowMaterial[] matlist, OperatingUnitProperties t_ou)
        {
            if (matlist != null)
            {
                foreach (FlowMaterial flowmat in matlist)
                {
                    IOMaterial t_mat = new IOMaterial(flowmat, t_ou);
                    Add(new CustomProp(this.GetType(), null, t_mat.Name, t_mat, "", "", t_mat.GetType(), false, true));
                }
            }
        }
        public IOMaterials(IOMaterials t_iomats, OperatingUnitProperties t_ou)
        {
            if (t_iomats != null)
            {
                foreach (CustomProp t_prop in t_iomats)
                {
                    IOMaterial t_mat = new IOMaterial((IOMaterial)t_prop.Value, t_ou);
                    Add(new CustomProp(this.GetType(), null, t_mat.Name, t_mat, "", "", t_mat.GetType(), false, true));
                }
            }
        }
        #endregion

        #region Operator overloadings
        public static bool operator ==(IOMaterials matlist1, IOMaterials matlist2)
        {
            if (ReferenceEquals(matlist1, null) && ReferenceEquals(matlist2, null)) return true;
            if (ReferenceEquals(matlist1, null) || ReferenceEquals(matlist2, null)) return false;
            if (matlist1.List.Count != matlist2.List.Count) return false;
            int i;
            for (i = 0; i < matlist1.List.Count; i++)
            {
                if ((IOMaterial)((CustomProp)matlist1.List[i]).Value != (IOMaterial)((CustomProp)matlist2.List[i]).Value) return false;
            }
            return true;
        }
        public static bool operator !=(IOMaterials matlist1, IOMaterials matlist2)
        {
            return !(matlist1 == matlist2);
        }
        public override bool Equals(object obj)
        {
            if (obj is IOMaterials) return this == (IOMaterials)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        internal System.Collections.IList list { get { return List; } }
        public void Insert(int t_index, IOMaterial t_mat)
        {
            List.Insert(t_index, new CustomProp(this.GetType(), null, t_mat.Name, t_mat, "", "", t_mat.GetType(), false, true));
        }
        public void RefreshList()
        {
            foreach (CustomProp t_prop in List)
            {
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                t_prop.Name = t_mat.Name;
                t_mat.Refresh();
            }
        }
        public void BuildTree(TreeNode t_node)
        {
            t_node.Nodes.Clear();
            foreach (CustomProp t_prop in List)
            {
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                t_node.Nodes.Add(t_mat.Name, t_mat.ToString());
            }
        }
        public void UpdateList()
        {
            foreach (CustomProp t_prop in List) ((IOMaterial)t_prop.Value).UpdateFields();
        }
        public bool UpdateFlowMaterialName(string oldname, string newname)
        {
            foreach (CustomProp t_prop in List)
            {
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                if (t_mat.Name == oldname)
                {
                    t_mat.Name = newname;
                    t_prop.Name = newname;
                    return true;
                }
            }
            return false;
        }
        public bool UpdateFlowMaterialCategory(string name, int t_choise)
        {
            foreach (CustomProp t_prop in List)
            {
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                if (t_mat.Name == name)
                {
                    double t_rate = t_mat.l_rate;
                    if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.min_mu) != 0) t_mat.DirectRateMU = t_mat.MatProp.MinMU;
                    else if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.max_mu) != 0) t_mat.DirectRateMU = t_mat.MatProp.MaxMU;
                    else if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.def_mu) != 0) t_mat.DirectRateMU = t_mat.MatProp.material_category.DefaultFlowMU;
                    if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.action_needed) != 0) t_mat.g_rate = def_Values.d_NperA;
                    else if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.keep_current) != 0) t_mat.g_rate = t_rate * DefaultMUsAndValues.ConvertMU(t_mat.RateMU, t_mat.MatProp.material_category.DefaultFlowMU, t_mat.OU.i_WorkingHoursPerYear, t_mat.OU.d_PayoutPeriod);
                    else if ((t_choise & 1 << (int)defaults.ChangeCategoryChoise.default_flowrate) != 0) t_mat.g_rate = DefaultMUsAndValues.DefaultValues.d_flowrate;
                    t_mat.Refresh();
                    return true;
                }
            }
            return false;
        }
        public void UpdateFlows()
        {
            foreach (CustomProp t_prop in List) ((IOMaterial)t_prop.Value).UpdateRateValue();
        }
        public FlowMaterial[] ToXML()
        {
            FlowMaterial[] mats = new FlowMaterial[List.Count];
            int i = 0;
            foreach (CustomProp t_prop in List) mats[i++] = ((IOMaterial)t_prop.Value).ToXML();
            return mats;
        }
        public override string ToString()
        {
            string t_ret = "";
            foreach (CustomProp t_prop in List)
            {
                if (t_ret != "") t_ret += "\n";
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                t_ret += "   - " + t_mat;
            }
            return t_ret;
        }
        public string GenOutput(double t_size)
        {
            string t_ret = "";
            foreach (CustomProp t_prop in List)
            {
                if (t_ret != "") t_ret += "\n";
                IOMaterial t_mat = (IOMaterial)t_prop.Value;
                t_ret += "   - " + t_mat.GenOutput(t_size);
            }
            return t_ret;
        }
        public void GenTreeOutput(double t_size, TreeNode t_node)
        {
            foreach (CustomProp t_prop in List)((IOMaterial)t_prop.Value).GenTreeOutput(t_size, t_node);
        }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class OperatingCost : CustomClass
    {
        #region Members
        private double f_fix;
        private double f_prop;
        private FractionMU f_fixmu;
        private FractionMU f_propmu;
        private OperatingUnitProperties m_ou;
        #endregion

        #region Constructors
        public OperatingCost(OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fix = DefaultMUsAndValues.DefaultValues.OperatingCost.d_fixed_charge;
            f_prop = DefaultMUsAndValues.DefaultValues.OperatingCost.d_prop_constant;
            f_fixmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            f_propmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            AddProperties();
        }
        public OperatingCost(OperatingUnit ou, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            if (ou.operatingFixSpecified) f_fix = ou.operatingFix;
            else f_fix = DefaultMUsAndValues.DefaultValues.OperatingCost.d_fixed_charge;
            if (ou.operatingPropSpecified) f_prop = ou.operatingProp;
            else f_prop = DefaultMUsAndValues.DefaultValues.OperatingCost.d_prop_constant;
            MU t_numerator;
            MU t_denominator;
            if (ou.operatingFixMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(ou.operatingFixMU.currency_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(ou.operatingFixMU.time_mu);
                f_fixmu = DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator);
            }
            else f_fixmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            if (ou.operatingPropMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(ou.operatingPropMU.currency_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(ou.operatingPropMU.time_mu);
                f_propmu = DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator);
            }
            else f_propmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            AddProperties();
        }
        public OperatingCost(OperatingCost t_cost, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fix = t_cost.f_fix;
            f_prop = t_cost.f_prop;
            f_fixmu = t_cost.f_fixmu;
            f_propmu = t_cost.f_propmu;
            AddProperties();
        }
        #endregion

        #region Operator overloading
        public static bool operator ==(OperatingCost cost1, OperatingCost cost2)
        {
            if (ReferenceEquals(cost1, null) && ReferenceEquals(cost2, null)) return true;
            if (ReferenceEquals(cost1, null) || ReferenceEquals(cost2, null)) return false;
            if (cost1.f_fix != cost2.f_fix) return false;
            if (cost1.f_prop != cost2.f_prop) return false;
            if (cost1.f_fixmu != cost2.f_fixmu) return false;
            if (cost1.f_propmu != cost2.f_propmu) return false;
            return true;
        }
        public static bool operator !=(OperatingCost cost1, OperatingCost cost2)
        {
            return !(cost1 == cost2);
        }
        public override bool Equals(object obj)
        {
            if (obj is OperatingCost) return this == (OperatingCost)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_ofix, s_fix, def_ou_prop.D_ofix, "", s_fix.GetType(), false, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CostMUs, def_ou_prop.DN_ofixmu, fixmu, def_ou_prop.D_ofixmu, "", fixmu.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_oprop, s_prop, def_ou_prop.D_oprop, "", s_prop.GetType(), false, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CostMUs, def_ou_prop.DN_opropmu, propmu, def_ou_prop.D_opropmu, "", propmu.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) s_fix = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) fixmu = (FractionMU)((CustomProp)List[1]).Value;
            if (List.Count > 2) s_prop = (string)((CustomProp)List[2]).Value;
            if (List.Count > 3) propmu = (FractionMU)((CustomProp)List[3]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = s_fix;
            if (List.Count > 1) ((CustomProp)List[1]).Value = fixmu;
            if (List.Count > 2) ((CustomProp)List[2]).Value = s_prop;
            if (List.Count > 3) ((CustomProp)List[3]).Value = propmu;
        }
        public void UpdateCostValues()
        {
            MU t_numerator = null;
            MU t_denominator = null;
            foreach (MU item in DefaultMUsAndValues.MUs.OldDefaultMUs)
            {
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_fixmu.Numerator)) t_numerator = item;
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_fixmu.Denominator)) t_denominator = item;
                if (t_numerator != null && t_denominator != null) break;
            }
            f_fix *= DefaultMUsAndValues.ConvertMU(DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator), DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
            f_prop *= DefaultMUsAndValues.ConvertMU(DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator), DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
        }
        public void ToXML(OperatingUnit xmlou)
        {
            xmlou.operatingFix = f_fix;
            xmlou.operatingFixSpecified = f_fix != DefaultMUsAndValues.DefaultValues.OperatingCost.d_fixed_charge;
            xmlou.operatingProp = f_prop;
            xmlou.operatingPropSpecified = f_prop != DefaultMUsAndValues.DefaultValues.OperatingCost.d_prop_constant;
            if (f_fixmu != DefaultMUsAndValues.MUs.DefaultCostMU) xmlou.operatingFixMU = f_fixmu.ToXMLCostMU();
            if (f_propmu != DefaultMUsAndValues.MUs.DefaultCostMU) xmlou.operatingPropMU = f_propmu.ToXMLCostMU();
        }
        public override string ToString()
        {
            return "fix:" + s_fix + "; prop.:" + s_prop;
        }
        public string GenOutput(double t_size)
        {
            return "("+ToString()+")  " + (f_fix + f_prop * t_size) + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
        }
        #endregion

        #region Properties
        public string s_fix 
        { 
            get { return l_fix + " " + f_fixmu; } 
            set 
            { 
                double t_value = Converters.ToDouble(0, value);
                if (t_value != def_Values.d_NperA) f_fix = t_value * FixToDefault;
                else f_fix = def_Values.o_fix * FixToDefault;
            } 
        }
        public double l_fix { get { return f_fix / FixToDefault; } }
        public double d_fix { get { return f_fix; } }
        private double FixToDefault { get { return DefaultMUsAndValues.ConvertMU(f_fixmu, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public FractionMU fixmu
        {
            get { return f_fixmu; }
            set
            {
                if (!m_ou.AutoConvert) f_fix *= DefaultMUsAndValues.ConvertMU(value, f_fixmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
                f_fixmu = value;
            }
        }
        public string s_prop 
        { 
            get { return l_prop + " " + f_propmu; } 
            set 
            {
                double t_value = Converters.ToDouble(0, value);
                if (t_value != def_Values.d_NperA) f_prop = t_value * PropToDefault;
                else f_prop = def_Values.o_prop * PropToDefault;
            } 
        }
        public double l_prop { get { return f_prop / PropToDefault; } }
        public double d_prop { get { return f_prop; } }
        private double PropToDefault { get { return DefaultMUsAndValues.ConvertMU(f_propmu, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public FractionMU propmu
        {
            get { return f_propmu; }
            set
            {
                if (!m_ou.AutoConvert) { f_prop *= DefaultMUsAndValues.ConvertMU(value, f_propmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); }
                f_propmu = value;
            }
        }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class InvestmentCost : CustomClass
    {
        #region Members
        private double f_fix;
        private double f_prop;
        private MU f_fixmu;
        private MU f_propmu;
        private OperatingUnitProperties m_ou;
        #endregion

        #region Constructors
        public InvestmentCost(OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fix = DefaultMUsAndValues.DefaultValues.InvestmentCost.d_fixed_charge;
            f_prop = DefaultMUsAndValues.DefaultValues.InvestmentCost.d_prop_constant;
            f_fixmu = DefaultMUsAndValues.MUs.DefaultCurrencyMU;
            f_propmu = DefaultMUsAndValues.MUs.DefaultCurrencyMU;
            AddProperties();
        }
        public InvestmentCost(OperatingUnit ou, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            if (ou.investmentFixSpecified) f_fix = ou.investmentFix;
            else f_fix = DefaultMUsAndValues.DefaultValues.InvestmentCost.d_fixed_charge;
            if (ou.investmentPropSpecified) f_prop = ou.investmentProp;
            else f_prop = DefaultMUsAndValues.DefaultValues.InvestmentCost.d_prop_constant;
            if (ou.investmentFixMU != null) f_fixmu = DefaultMUsAndValues.MUs.FindMU(ou.investmentFixMU);
            else f_fixmu = DefaultMUsAndValues.MUs.DefaultCurrencyMU;
            if (ou.investmentPropMU != null) f_propmu = DefaultMUsAndValues.MUs.FindMU(ou.investmentPropMU);
            else f_propmu = DefaultMUsAndValues.MUs.DefaultCurrencyMU;
            AddProperties();
        }
        public InvestmentCost(InvestmentCost t_cost, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fix = t_cost.f_fix;
            f_prop = t_cost.f_prop;
            f_fixmu = t_cost.f_fixmu;
            f_propmu = t_cost.f_propmu;
            AddProperties();
        }
        #endregion

        #region Operator overloading
        public static bool operator ==(InvestmentCost cost1, InvestmentCost cost2)
        {
            if (ReferenceEquals(cost1, null) && ReferenceEquals(cost2, null)) return true;
            if (ReferenceEquals(cost1, null) || ReferenceEquals(cost2, null)) return false;
            if (cost1.f_fix != cost2.f_fix) return false;
            if (cost1.f_prop != cost2.f_prop) return false;
            if (cost1.f_fixmu != cost2.f_fixmu) return false;
            if (cost1.f_propmu != cost2.f_propmu) return false;
            return true;
        }
        public static bool operator !=(InvestmentCost cost1, InvestmentCost cost2)
        {
            return !(cost1 == cost2);
        }
        public override bool Equals(object obj)
        {
            if (obj is InvestmentCost) return this == (InvestmentCost)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_ifix, s_fix, def_ou_prop.D_ifix, "", s_fix.GetType(), false, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CurrencyMUs, def_ou_prop.DN_ifixmu, fixmu, def_ou_prop.D_ifixmu, "", fixmu.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_iprop, s_prop, def_ou_prop.D_iprop, "", s_prop.GetType(), false, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CurrencyMUs, def_ou_prop.DN_ipropmu, propmu, def_ou_prop.D_ipropmu, "", propmu.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) s_fix = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) fixmu = (MU)((CustomProp)List[1]).Value;
            if (List.Count > 2) s_prop = (string)((CustomProp)List[2]).Value;
            if (List.Count > 3) propmu = (MU)((CustomProp)List[3]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = s_fix;
            if (List.Count > 1) ((CustomProp)List[1]).Value = fixmu;
            if (List.Count > 2) ((CustomProp)List[2]).Value = s_prop;
            if (List.Count > 3) ((CustomProp)List[3]).Value = propmu;
        }
        public void UpdateCostValues()
        {
            foreach (MU item in DefaultMUsAndValues.MUs.OldDefaultMUs)
            {
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_fixmu))
                {
                    f_fix *= DefaultMUsAndValues.ConvertMU(item, DefaultMUsAndValues.MUs.DefaultCurrencyMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
                    f_prop *= DefaultMUsAndValues.ConvertMU(item, DefaultMUsAndValues.MUs.DefaultCurrencyMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
                    break;
                }
            }
        }
        public void ToXML(OperatingUnit xmlou)
        {
            xmlou.investmentFix = f_fix;
            xmlou.investmentFixSpecified = f_fix!=DefaultMUsAndValues.DefaultValues.InvestmentCost.d_fixed_charge;
            xmlou.investmentProp = f_prop;
            xmlou.investmentPropSpecified = f_prop!=DefaultMUsAndValues.DefaultValues.InvestmentCost.d_prop_constant;
            if (f_fixmu != DefaultMUsAndValues.MUs.DefaultCurrencyMU) xmlou.investmentFixMU = f_fixmu.ToXMLMU();
            if (f_propmu != DefaultMUsAndValues.MUs.DefaultCurrencyMU) xmlou.investmentPropMU = f_propmu.ToXMLMU();
        }
        public override string ToString()
        {
            return "fix:" + s_fix + "; prop.:" + s_prop;
        }
        public string GenOutput(double t_size)
        {
            return "(" + ToString() + ") " + (g_fix + t_size * g_prop) + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
        }
        #endregion

        #region Properties
        public string s_fix 
        { 
            get { return l_fix + " " + f_fixmu; } 
            set 
            { 
                double t_value = Converters.ToDouble(0, value);
                if (t_value != def_Values.d_NperA) f_fix = t_value * FixToDefault;
                else f_fix = def_Values.i_fix * FixToDefault;
            } 
        }
        public double l_fix { get { return f_fix / FixToDefault; } }
        public double d_fix { get { return f_fix; } }
        private double FixToDefault { get { return DefaultMUsAndValues.ConvertMU(f_fixmu, DefaultMUsAndValues.MUs.DefaultCurrencyMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public MU fixmu
        {
            get { return f_fixmu; }
            set
            {
                if (!m_ou.AutoConvert) { f_fix *= DefaultMUsAndValues.ConvertMU(value, f_fixmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); }
                f_fixmu = value;
            }
        }
        public string s_prop 
        { 
            get { return l_prop + " " + f_propmu; ; } 
            set 
            {
                double t_value = Converters.ToDouble(0, value);
                if (t_value != def_Values.d_NperA) f_prop = t_value * PropToDefault;
                else f_prop = def_Values.i_prop * PropToDefault;
            } 
        }
        public double l_prop { get { return f_prop / PropToDefault; } }
        public double d_prop { get { return f_prop; } }
        private double PropToDefault { get { return DefaultMUsAndValues.ConvertMU(f_propmu, DefaultMUsAndValues.MUs.DefaultCurrencyMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public MU propmu
        {
            get { return f_propmu; }
            set
            {
                if (!m_ou.AutoConvert) { f_prop *= DefaultMUsAndValues.ConvertMU(value, f_propmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); }
                f_propmu = value;
            }
        }
        public FractionMU PayoutFixMU { get { return DefaultMUsAndValues.MUs.CostMUs.Find(f_fixmu, DefaultMUsAndValues.MUs.Categories.FindMU((int)defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.payout_period)); } }
        public FractionMU PayoutPropMU { get { return DefaultMUsAndValues.MUs.CostMUs.Find(f_propmu, DefaultMUsAndValues.MUs.Categories.FindMU((int)defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.payout_period)); } }
        public double g_fix { get { return l_fix * DefaultMUsAndValues.ConvertMU(PayoutFixMU, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public double g_prop { get { return l_prop * DefaultMUsAndValues.ConvertMU(PayoutPropMU, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class OverallCost : CustomClass
    {
        #region Members
        private FractionMU f_fixmu;
        private FractionMU f_propmu;
        private OperatingUnitProperties m_ou;
        #endregion

        #region Constructors
        public OverallCost(OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fixmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            f_propmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            AddProperties();
        }
        public OverallCost(OperatingUnit ou, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            MU t_numerator;
            MU t_denominator;
            if (ou.totalFixMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(ou.totalFixMU.currency_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(ou.totalFixMU.time_mu);
                f_fixmu = DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator);
            }
            else f_fixmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            if (ou.totalPropMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(ou.totalPropMU.currency_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(ou.totalPropMU.time_mu);
                f_propmu = DefaultMUsAndValues.MUs.CostMUs.Find(t_numerator, t_denominator);
            }
            else f_propmu = DefaultMUsAndValues.MUs.DefaultCostMU;
            AddProperties();
        }
        public OverallCost(OverallCost t_cost, OperatingUnitProperties t_ou)
        {
            m_ou = t_ou;
            f_fixmu = t_cost.f_fixmu;
            f_propmu = t_cost.f_propmu;
            AddProperties();
        }
        #endregion

        #region Operator overloading
        public static bool operator ==(OverallCost cost1, OverallCost cost2)
        {
            if (ReferenceEquals(cost1, null) && ReferenceEquals(cost2, null)) return true;
            if (ReferenceEquals(cost1, null) || ReferenceEquals(cost2, null)) return false;
            if (cost1.f_fixmu != cost2.f_fixmu) return false;
            if (cost1.f_propmu != cost2.f_propmu) return false;
            return true;
        }
        public static bool operator !=(OverallCost cost1, OverallCost cost2)
        {
            return !(cost1 == cost2);
        }
        public override bool Equals(object obj)
        {
            if (obj is OverallCost) return this == (OverallCost)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_fix, s_fix, def_ou_prop.D_fix, "", s_fix.GetType(), true, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CostMUs, def_ou_prop.DN_fixmu, f_fixmu, def_ou_prop.D_fixmu, "", f_fixmu.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_prop, s_prop, def_ou_prop.D_prop, "", s_prop.GetType(), true, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.CostMUs, def_ou_prop.DN_propmu, f_propmu, def_ou_prop.D_propmu, "", f_propmu.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 1) f_fixmu = (FractionMU)((CustomProp)List[1]).Value;
            if (List.Count > 3) f_propmu = (FractionMU)((CustomProp)List[3]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = s_fix;
            if (List.Count > 1) ((CustomProp)List[1]).Value = f_fixmu;
            if (List.Count > 2) ((CustomProp)List[2]).Value = s_prop;
            if (List.Count > 3) ((CustomProp)List[3]).Value = f_propmu;
        }
        public void ToXML(OperatingUnit xmlou)
        {
            xmlou.totalFix = g_fix;
            xmlou.totalProp = g_prop;
            if (f_fixmu != DefaultMUsAndValues.MUs.DefaultCostMU) xmlou.totalFixMU = f_fixmu.ToXMLCostMU();
            if (f_propmu != DefaultMUsAndValues.MUs.DefaultCostMU) xmlou.totalPropMU = f_propmu.ToXMLCostMU();
        }
        public override string ToString()
        {
            return "fix:" + s_fix + "; prop.:" + s_prop;
        }
        public string GenOutput(double t_size)
        {
            return "(" + ToString() + ")  " + (g_fix + g_prop * t_size) + DefaultMUsAndValues.MUs.DefaultCostMU;
        }
        #endregion

        #region Properties
        public string s_fix { get { return l_fix + " " + f_fixmu; } }
        public double l_fix
        {
            get
            {
                return m_ou.InvCost.l_fix * DefaultMUsAndValues.ConvertMU(m_ou.InvCost.PayoutFixMU, f_fixmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod) +
                m_ou.OpCost.l_fix * DefaultMUsAndValues.ConvertMU(m_ou.OpCost.fixmu, f_fixmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
            }
        }
        public double g_fix { get { return l_fix * DefaultMUsAndValues.ConvertMU(f_fixmu, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        public string s_prop { get { return l_prop + " " + f_propmu; } }
        public double l_prop
        {
            get
            {
                return m_ou.InvCost.l_prop * DefaultMUsAndValues.ConvertMU(m_ou.InvCost.PayoutPropMU, f_propmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod) +
                m_ou.OpCost.l_prop * DefaultMUsAndValues.ConvertMU(m_ou.OpCost.propmu, f_propmu, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod);
            }
        }
        public double g_prop { get { return l_prop * DefaultMUsAndValues.ConvertMU(f_propmu, DefaultMUsAndValues.MUs.DefaultCostMU, m_ou.i_WorkingHoursPerYear, m_ou.d_PayoutPeriod); } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class Bounds : CustomClass
    {
        #region Members
        private double f_lb;
        private double f_ub;
        #endregion

        #region Constructors
        public Bounds()
        {
            f_lb = def_Values.d_NperA;
            f_ub = def_Values.d_NperA;
            AddProperties();
        }
        public Bounds(OperatingUnit ou)
        {
            if (ou.lowerBoundSpecified) f_lb = ou.lowerBound;
            else f_lb = def_Values.d_NperA;
            if (ou.upperBoundSpecified) f_ub = ou.upperBound;
            else f_ub = def_Values.d_NperA;
            AddProperties();
        }
        public Bounds(Bounds t_cost)
        {
            f_lb = t_cost.f_lb;
            f_ub = t_cost.f_ub;
            AddProperties();
        }
        #endregion

        #region Operator overloading
        public static bool operator ==(Bounds par1, Bounds par2)
        {
            if (ReferenceEquals(par1, null) && ReferenceEquals(par2, null)) return true;
            if (ReferenceEquals(par1, null) || ReferenceEquals(par2, null)) return false;
            if (par1.f_lb != par2.f_lb) return false;
            if (par1.f_ub != par2.f_ub) return false;
            return true;
        }
        public static bool operator !=(Bounds par1, Bounds par2)
        {
            return !(par1 == par2);
        }
        public override bool Equals(object obj)
        {
            if (obj is Bounds) return this == (Bounds)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_lb, s_lb, def_ou_prop.D_lb, "", s_lb.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_ub, s_ub, def_ou_prop.D_ub, "", s_ub.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) s_lb = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) s_ub = (string)((CustomProp)List[1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = s_lb;
            if (List.Count > 1) ((CustomProp)List[1]).Value = s_ub;
        }
        public bool Validate()
        {
            if (d_lb > d_ub)
            {
                MessageBox.Show(Msg_lb_ub);
                return false;
            }
            return true;
        }
        public void ToXML(OperatingUnit xmlou)
        {
            xmlou.lowerBound = f_lb;
            xmlou.lowerBoundSpecified = xmlou.lowerBound != def_Values.d_NperA;
            xmlou.upperBound = f_ub;
            xmlou.upperBoundSpecified = xmlou.upperBound != def_Values.d_NperA;
        }
        public override string ToString()
        {
            return "lb.:" + s_lb + "; ub.:" + s_ub;
        }
        #endregion

        #region Properties
        public string s_lb
        {
            get { return d_lb + (f_lb == def_Values.d_NperA ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound) f_lb = t_value;
                else f_lb = def_Values.d_NperA;
            }
        }
        public double d_lb
        {
            get
            {
                if (f_lb != def_Values.d_NperA) return f_lb;
                return DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound;
            }
        }
        public string s_ub
        {
            get { return d_ub + (f_ub == def_Values.d_NperA ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub);
                if (t_value != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound) f_ub = t_value;
                else f_ub = def_Values.d_NperA;
            }
        }
        public double d_ub
        {
            get
            {
                if (f_ub != def_Values.d_NperA) return f_ub;
                return DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound;
            }
        }
        static public string Msg_lb_ub { get { return def_ou_prop.DN_lb + def_PnsStudio.Msg_must_be_less_or_equal_than + def_ou_prop.DN_ub; } }
        #endregion
    }

    [TypeConverter(typeof(CustomExpandableConverter))]
    public class OperatingUnitProperties : CustomClass
    {
        #region Members
        private Layout f_layout;
        private IOMaterials f_imats;
        private IOMaterials f_omats;
        private string f_currname;
        private string f_name;
        private OperatingCost f_op_cost;
        private InvestmentCost f_inv_cost;
        private OverallCost f_all_cost;
        private Bounds f_bounds;
        private bool f_isnewitem;
        private bool f_autoconvert;
        private int f_working_hours_per_year;
        private double f_payout_period;
        private DescriptionStr f_description;
        #endregion

        #region Constructors
        public OperatingUnitProperties(string name)
        {
            f_working_hours_per_year = (int)def_Values.d_NperA;
            f_payout_period = def_Values.d_NperA;
            f_imats = new IOMaterials();
            f_omats = new IOMaterials();
            f_op_cost = new OperatingCost(this);
            f_inv_cost = new InvestmentCost(this);
            f_all_cost = new OverallCost(this);
            f_bounds = new Bounds();
            f_name = f_currname = name;
            f_isnewitem = true;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public OperatingUnitProperties(OperatingUnit ou)
        {
            f_working_hours_per_year = ou.workingHoursPerYear;
            f_payout_period = ou.payoutPeriod;
            f_layout = ou.layout;
            f_imats = new IOMaterials(ou.inputMaterial, this);
            f_omats = new IOMaterials(ou.outputMaterial, this);
            f_op_cost = new OperatingCost(ou, this);
            f_inv_cost = new InvestmentCost(ou, this);
            f_all_cost = new OverallCost(ou, this);
            f_bounds = new Bounds(ou);
            f_name = f_currname = ou.name;
            f_isnewitem = false;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            if (ou.description != null) f_description = new DescriptionStr(ou.description);
            else f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public OperatingUnitProperties(OperatingUnitProperties ou)
        {
            f_working_hours_per_year = ou.f_working_hours_per_year;
            f_payout_period = ou.f_payout_period;
            f_layout = ou.f_layout;
            f_imats = new IOMaterials(ou.f_imats, this);
            f_omats = new IOMaterials(ou.f_omats, this);
            f_op_cost = new OperatingCost(ou.f_op_cost, this);
            f_inv_cost = new InvestmentCost(ou.f_inv_cost, this);
            f_all_cost = new OverallCost(ou.f_all_cost, this);
            f_bounds = new Bounds(ou.f_bounds);
            f_name = ou.f_name;
            f_currname = ou.f_currname;
            f_isnewitem = ou.f_isnewitem;
            f_autoconvert = ou.f_autoconvert;
            f_description = new DescriptionStr(ou.f_description.description);
            AddProperties();
        }
        #endregion

        #region Member functions
        public void BuildTree(TreeNode t_node)
        {
            TreeNode t_ioflows;
            f_imats.BuildTree((t_ioflows = PnsStudio.FindIOFlowsNode(t_node, true)) != null ? t_ioflows : t_node.Nodes.Add(def_ou_tree.InputMaterialsName, def_ou_tree.InputMaterialsText));
            f_omats.BuildTree((t_ioflows = PnsStudio.FindIOFlowsNode(t_node, false)) != null ? t_ioflows : t_node.Nodes.Add(def_ou_tree.OutputMaterialsName, def_ou_tree.OutputMaterialsText));
        }
        public void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_name, name, def_ou_prop.D_name, def_ou_prop.cat1, name.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_working_hours_per_year, WorkingHoursPerYear, def_ou_prop.D_working_hours_per_year, def_ou_prop.cat1, WorkingHoursPerYear.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_payout_period, PayoutPeriod, def_ou_prop.D_payout_period, def_ou_prop.cat1, PayoutPeriod.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_imats, imats, def_ou_prop.D_imats, def_ou_prop.cat2, imats.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_omats, omats, def_ou_prop.D_omats, def_ou_prop.cat2, omats.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_ocost, ocost, def_ou_prop.D_ocost, def_ou_prop.cat3, ocost.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_icost, icost, def_ou_prop.D_icost, def_ou_prop.cat3, icost.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_overallcost, overallcost, def_ou_prop.D_overallcost, def_ou_prop.cat3, overallcost.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_bounds, bounds, def_ou_prop.D_bounds, def_ou_prop.cat4, bounds.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_ou_prop.DN_description, description, def_ou_prop.D_description, def_ou_prop.cat5, description.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) name = (string)((CustomProp)List[0]).Value;
            if (List.Count > 1) WorkingHoursPerYear = (string)((CustomProp)List[1]).Value;
            if (List.Count > 2) PayoutPeriod = (string)((CustomProp)List[2]).Value;
            f_imats.UpdateList();
            f_omats.UpdateList();
            f_op_cost.UpdateFields();
            f_inv_cost.UpdateFields();
            f_all_cost.UpdateFields();
            f_bounds.UpdateFields();
            description = (DescriptionStr)((CustomProp)List[List.Count-1]).Value;
        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = name;
            if (List.Count > 1) ((CustomProp)List[1]).Value = WorkingHoursPerYear;
            if (List.Count > 2) ((CustomProp)List[2]).Value = PayoutPeriod;
            f_imats.RefreshList();
            f_omats.RefreshList();
            f_op_cost.Refresh();
            f_inv_cost.Refresh();
            f_all_cost.Refresh();
            f_bounds.Refresh();
            ((CustomProp)List[List.Count-1]).Value = description;
        }
        public IOMaterial InsertIOMaterial(string t_io_list_name, TreeNode t_material_node, int t_material_index, IOMaterial t_ioflow)
        {
            IOMaterial t_iomat;
            if (t_ioflow == null) t_iomat = new IOMaterial(t_material_node.Name, this);
            else t_iomat = new IOMaterial(t_ioflow, this);
            if (t_io_list_name == def_ou_tree.InputMaterialsName) f_imats.Insert(t_material_index, t_iomat);
            else f_omats.Insert(t_material_index, t_iomat);
            return t_iomat;
        }
        public void UpdateFlowMaterialName(string oldname, string newname)
        {
            if (!f_imats.UpdateFlowMaterialName(oldname, newname)) f_omats.UpdateFlowMaterialName(oldname, newname);
        }
        public void UpdateFlowMaterialCategory(string name, int t_choise)
        {
            if (!f_imats.UpdateFlowMaterialCategory(name, t_choise)) f_omats.UpdateFlowMaterialCategory(name, t_choise);
        }
        public void UpdateAllValues()
        {
            f_imats.UpdateFlows();
            f_omats.UpdateFlows();
            f_op_cost.UpdateCostValues();
            f_inv_cost.UpdateCostValues();
        }
        public bool Validate()
        {
            return f_bounds.Validate();
        }
        public OperatingUnit ToXML()
        {
            OperatingUnit xmlou = new OperatingUnit();
            xmlou.layout = f_layout;
            xmlou.name = name;
            xmlou.inputMaterial = imats.ToXML();
            xmlou.outputMaterial = omats.ToXML();
            f_inv_cost.ToXML(xmlou);
            f_op_cost.ToXML(xmlou);
            f_all_cost.ToXML(xmlou);
            f_bounds.ToXML(xmlou);
            xmlou.workingHoursPerYear = f_working_hours_per_year;
            xmlou.payoutPeriod = f_payout_period;
            xmlou.description = f_description.description; 
            return xmlou;
        }
        public bool new_or_modified(OperatingUnitProperties ou)
        {
            if (f_isnewitem) return true;
            if (f_layout != ou.f_layout) return true;
            if (f_imats != ou.f_imats) return true;
            if (f_omats != ou.f_omats) return true;
            if (f_currname != ou.f_currname) return true;
            if (f_name != ou.f_name) return true;
            if (f_op_cost != ou.f_op_cost) return true;
            if (f_inv_cost != ou.f_inv_cost) return true;
            if (f_bounds != ou.f_bounds) return true;
            if (f_autoconvert != ou.f_autoconvert) return true;
            if (f_working_hours_per_year != ou.f_working_hours_per_year) return true;
            if (f_payout_period != ou.f_payout_period) return true;
            if (f_description != ou.f_description) return true;
            return false;
        }
        public override string ToString()
        {
            return def_ou_prop.DN_name + ": " + name +
            "\n\n" + def_ou_prop.DN_working_hours_per_year + ": " + WorkingHoursPerYear +
            "\n" + def_ou_prop.DN_payout_period + ": " + PayoutPeriod +
            "\n\n" + def_ou_prop.DN_imats + ":\n" + imats +
            "\n" + def_ou_prop.DN_omats + ":\n" + omats +
            "\n\n" + def_ou_prop.DN_ocost + ": " + ocost +
            "\n" + def_ou_prop.DN_icost + ": " + icost +
            "\n" + def_ou_prop.DN_overallcost + ": " + overallcost +
            "\n\n" + def_ou_prop.DN_bounds + ": " + bounds +
            "\n\n" + def_ou_prop.DN_description + ": " + description;
        }
        public string GenOutput(ref double t_size)
        {
            string t_ret = name + ", " + def_Solution_Tree.text_size_factor + ": " + t_size + ", " + def_Solution_Tree.text_cost + ": " + (t_size * overallcost.g_prop + overallcost.g_fix) + " " + DefaultMUsAndValues.MUs.DefaultCostMU +
            "\n" + def_ou_prop.DN_working_hours_per_year + ": " + WorkingHoursPerYear +
            "\n" + def_ou_prop.DN_payout_period + ": " + PayoutPeriod +
            "\n" + def_ou_prop.DN_bounds + ": " + bounds +
            "\n\n" + def_ou_prop.DN_imats + ":\n" + imats.GenOutput(t_size) +
            "\n" + def_ou_prop.DN_omats + ":\n" + omats.GenOutput(t_size) +
            "\n\n" + def_ou_prop.DN_ocost + ": " + ocost.GenOutput(t_size) +
            "\n" + def_ou_prop.DN_icost + ": " + icost.GenOutput(t_size) +
            "\n" + def_ou_prop.DN_overallcost + ": " + overallcost.GenOutput(t_size) +
            "\n\n" + def_ou_prop.DN_description + ": " + description;
            t_size = t_size * overallcost.g_prop + overallcost.g_fix;
            return t_ret;
        }
        public double GenTreeOutput(double t_size, TreeNode t_node)
        {
            t_node = t_node.Nodes.Add(name + ", " + def_Solution_Tree.text_size_factor + ": " + t_size + ", " +
                def_Solution_Tree.text_cost + ": " + (t_size * overallcost.g_prop + overallcost.g_fix) + " " + DefaultMUsAndValues.MUs.DefaultCostMU);
            t_node.Nodes.Add(def_ou_prop.DN_working_hours_per_year + ": " + WorkingHoursPerYear);
            t_node.Nodes.Add(def_ou_prop.DN_payout_period + ": " + PayoutPeriod);
            t_node.Nodes.Add(def_ou_prop.DN_bounds + ": " + bounds);
            TreeNode t_subnode = t_node.Nodes.Add(def_ou_prop.DN_imats);
            imats.GenTreeOutput(t_size, t_subnode);
            t_subnode = t_node.Nodes.Add(def_ou_prop.DN_omats);
            omats.GenTreeOutput(t_size, t_subnode);
            t_subnode = t_node.Nodes.Add(def_ou_prop.DN_ocost + ": " + ocost.GenOutput(t_size));
            t_subnode = t_node.Nodes.Add(def_ou_prop.DN_icost + ": " + icost.GenOutput(t_size));
            t_subnode = t_node.Nodes.Add(def_ou_prop.DN_overallcost + ": " + overallcost.GenOutput(t_size));
            t_node.Nodes.Add(def_ou_prop.DN_description + ": " + description);
            return t_size * overallcost.g_prop + overallcost.g_fix;
        }
        #endregion

        #region Properties
        internal OperatingCost OpCost { get { return f_op_cost; } }
        internal InvestmentCost InvCost { get { return f_inv_cost; } }
        public string currname { get { return f_currname; } set { f_currname = value; } }
        public string name { get { return f_name; } set { f_name = Converters.ToNameString(value); } }
        public string WorkingHoursPerYear
        {
            get { return i_WorkingHoursPerYear + " h/yr" + (f_working_hours_per_year == def_Values.d_NperA ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                int t_value = (int)Converters.ToDouble(1, value, 8760);
                if (t_value != DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear) f_working_hours_per_year = t_value;
                else f_working_hours_per_year = (int)def_Values.d_NperA;
            }
        }
        public int i_WorkingHoursPerYear
        {
            get
            {
                if (f_working_hours_per_year != def_Values.d_NperA) return f_working_hours_per_year;
                return DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear;
            }
        }
        public string PayoutPeriod
        {
            get { return d_PayoutPeriod + " yr/payout period" + (f_payout_period == def_Values.d_NperA ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(1, value);
                if (t_value != DefaultMUsAndValues.MUs.DefaultPayoutPeriod) f_payout_period = t_value;
                else f_payout_period = def_Values.d_NperA;
            }
        }
        public double d_PayoutPeriod
        {
            get
            {
                if (f_payout_period != def_Values.d_NperA) return f_payout_period;
                return DefaultMUsAndValues.MUs.DefaultPayoutPeriod;
            }
        }
        public IOMaterials imats { get { return f_imats; } }
        public IOMaterials omats { get { return f_omats; } }
        public InvestmentCost icost { get { return f_inv_cost; } set { f_inv_cost = value; } }
        public OperatingCost ocost { get { return f_op_cost; } set { f_op_cost = value; } }
        public OverallCost overallcost { get { return f_all_cost; } }
        public Bounds bounds { get { return f_bounds; } set { f_bounds = value; } }
        public bool isnewitem { get { return f_isnewitem; } set { f_isnewitem = value; } }
        public bool AutoConvert { get { return f_autoconvert; } set { f_autoconvert = value; } }
        public DescriptionStr description { get { return f_description; } set { f_description = value; } }
        #endregion
    }
}
