using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    class Default
    {
        #region Enums
        public enum MassUnit
        {
            g,
            kg,
            t
        }

        public enum VolumeUnit
        {
            m3,
            dm3,
            cm3
        }

        public enum SubstanceUnit
        {
            mol,
            mmol,
            kmol
        }

        public enum EnergyUnit
        {
            J,
            kJ,
            MJ,
            GJ,
            TJ,
            Wh,
            kWh,
            MWh,
            GWh,
            TWh
        }

        public enum LengthUnit
        {
            m,
            km,
            dm,
            cm,
            mm,
            coll
        }

        public enum CurrentUnit
        {
            A,
            mA,
            kA
        }

        public enum AreaUnit
        {
            m2,
            km2,
            cm2,
            ha
        }

        public enum SpeedUnit
        {
            mps,
            kmph,
            Mph
        }

        public enum AccelerationUnit
        {
            mps2
        }

        public enum MassDensityUnit
        {
            kgpm3,
            tpm3
        }

        public enum ThermoTempUnit
        {
            K
        }

        public enum LuminIntensUnit
        {
            cd
        }

        public enum ConcentrationUnit
        {
            molpm3,
            molpdm3
        }

        public enum ForceUnit
        {
            N
        }

        public enum PressureUnit
        {
            Pa,
            kPa,
            MPa
        }

        public enum PowerUnit
        {
            W,
            kW,
            MW,
            GW,
            TW
        }

        public enum CapacityUnit
        {
            unit
        }

        public enum TimeUnit
        {
            s,
            h,
            d,
            w,
            m,
            y
        }

        public enum MoneyUnit
        {
            EUR,
            HUF,
            USD
        }
        #endregion

        #region Values
        static public double flow_rate_lower_bound = 0; //flow_rate_lower_bound
        static public double flow_rate_upper_bound = 1000000000; //flow_rate_upper_bound
        static public double price = 0; //price
        static public int type = Globals.MaterialTypes.Intermediate; //type

        static public string tempFolder = Path.GetTempPath(); //temporary folder

        static public int limit = 10;
        static public int processes = 1;
        static public bool online = true;

        static public string host = "193.6.33.141";
        static public int port = 51000;

        static public double o_fix = 0; //fix_cost
        static public double i_fix = 0; //fix_cost

        public static double fix_cost = o_fix + i_fix;

        static public double o_prop = 0; //proportional_cost
        static public double i_prop = 0; //proportional_cost

        public static double prop_cost = o_prop + i_prop;

        static public double capacity_lower_bound = 0; //capacity_lower_bound
        static public double capacity_upper_bound = 1000000000; //capacity_upper_bound

        static public double io_flowrate = 1; //flow rate

        static public MassUnit mass_mu = MassUnit.t;
        static public VolumeUnit vol_mu = VolumeUnit.m3;
        static public SubstanceUnit sub_mu = SubstanceUnit.mol;
        static public EnergyUnit energy_mu = EnergyUnit.J;
        static public LengthUnit length_mu = LengthUnit.m;
        static public CurrentUnit curr_mu = CurrentUnit.A;
        static public AreaUnit area_mu = AreaUnit.m2;
        static public SpeedUnit speed_mu = SpeedUnit.mps;
        static public AccelerationUnit acc_mu = AccelerationUnit.mps2;
        static public MassDensityUnit mdens_mu = MassDensityUnit.tpm3;
        static public ThermoTempUnit temp_mu = ThermoTempUnit.K;
        static public LuminIntensUnit lum_mu = LuminIntensUnit.cd;
        static public ConcentrationUnit conc_mu = ConcentrationUnit.molpm3;
        static public ForceUnit force_mu = ForceUnit.N;
        static public PressureUnit press_mu = PressureUnit.Pa;
        static public PowerUnit power_mu = PowerUnit.W;
        static public CapacityUnit cap_mu = CapacityUnit.unit;
        static public TimeUnit time_mu = TimeUnit.y;
        static public MoneyUnit money_mu = MoneyUnit.EUR;

        public const double d_NperA = -1;
        static public double payout_period = 10;
        static public int working_hours_per_year = 8000;

        static public double d_tolerance = 0.00000001;
        static public double d_solution_tolerance = 0.0001;
        static public int max_solutions = 10;
        static public bool b_auto_convert = false;
        #endregion

        public Default() { }
    }
}
