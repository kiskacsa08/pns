using System.Drawing;
using System.Windows.Forms;

namespace Pns
{
    public partial class TreeNodeGhost : Form
    {
        #region Members
        private Font m_font;
        private string m_text;
        private Point m_offset;
        private bool m_dopainting;
        private bool m_show;
        #endregion

        #region Constructors
        public TreeNodeGhost()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, false);
            DoPainting=false;
            m_show = false;
        }
        #endregion

        #region Member functions
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.ExStyle = 0x00000020 // WS_EX_TRANSPARENT
                            | 0x00000080 // WS_EX_TOOLWINDOW
                            | 0x08000000 // WS_EX_NOACTIVATE
                            | 0x00080000; // WS_EX_LAYERED
                return p;
            }
        }

        public void Show(TreeNode t_node)
        {
            m_text = t_node.Text;
            m_font = t_node.TreeView.Font;
            m_offset = t_node.Bounds.Location;
            Point t_point = t_node.TreeView.PointToClient(Control.MousePosition);
            m_offset.X -= t_point.X;
            m_offset.Y -= t_point.Y;
            SizeF t_size = CreateGraphics().MeasureString(m_text, m_font);
            ClientSize = new Size((int)t_size.Width, (int)t_size.Height);
            DoPainting=true;
            if (!m_show)
            {
                m_show = true;
                base.Show();
                t_node.TreeView.Focus();
            }
        }
        #endregion

        #region Event Handlers
        private void TreeNodeGhost_Paint(object sender, PaintEventArgs e)
        {
            if (DoPainting) e.Graphics.DrawString(m_text, m_font, new SolidBrush(Color.Black), 0, 0);
        }
        #endregion

        #region Properties
        new public Point Location
        {
            get { return base.Location; }
            set
            {
                value.Offset(m_offset);
                base.Location = value;
            }
        }
        public bool DoPainting 
        { 
            get { return m_dopainting; } 
            set 
            { 
                m_dopainting = value;
                Refresh();
            } 
        }
        #endregion
    }
}
