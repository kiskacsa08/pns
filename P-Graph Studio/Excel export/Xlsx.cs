/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw.Excel_export
{
    class ExcelDoc
    {
        private Application app;
        private Workbook workbook;
        private Worksheet ws_materials;
        private Worksheet ws_opunits;
        private Worksheet ws_flows = null;

        public ExcelDoc(bool t_brief)
        {
            try
            {
                app = new Application();
                app.Visible = false;
                workbook = app.Workbooks.Add(1);
                ws_materials = (Worksheet)workbook.Sheets[1];
                ws_opunits = (Worksheet)workbook.Sheets.Add(Type.Missing, ws_materials, Type.Missing, Type.Missing);
                if (!t_brief) ws_flows = (Worksheet)workbook.Sheets.Add(ws_materials, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        public string ColumnName(int t_column)
        {
            string result = "";
            string ch;
            while (t_column > 0)
            {
                t_column--;
                ch = System.Convert.ToChar(t_column % 26 + 'A').ToString();
                result = result.Insert(0, ch);
                t_column /= 26;
            }
            return result;
        }

        public string CellName(int t_row, int t_column)
        {
            return ColumnName(t_column) + t_row;
        }

        public void AddText(string data, int top_row, int left_column)
        {
            AddText(data, top_row, left_column, 1, 1, false, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false);
        }
        public void AddText(string data, int top_row, int left_column, bool t_bold)
        {
            AddText(data, top_row, left_column, 1, 1, false, System.Drawing.Color.Empty, System.Drawing.Color.Empty, t_bold);
        }
        public void AddText(string data, int top_row, int left_column, int n_rows, int n_columns, bool t_bold)
        {
            AddText(data, top_row, left_column, n_rows, n_columns, true, System.Drawing.Color.Empty, System.Drawing.Color.Empty, t_bold);
        }
        public void AddText(string data, int top_row, int left_column, int n_rows, int n_columns, bool merge, System.Drawing.Color bcolor, System.Drawing.Color fcolor, bool bold)
        {
            Worksheet ws = (Worksheet)workbook.ActiveSheet;
            ws.Cells[top_row, left_column] = data;
            Range workSheet_range = ws.get_Range(ws.Cells[top_row, left_column], ws.Cells[top_row + n_rows - 1, left_column + n_columns - 1]);
            if (merge) workSheet_range.Merge(false);
            if (bcolor != System.Drawing.Color.Empty) workSheet_range.Interior.Color = bcolor.ToArgb();
            if (fcolor != System.Drawing.Color.Empty) workSheet_range.Font.Color = fcolor.ToArgb();
            workSheet_range.Font.Bold = bold;
        }

        public void AddData(string data, string format, int row, int column)
        {
            AddData(data, format, row, column, false);
        }

        public void AddData(string data, string format, int row, int column, bool bold)
        {
            Worksheet ws = (Worksheet)workbook.ActiveSheet;
            ws.Cells[row, column] = data;
            Range workSheet_range = ws.get_Range(ws.Cells[row, column], ws.Cells[row, column]);
            workSheet_range.NumberFormat = format;
            workSheet_range.Font.Bold = bold;
        }

        public Range Cells(int row, int column, int n_rows, int n_columns)
        {
            Worksheet ws = (Worksheet)workbook.ActiveSheet;
            Range r1 = ws.Cells[row, column];
            Range r2 = ws.Cells[row + n_rows - 1, column + n_columns - 1];
            return ws.get_Range(r1, r2);
        }

        public void Align(Range range, def_Solution_Excel.HAlign h_pos, def_Solution_Excel.VAlign v_pos)
        {
            switch (h_pos)
            {
                case def_Solution_Excel.HAlign.left: range.HorizontalAlignment = XlHAlign.xlHAlignLeft; break;
                case def_Solution_Excel.HAlign.center: range.HorizontalAlignment = XlHAlign.xlHAlignCenter; break;
                case def_Solution_Excel.HAlign.right: range.HorizontalAlignment = XlHAlign.xlHAlignRight; break;
            }
            switch (v_pos)
            {
                case def_Solution_Excel.VAlign.top: range.VerticalAlignment = XlVAlign.xlVAlignTop; break;
                case def_Solution_Excel.VAlign.center: range.VerticalAlignment = XlVAlign.xlVAlignCenter; break;
                case def_Solution_Excel.VAlign.bottom: range.VerticalAlignment = XlVAlign.xlVAlignBottom; break;
            }

        }

        public void Save(string t_file, bool t_quit)
        {
            try
            {
                workbook.SaveCopyAs(t_file);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Excel export error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            workbook.Saved = true;
            if (t_quit) App.Quit();
        }

        public Application App { get { return app; } }
        public Workbook WorkBook { get { return workbook; } }
        public Worksheet WSMaterials { get { return ws_materials; } }
        public Worksheet WSOpUnits { get { return ws_opunits; } }
        public Worksheet WSFlows { get { return ws_flows; } }
    }
}
