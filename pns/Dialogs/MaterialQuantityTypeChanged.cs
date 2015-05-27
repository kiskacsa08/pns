using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Pns.Xml_Serialization.PnsGUI.Dialogs;
using Pns.Globals;


namespace Pns.Dialogs
{
    public partial class MaterialQuantityTypeChanged : Form
    {
        #region Members
        private defaults.ChangeCategoryChoise m_MUchoise;
        #endregion

        #region Constructors
        public MaterialQuantityTypeChanged(MaterialProperties t_matprop)
        {
            InitializeComponent();
            Text = def_MaterialQuantityTypeChanged.title;
            buttonOK.Text = def_MaterialQuantityTypeChanged.OkText;
            buttonCancel.Text = def_MaterialQuantityTypeChanged.CancelText;
            groupBoxFlowMUSettings.Text = def_MaterialQuantityTypeChanged.GroupBoxMUSettings;
            radioButtonMaterialMU.Text = def_MaterialQuantityTypeChanged.RadioButtonMaterialMU;
            radioButtonDefaultMU.Text = def_MaterialQuantityTypeChanged.RadioButtonDefMU;
            groupBoxFlowValueSettings.Text = def_MaterialQuantityTypeChanged.GroupBoxValueSettings;
            radioButtonSetToUserActionNeededState.Text = def_MaterialQuantityTypeChanged.RadioButtonActionNeeded;
            radioButtonKeepCurrentValue.Text = def_MaterialQuantityTypeChanged.RadioButtonKeepCurrent;
            radioButtonSetToDefaultFlowRate.Text = def_MaterialQuantityTypeChanged.RadioButtonDefFlow;
            if (t_matprop.MinMU != null || t_matprop.MaxMU != null)
            {
                radioButtonMaterialMU.Checked = true;
                if (t_matprop.MinMU != null)
                {
                    radioButtonMaterialMU.Text = def_MaterialQuantityTypeChanged.RadioButtonMinMU;
                    m_MUchoise = defaults.ChangeCategoryChoise.min_mu;
                }
                else
                {
                    radioButtonMaterialMU.Text = def_MaterialQuantityTypeChanged.RadioButtonMaxMU;
                    m_MUchoise = defaults.ChangeCategoryChoise.max_mu;
                }
            }
            else
            {
                radioButtonMaterialMU.Enabled = false;
                radioButtonDefaultMU.Checked = true;
                m_MUchoise = defaults.ChangeCategoryChoise.def_mu;
            }
            radioButtonSetToUserActionNeededState.Enabled = false;
            radioButtonKeepCurrentValue.Checked = true;
        }
        #endregion

        #region Event Handlers
        private void buttonOK_Click(object sender, EventArgs e) { Close(); }
        #endregion

        #region Properties
        public int Choise
        {
            get
            {
                int t_choise = 0;
                if (radioButtonMaterialMU.Checked) t_choise |= 1 << (int)m_MUchoise;
                if (radioButtonDefaultMU.Checked) t_choise |= 1 << (int)defaults.ChangeCategoryChoise.def_mu;
                if (radioButtonSetToUserActionNeededState.Checked) t_choise |= 1 << (int)defaults.ChangeCategoryChoise.action_needed;
                if (radioButtonKeepCurrentValue.Checked) t_choise |= 1 << (int)defaults.ChangeCategoryChoise.keep_current;
                if (radioButtonSetToDefaultFlowRate.Checked) t_choise |= 1 << (int)defaults.ChangeCategoryChoise.default_flowrate;
                return t_choise;
            }
        }
        #endregion
    }
}
