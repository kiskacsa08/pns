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

using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Deployment.Application;

namespace PNSDraw
{
    partial class AboutPGraphStudio : Form
    {
        public AboutPGraphStudio() { 
            InitializeComponent();
            InitializeText();
        }

        
        private void okButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void InitializeText()
        {
            this.labelProductName.Text = "P-Graph Studio";
            try
            {
                this.labelVersion.Text = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4) + " - July 1, 2015";
            }
            catch (InvalidDeploymentException ex)
            {
                this.labelVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " - July 1, 2015";
            }
            this.labelCopyright.Text = "Copyright (C) 2015";
            this.labelInformation.Text = "Visit http://www.p-graph.com for more information!";
            this.textBoxDescription.Text = @"Demonstration Program for Process-Network Synthesis

This demonstration program is the realization of the
algorithms MSG, SSG, and ABB published in, e.g.

Friedler, F., K. Tarjan, Y. W. Huang, and L. T. Fan
Combinatorial Algorithms for Process Synthesis
Comp. Chem. Eng., 16, S313-320 (1992).

Friedler, F., J. B. Varga, and L. T. Fan, Decision-Mapping:
A Tool for Consistent and Complete Decisions in Process Synthesis,
Chem. Eng. Sci., 50, 1755-1768 (1995).

Friedler, F., J. B. Varga, E. Feher, and L. T. Fan
Combinatorially Accelerated Branch-and-Bound Method for
Solving the MIP Model of Process Network Synthesis
Nonconvex Optimization and Its Applications
State of the Art in Global Optimization
Computational Methods and Applications, pp. 609-626
Kluwer Academic Publishers, Dordrecht, 1996.

Bertok, B., M. Barany, and F. Friedler, 
Generating and Analyzing Mathematical Programming Models of
Conceptual Process Design by P-graph Software, 
Industrial & Engineering Chemistry Research, 52(1), 166-171 (2013).

See also http://www.p-graph.com .

Adam Horvath, Attila Horvath
Department of Computer Science and Systems Technology,
University of Pannonia, 2015
(Release Build)";
        }
    }
}
