using System.Windows.Forms;
using System.IO;
using DynamicPropertyGrid;
using System.Globalization;
using System.Threading;
using Pns.ProblemToZIMPL;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns
{
    public partial class PnsStudio : Form
    {
        public void ProblemToZIMPL()
        {
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            FileStream fs = new FileStream(m_filename + ZIMPL_keys._EXTENSION, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            
            // Nyersanyagok halmaza
            string t_str = ZIMPL_keys._REM_RAWS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._RAWS + " := {";
            for (int i = 0; i < m_materials.m_rawlist.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + m_materials.m_rawlist[i].currname + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;
            
            // Intermedierek halmaza
            t_str += ZIMPL_keys._REM_INTERMEDIATES + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._INTERMEDIATES + " := {";
            for (int i = 0; i < m_materials.m_intermediatelist.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + m_materials.m_intermediatelist[i].currname + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;
            
            // Termékek halmaza
            t_str += ZIMPL_keys._REM_PRODUCTS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._PRODUCTS + " := {";
            for (int i = 0; i < m_materials.m_productlist.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + m_materials.m_productlist[i].currname + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;
            
            // Összes anyag halmaza
            t_str += ZIMPL_keys._REM_MATERIALS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._MATERIALS + " := " + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._INTERMEDIATES + " union " + ZIMPL_keys._PRODUCTS + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Alsó korlát paraméterek nyersanyagokra és termékekre
            t_str += ZIMPL_keys._REM_MAT_LB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._MAT_LB + "[" + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._PRODUCTS + "] := " + ZIMPL_keys._NEWLINE;
            string t_list="", def="";
            foreach (MaterialProperties matprop in m_materials.m_rawlist)
                t_list += matprop.gmin != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gmin + "," + ZIMPL_keys._NEWLINE : "";
            foreach (MaterialProperties matprop in m_materials.m_productlist)
                t_list += matprop.gmin != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gmin + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + DefaultMUsAndValues.DefaultValues.d_minimum_flow + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Felső korlát paraméterek az összes anyagra
            t_str += ZIMPL_keys._REM_MAT_UB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._MAT_UB + "[" + ZIMPL_keys._MATERIALS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (MaterialProperties matprop in m_materials.m_rawlist)
                t_list += matprop.gmax != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gmax + "," + ZIMPL_keys._NEWLINE : "";
            foreach (MaterialProperties matprop in m_materials.m_intermediatelist)
                t_list += matprop.gmax != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gmax + "," + ZIMPL_keys._NEWLINE : "";
            foreach (MaterialProperties matprop in m_materials.m_productlist)
                t_list += matprop.gmax != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gmax + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + DefaultMUsAndValues.DefaultValues.d_maximum_flow + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Nyersanyagok és termékek árai
            t_str += ZIMPL_keys._REM_PRICE + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._PRICE + "[" + ZIMPL_keys._RAWS + " union " + ZIMPL_keys._PRODUCTS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (MaterialProperties matprop in m_materials.m_rawlist)
                t_list += matprop.gprice != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gprice + "," + ZIMPL_keys._NEWLINE : "";
            foreach (MaterialProperties matprop in m_materials.m_productlist)
                t_list += matprop.gprice != def_Values.d_NperA ? ZIMPL_keys._TAB + "<\"" + matprop.currname + "\"> " + matprop.gprice + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + DefaultMUsAndValues.DefaultValues.d_price + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek halmaza
            t_str += ZIMPL_keys._REM_OPUNITS + ZIMPL_keys._NEWLINE;
            t_str += "set " + ZIMPL_keys._OPUNITS + " := {";
            for (int i = 0; i < m_operatingunitlist.Count; i++)
                t_str += (i == 0 ? "\"" : i % ZIMPL_keys._N == 0 ? "," + ZIMPL_keys._NEWLINE + ZIMPL_keys._TAB + "\"" : ", \"") + m_operatingunitlist[i].currname + "\"";
            t_str += ZIMPL_keys._NEWLINE + "};" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek kapacitás alsó korlát paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_LB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_LB + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
                t_list += ouprop.bounds.d_lb != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound ? ZIMPL_keys._TAB + "<\"" + ouprop.currname + "\"> " + ouprop.bounds.d_lb + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek kapacitás felső korlát paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_UB + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_UB + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "";
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
                t_list += ouprop.bounds.d_ub != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound ? ZIMPL_keys._TAB + "<\"" + ouprop.currname + "\"> " + ouprop.bounds.d_ub + "," + ZIMPL_keys._NEWLINE : "";
            if (t_list != "")
            {
                def = "default ";
                t_list = t_list.Remove(t_list.LastIndexOf(','), 1);
            }
            t_str += t_list + ZIMPL_keys._TAB + def + DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound + ";" + ZIMPL_keys._NEWLINE + ZIMPL_keys._NEWLINE;

            // Műveleti egységek fix költség paraméterei
            t_str += ZIMPL_keys._REM_OPUNIT_FIX + ZIMPL_keys._NEWLINE;
            t_str += "param " + ZIMPL_keys._OPUNIT_FIX + "[" + ZIMPL_keys._OPUNITS + "] := " + ZIMPL_keys._NEWLINE;
            t_list = "";
            def = "0;";
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
                t_list += ouprop.overallcost.g_fix != 0 ? ZIMPL_keys._TAB + "<\"" + ouprop.currname + "\"> " + ouprop.overallcost.g_fix + "," + ZIMPL_keys._NEWLINE : "";
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
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
                t_list += ouprop.overallcost.g_prop != 0 ? ZIMPL_keys._TAB + "<\"" + ouprop.currname + "\"> " + ouprop.overallcost.g_prop + "," + ZIMPL_keys._NEWLINE : "";
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
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
            {
                t_list += "# " + ouprop.currname + ": ";
                string t_str2 = "";
                string t_str3 = "";
                foreach (CustomProp prop in ouprop.imats.list)
                {
                    IOMaterial mat = (IOMaterial)prop.Value;
                    if (t_str2 != "") t_str2 += " + ";
                    if (mat.g_rate != 1) t_str2 += mat.g_rate + " ";
                    t_str2 += mat.Name;
                    t_str3 += ZIMPL_keys._TAB + "<\"" + mat.Name + "\", \"" + ouprop.currname + "\"> -" + mat.g_rate + "," + ZIMPL_keys._NEWLINE;
                }
                t_list += t_str2 + " => ";
                t_str2 = "";
                foreach (CustomProp prop in ouprop.omats.list)
                {
                    IOMaterial mat = (IOMaterial)prop.Value;
                    if (t_str2 != "") t_str2 += " + ";
                    if (mat.g_rate != 1) t_str2 += mat.g_rate + " ";
                    t_str2 += mat.Name;
                    t_str3 += ZIMPL_keys._TAB + "<\"" + mat.Name + "\", \"" + ouprop.currname + "\"> " + mat.g_rate + "," + ZIMPL_keys._NEWLINE;
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
