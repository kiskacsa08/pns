using System;
using System.ComponentModel;
using System.Windows.Forms;
using DynamicPropertyGrid;
using System.Drawing.Design;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.ExceptionMsgs;
using Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Globals;


namespace Pns.Dialogs
{
    public partial class MaterialPanel : UserControl
    {
        #region Members
        private MaterialProperties cmpmat;
        #endregion

        #region Constructors
        public MaterialPanel(MaterialProperties mat)
        {
            InitializeComponent();
            cmpmat = new MaterialProperties(mat);
            propertyGridMaterial.SelectedObject = mat;
            labelText = mat.currname;
            if (cmpmat.isnewitem)
            {
                buttonUpdate.Text = def_mat_panel.UpdateText_new;
                buttonCancel.Text = def_mat_panel.CancelText_new;
                buttonCancel.Enabled = false;
                buttonDelete.Text = def_mat_panel.DeleteText_new;
            }
            else
            {
                buttonUpdate.Text = def_mat_panel.UpdateText;
                buttonCancel.Text = def_mat_panel.CancelText;
                buttonCancel.Enabled = true;
                buttonDelete.Text = def_mat_panel.DeleteText;
            }
            checkBoxAutoConvert.Text = def_mat_panel.AutoConvertText;
            Name = mat.currname;
            events.MaterialPanelChange += new SelectMaterialPanelEventHandler(events_MaterialPanelChange);
            checkBoxAutoConvert.Checked = mat.AutoConvert;
        }
        #endregion

        #region Event Handlers
        internal void events_MaterialPanelChange(object sender, EventArgs e)
        {
            if (sender == this)
            {
                labelMaterial.ForeColor = System.Drawing.SystemColors.HighlightText;
                labelMaterial.BackColor = System.Drawing.SystemColors.Highlight;
                PnsStudio.MatTree.Hide();
                PnsStudio.MatTree.SelectedNode = PnsStudio.FindMatNode(cmpmat.currname);
                PnsStudio.MatTree.Show();
            }
            else
            {
                labelMaterial.ForeColor = System.Drawing.SystemColors.ControlText;
                labelMaterial.BackColor = System.Drawing.SystemColors.Control;
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            PnsStudio.MatPanelEnsureVisible(this); 
            MaterialProperties t_matprop = (MaterialProperties)propertyGridMaterial.SelectedObject;
            t_matprop.UpdateFields();
            if (t_matprop.Validate()) events.OnMatPropChange(this, new MaterialPropertyEventArgs(t_matprop, defaults.MatPropButtons.update));
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            PnsStudio.MatPanelEnsureVisible(this);
            events.OnMatPropChange(this, new MaterialPropertyEventArgs((MaterialProperties)propertyGridMaterial.SelectedObject, defaults.MatPropButtons.cancel));
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            PnsStudio.MatPanelEnsureVisible(this);
            events.OnMatPropChange(this, new MaterialPropertyEventArgs((MaterialProperties)propertyGridMaterial.SelectedObject, defaults.MatPropButtons.delete));
        }
        private void propertyGridMaterial_Leave(object sender, EventArgs e)
        {
            if (MatPropGrid == sender)
            {
                labelMaterial.ForeColor = System.Drawing.SystemColors.HighlightText;
                labelMaterial.BackColor = System.Drawing.SystemColors.ControlDark;
            }
        }
        private void propertyGridMaterial_Enter(object sender, EventArgs e) { events.OnMaterialPanelChange(this, e); }
        private void propertyGridMaterial_Click(object sender, EventArgs e) { PnsStudio.MatPanelEnsureVisible(this); }
        private void labelMaterial_Click(object sender, EventArgs e) { PnsStudio.MatPanelEnsureVisible(this); }
        private void MaterialPanel_Click(object sender, EventArgs e) { PnsStudio.MatPanelEnsureVisible(this); }
        private void tableLayoutPanelMaterial_Click(object sender, EventArgs e) { PnsStudio.MatPanelEnsureVisible(this); }
        private void propertyGridMaterial_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MaterialProperties t_matprop = (MaterialProperties)propertyGridMaterial.SelectedObject;
            t_matprop.UpdateFields();
            if (e.ChangedItem.Value.GetType() == typeof(MUCategory)) t_matprop.UpdateMUs();
            t_matprop.Refresh();
            MatPropGrid.Refresh();
        }
        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
            PnsStudio.MatPanelEnsureVisible(this); 
            ((MaterialProperties)propertyGridMaterial.SelectedObject).AutoConvert = checkBoxAutoConvert.Checked;
        }
        #endregion

