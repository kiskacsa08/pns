using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    class Adapter
    {
        static Pns.PnsStudio ps = new Pns.PnsStudio();


        // TODO: MutualExclusionOperatingUnits

        public static string StartSolver(string method, PGraph problem)
        {
            TransferPns(problem);
            string sol = ps.StartSolver(method);
            return sol;
        }

        private static void TransferPns(PGraph problem)
        {
            Pns.Xml_Serialization.PnsProblem.Problem studioProblem = new Pns.Xml_Serialization.PnsProblem.Problem();
            ArrayList rawMats = new ArrayList();
            ArrayList intMats = new ArrayList();
            ArrayList prodMats = new ArrayList();
            foreach (Material mat in problem.Materials)
	        {
		        switch (mat.Type)
                {
                    case Globals.MaterialTypes.Raw:
                        Pns.Xml_Serialization.PnsProblem.RawMaterial rawMat = DrawRMatToStudioRMat(mat);
                        rawMats.Add(rawMat);
                        break;
                    case Globals.MaterialTypes.Intermediate:
                        Pns.Xml_Serialization.PnsProblem.IntermediateMaterial intMat = DrawIMatToStudioIMat(mat);
                        intMats.Add(intMat);
                        break;
                    case Globals.MaterialTypes.Product:
                        Pns.Xml_Serialization.PnsProblem.ProductMaterial prodMat = DrawPMatToStudioPMat(mat);
                        prodMats.Add(prodMat);
                        break;
                    default:
                        break;
	            }
            }

            rawMats.CopyTo(studioProblem.materials.rawMaterial);
            intMats.CopyTo(studioProblem.materials.intermediateMaterial);
            prodMats.CopyTo(studioProblem.materials.productMaterial);

            ArrayList opUnits = new ArrayList();
            foreach (OperatingUnit ou in problem.OperatingUnits)
            {
                opUnits.Add(DrawOpUnitToStudioOpUnit(ou));
            }

            opUnits.CopyTo(studioProblem.operatingUnits);
        }

        private static Pns.Xml_Serialization.PnsProblem.RawMaterial DrawRMatToStudioRMat(Material rMat)
        {
            Pns.Xml_Serialization.PnsProblem.RawMaterial rawMat = new Pns.Xml_Serialization.PnsProblem.RawMaterial();
            rawMat.name = rMat.Name;
            rawMat.price = rMat.PriceProp.Value;
            rawMat.description = rMat.CommentText;
            rawMat.max = rMat.MaxFlowProp.Value;
            rawMat.min = rMat.ReqFlowProp.Value;

            return rawMat;
        }

        private static Pns.Xml_Serialization.PnsProblem.IntermediateMaterial DrawIMatToStudioIMat(Material iMat)
        {
            Pns.Xml_Serialization.PnsProblem.IntermediateMaterial intMat = new Pns.Xml_Serialization.PnsProblem.IntermediateMaterial();
            intMat.name = iMat.Name;
            intMat.penaltySpecified = false;
            intMat.description = iMat.CommentText;
            intMat.max = iMat.MaxFlowProp.Value;

            return intMat;
        }

        private static Pns.Xml_Serialization.PnsProblem.ProductMaterial DrawPMatToStudioPMat(Material pMat)
        {
            Pns.Xml_Serialization.PnsProblem.ProductMaterial prodMat = new Pns.Xml_Serialization.PnsProblem.ProductMaterial();
            prodMat.name = pMat.Name;
            prodMat.price = pMat.PriceProp.Value;
            prodMat.description = pMat.CommentText;
            prodMat.max = pMat.MaxFlowProp.Value;
            prodMat.min = pMat.ReqFlowProp.Value;

            return prodMat;
        }

        private static Pns.Xml_Serialization.PnsProblem.OperatingUnit DrawOpUnitToStudioOpUnit(OperatingUnit ou)
        {
            Pns.Xml_Serialization.PnsProblem.OperatingUnit opUnit = new Pns.Xml_Serialization.PnsProblem.OperatingUnit();
            opUnit.name = ou.Name;
            opUnit.description = ou.CommentText;
            opUnit.investmentFix = ou.InvestmentCostFixProp.Value;
            // TODO: input és output materialokat hozzárendelni az opunithoz
            //opUnit.inputMaterial
            //opUnit.outputMaterial
            opUnit.investmentProp = ou.InvestmentCostPropProp.Value;
            opUnit.lowerBoundSpecified = false;
            opUnit.operatingFix = ou.OperatingCostFixProp.Value;
            opUnit.operatingProp = ou.OperatingCostPropProp.Value;
            opUnit.payoutPeriod = ou.PayoutPeriodProp.Value;
            opUnit.totalFix = ou.OperatingCostFixProp.Value + ou.InvestmentCostFixProp.Value;
            opUnit.totalProp = ou.OperatingCostPropProp.Value + ou.InvestmentCostPropProp.Value;
            opUnit.workingHoursPerYear = (int)ou.WorkingHourProp.Value;

            return opUnit;
        }
    }
}
