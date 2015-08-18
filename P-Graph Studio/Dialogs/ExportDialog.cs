using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw.Dialogs
{
    public partial class ExportDialog : Form
    {
        Size canvasSize;

        Action<object, string, Form1.ExportExtensions, Size, Form1.ExcelExportType> Export;

        string filename;

        public ExportDialog(
            Size pcanvasSize,
            string pcurrentFile,
            Action<object, string, Form1.ExportExtensions, Size, Form1.ExcelExportType> pExport,
            int solutionSize
            )
        {
            InitializeComponent();

            this.canvasSize = pcanvasSize;

            if (pcurrentFile != "")
            {
                this.filename = pcurrentFile;
            }
            else
            {
                this.filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/untitled";
            }

            Export = pExport;

            largeRadioButton.Text = "Large (" + canvasSize.Width + "×" + canvasSize.Height + ")";
            mediumRadioButton.Text = "Medium (" + (canvasSize.Width / 2) + "x" + (canvasSize.Height / 2) + ")";
            smallRadioButton.Text = "Small (" + (canvasSize.Width / 4) + "x" + (canvasSize.Height / 4) + ")";

            int sizelimit = Config.Instance.ImageWidth * Config.Instance.ImageHeight;

            int bmpsize = canvasSize.Height * canvasSize.Width;

            if (bmpsize > sizelimit)
            {
                largeRadioButton.Enabled = false;
            }
            else
            {
                largeRadioButton.Enabled = true;
            }

            if (bmpsize / 4 > sizelimit)
            {
                mediumRadioButton.Enabled = false;
            }
            else
            {
                mediumRadioButton.Enabled = true;
            }

            if (bmpsize / 16 > sizelimit)
            {
                smallRadioButton.Enabled = false;
            }
            else
            {
                smallRadioButton.Enabled = true;
            }

            jpgRadionButton.Checked = true;
            smallRadioButton.Checked = true;
            excelProblemRadioButton.Checked = true;

            if (solutionSize <= 0)
            {
                excelBriefRadioButton.Enabled = false;
                excelDetailedRadioButton.Enabled = false;
                excelSummaryRadioButton.Enabled = false;
                excelReviewButton.Enabled = false;

                excelBriefRadioButton.Text += " (after solve)";
                excelDetailedRadioButton.Text += " (after solve)";
                excelSummaryRadioButton.Text += " (after solve)";
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (!Utils.Instance.CheckLogin())
            {
                Close();
                return;
            }

            var checkedFormatButton = this.formatGroupBox.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);

            var checkedSizeButton = this.sizeGroupBox.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);

            var checkedExcelButton = this.excelGroupBox.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);


            if (checkedFormatButton == jpgRadionButton)
            {
                ExportToJPG(sender, checkedSizeButton);
            }
            else if (checkedFormatButton == pngRadioButton)
            {
                ExportToPNG(sender, checkedSizeButton);
            }
            else if (checkedFormatButton == svgRadionButton)
            {
                ExportToSVG(sender);
            }
            else if (checkedFormatButton == excelRadioButton)
            {
                ExportToExcel(sender, checkedExcelButton);
            }
            else if (checkedFormatButton == zimplRadionButton)
            {
                ExportToZimpl(sender);
            }

            Close();
        }

        private void ExportToJPG(object sender, RadioButton checkedSizeButton)
        {
            Size size = new Size(640, 480);
            if (checkedSizeButton == smallRadioButton)
            {
                size = new Size(canvasSize.Width / 4, canvasSize.Height / 4);
            }
            else if (checkedSizeButton == mediumRadioButton)
            {
                size = new Size(canvasSize.Width / 2, canvasSize.Height / 2);
            }
            else if (checkedSizeButton == largeRadioButton)
            {
                size = new Size(canvasSize.Width, canvasSize.Height);
            }
            else if (checkedSizeButton == otherRadioButton)
            {
                int width = Decimal.ToInt32(otherSizeWidth.Value);
                int height = Decimal.ToInt32(otherSizeHeight.Value);

                size = new Size(width, height);
            }

            Export(sender, filename, Form1.ExportExtensions.JPG, size, Form1.ExcelExportType.graph_export);
        }

        private void ExportToPNG(object sender, RadioButton checkedSizeButton)
        {
            Size size = new Size(640, 480);
            if (checkedSizeButton == smallRadioButton)
            {
                size = new Size(canvasSize.Width / 4, canvasSize.Height / 4);
            }
            else if (checkedSizeButton == mediumRadioButton)
            {
                size = new Size(canvasSize.Width / 2, canvasSize.Height / 2);
            }
            else if (checkedSizeButton == largeRadioButton)
            {
                size = new Size(canvasSize.Width, canvasSize.Height);
            }
            else if (checkedSizeButton == otherRadioButton)
            {
                int width = Decimal.ToInt32(otherSizeWidth.Value);
                int height = Decimal.ToInt32(otherSizeHeight.Value);

                size = new Size(width, height);
            }

            Export(sender, filename, Form1.ExportExtensions.PNG, size, Form1.ExcelExportType.graph_export);
        }

        private void ExportToSVG(object sender)
        {
            Export(sender, filename, Form1.ExportExtensions.SVG, new Size(0,0), Form1.ExcelExportType.graph_export);
        }

        private void ExportToExcel(object sender, RadioButton checkedExcelButton)
        {
            if (checkedExcelButton == excelProblemRadioButton)
            {
                Export(sender, filename, Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.graph_export);
            }
            else if (checkedExcelButton == excelBriefRadioButton)
            {
                Export(sender, filename, Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.brief_export);
            }
            else if (checkedExcelButton == excelDetailedRadioButton)
            {
                Export(sender, filename, Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.detailed_export);
            }
            else if (checkedExcelButton == excelSummaryRadioButton)
            {
                Export(sender, filename, Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.export_summary_of_results);
            }
        }

        private void ExportToZimpl(object sender)
        {
            Export(sender, filename, Form1.ExportExtensions.ZIMPL, new Size(0, 0), Form1.ExcelExportType.graph_export);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void formatRadio_CheckedChange(object sender, EventArgs e)
        {
            var checkedButton = this.formatGroupBox.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);

            if (checkedButton == jpgRadionButton)
            {
                sizeGroupBox.Visible = true;
                excelGroupBox.Visible = false;
            }
            else if (checkedButton == pngRadioButton)
            {
                sizeGroupBox.Visible = true;
                excelGroupBox.Visible = false;    
            }
            else if (checkedButton == svgRadionButton)
            {
                sizeGroupBox.Visible = false;
                excelGroupBox.Visible = false;
            }
            else if (checkedButton == excelRadioButton)
            {
                sizeGroupBox.Visible = false;
                excelGroupBox.Visible = true;
            }
            else if (checkedButton == zimplRadionButton)
            {
                sizeGroupBox.Visible = false;
                excelGroupBox.Visible = false;
            }
        }

        private void excelReviewButton_Click(object sender, EventArgs e)
        {
            var checkedButton = this.excelGroupBox.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);

            if (checkedButton == excelBriefRadioButton)
            {
                Export(sender, "file.xls", Form1.ExportExtensions.XLS, new Size(0,0), Form1.ExcelExportType.brief_view);
            }
            else if (checkedButton == excelDetailedRadioButton)
            {
                Export(sender, "file.xls", Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.detailed_view);
            }
            else if (checkedButton == excelSummaryRadioButton)
            {
                Export(sender, "file.xls", Form1.ExportExtensions.XLS, new Size(0, 0), Form1.ExcelExportType.view_summary_of_results);
            }
            
        }
    }
}
