using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Pns;
using Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial;
using Pns.Xml_Serialization.PnsGUI.Dialogs.OpUnit;
using Pns.Dialogs;

namespace DynamicPropertyGrid
{
    public class NoEditor : UITypeEditor
    {
        #region Member functions
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
        { 
            return UITypeEditorEditStyle.None;
        }
        #endregion
    }

    public class DescriptionEditor : UITypeEditor
    {
        #region Member functions
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                Description t_description = new Description();
                if (context.Instance.GetType() == typeof(MaterialProperties))
                {
                    MaterialProperties t_matprop = (MaterialProperties)context.Instance;
                    t_description.Text = def_mat_prop.DN_description + " - " + t_matprop.currname;
                    t_description.richTextBoxDescription.Text = ((DescriptionStr)value).description;
                }
                else if (context.Instance.GetType() == typeof(OperatingUnitProperties))
                {
                    OperatingUnitProperties t_ouprop = (OperatingUnitProperties)context.Instance;
                    t_description.Text = def_ou_prop.DN_description + " - " + t_ouprop.currname;
                    t_description.richTextBoxDescription.Text = ((DescriptionStr)value).description;
                }
                t_description.ShowDialog();
                if (t_description.DialogResult == DialogResult.OK) ((DescriptionStr)value).description = t_description.richTextBoxDescription.Text;
            }
            return value;
        }

        #endregion
    }

    [Editor(typeof(NoEditor), typeof(UITypeEditor))]
    public class CustomClass : CollectionBase, ICustomTypeDescriptor 
    {
        #region Member functions
        public void Add(CustomProp Value)
        {
            base.List.Add(Value);
        }
        public CustomProp FindRemove(string Name, bool t_remove)
        {
            foreach (CustomProp prop in base.List)
            {
                if (prop.Name == Name)
                {
                    if (t_remove) base.List.Remove(prop);
                    return prop;
                }
            }
            return null;
        }
        #endregion

        #region TypeDescriptor Implementation
        public String GetClassName() { return TypeDescriptor.GetClassName(this, true); }
        public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
        public String GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
        public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }
        public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
        public PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
        public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }
        public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
        public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }
        public PropertyDescriptorCollection GetProperties() { return TypeDescriptor.GetProperties(this, true); }
        public object GetPropertyOwner(PropertyDescriptor pd) { return this; }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] newProps = new PropertyDescriptor[Count];
            for (int i = 0; i < Count; i++)
            {
                CustomProp prop = (CustomProp)base.List[i];
                newProps[i] = new CustomPropertyDescriptor(ref prop, attributes);
            }
            return new PropertyDescriptorCollection(newProps);
        }
        #endregion
    }

    public class CustomProp
    {
        #region Members
        private Type f_component_type;
        private object f_select_list;
        private string f_name;
        private object f_value;
        private string f_description;
        private string f_category;
        private Type f_type;
        private bool f_readonly;
        private bool f_visible;
        #endregion

        #region Constructors
        public CustomProp(Type t_component_type, object t_selectlist, string t_name, object t_value, string t_description, string t_category, Type t_type, bool t_readonly, bool t_visible)
        {
            f_component_type = t_component_type;
            f_select_list = t_selectlist;
            f_name = t_name;
            f_value = t_value;
            f_type = t_type;
            f_readonly = t_readonly;
            f_visible = t_visible;
            f_description = t_description;
            f_category = t_category;
        }
        #endregion

        #region Properties
        public Type ComponentType { get { return f_component_type; } }
        public object SelectList { get { return f_select_list; } set { f_select_list = value; } }
        public string Name { get { return f_name; } set { f_name = value; } }
        public object Value { get { return f_value; } set { f_value = value; } }
        public string Description { get { return f_description; } }
        public string Category { get { return f_category; } }
        public Type Type { get { return f_type; } }
        public bool ReadOnly { get { return f_readonly; } }
        public bool Visible { get { return f_visible; } }
        #endregion
    }

    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        #region Members
        CustomProp f_property;
        #endregion

        #region Constructors
        public CustomPropertyDescriptor(ref CustomProp t_property, Attribute[] attrs)
            : base(t_property.Name, attrs)
        {
            f_property = t_property;
        }
        #endregion

        #region PropertyDescriptor specific
        public object SelectList { get { return f_property.SelectList; } }
        public override bool CanResetValue(object component) { return false; }
        public override Type ComponentType { get { return f_property.ComponentType; } }
        public override object GetValue(object component) { return f_property.Value; }
        public override string Description { get { return f_property.Description; } }
        public override string Category { get { return f_property.Category; } }
        public override string DisplayName { get { return f_property.Name; } }
        public override bool IsReadOnly { get { return f_property.ReadOnly; } }
        public override void ResetValue(object component) { }
        public override bool ShouldSerializeValue(object component) { return false; }
        public override void SetValue(object component, object value) { f_property.Value = value; }
        public override Type PropertyType { get { return f_property.Type; } }
        public override object GetEditor(Type editorBaseType)
        {
            if (f_property.Type != typeof(DescriptionStr)) return base.GetEditor(editorBaseType);
            else return new DescriptionEditor();
        }
        #endregion
    }
}
