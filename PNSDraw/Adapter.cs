//using PNSDraw.Entities;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PNSDraw
//{
//    // A Draw és a Studio közötti kommunikációért felel ez az osztály
//    class Adapter
//    {
//        static Pns.PnsStudio ps = new Pns.PnsStudio();

//        //Indítja a solvert
//        public static string StartSolver(string method, PGraph problem)
//        {
//            TransferPns(problem);

//            string sol = ps.StartSolver(method);
//            return sol;
//        }

//        //A Draw-féle problémából készít a Studio számára értelmezhető problémát
//        private static void TransferPns(PGraph problem)
//        {
//            Pns.Xml_Serialization.PnsProblem.Problem studioProblem = new Pns.Xml_Serialization.PnsProblem.Problem();
//            ArrayList rawMats = new ArrayList();
//            ArrayList intMats = new ArrayList();
//            ArrayList prodMats = new ArrayList();
//            foreach (Material mat in problem.Materials)
//            {
//                switch (mat.Type)
//                {
//                    case Globals.MaterialTypes.Raw:
//                        Pns.Xml_Serialization.PnsProblem.RawMaterial rawMat = DrawRMatToStudioRMat(mat);
//                        rawMats.Add(rawMat);
//                        break;
//                    case Globals.MaterialTypes.Intermediate:
//                        Pns.Xml_Serialization.PnsProblem.IntermediateMaterial intMat = DrawIMatToStudioIMat(mat);
//                        intMats.Add(intMat);
//                        break;
//                    case Globals.MaterialTypes.Product:
//                        Pns.Xml_Serialization.PnsProblem.ProductMaterial prodMat = DrawPMatToStudioPMat(mat);
//                        prodMats.Add(prodMat);
//                        break;
//                    default:
//                        break;
//                }
//            }

//            studioProblem.materials = new Pns.Xml_Serialization.PnsProblem.Materials();

//            studioProblem.materials.rawMaterial = (Pns.Xml_Serialization.PnsProblem.RawMaterial[])rawMats.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.RawMaterial));
//            studioProblem.materials.intermediateMaterial = (Pns.Xml_Serialization.PnsProblem.IntermediateMaterial[])intMats.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.IntermediateMaterial));
//            studioProblem.materials.productMaterial = (Pns.Xml_Serialization.PnsProblem.ProductMaterial[])prodMats.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.ProductMaterial));

//            ArrayList opUnits = new ArrayList();
//            foreach (OperatingUnit ou in problem.OperatingUnits)
//            {
//                opUnits.Add(DrawOpUnitToStudioOpUnit(ou, problem));
//            }

//            studioProblem.operatingUnits = (Pns.Xml_Serialization.PnsProblem.OperatingUnit[])opUnits.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.OperatingUnit));

//            ArrayList mutExcls = new ArrayList();
//            foreach (MutualExclusion mutExcl in problem.MutualExclusions)
//            {
//                mutExcls.Add(DrawMutualExclusionToStudioMutualExclusion(mutExcl));
//            }

//            studioProblem.mutualExclusionSets = (Pns.Xml_Serialization.PnsProblem.MutualExclusionSet[])mutExcls.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.MutualExclusionSet));
//            new Pns.DefaultMUsAndValues(studioProblem);
//            ps.LoadMaterials(studioProblem.materials);
//            ps.LoadOperatingUnits(studioProblem.operatingUnits);
//        }

//        //Draw Raw Material-ból Studio-féle Raw Materialt készít
//        private static Pns.Xml_Serialization.PnsProblem.RawMaterial DrawRMatToStudioRMat(Material rMat)
//        {
//            Pns.Xml_Serialization.PnsProblem.RawMaterial rawMat = new Pns.Xml_Serialization.PnsProblem.RawMaterial();
//            rawMat.name = rMat.Name;
//            rawMat.price = rMat.PriceProp.Value;
//            rawMat.description = rMat.CommentText;
//            rawMat.max = rMat.MaxFlowProp.Value;
//            rawMat.min = rMat.ReqFlowProp.Value;

//            return rawMat;
//        }

//        //Draw Intermediate Material-ból Studio-féle Intermediate Materialt készít
//        private static Pns.Xml_Serialization.PnsProblem.IntermediateMaterial DrawIMatToStudioIMat(Material iMat)
//        {
//            Pns.Xml_Serialization.PnsProblem.IntermediateMaterial intMat = new Pns.Xml_Serialization.PnsProblem.IntermediateMaterial();
//            intMat.name = iMat.Name;
//            intMat.penaltySpecified = false;
//            intMat.description = iMat.CommentText;
//            intMat.max = iMat.MaxFlowProp.Value;

//            return intMat;
//        }

//        //Draw Product Material-ból Studio-féle Product Materialt készít
//        private static Pns.Xml_Serialization.PnsProblem.ProductMaterial DrawPMatToStudioPMat(Material pMat)
//        {
//            Pns.Xml_Serialization.PnsProblem.ProductMaterial prodMat = new Pns.Xml_Serialization.PnsProblem.ProductMaterial();
//            prodMat.name = pMat.Name;
//            prodMat.price = pMat.PriceProp.Value;
//            prodMat.description = pMat.CommentText;
//            prodMat.max = pMat.MaxFlowProp.Value;
//            prodMat.min = pMat.ReqFlowProp.Value;

