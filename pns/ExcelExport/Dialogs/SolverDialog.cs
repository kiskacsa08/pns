using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Pns.Dialogs
{
    public partial class SolverDialog : Form
    {
        #region Members
        Thread m_solver_thread;
        Process m_solver_process;
        System.Windows.Forms.Timer m_timer;
        #endregion

        #region Constructors
        public SolverDialog(Thread t_thread)
        {
            InitializeComponent();
            m_solver_thread = t_thread;
            m_solver_process = null;
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Interval = 500;
            m_timer.Tick += new EventHandler(m_timer_Tick);
        }
        #endregion

        #region Event Handlers
        void m_timer_Tick(object sender, EventArgs e)
        {
            if (!richTextBoxSolverOutput.IsDisposed)
            {
                string str = PnsStudio.SolutionsString;
                if (str.Length > 0)
                {
                    richTextBoxSolverOutput.AppendText(str);
                    richTextBoxSolverOutput.ScrollToCaret();
                }
            }
            if (m_solver_thread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                m_timer.Enabled=false;
                Close();
            }
        }
        private void Solver_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_solver_thread.ThreadState != System.Threading.ThreadState.Stopped)
            {
                if (m_solver_process != null) m_solver_process.Kill();
                m_solver_thread.Abort();
            }
            else PnsStudio.ShowResults();
        }
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            m_timer.Enabled = false;
            Close();
        }
        #endregion

        #region Member functions
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_timer.Enabled = true;
        }
        #endregion

        #region Properties
        public Process SolverProcess { set { m_solver_process = value; } }
        #endregion
    }
}
