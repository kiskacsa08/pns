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

namespace PNSDraw.Canvas
{
    partial class PNSCanvas
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PNSCanvas
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.DoubleBuffered = true;
            this.Name = "PNSCanvas";
            this.Size = new System.Drawing.Size(223, 181);
            this.SizeChanged += new System.EventHandler(this.PNSCanvas_SizeChanged);
            this.Click += new System.EventHandler(this.PNSCanvas_Click);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PNSCanvas_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PNSCanvas_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.PNSCanvas_DragOver);
            this.DragLeave += new System.EventHandler(this.PNSCanvas_DragLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PNSCanvas_Paint);
            this.DoubleClick += new System.EventHandler(this.PNSCanvas_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PNSCanvas_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PNSCanvas_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PNSCanvas_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PNSCanvas_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PNSCanvas_MouseUp);
            this.Resize += new System.EventHandler(this.PNSCanvas_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