//            return prodMat;
//        }

//        //Draw Operating Unit-ból Studio-féle Operating Unitot készít
//        private static Pns.Xml_Serialization.PnsProblem.OperatingUnit DrawOpUnitToStudioOpUnit(OperatingUnit ou, PGraph graph)
//        {
//            Pns.Xml_Serialization.PnsProblem.OperatingUnit opUnit = new Pns.Xml_Serialization.PnsProblem.OperatingUnit();
//            opUnit.name = ou.Name;
//            opUnit.description = ou.CommentText;
//            opUnit.investmentFix = ou.InvestmentCostFixProp.Value;
//            opUnit.investmentProp = ou.InvestmentCostPropProp.Value;
//            opUnit.lowerBoundSpecified = false;
//            opUnit.operatingFix = ou.OperatingCostFixProp.Value;
//            opUnit.operatingProp = ou.OperatingCostPropProp.Value;
//            opUnit.payoutPeriod = ou.PayoutPeriodProp.Value;
//            opUnit.totalFix = ou.OperatingCostFixProp.Value + ou.InvestmentCostFixProp.Value;
//            opUnit.totalProp = ou.OperatingCostPropProp.Value + ou.InvestmentCostPropProp.Value;
//            opUnit.workingHoursPerYear = (int)ou.WorkingHourProp.Value;

//            ArrayList inputMaterials = GetFlowMaterials(GetOpUnitBeginEnd(ou, graph)["input"]);
//            ArrayList outputMaterials = GetFlowMaterials(GetOpUnitBeginEnd(ou, graph)["output"]);

//            opUnit.inputMaterial = (Pns.Xml_Serialization.PnsProblem.FlowMaterial[])inputMaterials.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.FlowMaterial));
//            opUnit.outputMaterial = (Pns.Xml_Serialization.PnsProblem.FlowMaterial[])outputMaterials.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.FlowMaterial));

//            return opUnit;
//        }

//        //Draw Material-ból és Edge-ből Studio-féle FlowMaterialt készít
//        private static ArrayList GetFlowMaterials(Dictionary<Material, double> mats)
//        {
//            ArrayList materials = new ArrayList();

//            foreach (KeyValuePair<Material, double> mat in mats)
//            {
//                Pns.Xml_Serialization.PnsProblem.FlowMaterial fm = new Pns.Xml_Serialization.PnsProblem.FlowMaterial();
//                fm.name = mat.Key.Name;
//                fm.rate = mat.Value;
//                materials.Add(fm);
//            }

//            return materials;
//        }

//        //Egy Operating Unitnak az input és az output materialjait adja vissza
//        private static Dictionary<string, Dictionary<Material, double>> GetOpUnitBeginEnd(OperatingUnit ou, PGraph graph)
//        {
//            Dictionary<string, Dictionary<Material, double>> beginEnd = new Dictionary<string, Dictionary<Material, double>>();
//            Dictionary<Material, double> inputMats = new Dictionary<Material, double>();
//            Dictionary<Material, double> outputMats = new Dictionary<Material, double>();
//            foreach (Edge edge in graph.Edges)
//            {
//                if (edge.begin.GetType().Equals(typeof(OperatingUnit)))
//                {
//                    if (((OperatingUnit)edge.begin).Name.Equals(ou.Name))
//                    {
//                        outputMats.Add((Material)edge.end, edge.Rate);
//                    }
//                }

//                if (edge.end.GetType().Equals(typeof(OperatingUnit)))
//                {
//                    if (((OperatingUnit)edge.end).Name.Equals(ou.Name))
//                    {
//                        inputMats.Add((Material)edge.begin, edge.Rate);
//                    }
//                }
//            }

//            beginEnd.Add("input", inputMats);
//            beginEnd.Add("output", outputMats);

//            return beginEnd;
//        }

//        //Draw MutualExclusion-ből Studio-féle MutualExclusionSet-et készít
//        private static Pns.Xml_Serialization.PnsProblem.MutualExclusionSet DrawMutualExclusionToStudioMutualExclusion(MutualExclusion mutExcl)
//        {
//            Pns.Xml_Serialization.PnsProblem.MutualExclusionSet mutExclSet = new Pns.Xml_Serialization.PnsProblem.MutualExclusionSet();
//            ArrayList OUs = new ArrayList();
//            foreach (OperatingUnit ou in mutExcl.OpUnits)
//            {
//                Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit mutExclOU = DrawMutExclOUToStudioMutExclOU(ou);
//                OUs.Add(mutExclOU);
//            }

//            mutExclSet.mutualExclusionOperatingUnit = (Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit[])OUs.ToArray(typeof(Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit));

//            return mutExclSet;
//        }

//        //Draw MutualExclusion OperatingUnitjaiból Studio-féle MutualExclusionOperatingUnitot készít
//        private static Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit DrawMutExclOUToStudioMutExclOU(OperatingUnit ou)
//        {
//            Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit mutExclOu = new Pns.Xml_Serialization.PnsProblem.MutualExclusionOperatingUnit();
//            mutExclOu.name = ou.Name;
//            return mutExclOu;
//        }
//    }
//}
