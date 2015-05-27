using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Pns
{
    public class RecentFile
    {
        #region Members
        private string m_file;
        #endregion

        #region Constructors
        public RecentFile(string t_file) { m_file = t_file; }
        #endregion

        #region Properties
        public string Name { get { return m_file.Substring(m_file.LastIndexOf('\\') + 1); } }
        public string Path { get { return m_file; } }
        #endregion
    }

    public class RecentFileList
    {
        #region Members
        private string m_sub_key = "Software\\" + Application.ProductName + "\\Recent File List";
        private int m_max_file;
        #endregion

        #region Constructors
        public RecentFileList(int t_max_file) { m_max_file = t_max_file; }
        #endregion

        #region Member functions
        private string Read(string t_key_name)
        {
            try { return (string)Registry.CurrentUser.OpenSubKey(m_sub_key).GetValue(t_key_name); }
            catch { return null; }
        }
        private void Write(string t_key_name, object t_value)
        {
            try { Registry.CurrentUser.CreateSubKey(m_sub_key).SetValue(t_key_name, t_value); }
            catch { }
        }
        private void DeleteSubKeyTree()
        {
            try { Registry.CurrentUser.DeleteSubKeyTree(m_sub_key); }
            catch { }
        }
        private int ValueCount()
        {
            try { return Registry.CurrentUser.OpenSubKey(m_sub_key).ValueCount; }
            catch { return 0; }
        }
        private List<RecentFile> GetRecentFileList()
        {
            List<RecentFile> t_list = new List<RecentFile>();
            int n = ValueCount();
            for (int i = 0; i < n; ) t_list.Add(new RecentFile(Read("File" + (++i).ToString())));
            return t_list;
        }
        private void SetRecentFileList(List<RecentFile> t_list)
        {
            DeleteSubKeyTree();
            int i = 0;
            foreach (RecentFile item in t_list)
            {
                if (i < m_max_file) Write("File" + (++i).ToString(), item.Path);
                else break;
            }
        }
        public void UpdateRecentList(ToolStripMenuItem recents, string newitem)
        {
            List<RecentFile> t_list = GetRecentFileList();
            if (newitem != "")
            {
                RecentFile t_recent = null;
                foreach (RecentFile item in t_list) if (item.Path == newitem) t_recent = item;
                if (t_recent != null) t_list.Remove(t_recent);
                t_list.Insert(0, new RecentFile(newitem));
            }
            recents.DropDown.Items.Clear();
            int i = 0;
            foreach (RecentFile item in t_list)
            {
                if (i < m_max_file)
                {
                    if (File.Exists(item.Path))
                    {
                        ToolStripMenuItem t_menuitem = new ToolStripMenuItem(item.Name);
                        t_menuitem.Tag = item;
                        t_menuitem.ToolTipText = item.Path;
                        recents.DropDown.Items.Add(t_menuitem);
                        i++;
                    }
                }
                else break;
            }
            SetRecentFileList(t_list);
        }
        #endregion
    }
}