        #region Properties
        public defaults.MatTypes cmp_mattype { get { return cmpmat.type; } }
        public string cmp_currname { set { cmpmat.currname = value; } }
        public bool isModified { get { return cmpmat.new_or_modified((MaterialProperties)propertyGridMaterial.SelectedObject); } }
        public string labelText
        {
            set
            {
                labelMaterial.Text = value + def_mat_panel.title_p1;
                if (cmpmat.isnewitem) labelMaterial.Text += def_mat_panel.title_p2;
            }
        }
        public Label MatPropLabel { get { return labelMaterial; } }
        public PropertyGrid MatPropGrid { get { return propertyGridMaterial; } }
        public Button MatPropUpdate { get { return buttonUpdate; } }
        #endregion
    }

    public partial class MatMU
    {
        #region Members
        private FractionMU f_reqFlowMU;
        private FractionMU f_maxFlowMU;
        private FractionMU f_priceMU;
        #endregion

        #region Constructors
        public MatMU(MUCategory t_cat)
        {
            f_reqFlowMU = t_cat.DefaultFlowMU;
            f_maxFlowMU = t_cat.DefaultFlowMU;
            f_priceMU = t_cat.DefaultPriceMU;
        }
        public MatMU(MatMU matmu)
        {
            f_reqFlowMU = matmu.f_reqFlowMU;
            f_maxFlowMU = matmu.f_maxFlowMU;
            f_priceMU = matmu.f_priceMU;
        }
        public MatMU(XMLMatMU matmu, MUCategory t_cat)
        {
            MU t_numerator;
            MU t_denominator;
            if (matmu.reqFlowMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(matmu.reqFlowMU.quantity_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(matmu.reqFlowMU.time_mu);
                f_reqFlowMU = t_cat.FlowMUs.Find(t_numerator, t_denominator);
            }
            else f_reqFlowMU = t_cat.DefaultFlowMU;
            if (matmu.maxFlowMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(matmu.maxFlowMU.quantity_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(matmu.maxFlowMU.time_mu);
                f_maxFlowMU = t_cat.FlowMUs.Find(t_numerator, t_denominator);
            }
            else f_maxFlowMU = t_cat.DefaultFlowMU;
            if (matmu.priceMU != null)
            {
                t_numerator = DefaultMUsAndValues.MUs.FindMU(matmu.priceMU.currency_mu);
                t_denominator = DefaultMUsAndValues.MUs.FindMU(matmu.priceMU.quantity_mu);
                f_priceMU = t_cat.PriceMUs.Find(t_numerator, t_denominator);
            }
            else f_priceMU = t_cat.DefaultPriceMU;
        }
        #endregion

        #region Operator overloadings
        public static bool operator ==(MatMU matmu1, MatMU matmu2)
        {
            if (ReferenceEquals(matmu1, null) && ReferenceEquals(matmu2, null)) return true;
            if (ReferenceEquals(matmu1, null) || ReferenceEquals(matmu2, null)) return false;
            if (matmu1.f_reqFlowMU != matmu2.f_reqFlowMU) return false;
            if (matmu1.f_maxFlowMU != matmu2.f_maxFlowMU) return false;
            if (matmu1.f_priceMU != matmu2.f_priceMU) return false;
            return true;
        }
        public static bool operator !=(MatMU matmu1, MatMU matmu2)
        {
            return !(matmu1 == matmu2);
        }
        public override bool Equals(object obj)
        {
            if (obj is MatMU) return this == (MatMU)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Member functions
        public XMLMatMU ToXMLMatMU(MUCategory t_cat)
        {
            if (f_reqFlowMU == t_cat.DefaultFlowMU && f_maxFlowMU == t_cat.DefaultFlowMU && f_priceMU == t_cat.DefaultPriceMU) return null;
            XMLMatMU xmlmatmu = new XMLMatMU();
            if (f_reqFlowMU != t_cat.DefaultFlowMU) xmlmatmu.reqFlowMU = f_reqFlowMU.ToXMLFlowMU();
            if (f_maxFlowMU != t_cat.DefaultFlowMU) xmlmatmu.maxFlowMU = f_maxFlowMU.ToXMLFlowMU();
            if (f_priceMU != t_cat.DefaultPriceMU) xmlmatmu.priceMU = f_priceMU.ToXMLPriceMU();
            return xmlmatmu;
        }
        #endregion

        #region Properties
        public FractionMU minmu { get { return f_reqFlowMU; } set { f_reqFlowMU = value; } }
        public FractionMU maxmu { get { return f_maxFlowMU; } set { f_maxFlowMU = value; } }
        public FractionMU pricemu { get { return f_priceMU; } set { f_priceMU = value; } }
        #endregion
    }

    public class MaterialProperties : CustomClass
    {
        #region Members
        private bool f_isnewitem;
        private Layout f_layout;
        private string f_currname;
        private string f_name;
        private double f_minflow;
        private double f_maxflow;
        private double f_price;
        private double f_penalty;
        private MatMU f_matmu;
        private defaults.MatTypes f_mattype;
        private MUCategory f_mat_category;
        private bool f_autoconvert;
        private DescriptionStr f_description;
        #endregion

        #region Constructors
        public MaterialProperties(string name, defaults.MatTypes type)
        {
            f_mat_category = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU);
            f_layout = null;
            f_minflow = def_Values.d_NperA;
            f_maxflow = def_Values.d_NperA;
            f_price = def_Values.d_NperA;
            f_matmu = new MatMU(f_mat_category);
            f_name = f_currname = name;
            f_mattype = type;
            f_isnewitem = true;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public MaterialProperties(RawMaterial mat)
        {
            if (mat.category != def_Values.d_NperA) f_mat_category = DefaultMUsAndValues.MUs.Categories.Find(mat.category);
            else f_mat_category = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU);
            f_layout = mat.layout;
            if (mat.min != DefaultMUsAndValues.DefaultValues.d_minimum_flow) f_minflow = mat.min;
            else f_minflow = def_Values.d_NperA;
            if (mat.max != DefaultMUsAndValues.DefaultValues.d_maximum_flow) f_maxflow = mat.max;
            else f_maxflow = def_Values.d_NperA;
            if (mat.price != DefaultMUsAndValues.DefaultValues.d_price) f_price = mat.price;
            else f_price = def_Values.d_NperA;
            if (mat.matMU != null) f_matmu = new MatMU(mat.matMU, f_mat_category);
            else f_matmu = new MatMU(f_mat_category);
            f_name = f_currname = mat.name;
            f_mattype = defaults.MatTypes.raw;
            f_isnewitem = false;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            if (mat.description != null) f_description = new DescriptionStr(mat.description);
            else f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public MaterialProperties(IntermediateMaterial mat)
        {
            if (mat.category != def_Values.d_NperA) f_mat_category = DefaultMUsAndValues.MUs.Categories.Find(mat.category);
            else f_mat_category = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU);
            f_layout = mat.layout;
            if (mat.penaltySpecified) f_penalty = mat.penalty;
            else f_penalty = def_Values.d_NperA;
            if (mat.max != DefaultMUsAndValues.DefaultValues.d_maximum_flow) f_maxflow = mat.max;
            else f_maxflow = def_Values.d_NperA;
            if (mat.matMU != null) f_matmu = new MatMU(mat.matMU, f_mat_category);
            else f_matmu = new MatMU(f_mat_category);
            f_name = f_currname = mat.name;
            f_mattype = defaults.MatTypes.intermediate;
            f_isnewitem = false;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            if (mat.description != null) f_description = new DescriptionStr(mat.description);
            else f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public MaterialProperties(ProductMaterial mat)
        {
            if (mat.category != def_Values.d_NperA) f_mat_category = DefaultMUsAndValues.MUs.Categories.Find(mat.category);
            else f_mat_category = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(DefaultMUsAndValues.MUs.DefaultMaterialMU);
            f_layout = mat.layout;
            if (mat.min != DefaultMUsAndValues.DefaultValues.d_minimum_flow) f_minflow = mat.min;
            else f_minflow = def_Values.d_NperA;
            if (mat.max != DefaultMUsAndValues.DefaultValues.d_maximum_flow) f_maxflow = mat.max;
            else f_maxflow = def_Values.d_NperA;
            if (mat.price != DefaultMUsAndValues.DefaultValues.d_price) f_price = mat.price;
            else f_price = def_Values.d_NperA;
            if (mat.matMU != null) f_matmu = new MatMU(mat.matMU, f_mat_category);
            else f_matmu = new MatMU(f_mat_category);
            f_name = f_currname = mat.name;
            f_mattype = defaults.MatTypes.product;
            f_isnewitem = false;
            f_autoconvert = DefaultMUsAndValues.MUs.AutoConvert;
            if (mat.description != null) f_description = new DescriptionStr(mat.description);
            else f_description = new DescriptionStr(def_mat_prop.D_description);
            AddProperties();
        }
        public MaterialProperties(MaterialProperties mat)
        {
            f_mat_category = mat.f_mat_category;
            f_layout = mat.f_layout;
            f_minflow = mat.f_minflow;
            f_maxflow = mat.f_maxflow;
            f_price = mat.f_price;
            f_matmu = new MatMU(mat.f_matmu);
            f_name = mat.f_name;
            f_currname = mat.f_currname;
            f_mattype = mat.f_mattype;
            f_isnewitem = mat.f_isnewitem;
            f_autoconvert = mat.f_autoconvert;
            f_description = new DescriptionStr(mat.f_description.description);
            AddProperties();
        }
        #endregion

        #region Member functions
        private void AddProperties()
        {
            Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_name, name, def_mat_prop.D_name, def_mat_prop.cat1, name.GetType(), false, true));
            Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_typename, typename, def_mat_prop.D_typename, def_mat_prop.cat1, typename.GetType(), true, true));
            Add(new CustomProp(this.GetType(), DefaultMUsAndValues.MUs.Categories, def_mat_prop.DN_quantity, material_category, def_mat_prop.D_quantity, def_mat_prop.cat1, material_category.GetType(), false, true));
            if (f_mattype == defaults.MatTypes.raw)
            {
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_raw_min, min, def_mat_prop.D_raw_min, def_mat_prop.cat1, min.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.FlowMUs, def_mat_prop.DN_raw_minmu, MinMU, def_mat_prop.D_raw_minmu, def_mat_prop.cat1, MinMU.GetType(), false, true));
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_raw_max, max, def_mat_prop.D_raw_max, def_mat_prop.cat1, max.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.FlowMUs, def_mat_prop.DN_raw_maxmu, MaxMU, def_mat_prop.D_raw_maxmu, def_mat_prop.cat1, MaxMU.GetType(), false, true));
            }
            else if (f_mattype == defaults.MatTypes.product)
            {
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_product_min, min, def_mat_prop.D_product_min, def_mat_prop.cat1, min.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.FlowMUs, def_mat_prop.DN_product_minmu, MinMU, def_mat_prop.D_product_minmu, def_mat_prop.cat1, MinMU.GetType(), false, true));
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_product_max, max, def_mat_prop.D_product_max, def_mat_prop.cat1, max.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.FlowMUs, def_mat_prop.DN_product_maxmu, MaxMU, def_mat_prop.D_product_maxmu, def_mat_prop.cat1, MaxMU.GetType(), false, true));
            }
            else
            {
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_intermediate_max, max, def_mat_prop.D_intermediate_max, def_mat_prop.cat1, max.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.FlowMUs, def_mat_prop.DN_intermediate_maxmu, MaxMU, def_mat_prop.D_intermediate_maxmu, def_mat_prop.cat1, MaxMU.GetType(), false, true));
            }
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product)
            {
                Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_price, price, def_mat_prop.D_price, def_mat_prop.cat1, price.GetType(), false, true));
                Add(new CustomProp(this.GetType(), material_category.PriceMUs, def_mat_prop.DN_pricemu, PriceMU, def_mat_prop.D_pricemu, def_mat_prop.cat1, PriceMU.GetType(), false, true));
            }
            Add(new CustomProp(this.GetType(), null, def_mat_prop.DN_description, description, def_mat_prop.D_description, def_mat_prop.cat1, description.GetType(), false, true));
        }
        public void UpdateFields()
        {
            if (List.Count > 0) name = (string)((CustomProp)List[0]).Value;
            if (List.Count > 2) material_category = (MUCategory)((CustomProp)List[2]).Value;
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product)
            {
                if (List.Count > 3) min = (string)((CustomProp)List[3]).Value;
                if (List.Count > 4) MinMU = (FractionMU)((CustomProp)List[4]).Value;
                if (List.Count > 5) max = (string)((CustomProp)List[5]).Value;
                if (List.Count > 6) MaxMU = (FractionMU)((CustomProp)List[6]).Value;
                if (List.Count > 7) price = (string)((CustomProp)List[7]).Value;
                if (List.Count > 8) PriceMU = (FractionMU)((CustomProp)List[8]).Value;
                if (List.Count > 9) description = (DescriptionStr)((CustomProp)List[9]).Value;
            }
            else
            {
                if (List.Count > 3) max = (string)((CustomProp)List[3]).Value;
                if (List.Count > 4) MaxMU = (FractionMU)((CustomProp)List[4]).Value;
                if (List.Count > 5) description = (DescriptionStr)((CustomProp)List[5]).Value;
            }

        }
        public void Refresh()
        {
            if (List.Count > 0) ((CustomProp)List[0]).Value = name;
            if (List.Count > 1) ((CustomProp)List[1]).Value = typename;
            if (List.Count > 2) ((CustomProp)List[2]).Value = material_category;
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product)
            {
                if (List.Count > 3) ((CustomProp)List[3]).Value = min;
                if (List.Count > 4)
                {
                    ((CustomProp)List[4]).Value = MinMU;
                    ((CustomProp)List[4]).SelectList = material_category.FlowMUs;
                }

                if (List.Count > 5) ((CustomProp)List[5]).Value = max;
                if (List.Count > 6)
                {
                    ((CustomProp)List[6]).Value = MaxMU;
                    ((CustomProp)List[6]).SelectList = material_category.FlowMUs;
                }
                if (List.Count > 7) ((CustomProp)List[7]).Value = price;
                if (List.Count > 8)
                {
                    ((CustomProp)List[8]).Value = PriceMU;
                    ((CustomProp)List[8]).SelectList = material_category.PriceMUs;
                }
                if (List.Count > 9) ((CustomProp)List[9]).Value = description;
            }
            else
            {
                if (List.Count > 3) ((CustomProp)List[3]).Value = max;
                if (List.Count > 4)
                {
                    ((CustomProp)List[4]).Value = MaxMU;
                    ((CustomProp)List[4]).SelectList = material_category.FlowMUs;
                }
                if (List.Count > 5) ((CustomProp)List[5]).Value = description;
            }
        }
        public bool Validate()
        {
            if (dmin > dmax)
            {
                MessageBox.Show(f_mattype == defaults.MatTypes.raw ? Msg_raw_minflow_maxflow : 
                    f_mattype == defaults.MatTypes.product ? Msg_product_minflow_maxflow : 
                    Msg_intermediate_minflow_maxflow);
                return false;
            }
            return true;
        }
        public void BuildTree(TreeNode t_node) { }
        public bool new_or_modified(MaterialProperties mat)
        {
            if (f_isnewitem) return true;
            if (f_layout != mat.f_layout) return true;
            if (f_currname != mat.f_currname) return true;
            if (f_name != mat.f_name) return true;
            if (f_minflow != mat.f_minflow) return true;
            if (f_maxflow != mat.f_maxflow) return true;
            if (f_price != mat.f_price) return true;
            if (f_penalty != mat.f_penalty) return true;
            if (f_matmu != mat.f_matmu) return true;
            if (f_mattype != mat.f_mattype) return true;
            if (f_mat_category != mat.f_mat_category) return true;
            if (f_autoconvert != mat.f_autoconvert) return true;
            if (f_description != mat.f_description) return true;
            return false;
        }
        public RawMaterial rToXML()
        {
            RawMaterial xmlmat = new RawMaterial();
            xmlmat.layout = f_layout;
            xmlmat.name = name;
            xmlmat.category = f_mat_category.CategoryID;
            xmlmat.min = f_minflow;
            //            if (MinMU != f_mat_category.DefaultFlowMU) 
            xmlmat.requiredFlowMu = MinMU.ToString();
            xmlmat.max = f_maxflow;
            //            if (MaxMU != f_mat_category.DefaultFlowMU) 
            xmlmat.maximumFlowMu = MaxMU.ToString();
            xmlmat.price = f_price;
            //            if (PriceMU != f_mat_category.DefaultPriceMU) 
            xmlmat.priceMu = PriceMU.ToString();
            xmlmat.matMU = f_matmu.ToXMLMatMU(f_mat_category);
            xmlmat.description = f_description.description;
            return xmlmat;
        }
        public IntermediateMaterial iToXML()
        {
            IntermediateMaterial xmlmat = new IntermediateMaterial();
            xmlmat.layout = f_layout;
            xmlmat.penalty = f_penalty;
            xmlmat.penaltySpecified = xmlmat.penalty > 0;
            xmlmat.name = name;
            xmlmat.category = f_mat_category.CategoryID;
            //            if (MinMU != f_mat_category.DefaultFlowMU) 
            xmlmat.requiredFlowMu = MinMU.ToString();
            xmlmat.max = f_maxflow;
            //            if (MaxMU != f_mat_category.DefaultFlowMU) 
            xmlmat.maximumFlowMu = MaxMU.ToString();
            xmlmat.matMU = f_matmu.ToXMLMatMU(f_mat_category);
            xmlmat.description = f_description.description;
            return xmlmat;
        }
        public ProductMaterial pToXML()
        {
            ProductMaterial xmlmat = new ProductMaterial();
            xmlmat.layout = f_layout;
            xmlmat.name = name;
            xmlmat.category = f_mat_category.CategoryID;
            xmlmat.min = f_minflow;
            //            if (MinMU != f_mat_category.DefaultFlowMU) 
            xmlmat.requiredFlowMu = MinMU.ToString();
            xmlmat.max = f_maxflow;
            //            if (MaxMU != f_mat_category.DefaultFlowMU) 
            xmlmat.maximumFlowMu = MaxMU.ToString();
            xmlmat.price = f_price;
            //            if (PriceMU != f_mat_category.DefaultPriceMU) 
            xmlmat.priceMu = PriceMU.ToString();
            xmlmat.matMU = f_matmu.ToXMLMatMU(f_mat_category);
            xmlmat.description = f_description.description;
            return xmlmat;
        }
        public void UpdateMUs()
        {
            f_matmu = new MatMU(f_mat_category);
            //            f_minflow = def_Values.d_NperA;
            //            f_maxflow = def_Values.d_NperA;
            //            f_price = def_Values.d_NperA;
        }
        public void UpdateAllValues()
        {
            FractionMU t_new_mu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_matmu.minmu.Numerator).DefaultFlowMU;
            MU t_numerator = null;
            MU t_denominator = null;
            foreach (MU item in DefaultMUsAndValues.MUs.OldDefaultMUs)
            {
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator)) t_numerator = item;
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Denominator)) t_denominator = item;
                if (t_numerator != null && t_denominator != null) break;
            }
            FractionMU t_old_mu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator).FlowMUs.Find(t_numerator, t_denominator);
            if (f_minflow != def_Values.d_NperA) f_minflow *= DefaultMUsAndValues.ConvertMU(t_old_mu, t_new_mu, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
            if (f_maxflow != def_Values.d_NperA) f_maxflow *= DefaultMUsAndValues.ConvertMU(t_old_mu, t_new_mu, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
            t_new_mu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator).DefaultPriceMU;
            t_numerator = null;
            t_denominator = null;
            foreach (MU item in DefaultMUsAndValues.MUs.OldDefaultMUs)
            {
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(PriceMU.Numerator)) t_numerator = item;
                if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(item) == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(PriceMU.Denominator)) t_denominator = item;
                if (t_numerator != null && t_denominator != null) break;
            }
            t_old_mu = DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator).PriceMUs.Find(t_numerator, t_denominator);
            if (f_price != def_Values.d_NperA) f_price *= DefaultMUsAndValues.ConvertMU(t_old_mu, t_new_mu, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
        }
        public override string ToString()
        {
            string t_ret = def_mat_prop.DN_name + ": " + name +
            "\n\n" + def_mat_prop.DN_typename + ": " + typename +
            "\n" + def_mat_prop.DN_quantity + ": " + material_category;
            if (f_mattype == defaults.MatTypes.raw)
            {
                t_ret += "\n\n" + def_mat_prop.DN_raw_min + ": " + min +
                "\n" + def_mat_prop.DN_raw_max + ": " + max;
            }
            else if (f_mattype == defaults.MatTypes.product)
            {
                t_ret += "\n\n" + def_mat_prop.DN_product_min + ": " + min +
                "\n" + def_mat_prop.DN_product_max + ": " + max;
            }
            else
            {
                t_ret += "\n\n" + def_mat_prop.DN_intermediate_max + ": " + max;
            }
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product)
            {
                t_ret += "\n\n" + def_mat_prop.DN_price + ": " + price;
            }
            t_ret += "\n\n" + def_mat_prop.DN_description + ": " + description;
            return t_ret;
        }
        public string GenOutput(ref double t_amount)
        {
            string t_ret = name + ": ";
            if (t_amount < 0) t_ret += def_Solution_Tree.text_consumed + " = " + -t_amount + " " + DefaultFlowMU;
            else if (t_amount > 0) t_ret += def_Solution_Tree.text_produced + " = " + t_amount + " " + DefaultFlowMU;
            else t_ret += def_Solution_Tree.text_balanced;
            double t_cost=-dprice * t_amount;
            if ((f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product) && t_amount != 0) t_ret += ", " + def_Solution_Tree.text_cost + ": " + t_cost + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
            else t_cost = 0;
            t_ret += "\n   - " + def_mat_prop.DN_typename + ": " + typename + "\n   - " + def_mat_prop.DN_quantity + ": " + material_category + MinMaxInfo != "" ? "\n   - " + MinMaxInfo : "";
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product) t_ret += "\n   - " + def_mat_prop.DN_price + ": " + dprice + " " + DefaultPriceMU;
            t_ret += "\n   - " + def_mat_prop.DN_description + ": " + description; 
            t_ret += "\n\n";
            t_amount = t_cost;
            return t_ret;
        }
        public double GenTreeOutput(double t_amount, TreeNode t_node)
        {
            string t_ret = name + ": ";
            if (t_amount < 0) t_ret += def_Solution_Tree.text_consumed + " = " + -t_amount + " " + DefaultFlowMU;
            else if (t_amount > 0) t_ret += def_Solution_Tree.text_produced + " = " + t_amount + " " + DefaultFlowMU;
            else t_ret += def_Solution_Tree.text_balanced;
            double t_cost = -dprice * t_amount;
            if ((f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product) && t_amount != 0) t_ret += ", " + def_Solution_Tree.text_cost + ": " + t_cost.ToString() + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
            else t_cost = 0;
            t_node = t_node.Nodes.Add(t_ret);
            t_node.Nodes.Add(def_mat_prop.DN_typename + ": " + typename);
            t_node.Nodes.Add(def_mat_prop.DN_quantity + ": " + material_category);
            if (MinMaxInfo != "") t_node.Nodes.Add(MinMaxInfo);
            if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product) t_node.Nodes.Add(def_mat_prop.DN_price + ": " + dprice + " " + DefaultPriceMU);
            t_node.Nodes.Add(def_mat_prop.DN_description + ": " + description); 
            return t_cost;
        }
        #endregion

        #region Properties
        public string currname { get { return f_currname; } set { f_currname = value; } }
        public string name { get { return f_name; } set { f_name = Converters.ToNameString(value); } }
        public string typename
        {
            get
            {
                switch (f_mattype)
                {
                    case defaults.MatTypes.raw: return def_mat_prop.V_typename_r;
                    case defaults.MatTypes.intermediate: return def_mat_prop.V_typename_i;
                    case defaults.MatTypes.product: return def_mat_prop.V_typename_p;
                    default: throw new Exception(def_mat_ex.Ex_unknown_material_type);
                }
            }
        }
        public MUCategory material_category { get { return f_mat_category; } set { f_mat_category = value; } }
        public string min
        {
            get { return lmin.ToString() + " " + MinMU.ToString() + ((f_minflow == def_Values.d_NperA) ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub / MinToDefault) * MinToDefault;
                if (!Converters.IsDoubleEqual(t_value, DefaultMUsAndValues.DefaultValues.d_minimum_flow)) f_minflow = t_value;
                else f_minflow = def_Values.d_NperA;
            }
        }
        public double lmin
        {
            get
            {
                if (f_minflow != def_Values.d_NperA) return f_minflow / MinToDefault;
                return DefaultMUsAndValues.DefaultValues.d_minimum_flow / MinToDefault;
            }
        }
        public double dmin
        {
            get
            {
                if (f_minflow != def_Values.d_NperA) return f_minflow;
                return DefaultMUsAndValues.DefaultValues.d_minimum_flow;
            }
        }
        public double gmin { get { return f_minflow; } }
        private double MinToDefault { get { return DefaultMUsAndValues.ConvertMU(MinMU, DefaultFlowMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod); } }
        public FractionMU MinMU
        {
            get { return f_matmu.minmu; }
            set
            {
                if (f_minflow != def_Values.d_NperA && !AutoConvert)
                {
                    f_minflow *= DefaultMUsAndValues.ConvertMU(value, MinMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
                }
                f_matmu.minmu = value;
            }
        }
        public string max
        {
            get { return lmax.ToString() + " " + MaxMU.ToString() + ((f_maxflow == def_Values.d_NperA) ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(0, value, DefaultMUsAndValues.DefaultValues.d_solver_ub / MaxToDefault) * MaxToDefault;
                if (!Converters.IsDoubleEqual(t_value, DefaultMUsAndValues.DefaultValues.d_maximum_flow)) f_maxflow = t_value;
                else f_maxflow = def_Values.d_NperA;
            }
        }
        public double lmax
        {
            get
            {
                if (f_maxflow != def_Values.d_NperA) return f_maxflow / MaxToDefault;
                return DefaultMUsAndValues.DefaultValues.d_maximum_flow / MaxToDefault;
            }
        }
        public double dmax
        {
            get
            {
                if (f_maxflow != def_Values.d_NperA) return f_maxflow;
                return DefaultMUsAndValues.DefaultValues.d_maximum_flow;
            }
        }
        public double gmax { get { return f_maxflow; } }
        private double MaxToDefault { get { return DefaultMUsAndValues.ConvertMU(MaxMU, DefaultFlowMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod); } }
        public FractionMU MaxMU
        {
            get { return f_matmu.maxmu; }
            set
            {
                if (f_maxflow != def_Values.d_NperA && !AutoConvert)
                {
                    f_maxflow *= DefaultMUsAndValues.ConvertMU(value, MaxMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
                }
                f_matmu.maxmu = value;
            }
        }
        public string price
        {
            get { return lprice.ToString() + " " + PriceMU.ToString() + ((f_price == def_Values.d_NperA) ? " " + def_PnsStudio.NperA : ""); }
            set
            {
                double t_value = Converters.ToDouble(-DefaultMUsAndValues.DefaultValues.d_solver_ub / PriceToDefault, value, DefaultMUsAndValues.DefaultValues.d_solver_ub / PriceToDefault) * PriceToDefault;
                if (!Converters.IsDoubleEqual(t_value, DefaultMUsAndValues.DefaultValues.d_price)) f_price = t_value;
                else f_price = def_Values.d_NperA;
            }
        }
        public double lprice
        {
            get
            {
                if (f_price != def_Values.d_NperA) return f_price / PriceToDefault;
                return DefaultMUsAndValues.DefaultValues.d_price / PriceToDefault;
            }
        }
        public double dprice
        {
            get
            {
                if (f_price != def_Values.d_NperA) return f_price;
                return DefaultMUsAndValues.DefaultValues.d_price;
            }
        }
        public double gprice { get { return f_price; } }
        private double PriceToDefault { get { return DefaultMUsAndValues.ConvertMU(PriceMU, DefaultPriceMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod); } }
        public FractionMU PriceMU
        {
            get { return f_matmu.pricemu; }
            set
            {
                if (f_price != def_Values.d_NperA && !AutoConvert)
                {
                    f_price *= DefaultMUsAndValues.ConvertMU(value, PriceMU, DefaultMUsAndValues.MUs.DefaultWorkingHoursPerYear, DefaultMUsAndValues.MUs.DefaultPayoutPeriod);
                }
                f_matmu.pricemu = value;
            }
        }
        public int typeindex { get { return (int)f_mattype; } }
        public defaults.MatTypes type 
        { 
            get { return f_mattype; } 
            set 
            {
                if (f_mattype != value) 
                { 
                    f_mattype = value;
                    List.Clear();
                    AddProperties();
                }
            } 
        }
        public bool isnewitem { get { return f_isnewitem; } set { f_isnewitem = value; } }
        public bool AutoConvert { get { return f_autoconvert; } set { f_autoconvert = value; } }
        public DescriptionStr description { get { return f_description; } set { f_description = value; } }
        public FractionMU DefaultFlowMU { get { return DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator).DefaultFlowMU; } }
        public FractionMU DefaultPriceMU { get { return DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(MinMU.Numerator).DefaultPriceMU; } }
        public string MinMaxInfo
        {
            get
            {
                if (f_mattype == defaults.MatTypes.raw || f_mattype == defaults.MatTypes.product) return "Min - Max: [" + dmin + " - " + dmax + " " + DefaultFlowMU + "]";
                return "";
            }
        }

        public string Msg_raw_minflow_maxflow { get { return def_mat_prop.DN_raw_min + def_PnsStudio.Msg_must_be_less_or_equal_than + def_mat_prop.DN_raw_max; } }
        public string Msg_intermediate_minflow_maxflow { get { return def_mat_prop.DN_intermediate_min + def_PnsStudio.Msg_must_be_less_or_equal_than + def_mat_prop.DN_intermediate_max; } }
        public string Msg_product_minflow_maxflow { get { return def_mat_prop.DN_product_min + def_PnsStudio.Msg_must_be_less_or_equal_than + def_mat_prop.DN_product_max; } }
        #endregion
    }
}
