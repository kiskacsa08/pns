using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw.ZIMPL_export
{
    class ProblemToZimpl
    {
        public static void ProblemToZIMPL(string filename, PGraph graph)
        {
            List<Material> raw_list = new List<Material>();
            List<Material> int_list = new List<Material>();
            List<Material> prod_list = new List<Material>();

            string t_path = "";
            t_path = Path.GetDirectoryName(filename);
            if (t_path == "") t_path = Directory.GetCurrentDirectory();
            SaveFileDialog t_dialog;
            t_dialog = new SaveFileDialog();
            t_dialog.InitialDirectory = t_path;
            t_dialog.Filter = "ZIMPL files (*.zpl)|*.zpl";
            t_dialog.FileName = Path.GetFileNameWithoutExtension(filename);
            if (t_dialog.ShowDialog() != DialogResult.OK) return;
            t_path = t_dialog.FileName;

            foreach (Material mat in graph.Materials)
            {
                if (mat.Type == Globals.MaterialTypes.Raw)
                {
                    raw_list.Add(mat);
                }
                else if (mat.Type == Globals.MaterialTypes.Intermediate)
                {
                    int_list.Add(mat);
                }
                else
                {
                    prod_list.Add(mat);
                }
            }

            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            FileStream fs = new FileStream(t_path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            // Nyersanyagok halmaza
            string t_str = ZIMPL_keys._REM_RAWS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._RAWS + " := {";
            for (int i = 0; i < raw_list.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + raw_list[i].Name + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Intermedierek halmaza
            t_str += ZIMPL_keys._REM_INTERMEDIATES + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._INTERMEDIATES + " := {";
            for (int i = 0; i < int_list.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + int_list[i].Name + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Termékek halmaza
            t_str += ZIMPL_keys._REM_PRODUCTS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._PRODUCTS + " := {";
            for (int i = 0; i < prod_list.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + prod_list[i].Name + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Összes anyag halmaza
            t_str += ZIMPL_keys._REM_MATERIALS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._MATERIALS + " := " + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._INTERMEDIATES + " union " + ZIMPL_keys._PRODUCTS + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Alsó korlát paraméterek nyersanyagokra és termékekre
            t_str += ZIMPL_keys._REM_MAT_LB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._MAT_LB + "[" + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._PRODUCTS + "] := " + ZIMPL_keys._NEWLINE;
            string t_list = "", def = "";
            foreach (Material matprop in raw_list)
                t_list += matprop.ReqFlowProp.Value != matprop.ParameterList["reqflow"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.ReqFlowProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            foreach (Material matprop in prod_list)
                t_list += matprop.ReqFlowProp.Value != matprop.ParameterList["reqflow"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.ReqFlowProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + Default.flow_rate_lower_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Felső korlát paraméterek az összes anyagra
            t_str += ZIMPL_keys._REM_MAT_UB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._MAT_UB + "[" + ZIMPL_keys._MATERIALS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (Material matprop in raw_list)
                t_list += matprop.MaxFlowProp.Value != matprop.ParameterList["maxflow"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.MaxFlowProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            foreach (Material matprop in int_list)
                t_list += matprop.MaxFlowProp.Value != matprop.ParameterList["maxflow"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.MaxFlowProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            foreach (Material matprop in prod_list)
                t_list += matprop.MaxFlowProp.Value != matprop.ParameterList["maxflow"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.MaxFlowProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + Default.flow_rate_upper_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Nyersanyagok és termékek árai
            t_str += ZIMPL_keys._REM_PRICE + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._PRICE + "[" + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._PRODUCTS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (Material matprop in raw_list)
                t_list += matprop.PriceProp.Value != matprop.ParameterList["price"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.PriceProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            foreach (Material matprop in prod_list)
                t_list += matprop.PriceProp.Value != matprop.ParameterList["price"].NonValue ? ZIMPL_keys._TAB + "<\"" + matprop.Name + "\"> " + matprop.PriceProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + Default.price + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek halmaza
            t_str += ZIMPL_keys._REM_OPUNITS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._OPUNITS + " := {";
            for (int i = 0; i < graph.OperatingUnits.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + graph.OperatingUnits[i].Name + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek kapacitás alsó korlát paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_LB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_LB + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (OperatingUnit ouprop in graph.OperatingUnits)
                t_list += ouprop.CapacityLowerProp.Value != ouprop.ParameterList["caplower"].NonValue ? ZIMPL_keys._TAB + "<\"" + ouprop.Name + "\"> " + ouprop.CapacityLowerProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + Default.capacity_lower_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek kapacitás felső korlát paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_UB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_UB + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (OperatingUnit ouprop in graph.OperatingUnits)
                t_list += ouprop.CapacityUpperProp.Value != ouprop.ParameterList["capupper"].NonValue ? ZIMPL_keys._TAB + "<\"" + ouprop.Name + "\"> " + ouprop.CapacityUpperProp.Value + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + Default.capacity_upper_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek fix költség paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_FIX + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_FIX + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "0;";
            foreach (OperatingUnit ouprop in graph.OperatingUnits)
            {
                double ouFixCost = ouprop.InvestmentCostFixProp.Value / (ouprop.PayoutPeriodProp.Value * ouprop.WorkingHourProp.Value) + ouprop.OperatingCostFixProp.Value;
                t_list += ouFixCost != 0 ? ZIMPL_keys._TAB + "<\"" + ouprop.Name + "\"> " + ouFixCost + "," + ZIMPL_keys._NEWLINE : "";
            }
            if (t_list != "")
            {
                def = "default 0;";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek arányos költség paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_PROP + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_PROP + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "0;";
            foreach (OperatingUnit ouprop in graph.OperatingUnits)
            {
                double ouPropCost = ouprop.InvestmentCostPropProp.Value / (ouprop.PayoutPeriodProp.Value * ouprop.WorkingHourProp.Value) + ouprop.OperatingCostPropProp.Value;
                t_list += ouPropCost != 0 ? ZIMPL_keys._TAB + "<\"" + ouprop.Name + "\"> " + ouPropCost + "," + ZIMPL_keys._NEWLINE : "";
            }
            if (t_list != "")
            {
                def = "default 0;";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek be és kimeneti anyagáramainak együttható mátrixa
            t_str += ZIMPL_keys._REM_OPUNIT_RATES + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_RATES + "[" + ZIMPL_keys._MATERIALS + "*" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "0;";
            foreach (OperatingUnit ouprop in graph.OperatingUnits)
            {
                Dictionary<Material, double> inMats = FileConnector.GetOpUnitBeginEnd(ouprop, graph)["input"];
                Dictionary<Material, double> outMats = FileConnector.GetOpUnitBeginEnd(ouprop, graph)["output"];
                t_list += "# " + ouprop.Name + ": ";
                string t_str2 = "";
                string t_str3 = "";
                foreach (KeyValuePair<Material, double> mat in inMats)
                {
                    if (t_str2 != "") t_str2 += " + ";
                    if (mat.Value != 1) t_str2 += mat.Value + " ";
                    t_str2 += mat.Key.Name;
                    t_str3 += ZIMPL_keys._TAB + "<\"" + mat.Key.Name + "\", \"" + ouprop.Name + "\"> -" + mat.Value + "," + ZIMPL_keys._NEWLINE;
                }
                t_list += t_str2 + " => ";
                t_str2 = "";
                foreach (KeyValuePair<Material, double> mat in outMats)
                {
                    //IOMaterial mat = (IOMaterial)prop.Value;
                    if (t_str2 != "") t_str2 += " + ";
                    if (mat.Value != 1) t_str2 += mat.Value + " ";
                    t_str2 += mat.Key.Name;
                    t_str3 += ZIMPL_keys._TAB + "<\"" + mat.Key.Name + "\", \"" + ouprop.Name + "\"> " + mat.Value + "," + ZIMPL_keys._NEWLINE;
                }
                t_list += t_str2 + ZIMPL_keys._NEWLINE;
                t_list += t_str3;
            }
            if (t_list != "")
            {
                def = "default 0;";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;


            // Matematikai modell generálása

            // Bináris változók a műveleti egységek beválasztásához, kizárásához
            t_str += "var exist[" + ZIMPL_keys._OPUNITS + "] binary;" + ZIMPL_keys._NEWLINE;

            // A műveleti egységek méret változói. 
            t_str += "var size[<o> in " + ZIMPL_keys._OPUNITS + "] real >= 0 <= " + ZIMPL_keys._OPUNIT_UB + "[o];" + ZIMPL_keys._NEWLINE;

            // Költségfüggvény
            t_str += "minimize cost:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "(sum<o> in " + ZIMPL_keys._OPUNITS + ": size[o]*" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "(" + ZIMPL_keys._OPUNIT_PROP + "[o] -" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + " sum<m> in (" + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._PRODUCTS + "):" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + ZIMPL_keys._TAB + " " + ZIMPL_keys._PRICE + "[m]*" + ZIMPL_keys._OPUNIT_RATES + "[m, o]" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + ")" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ") +" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "(sum<o> in " + ZIMPL_keys._OPUNITS + ":" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "exist[o] * " + ZIMPL_keys._OPUNIT_FIX + "[o]);" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a műveleti egységek méretének maximumára
            t_str += "subto size_ub:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <o> in " + ZIMPL_keys._OPUNITS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "size[o] <= exist[o] * " + ZIMPL_keys._OPUNIT_UB + "[o];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a műveleti egységek méretének minimumára
            t_str += "subto size_lb:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <o> in " + ZIMPL_keys._OPUNITS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "size[o] >= exist[o] * " + ZIMPL_keys._OPUNIT_LB + "[o];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a minimálisan felhasználandó nyersanyag mennyiségekre
            t_str += "subto raw_lb:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <r> in " + ZIMPL_keys._RAWS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": -1 * " + ZIMPL_keys._OPUNIT_RATES + "[r, o] * size[o] >= " + ZIMPL_keys._MAT_LB + "[r];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek intermedierekre (a köztes anyagokból legalább annyit elő kell állítani, mint amennyit felhasználunk)
            t_str += "subto inter_lb:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <i> in " + ZIMPL_keys._INTERMEDIATES + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": " + ZIMPL_keys._OPUNIT_RATES + "[i, o] * size[o] >= 0;" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a minimálisan előállítandó termék mennyiségekre
            t_str += "subto prod_lb:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <p> in " + ZIMPL_keys._PRODUCTS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": " + ZIMPL_keys._OPUNIT_RATES + "[p, o] * size[o] >= " + ZIMPL_keys._MAT_LB + "[p];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a nyersanyagok rendelkezésre álló mennyiségeire
            t_str += "subto raw_ub:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <r> in " + ZIMPL_keys._RAWS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": -1 * " + ZIMPL_keys._OPUNIT_RATES + "[r, o] * size[o] <= " + ZIMPL_keys._MAT_UB + "[r];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek az intermedierek maximálisan előállítható mennyiségeire
            t_str += "subto inter_ub:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <i> in " + ZIMPL_keys._INTERMEDIATES + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": " + ZIMPL_keys._OPUNIT_RATES + "[i, o] * size[o] <= " + ZIMPL_keys._MAT_UB + "[i];" + ZIMPL_keys._NEWLINE;

            // Korlátozó feltételek a termékek maximálisan előállítható mennyiségeire
            t_str += "subto prod_ub:" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + "forall <p> in " + ZIMPL_keys._PRODUCTS + " do" + ZIMPL_keys._NEWLINE;
            t_str += ZIMPL_keys._TAB + ZIMPL_keys._TAB + "sum<o> in " + ZIMPL_keys._OPUNITS + ": " + ZIMPL_keys._OPUNIT_RATES + "[p, o] * size[o] <= " + ZIMPL_keys._MAT_UB + "[p];" + ZIMPL_keys._NEWLINE;

            sw.Write(t_str);
            sw.Close();
            fs.Close();
            Thread.CurrentThread.CurrentCulture = t_original_culture;
        }
    }
}
