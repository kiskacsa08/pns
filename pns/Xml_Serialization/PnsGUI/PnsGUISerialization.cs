using System.Xml.Serialization;

using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI;
using Pns.PnsSolver;
using Pns.Xml_Serialization.PnsGUI.Dialogs;
using Pns.Xml_Serialization.ExcelExport;
using Pns.Xml_Serialization.PnsGUI.Dialogs.DefValues;
using Pns.Xml_Serialization.PnsGUI.Dialogs.DefMUs;
using Pns.Xml_Serialization.PnsGUI.MessageBoxes;
using Pns.Xml_Serialization.PnsGUI.ExceptionMsgs;
using Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial;
using Pns.Xml_Serialization.PnsGUI.Dialogs.OpUnit;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns.Xml_Serialization.PnsGUI
{
    public class PnsUserInterfaceTexts
    {
        #region Members
        def_PnsStudio defpnsstudio;
        def_mat_msg defmatmsg;
        def_mat_ex defmatex;
        def_mat_tree defmattree;
        def_mat_prop matpropfield;
        def_mat_panel defmatpanel;
        def_mat_delete_confirm_msg defmatpanelmsg;
        def_ou_msg defoumsg;
        def_ou_ex defouex;
        def_ou_tree defoutree;
        def_iomat_prop defiomatprop;
        def_ou_prop oupropfield;
        def_ou_panel defoupanel;
        def_ou_delete_confirm_msg defoupanelmsg;
        def_MeasurementUnitsPropGrid defmusprop;
        def_DefaultMU defaultmusdialog;
        def_DefaultValuesPropGrid defvalprop;
        def_DefaultValuesDlg defaultvaluesdialog;
        def_Convert_msg defconvertmsg;
        def_MaterialQuantityTypeChanged defquantitychanged;
        def_Solutions defsolutions;
        def_Solution_Tree defsolutiontree;
        def_Solution_Excel defsolutionexcel;
        def_Problem_Excel defproblemexcel;
        #endregion

        #region Constructors
        public PnsUserInterfaceTexts()
        {
            defpnsstudio = new def_PnsStudio();
            defmatmsg = new def_mat_msg();
            defmatex = new def_mat_ex();
            defmattree = new def_mat_tree();
            matpropfield = new def_mat_prop();
            defmatpanel = new def_mat_panel();
            defmatpanelmsg = new def_mat_delete_confirm_msg();
            defoumsg = new def_ou_msg();
            defouex = new def_ou_ex();
            defoutree = new def_ou_tree();
            defiomatprop = new def_iomat_prop();
            oupropfield = new def_ou_prop();
            defoupanel = new def_ou_panel();
            defoupanelmsg = new def_ou_delete_confirm_msg();
            defmusprop = new def_MeasurementUnitsPropGrid();
            defaultmusdialog = new def_DefaultMU();
            defvalprop = new def_DefaultValuesPropGrid();
            defaultvaluesdialog = new def_DefaultValuesDlg();
            defconvertmsg = new def_Convert_msg();
            defquantitychanged = new def_MaterialQuantityTypeChanged();
            defsolutions = new def_Solutions();
            defsolutiontree = new def_Solution_Tree();
            defsolutionexcel = new def_Solution_Excel();
            defproblemexcel = new def_Problem_Excel();
        }
        #endregion

        #region Properties
        public def_PnsStudio PnsApplication { get { return defpnsstudio; } set { defpnsstudio = value; } }
        public def_mat_msg MaterialsMessages { get { return defmatmsg; } set { defmatmsg = value; } }
        public def_mat_ex MaterialsExeptions { get { return defmatex; } set { defmatex = value; } }
        public def_mat_tree MaterialsTreeTexts { get { return defmattree; } set { defmattree = value; } }
        public def_mat_prop MaterialProperties { get { return matpropfield; } set { matpropfield = value; } }
        public def_mat_panel MaterialPropertiesPanel { get { return defmatpanel; } set { defmatpanel = value; } }
        public def_mat_delete_confirm_msg MaterialPropertiesPanelMessages { get { return defmatpanelmsg; } set { defmatpanelmsg = value; } }
        public def_ou_msg OperatingUnitsMessages { get { return defoumsg; } set { defoumsg = value; } }
        public def_ou_ex OperatingUnitsExeptions { get { return defouex; } set { defouex = value; } }
        public def_ou_tree OperatingUnitsTreeTexts { get { return defoutree; } set { defoutree = value; } }
        public def_iomat_prop FlowProperties { get { return defiomatprop; } set { defiomatprop = value; } }
        public def_ou_prop OperatingUnitProperties { get { return oupropfield; } set { oupropfield = value; } }
        public def_ou_panel OperatingUnitPropertiesPanel { get { return defoupanel; } set { defoupanel = value; } }
        public def_ou_delete_confirm_msg OperatingUnitPropertiesPanelMessages { get { return defoupanelmsg; } set { defoupanelmsg = value; } }
        public def_MeasurementUnitsPropGrid MeasurementUnitsProperties { get { return defmusprop; } set { defmusprop = value; } }
        public def_DefaultMU DefaultMUsDialog { get { return defaultmusdialog; } set { defaultmusdialog = value; } }
        public def_DefaultValuesPropGrid DefaultValuesProperties { get { return defvalprop; } set { defvalprop = value; } }
        public def_DefaultValuesDlg DefaultValuesDialog { get { return defaultvaluesdialog; } set { defaultvaluesdialog = value; } }
        public def_Convert_msg DoubleConverterMessages { get { return defconvertmsg; } set { defconvertmsg = value; } }
        public def_MaterialQuantityTypeChanged MaterialQuantityTypeChanged { get { return defquantitychanged; } set { defquantitychanged = value; } }
        public def_Solutions Solutions { get { return defsolutions; } set { defsolutions = value; } }
        public def_Solution_Tree SolutionTree { get { return defsolutiontree; } set { defsolutiontree = value; } }
        public def_Solution_Excel SolutionExcel { get { return defsolutionexcel; } set { defsolutionexcel = value; } }
        public def_Problem_Excel ProblemExcel { get { return defproblemexcel; } set { defproblemexcel = value; } }
        #endregion
    }
}
