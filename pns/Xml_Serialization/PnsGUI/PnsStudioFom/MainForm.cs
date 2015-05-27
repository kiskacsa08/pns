using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.PnsStudioFom
{
    public class def_PnsStudio
    {
        #region Members
        static public string deffilename = "Untitled";
        static public string defappname = "Pns Studio";
        static public string NperA = "(default)";
        static public string Msg_file_changed = "File has changed. Save changes?";
        static public string Msg_file_changed_title = "Warning";
        static public string Ex_file_open_error = "File open error";
        static public string Ex_fatal_error = "Fatal error";
        static public string Msg_close_confirm = "Do you really want to close Pns Studio?";
        static public string Msg_close_confirm_title = "Confirmation";
        static public string Msg_mu_file_missing = "Defaults.xml file does not exist. Defaults.xml file has been created.";
        static public string Msg_mu_file_missing_title = "Warning";
        static public string Msg_text_file_missing = "PnsUserInterfaceTexts.xml file does not exist. PnsUserInterfaceTexts.xml file has been created.";
        static public string Msg_text_file_missing_title = "Warning";
        static public string PathDelimiter = ".";
        static public string Msg_must_be_less_or_equal_than = " must be <= than ";
        static public string def_no_name = "NO_NAME";
        static public string Msg_multi_file_drop = "Due to single document interface the first file will be opened.";
        static public string Msg_multi_file_drop_title = "Warning";
        static public string Ex_excel_export_error = "Excel export error";
        static public string ProblemTabTextField = "Problem";
        static public string SolutionsTabTextField = "Solutions";
        #endregion

        #region Properties
        public string DefaultFileName { get { return deffilename; } set { deffilename = value; } }
        public string ApplicationName { get { return defappname; } set { defappname = value; } }
        public string DefaultValueText { get { return NperA; } set { NperA = value; } }
        public MessageBoxXMLTag MessageBoxFileChanged
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_file_changed), Msg_file_changed_title); }
            set
            {
                Msg_file_changed = value.Message.TextPart1;
                Msg_file_changed_title = value.Title;
            }
        }
        public MessageBoxXMLTag MessageBoxFileOpenError { get { return new MessageBoxXMLTag(null, Ex_file_open_error); } set { Ex_file_open_error = value.Title; } }
        public MessageBoxXMLTag MessageBoxFatalError { get { return new MessageBoxXMLTag(null, Ex_fatal_error); } set { Ex_fatal_error = value.Title; } }
        public MessageBoxXMLTag MessageBoxPnsClose
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_close_confirm), Msg_close_confirm_title); }
            set
            {
                Msg_close_confirm = value.Message.TextPart1;
                Msg_close_confirm_title = value.Title;
            }
        }
        public MessageBoxXMLTag MessageBoxPnsDefaultsFileMissing
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_mu_file_missing), Msg_mu_file_missing_title); }
            set
            {
                Msg_mu_file_missing = value.Message.TextPart1;
                Msg_mu_file_missing_title = value.Title;
            }
        }
        public MessageBoxXMLTag MessageBoxPnsUserInterfaceTextsFileMissing
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_text_file_missing), Msg_text_file_missing_title); }
            set
            {
                Msg_text_file_missing = value.Message.TextPart1;
                Msg_text_file_missing_title = value.Title;
            }
        }
        public string PathDelimiterSymbol { get { return PathDelimiter; } set { PathDelimiter = value; } }
        public string MessageTextLessOrEqual { get { return Msg_must_be_less_or_equal_than; } set { Msg_must_be_less_or_equal_than = value; } }
        public string MessageTextNoName { get { return def_no_name; } set { def_no_name = value; } }
        public MessageBoxXMLTag MessageBoxMultiFileDrop
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_multi_file_drop), Msg_multi_file_drop_title); }
            set
            {
                Msg_multi_file_drop = value.Message.TextPart1;
                Msg_multi_file_drop_title = value.Title;
            }
        }
        public MessageBoxXMLTag MessageBoxExcelExportError { get { return new MessageBoxXMLTag(null, Ex_excel_export_error); } set { Ex_excel_export_error = value.Title; } }
        public string ProblemTabText { get { return ProblemTabTextField; } set { ProblemTabTextField = value; } }
        public string SolutionsTabText { get { return SolutionsTabTextField; } set { SolutionsTabTextField = value; } }
        #endregion
    }
}
