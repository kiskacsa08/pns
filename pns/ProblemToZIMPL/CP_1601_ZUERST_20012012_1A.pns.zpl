# Set of raw materials
set raw_materials := {"agricultural_field", "biogas_pot_from_manure", "solar_area_for_PV_parks", "solar_area_for_thermal_and_PV", "wind_potential",
   "electricity", "methanol", "KOH", "inv_cost", "op_cost",
   "transport_costs", "wood", "grass_silage", "CaCO3", "diesel"
};

# Set of intermediates
set intermediates := {"unspec_cereal", "sugar_beet", "corn_grains", "wheat", "barley",
   "rapeseed", "unspec_biomass_for_burning", "rapeseed_dried", "sunflower", "sunflower_dried",
   "unspec_oil_seed", "Miscanthus_chopped", "Miscanthus", "shortrotation", "unspec_biomass_for_biogas",
   "corn_silage", "stillage_DM", "press_cake", "heat_LT", "heat_HT",
   "biogas", "biogas_input", "glycerin", "ethanol_input", "wood_furnace_input",
   "biomass_furnace_input", "ORC_input", "dried_woodchips", "ip_wood", "ip_grass_silage",
   "wood_dryer_input", "wood_chopper_and_splitter_input", "woodchips", "split_logs", "solar_area_for_PV"
};

# Set of required or potential products
set products := {"ethanol", "vegetable_oil", "biodiesel", "ash", "biogas_manure",
   "upgraded_biogas", "electricity_biomass", "electricity_biogas", "pellets", "industrial_heat_demand",
   "district_heat_demand", "individual_heating", "electricity_PV", "electricity_wind", "K3PO4",
   "waste_heat"
};

# Set of all materials
set materials := raw_materials union intermediates union products;

# Lower bounds of materials flow rates
param flow_rate_lower_bound[raw_materials union products] := 
   <"industrial_heat_demand"> 18960,
   <"individual_heating"> 86554
   default 0;

#  Upper bounds of materials flow rates
param flow_rate_upper_bound[materials] := 
   <"agricultural_field"> 1158,
   <"biogas_pot_from_manure"> 2732900,
   <"solar_area_for_PV_parks"> 0,
   <"solar_area_for_thermal_and_PV"> 2.1324,
   <"wind_potential"> 0,
   <"wood"> 50332,
   <"grass_silage"> 6711,
   <"heat_LT"> 0,
   <"heat_HT"> 0,
   <"industrial_heat_demand"> 18960,
   <"district_heat_demand"> 0,
   <"individual_heating"> 86554,
   <"waste_heat"> 10000
   default 100000000;

# Prices of raw materials and products
param price[raw_materials union products] := 
   <"electricity"> 110,
   <"methanol"> 230,
   <"inv_cost"> 1,
   <"op_cost"> 1,
   <"transport_costs"> 1,
   <"CaCO3"> 30,
   <"diesel"> 1450,
   <"ethanol"> 569.62,
   <"vegetable_oil"> 975,
   <"biodiesel"> 955,
   <"upgraded_biogas"> 0.752,
   <"electricity_biomass"> 156,
   <"electricity_biogas"> 156,
   <"pellets"> 197,
   <"industrial_heat_demand"> 45,
   <"district_heat_demand"> 45,
   <"individual_heating"> 45,
   <"electricity_PV"> 380,
   <"electricity_wind"> 97
   default 0;

# Set of operating units
set operating_units := {"field_unspec_cereal", "field_sugar_beet", "field_corn_grains", "field_wheat", "field_barley",
   "field_rapeseed", "field_sunflower", "field_unspec_oil_seed", "field_unspec_biomass_for_burning", "field_Misanthus",
   "field_shortrotation", "field_unspec_biomass_for_biogas", "field_corn_silage", "wood_price", "grass_silage_price",
   "biogas_input_potential_manure", "biogas_input_unspec_biomass", "biogas_input_corn_silage", "biogas_input_grass_silage", "biogas_input_stillage",
   "biogas_input_glycerin", "biogas_input_press_cake", "Fermentation_45_5m3", "Fermentation_284_3m3", "Fermentation_above_284_3m3",
   "Biogas_cleaning_260m3", "Drying_rapeseed", "Drying_rapeseed_x10", "Dyring_sunflower", "Dyring_sunflower_x10",
   "Pressing_rapeseed", "Pressing_sunflower", "Biodiesel_plant_8000", "Biodiesel_plant_25000", "Biodiesel_plant_100000",
   "Biogas_CHP_300kW", "Biogas_CHP_1MW", "Biogas_CHP_above1MW", "Biogas_burner_HT_300kW", "Biogas_burner_HT_1MW",
   "Biogas_burner_HT_above1MW", "Biogas_burner_LT_300kW", "Biogas_burner_LT_1MW", "Biogas_burner_LT_above1MW", "Ethanol_corn_10000",
   "Ethanol_corn_100000", "Ethanol_corn_1000000", "Ethanol_sugarbeet_10000", "Ethanol_sugarbeet_100000", "Ethanol_sugarbeet_1000000",
   "Ethanol_wheat_barley_unsp_10000", "Ethanol_wheat_barley_unsp_100000", "Ethanol_wheat_barley_unsp_1000000", "wheat_to_ethanol_input", "barley_to_ethanol_input",
   "unspec_to_ethanol_input", "woodchips_to_furnace", "Wood_furnace_HT_300kW", "Wood_furnace_HT_1MW", "Wood_furnace_HT_above1MW",
   "Wood_furnace_LT_300kW", "Wood_furnace_LT_1MW", "Wood_furnace_LT_above1MW", "miscanthus_to_biomass_furnace", "unspec_to_biomass_furnace",
   "Biomass_furnace_HT_300kW", "Biomass_furnace_HT_1MW", "Biomass_furnace_HT_above1MW", "Biomass_furnace_LT_300kW", "Biomass_furnace_LT_1MW",
   "Biomass_furnace_LT_above1MW", "shortrotation_to_ORC_input", "woodchips_to_ORC_input", "ORC", "shortrotation_to_dryer",
   "woodchips_to_dryer_input", "Drying_wood_10000", "Drying_wood_100000", "Drying_wood_1000000", "Pelletizing_wood_10000",
   "Pelletizing_wood_100000", "Pelletizing_wood_1000000", "PV_parks_to_solar_area_PV", "solar_area_for_PV", "PV",
   "Solar_thermal", "Wind_potential", "district_heating", "industrial_heating", "pellets_individual_heating",
   "split_logs_individual_heating", "Gasification_10000", "Gasification_100000", "Gasification_1000000", "wood_to_chopper_and_splitter",
   "shortrotation_to_chopper_and_splitter", "Chopper_7m3", "Chopper_22_5m3", "Chopper_90m3", "Chopper_Miscanthus",
   "Splitter_wood", "heat_LT_to_wastheat", "heat_HT_to_wastheat"
};

# Lower bounds of operating units capacities
param capacity_lower_bound[operating_units] := 
   <"Fermentation_284_3m3"> 1.01,
   <"Fermentation_above_284_3m3"> 6.26,
   <"Drying_rapeseed_x10"> 1.01,
   <"Dyring_sunflower_x10"> 1.01,
   <"Biodiesel_plant_25000"> 1.01,
   <"Biodiesel_plant_100000"> 3.126,
   <"Biogas_CHP_1MW"> 1.01,
   <"Biogas_CHP_above1MW"> 3.34,
   <"Biogas_burner_HT_1MW"> 1.01,
   <"Biogas_burner_HT_above1MW"> 3.34,
   <"Biogas_burner_LT_1MW"> 1.01,
   <"Biogas_burner_LT_above1MW"> 3.34,
   <"Ethanol_corn_100000"> 1.01,
   <"Ethanol_corn_1000000"> 10.1,
   <"Ethanol_sugarbeet_100000"> 1.01,
   <"Ethanol_sugarbeet_1000000"> 10.01,
   <"Ethanol_wheat_barley_unsp_100000"> 1.01,
   <"Ethanol_wheat_barley_unsp_1000000"> 10.01,
   <"Wood_furnace_HT_1MW"> 1.01,
   <"Wood_furnace_HT_above1MW"> 3.34,
   <"Wood_furnace_LT_1MW"> 1.01,
   <"Wood_furnace_LT_above1MW"> 3.34,
   <"Biomass_furnace_HT_1MW"> 1.01,
   <"Biomass_furnace_HT_above1MW"> 3.34,
   <"Biomass_furnace_LT_1MW"> 1.01,
   <"Biomass_furnace_LT_above1MW"> 3.34,
   <"ORC"> 0.3,
   <"Drying_wood_100000"> 1.01,
   <"Drying_wood_1000000"> 10.01,
   <"Pelletizing_wood_100000"> 1.01,
   <"Pelletizing_wood_1000000"> 10.01,
   <"Gasification_100000"> 1.01,
   <"Gasification_1000000"> 10.01,
   <"Chopper_22_5m3"> 0.311,
   <"Chopper_90m3"> 0.25
   default 0;

# Upper bounds of operating units capacities
param capacity_upper_bound[operating_units] := 
   <"Fermentation_45_5m3"> 1,
   <"Fermentation_284_3m3"> 6.25,
   <"Drying_rapeseed"> 1,
   <"Drying_rapeseed_x10"> 10,
   <"Dyring_sunflower"> 1,
   <"Dyring_sunflower_x10"> 10,
   <"Biodiesel_plant_8000"> 1,
   <"Biodiesel_plant_25000"> 3.125,
   <"Biogas_CHP_1MW"> 3.33,
   <"Biogas_burner_HT_300kW"> 1,
   <"Biogas_burner_HT_1MW"> 3.33,
   <"Biogas_burner_LT_300kW"> 1,
   <"Biogas_burner_LT_1MW"> 3.33,
   <"Ethanol_corn_10000"> 1,
   <"Ethanol_corn_100000"> 10,
   <"Ethanol_sugarbeet_10000"> 1,
   <"Ethanol_sugarbeet_100000"> 10,
   <"Ethanol_wheat_barley_unsp_10000"> 1,
   <"Ethanol_wheat_barley_unsp_100000"> 10,
   <"Wood_furnace_HT_300kW"> 1,
   <"Wood_furnace_HT_1MW"> 3.33,
   <"Wood_furnace_LT_300kW"> 1,
   <"Wood_furnace_LT_1MW"> 3.33,
   <"Biomass_furnace_HT_300kW"> 1,
   <"Biomass_furnace_HT_1MW"> 3.33,
   <"Biomass_furnace_LT_300kW"> 1,
   <"Biomass_furnace_LT_1MW"> 3.33,
   <"ORC"> 1,
   <"Drying_wood_10000"> 1,
   <"Drying_wood_100000"> 10,
   <"Pelletizing_wood_10000"> 1,
   <"Pelletizing_wood_100000"> 10,
   <"Gasification_10000"> 1,
   <"Gasification_100000"> 10,
   <"Chopper_7m3"> 1,
   <"Chopper_22_5m3"> 1
   default 100000;

# Fix costs of operating units
param fix_cost[operating_units] := 
   0;

# Proportional costs of operating units
param proportional_cost[operating_units] := 
   0;

# Input and output flow rates of materials to operating units
param material_to_operating_unit_flow_rates[materials*operating_units] := 
# field_unspec_cereal: agricultural_field + 0 op_cost + 0 transport_costs => 0 unspec_cereal
   <"agricultural_field", "field_unspec_cereal"> -1,
   <"op_cost", "field_unspec_cereal"> -0,
   <"transport_costs", "field_unspec_cereal"> -0,
   <"unspec_cereal", "field_unspec_cereal"> 0,
# field_sugar_beet: agricultural_field + 1068.2 op_cost + 0 transport_costs => 0 sugar_beet
   <"agricultural_field", "field_sugar_beet"> -1,
   <"op_cost", "field_sugar_beet"> -1068.2,
   <"transport_costs", "field_sugar_beet"> -0,
   <"sugar_beet", "field_sugar_beet"> 0,
# field_corn_grains: agricultural_field + 576.5 op_cost + 0 transport_costs => 0 corn_grains
   <"agricultural_field", "field_corn_grains"> -1,
   <"op_cost", "field_corn_grains"> -576.5,
   <"transport_costs", "field_corn_grains"> -0,
   <"corn_grains", "field_corn_grains"> 0,
# field_wheat: agricultural_field + 451.57 op_cost + 0 transport_costs => 0 wheat
   <"agricultural_field", "field_wheat"> -1,
   <"op_cost", "field_wheat"> -451.57,
   <"transport_costs", "field_wheat"> -0,
   <"wheat", "field_wheat"> 0,
# field_barley: agricultural_field + 403.6 op_cost + 0 transport_costs => 0 barley
   <"agricultural_field", "field_barley"> -1,
   <"op_cost", "field_barley"> -403.6,
   <"transport_costs", "field_barley"> -0,
   <"barley", "field_barley"> 0,
# field_rapeseed: agricultural_field + 513.6 op_cost + 0 transport_costs => 3 rapeseed
   <"agricultural_field", "field_rapeseed"> -1,
   <"op_cost", "field_rapeseed"> -513.6,
   <"transport_costs", "field_rapeseed"> -0,
   <"rapeseed", "field_rapeseed"> 3,
# field_sunflower: agricultural_field + 493.99 op_cost + 0 transport_costs => 2.3 sunflower
   <"agricultural_field", "field_sunflower"> -1,
   <"op_cost", "field_sunflower"> -493.99,
   <"transport_costs", "field_sunflower"> -0,
   <"sunflower", "field_sunflower"> 2.3,
# field_unspec_oil_seed: agricultural_field + 0 op_cost + 0 transport_costs => 0 unspec_oil_seed
   <"agricultural_field", "field_unspec_oil_seed"> -1,
   <"op_cost", "field_unspec_oil_seed"> -0,
   <"transport_costs", "field_unspec_oil_seed"> -0,
   <"unspec_oil_seed", "field_unspec_oil_seed"> 0,
# field_unspec_biomass_for_burning: agricultural_field + 0 op_cost + 0 transport_costs => 0 unspec_biomass_for_burning
   <"agricultural_field", "field_unspec_biomass_for_burning"> -1,
   <"op_cost", "field_unspec_biomass_for_burning"> -0,
   <"transport_costs", "field_unspec_biomass_for_burning"> -0,
   <"unspec_biomass_for_burning", "field_unspec_biomass_for_burning"> 0,
# field_Misanthus: agricultural_field + 2337 op_cost + 0 transport_costs => 19 Miscanthus
   <"agricultural_field", "field_Misanthus"> -1,
   <"op_cost", "field_Misanthus"> -2337,
   <"transport_costs", "field_Misanthus"> -0,
   <"Miscanthus", "field_Misanthus"> 19,
# field_shortrotation: agricultural_field + 420 op_cost + 0 transport_costs => 0 shortrotation
   <"agricultural_field", "field_shortrotation"> -1,
   <"op_cost", "field_shortrotation"> -420,
   <"transport_costs", "field_shortrotation"> -0,
   <"shortrotation", "field_shortrotation"> 0,
# field_unspec_biomass_for_biogas: agricultural_field + 0 op_cost + 0 transport_costs => 0 unspec_biomass_for_biogas
   <"agricultural_field", "field_unspec_biomass_for_biogas"> -1,
   <"op_cost", "field_unspec_biomass_for_biogas"> -0,
   <"transport_costs", "field_unspec_biomass_for_biogas"> -0,
   <"unspec_biomass_for_biogas", "field_unspec_biomass_for_biogas"> 0,
# field_corn_silage: agricultural_field + 1732.5 op_cost + 0 transport_costs => 49.5 corn_silage
   <"agricultural_field", "field_corn_silage"> -1,
   <"op_cost", "field_corn_silage"> -1732.5,
   <"transport_costs", "field_corn_silage"> -0,
   <"corn_silage", "field_corn_silage"> 49.5,
# wood_price: 42 op_cost + 0 transport_costs + wood => ip_wood
   <"op_cost", "wood_price"> -42,
   <"transport_costs", "wood_price"> -0,
   <"wood", "wood_price"> -1,
   <"ip_wood", "wood_price"> 1,
# grass_silage_price: 25 op_cost + 0 transport_costs + grass_silage => ip_grass_silage
   <"op_cost", "grass_silage_price"> -25,
   <"transport_costs", "grass_silage_price"> -0,
   <"grass_silage", "grass_silage_price"> -1,
   <"ip_grass_silage", "grass_silage_price"> 1,
# biogas_input_potential_manure: biogas_pot_from_manure + 0 transport_costs => biogas_input
   <"biogas_pot_from_manure", "biogas_input_potential_manure"> -1,
   <"transport_costs", "biogas_input_potential_manure"> -0,
   <"biogas_input", "biogas_input_potential_manure"> 1,
# biogas_input_unspec_biomass: unspec_biomass_for_biogas => 0 biogas_input
   <"unspec_biomass_for_biogas", "biogas_input_unspec_biomass"> -1,
   <"biogas_input", "biogas_input_unspec_biomass"> 0,
# biogas_input_corn_silage: corn_silage => 199 biogas_input
   <"corn_silage", "biogas_input_corn_silage"> -1,
   <"biogas_input", "biogas_input_corn_silage"> 199,
# biogas_input_grass_silage: ip_grass_silage => 207 biogas_input
   <"ip_grass_silage", "biogas_input_grass_silage"> -1,
   <"biogas_input", "biogas_input_grass_silage"> 207,
# biogas_input_stillage: stillage_DM => 603 biogas_input
   <"stillage_DM", "biogas_input_stillage"> -1,
   <"biogas_input", "biogas_input_stillage"> 603,
# biogas_input_glycerin: glycerin => 1378 biogas_input
   <"glycerin", "biogas_input_glycerin"> -1,
   <"biogas_input", "biogas_input_glycerin"> 1378,
# biogas_input_press_cake: press_cake => 625 biogas_input
   <"press_cake", "biogas_input_press_cake"> -1,
   <"biogas_input", "biogas_input_press_cake"> 625,
# Fermentation_45_5m3: 364000 biogas_input + 28337 inv_cost + 86111 op_cost => 364000 biogas + 1640 biogas_manure
   <"biogas_input", "Fermentation_45_5m3"> -364000,
   <"inv_cost", "Fermentation_45_5m3"> -28337,
   <"op_cost", "Fermentation_45_5m3"> -86111,
   <"biogas", "Fermentation_45_5m3"> 364000,
   <"biogas_manure", "Fermentation_45_5m3"> 1640,
# Fermentation_284_3m3: 364000 biogas_input + 16353 inv_cost + 86111 op_cost => 364000 biogas + 1640 biogas_manure
   <"biogas_input", "Fermentation_284_3m3"> -364000,
   <"inv_cost", "Fermentation_284_3m3"> -16353,
   <"op_cost", "Fermentation_284_3m3"> -86111,
   <"biogas", "Fermentation_284_3m3"> 364000,
   <"biogas_manure", "Fermentation_284_3m3"> 1640,
# Fermentation_above_284_3m3: 364000 biogas_input + 10789 inv_cost + 86111 op_cost => 364000 biogas + 1640 biogas_manure
   <"biogas_input", "Fermentation_above_284_3m3"> -364000,
   <"inv_cost", "Fermentation_above_284_3m3"> -10789,
   <"op_cost", "Fermentation_above_284_3m3"> -86111,
   <"biogas", "Fermentation_above_284_3m3"> 364000,
   <"biogas_manure", "Fermentation_above_284_3m3"> 1640,
# Biogas_cleaning_260m3: 3464000 biogas + 686.4 electricity + 54322 inv_cost + 50463 op_cost => 2080000 upgraded_biogas
   <"biogas", "Biogas_cleaning_260m3"> -3464000,
   <"electricity", "Biogas_cleaning_260m3"> -686.4,
   <"inv_cost", "Biogas_cleaning_260m3"> -54322,
   <"op_cost", "Biogas_cleaning_260m3"> -50463,
   <"upgraded_biogas", "Biogas_cleaning_260m3"> 2080000,
# Drying_rapeseed: 1333 rapeseed + 37232 inv_cost + 138.632 heat_HT + 44983 op_cost => 1133 rapeseed_dried
   <"rapeseed", "Drying_rapeseed"> -1333,
   <"inv_cost", "Drying_rapeseed"> -37232,
   <"heat_HT", "Drying_rapeseed"> -138.632,
   <"op_cost", "Drying_rapeseed"> -44983,
   <"rapeseed_dried", "Drying_rapeseed"> 1133,
# Drying_rapeseed_x10: 1333 rapeseed + 18660 inv_cost + 138.632 heat_HT + 44983 op_cost => 1133 rapeseed_dried
   <"rapeseed", "Drying_rapeseed_x10"> -1333,
   <"inv_cost", "Drying_rapeseed_x10"> -18660,
   <"heat_HT", "Drying_rapeseed_x10"> -138.632,
   <"op_cost", "Drying_rapeseed_x10"> -44983,
   <"rapeseed_dried", "Drying_rapeseed_x10"> 1133,
# Dyring_sunflower: 1333 sunflower + 21688 inv_cost + 103.308 heat_HT + 44983 op_cost => 1173 sunflower_dried
   <"sunflower", "Dyring_sunflower"> -1333,
   <"inv_cost", "Dyring_sunflower"> -21688,
   <"heat_HT", "Dyring_sunflower"> -103.308,
   <"op_cost", "Dyring_sunflower"> -44983,
   <"sunflower_dried", "Dyring_sunflower"> 1173,
# Dyring_sunflower_x10: 1333 sunflower + 10870 inv_cost + 103.308 heat_HT + 44983 op_cost => 1173 sunflower_dried
   <"sunflower", "Dyring_sunflower_x10"> -1333,
   <"inv_cost", "Dyring_sunflower_x10"> -10870,
   <"heat_HT", "Dyring_sunflower_x10"> -103.308,
   <"op_cost", "Dyring_sunflower_x10"> -44983,
   <"sunflower_dried", "Dyring_sunflower_x10"> 1173,
# Pressing_rapeseed: 1440 rapeseed_dried + 12394 inv_cost + 117949 op_cost => 475.2 vegetable_oil + 964.8 press_cake
   <"rapeseed_dried", "Pressing_rapeseed"> -1440,
   <"inv_cost", "Pressing_rapeseed"> -12394,
   <"op_cost", "Pressing_rapeseed"> -117949,
   <"vegetable_oil", "Pressing_rapeseed"> 475.2,
   <"press_cake", "Pressing_rapeseed"> 964.8,
# Pressing_sunflower: 12919 inv_cost + 117949 op_cost + 1440 sunflower_dried => 504 vegetable_oil + 936 press_cake
   <"inv_cost", "Pressing_sunflower"> -12919,
   <"op_cost", "Pressing_sunflower"> -117949,
   <"sunflower_dried", "Pressing_sunflower"> -1440,
   <"vegetable_oil", "Pressing_sunflower"> 504,
   <"press_cake", "Pressing_sunflower"> 936,
# Biodiesel_plant_8000: 8000 vegetable_oil + 800 methanol + 8 KOH + 631291 inv_cost + 396788 op_cost + 0.08 electricity => 8000 biodiesel + 800 glycerin + 8 K3PO4
   <"vegetable_oil", "Biodiesel_plant_8000"> -8000,
   <"methanol", "Biodiesel_plant_8000"> -800,
   <"KOH", "Biodiesel_plant_8000"> -8,
   <"inv_cost", "Biodiesel_plant_8000"> -631291,
   <"op_cost", "Biodiesel_plant_8000"> -396788,
   <"electricity", "Biodiesel_plant_8000"> -0.08,
   <"biodiesel", "Biodiesel_plant_8000"> 8000,
   <"glycerin", "Biodiesel_plant_8000"> 800,
   <"K3PO4", "Biodiesel_plant_8000"> 8,
# Biodiesel_plant_25000: 8000 vegetable_oil + 800 methanol + 8 KOH + 448513 inv_cost + 396788 op_cost + 0.08 electricity => 8000 biodiesel + 800 glycerin + 8 K3PO4
   <"vegetable_oil", "Biodiesel_plant_25000"> -8000,
   <"methanol", "Biodiesel_plant_25000"> -800,
   <"KOH", "Biodiesel_plant_25000"> -8,
   <"inv_cost", "Biodiesel_plant_25000"> -448513,
   <"op_cost", "Biodiesel_plant_25000"> -396788,
   <"electricity", "Biodiesel_plant_25000"> -0.08,
   <"biodiesel", "Biodiesel_plant_25000"> 8000,
   <"glycerin", "Biodiesel_plant_25000"> 800,
   <"K3PO4", "Biodiesel_plant_25000"> 8,
# Biodiesel_plant_100000: 8000 vegetable_oil + 800 methanol + 8 KOH + 295908 inv_cost + 396788 op_cost + 0.08 electricity => 8000 biodiesel + 800 glycerin + 8 K3PO4
   <"vegetable_oil", "Biodiesel_plant_100000"> -8000,
   <"methanol", "Biodiesel_plant_100000"> -800,
   <"KOH", "Biodiesel_plant_100000"> -8,
   <"inv_cost", "Biodiesel_plant_100000"> -295908,
   <"op_cost", "Biodiesel_plant_100000"> -396788,
   <"electricity", "Biodiesel_plant_100000"> -0.08,
   <"biodiesel", "Biodiesel_plant_100000"> 8000,
   <"glycerin", "Biodiesel_plant_100000"> 800,
   <"K3PO4", "Biodiesel_plant_100000"> 8,
# Biogas_CHP_300kW: 722952 biogas + 26267 inv_cost + 17184 op_cost => 1528 heat_LT + 2400 electricity_biogas
   <"biogas", "Biogas_CHP_300kW"> -722952,
   <"inv_cost", "Biogas_CHP_300kW"> -26267,
   <"op_cost", "Biogas_CHP_300kW"> -17184,
   <"heat_LT", "Biogas_CHP_300kW"> 1528,
   <"electricity_biogas", "Biogas_CHP_300kW"> 2400,
# Biogas_CHP_1MW: 722952 biogas + 18305 inv_cost + 17184 op_cost => 1528 heat_LT + 2400 electricity_biogas
   <"biogas", "Biogas_CHP_1MW"> -722952,
   <"inv_cost", "Biogas_CHP_1MW"> -18305,
   <"op_cost", "Biogas_CHP_1MW"> -17184,
   <"heat_LT", "Biogas_CHP_1MW"> 1528,
   <"electricity_biogas", "Biogas_CHP_1MW"> 2400,
# Biogas_CHP_above1MW: 722952 biogas + 11294 inv_cost + 17184 op_cost => 1528 heat_LT + 2400 electricity_biogas
   <"biogas", "Biogas_CHP_above1MW"> -722952,
   <"inv_cost", "Biogas_CHP_above1MW"> -11294,
   <"op_cost", "Biogas_CHP_above1MW"> -17184,
   <"heat_LT", "Biogas_CHP_above1MW"> 1528,
   <"electricity_biogas", "Biogas_CHP_above1MW"> 2400,
# Biogas_burner_HT_300kW: 681824 biogas + 2829 inv_cost + 3678 op_cost + 3.8 electricity => 2400 heat_HT + 872 heat_LT
   <"biogas", "Biogas_burner_HT_300kW"> -681824,
   <"inv_cost", "Biogas_burner_HT_300kW"> -2829,
   <"op_cost", "Biogas_burner_HT_300kW"> -3678,
   <"electricity", "Biogas_burner_HT_300kW"> -3.8,
   <"heat_HT", "Biogas_burner_HT_300kW"> 2400,
   <"heat_LT", "Biogas_burner_HT_300kW"> 872,
# Biogas_burner_HT_1MW: 681824 biogas + 1972 inv_cost + 3678 op_cost + 3.8 electricity => 2400 heat_HT + 872 heat_LT
   <"biogas", "Biogas_burner_HT_1MW"> -681824,
   <"inv_cost", "Biogas_burner_HT_1MW"> -1972,
   <"op_cost", "Biogas_burner_HT_1MW"> -3678,
   <"electricity", "Biogas_burner_HT_1MW"> -3.8,
   <"heat_HT", "Biogas_burner_HT_1MW"> 2400,
   <"heat_LT", "Biogas_burner_HT_1MW"> 872,
# Biogas_burner_HT_above1MW: 681824 biogas + 1216 inv_cost + 3678 op_cost + 3.8 electricity => 2400 heat_HT + 872 heat_LT
   <"biogas", "Biogas_burner_HT_above1MW"> -681824,
   <"inv_cost", "Biogas_burner_HT_above1MW"> -1216,
   <"op_cost", "Biogas_burner_HT_above1MW"> -3678,
   <"electricity", "Biogas_burner_HT_above1MW"> -3.8,
   <"heat_HT", "Biogas_burner_HT_above1MW"> 2400,
   <"heat_LT", "Biogas_burner_HT_above1MW"> 872,
# Biogas_burner_LT_300kW: 428067 biogas + 1888 inv_cost + 2064 op_cost + 2.39 electricity => 2400 heat_LT
   <"biogas", "Biogas_burner_LT_300kW"> -428067,
   <"inv_cost", "Biogas_burner_LT_300kW"> -1888,
   <"op_cost", "Biogas_burner_LT_300kW"> -2064,
   <"electricity", "Biogas_burner_LT_300kW"> -2.39,
   <"heat_LT", "Biogas_burner_LT_300kW"> 2400,
# Biogas_burner_LT_1MW: 428067 biogas + 1316 inv_cost + 2064 op_cost + 2.39 electricity => 2400 heat_LT
   <"biogas", "Biogas_burner_LT_1MW"> -428067,
   <"inv_cost", "Biogas_burner_LT_1MW"> -1316,
   <"op_cost", "Biogas_burner_LT_1MW"> -2064,
   <"electricity", "Biogas_burner_LT_1MW"> -2.39,
   <"heat_LT", "Biogas_burner_LT_1MW"> 2400,
# Biogas_burner_LT_above1MW: 428067 biogas + 812 inv_cost + 2064 op_cost + 2.39 electricity => 2400 heat_LT
   <"biogas", "Biogas_burner_LT_above1MW"> -428067,
   <"inv_cost", "Biogas_burner_LT_above1MW"> -812,
   <"op_cost", "Biogas_burner_LT_above1MW"> -2064,
   <"electricity", "Biogas_burner_LT_above1MW"> -2.39,
   <"heat_LT", "Biogas_burner_LT_above1MW"> 2400,
# Ethanol_corn_10000: 33150 corn_grains + 4452 heat_HT + 537.6 electricity + 448169 inv_cost + 1206324 op_cost => 12540 stillage_DM + 10000 ethanol
   <"corn_grains", "Ethanol_corn_10000"> -33150,
   <"heat_HT", "Ethanol_corn_10000"> -4452,
   <"electricity", "Ethanol_corn_10000"> -537.6,
   <"inv_cost", "Ethanol_corn_10000"> -448169,
   <"op_cost", "Ethanol_corn_10000"> -1206324,
   <"stillage_DM", "Ethanol_corn_10000"> 12540,
   <"ethanol", "Ethanol_corn_10000"> 10000,
# Ethanol_corn_100000: 33150 corn_grains + 4452 heat_HT + 537.6 electricity + 224617 inv_cost + 1206324 op_cost => 12540 stillage_DM + 10000 ethanol
   <"corn_grains", "Ethanol_corn_100000"> -33150,
   <"heat_HT", "Ethanol_corn_100000"> -4452,
   <"electricity", "Ethanol_corn_100000"> -537.6,
   <"inv_cost", "Ethanol_corn_100000"> -224617,
   <"op_cost", "Ethanol_corn_100000"> -1206324,
   <"stillage_DM", "Ethanol_corn_100000"> 12540,
   <"ethanol", "Ethanol_corn_100000"> 10000,
# Ethanol_corn_1000000: 33150 corn_grains + 4452 heat_HT + 537.6 electricity + 161550 inv_cost + 1206324 op_cost => 12540 stillage_DM + 10000 ethanol
   <"corn_grains", "Ethanol_corn_1000000"> -33150,
   <"heat_HT", "Ethanol_corn_1000000"> -4452,
   <"electricity", "Ethanol_corn_1000000"> -537.6,
   <"inv_cost", "Ethanol_corn_1000000"> -161550,
   <"op_cost", "Ethanol_corn_1000000"> -1206324,
   <"stillage_DM", "Ethanol_corn_1000000"> 12540,
   <"ethanol", "Ethanol_corn_1000000"> 10000,
# Ethanol_sugarbeet_10000: 10354.69 heat_HT + 1481.67 electricity + 448169 inv_cost + 1206324 op_cost + 75534.42 sugar_beet => 16383.6 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_sugarbeet_10000"> -10354.69,
   <"electricity", "Ethanol_sugarbeet_10000"> -1481.67,
   <"inv_cost", "Ethanol_sugarbeet_10000"> -448169,
   <"op_cost", "Ethanol_sugarbeet_10000"> -1206324,
   <"sugar_beet", "Ethanol_sugarbeet_10000"> -75534.42,
   <"stillage_DM", "Ethanol_sugarbeet_10000"> 16383.6,
   <"ethanol", "Ethanol_sugarbeet_10000"> 10000,
# Ethanol_sugarbeet_100000: 10354.69 heat_HT + 1481.67 electricity + 224617 inv_cost + 1206324 op_cost + 75534.42 sugar_beet => 16383.6 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_sugarbeet_100000"> -10354.69,
   <"electricity", "Ethanol_sugarbeet_100000"> -1481.67,
   <"inv_cost", "Ethanol_sugarbeet_100000"> -224617,
   <"op_cost", "Ethanol_sugarbeet_100000"> -1206324,
   <"sugar_beet", "Ethanol_sugarbeet_100000"> -75534.42,
   <"stillage_DM", "Ethanol_sugarbeet_100000"> 16383.6,
   <"ethanol", "Ethanol_sugarbeet_100000"> 10000,
# Ethanol_sugarbeet_1000000: 10354.69 heat_HT + 1481.67 electricity + 161550 inv_cost + 1206324 op_cost + 75534.2 sugar_beet => 16383.6 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_sugarbeet_1000000"> -10354.69,
   <"electricity", "Ethanol_sugarbeet_1000000"> -1481.67,
   <"inv_cost", "Ethanol_sugarbeet_1000000"> -161550,
   <"op_cost", "Ethanol_sugarbeet_1000000"> -1206324,
   <"sugar_beet", "Ethanol_sugarbeet_1000000"> -75534.2,
   <"stillage_DM", "Ethanol_sugarbeet_1000000"> 16383.6,
   <"ethanol", "Ethanol_sugarbeet_1000000"> 10000,
# Ethanol_wheat_barley_unsp_10000: 4452 heat_HT + 537.6 electricity + 448169 inv_cost + 1206324 op_cost + 35577 ethanol_input => 9120 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_wheat_barley_unsp_10000"> -4452,
   <"electricity", "Ethanol_wheat_barley_unsp_10000"> -537.6,
   <"inv_cost", "Ethanol_wheat_barley_unsp_10000"> -448169,
   <"op_cost", "Ethanol_wheat_barley_unsp_10000"> -1206324,
   <"ethanol_input", "Ethanol_wheat_barley_unsp_10000"> -35577,
   <"stillage_DM", "Ethanol_wheat_barley_unsp_10000"> 9120,
   <"ethanol", "Ethanol_wheat_barley_unsp_10000"> 10000,
# Ethanol_wheat_barley_unsp_100000: 4452 heat_HT + 537.6 electricity + 224617 inv_cost + 1206324 op_cost + 35577 ethanol_input => 9120 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_wheat_barley_unsp_100000"> -4452,
   <"electricity", "Ethanol_wheat_barley_unsp_100000"> -537.6,
   <"inv_cost", "Ethanol_wheat_barley_unsp_100000"> -224617,
   <"op_cost", "Ethanol_wheat_barley_unsp_100000"> -1206324,
   <"ethanol_input", "Ethanol_wheat_barley_unsp_100000"> -35577,
   <"stillage_DM", "Ethanol_wheat_barley_unsp_100000"> 9120,
   <"ethanol", "Ethanol_wheat_barley_unsp_100000"> 10000,
# Ethanol_wheat_barley_unsp_1000000: 4452 heat_HT + 537.6 electricity + 161550 inv_cost + 1206324 op_cost + 33150 ethanol_input => 9120 stillage_DM + 10000 ethanol
   <"heat_HT", "Ethanol_wheat_barley_unsp_1000000"> -4452,
   <"electricity", "Ethanol_wheat_barley_unsp_1000000"> -537.6,
   <"inv_cost", "Ethanol_wheat_barley_unsp_1000000"> -161550,
   <"op_cost", "Ethanol_wheat_barley_unsp_1000000"> -1206324,
   <"ethanol_input", "Ethanol_wheat_barley_unsp_1000000"> -33150,
   <"stillage_DM", "Ethanol_wheat_barley_unsp_1000000"> 9120,
   <"ethanol", "Ethanol_wheat_barley_unsp_1000000"> 10000,
# wheat_to_ethanol_input: wheat => ethanol_input
   <"wheat", "wheat_to_ethanol_input"> -1,
   <"ethanol_input", "wheat_to_ethanol_input"> 1,
# barley_to_ethanol_input: barley => ethanol_input
   <"barley", "barley_to_ethanol_input"> -1,
   <"ethanol_input", "barley_to_ethanol_input"> 1,
# unspec_to_ethanol_input: unspec_cereal => 0 ethanol_input
   <"unspec_cereal", "unspec_to_ethanol_input"> -1,
   <"ethanol_input", "unspec_to_ethanol_input"> 0,
# woodchips_to_furnace: woodchips => 3.12 wood_furnace_input
   <"woodchips", "woodchips_to_furnace"> -1,
   <"wood_furnace_input", "woodchips_to_furnace"> 3.12,
# Wood_furnace_HT_300kW: 4176 wood_furnace_input + 2.31 electricity + 23846 inv_cost + 3321 op_cost => 2400 heat_HT + 856 heat_LT + 8 ash
   <"wood_furnace_input", "Wood_furnace_HT_300kW"> -4176,
   <"electricity", "Wood_furnace_HT_300kW"> -2.31,
   <"inv_cost", "Wood_furnace_HT_300kW"> -23846,
   <"op_cost", "Wood_furnace_HT_300kW"> -3321,
   <"heat_HT", "Wood_furnace_HT_300kW"> 2400,
   <"heat_LT", "Wood_furnace_HT_300kW"> 856,
   <"ash", "Wood_furnace_HT_300kW"> 8,
# Wood_furnace_HT_1MW: 4176 wood_furnace_input + 2.31 electricity + 16617 inv_cost + 3321 op_cost => 2400 heat_HT + 856 heat_LT + 8 ash
   <"wood_furnace_input", "Wood_furnace_HT_1MW"> -4176,
   <"electricity", "Wood_furnace_HT_1MW"> -2.31,
   <"inv_cost", "Wood_furnace_HT_1MW"> -16617,
   <"op_cost", "Wood_furnace_HT_1MW"> -3321,
   <"heat_HT", "Wood_furnace_HT_1MW"> 2400,
   <"heat_LT", "Wood_furnace_HT_1MW"> 856,
   <"ash", "Wood_furnace_HT_1MW"> 8,
# Wood_furnace_HT_above1MW: 4176 wood_furnace_input + 2.31 electricity + 10253 inv_cost + 3321 op_cost => 2400 heat_HT + 856 heat_LT + 8 ash
   <"wood_furnace_input", "Wood_furnace_HT_above1MW"> -4176,
   <"electricity", "Wood_furnace_HT_above1MW"> -2.31,
   <"inv_cost", "Wood_furnace_HT_above1MW"> -10253,
   <"op_cost", "Wood_furnace_HT_above1MW"> -3321,
   <"heat_HT", "Wood_furnace_HT_above1MW"> 2400,
   <"heat_LT", "Wood_furnace_HT_above1MW"> 856,
   <"ash", "Wood_furnace_HT_above1MW"> 8,
# Wood_furnace_LT_300kW: 2572 wood_furnace_input + 1.42 electricity + 16108 inv_cost + 1315 op_cost => 2400 heat_LT + 5.12 ash
   <"wood_furnace_input", "Wood_furnace_LT_300kW"> -2572,
   <"electricity", "Wood_furnace_LT_300kW"> -1.42,
   <"inv_cost", "Wood_furnace_LT_300kW"> -16108,
   <"op_cost", "Wood_furnace_LT_300kW"> -1315,
   <"heat_LT", "Wood_furnace_LT_300kW"> 2400,
   <"ash", "Wood_furnace_LT_300kW"> 5.12,
# Wood_furnace_LT_1MW: 2572 wood_furnace_input + 1.42 electricity + 11225 inv_cost + 1315 op_cost => 2400 heat_LT + 5.12 ash
   <"wood_furnace_input", "Wood_furnace_LT_1MW"> -2572,
   <"electricity", "Wood_furnace_LT_1MW"> -1.42,
   <"inv_cost", "Wood_furnace_LT_1MW"> -11225,
   <"op_cost", "Wood_furnace_LT_1MW"> -1315,
   <"heat_LT", "Wood_furnace_LT_1MW"> 2400,
   <"ash", "Wood_furnace_LT_1MW"> 5.12,
# Wood_furnace_LT_above1MW: 2572 wood_furnace_input + 1.42 electricity + 6926 inv_cost + 1315 op_cost => 2400 heat_LT + 5.12 ash
   <"wood_furnace_input", "Wood_furnace_LT_above1MW"> -2572,
   <"electricity", "Wood_furnace_LT_above1MW"> -1.42,
   <"inv_cost", "Wood_furnace_LT_above1MW"> -6926,
   <"op_cost", "Wood_furnace_LT_above1MW"> -1315,
   <"heat_LT", "Wood_furnace_LT_above1MW"> 2400,
   <"ash", "Wood_furnace_LT_above1MW"> 5.12,
# miscanthus_to_biomass_furnace: Miscanthus_chopped => 3.98 biomass_furnace_input
   <"Miscanthus_chopped", "miscanthus_to_biomass_furnace"> -1,
   <"biomass_furnace_input", "miscanthus_to_biomass_furnace"> 3.98,
# unspec_to_biomass_furnace: unspec_biomass_for_burning => 0 biomass_furnace_input
   <"unspec_biomass_for_burning", "unspec_to_biomass_furnace"> -1,
   <"biomass_furnace_input", "unspec_to_biomass_furnace"> 0,
# Biomass_furnace_HT_300kW: 4176 biomass_furnace_input + 1.61 electricity + 29976 inv_cost + 24107 op_cost => 2400 heat_HT + 856 heat_LT + 64.29 ash
   <"biomass_furnace_input", "Biomass_furnace_HT_300kW"> -4176,
   <"electricity", "Biomass_furnace_HT_300kW"> -1.61,
   <"inv_cost", "Biomass_furnace_HT_300kW"> -29976,
   <"op_cost", "Biomass_furnace_HT_300kW"> -24107,
   <"heat_HT", "Biomass_furnace_HT_300kW"> 2400,
   <"heat_LT", "Biomass_furnace_HT_300kW"> 856,
   <"ash", "Biomass_furnace_HT_300kW"> 64.29,
# Biomass_furnace_HT_1MW: 4176 biomass_furnace_input + 1.61 electricity + 20889 inv_cost + 24107 op_cost => 2400 heat_HT + 856 heat_LT + 64.29 ash
   <"biomass_furnace_input", "Biomass_furnace_HT_1MW"> -4176,
   <"electricity", "Biomass_furnace_HT_1MW"> -1.61,
   <"inv_cost", "Biomass_furnace_HT_1MW"> -20889,
   <"op_cost", "Biomass_furnace_HT_1MW"> -24107,
   <"heat_HT", "Biomass_furnace_HT_1MW"> 2400,
   <"heat_LT", "Biomass_furnace_HT_1MW"> 856,
   <"ash", "Biomass_furnace_HT_1MW"> 64.29,
# Biomass_furnace_HT_above1MW: 4176 biomass_furnace_input + 1.61 electricity + 12889 inv_cost + 24107 op_cost => 2400 heat_HT + 856 heat_LT + 64.29 ash
   <"biomass_furnace_input", "Biomass_furnace_HT_above1MW"> -4176,
   <"electricity", "Biomass_furnace_HT_above1MW"> -1.61,
   <"inv_cost", "Biomass_furnace_HT_above1MW"> -12889,
   <"op_cost", "Biomass_furnace_HT_above1MW"> -24107,
   <"heat_HT", "Biomass_furnace_HT_above1MW"> 2400,
   <"heat_LT", "Biomass_furnace_HT_above1MW"> 856,
   <"ash", "Biomass_furnace_HT_above1MW"> 64.29,
# Biomass_furnace_LT_300kW: 2728 biomass_furnace_input + 1.05 electricity + 22252 inv_cost + 15748 op_cost => 2400 heat_LT + 42 ash
   <"biomass_furnace_input", "Biomass_furnace_LT_300kW"> -2728,
   <"electricity", "Biomass_furnace_LT_300kW"> -1.05,
   <"inv_cost", "Biomass_furnace_LT_300kW"> -22252,
   <"op_cost", "Biomass_furnace_LT_300kW"> -15748,
   <"heat_LT", "Biomass_furnace_LT_300kW"> 2400,
   <"ash", "Biomass_furnace_LT_300kW"> 42,
# Biomass_furnace_LT_1MW: 2728 biomass_furnace_input + 1.05 electricity + 15507 inv_cost + 15748 op_cost => 2400 heat_LT + 42 ash
   <"biomass_furnace_input", "Biomass_furnace_LT_1MW"> -2728,
   <"electricity", "Biomass_furnace_LT_1MW"> -1.05,
   <"inv_cost", "Biomass_furnace_LT_1MW"> -15507,
   <"op_cost", "Biomass_furnace_LT_1MW"> -15748,
   <"heat_LT", "Biomass_furnace_LT_1MW"> 2400,
   <"ash", "Biomass_furnace_LT_1MW"> 42,
# Biomass_furnace_LT_above1MW: 2728 biomass_furnace_input + 1.05 electricity + 9568 inv_cost + 15748 op_cost => 2400 heat_LT + 42 ash
   <"biomass_furnace_input", "Biomass_furnace_LT_above1MW"> -2728,
   <"electricity", "Biomass_furnace_LT_above1MW"> -1.05,
   <"inv_cost", "Biomass_furnace_LT_above1MW"> -9568,
   <"op_cost", "Biomass_furnace_LT_above1MW"> -15748,
   <"heat_LT", "Biomass_furnace_LT_above1MW"> 2400,
   <"ash", "Biomass_furnace_LT_above1MW"> 42,
# shortrotation_to_ORC_input: shortrotation => 3.12 ORC_input
   <"shortrotation", "shortrotation_to_ORC_input"> -1,
   <"ORC_input", "shortrotation_to_ORC_input"> 3.12,
# woodchips_to_ORC_input: ip_wood => 3.12 ORC_input
   <"ip_wood", "woodchips_to_ORC_input"> -1,
   <"ORC_input", "woodchips_to_ORC_input"> 3.12,
# ORC: 44480 ORC_input + 261998 inv_cost + 561362 op_cost => 8000 electricity_biomass + 34640 heat_LT
   <"ORC_input", "ORC"> -44480,
   <"inv_cost", "ORC"> -261998,
   <"op_cost", "ORC"> -561362,
   <"electricity_biomass", "ORC"> 8000,
   <"heat_LT", "ORC"> 34640,
# shortrotation_to_dryer: shortrotation => wood_dryer_input
   <"shortrotation", "shortrotation_to_dryer"> -1,
   <"wood_dryer_input", "shortrotation_to_dryer"> 1,
# woodchips_to_dryer_input: ip_wood => wood_dryer_input
   <"ip_wood", "woodchips_to_dryer_input"> -1,
   <"wood_dryer_input", "woodchips_to_dryer_input"> 1,
# Drying_wood_10000: 2832 heat_HT + 192 electricity + 67020 inv_cost + 105000 op_cost + 12576 wood_dryer_input => 10000 dried_woodchips
   <"heat_HT", "Drying_wood_10000"> -2832,
   <"electricity", "Drying_wood_10000"> -192,
   <"inv_cost", "Drying_wood_10000"> -67020,
   <"op_cost", "Drying_wood_10000"> -105000,
   <"wood_dryer_input", "Drying_wood_10000"> -12576,
   <"dried_woodchips", "Drying_wood_10000"> 10000,
# Drying_wood_100000: 2832 heat_HT + 192 electricity + 33590 inv_cost + 105000 op_cost + 12576 wood_dryer_input => 10000 dried_woodchips
   <"heat_HT", "Drying_wood_100000"> -2832,
   <"electricity", "Drying_wood_100000"> -192,
   <"inv_cost", "Drying_wood_100000"> -33590,
   <"op_cost", "Drying_wood_100000"> -105000,
   <"wood_dryer_input", "Drying_wood_100000"> -12576,
   <"dried_woodchips", "Drying_wood_100000"> 10000,
# Drying_wood_1000000: 2832 heat_HT + 192 electricity + 16835 inv_cost + 105000 op_cost + 12576 wood_dryer_input => 10000 dried_woodchips
   <"heat_HT", "Drying_wood_1000000"> -2832,
   <"electricity", "Drying_wood_1000000"> -192,
   <"inv_cost", "Drying_wood_1000000"> -16835,
   <"op_cost", "Drying_wood_1000000"> -105000,
   <"wood_dryer_input", "Drying_wood_1000000"> -12576,
   <"dried_woodchips", "Drying_wood_1000000"> 10000,
# Pelletizing_wood_10000: 10000 dried_woodchips + 1000 electricity + 119101 inv_cost + 288588 op_cost => 10000 pellets
   <"dried_woodchips", "Pelletizing_wood_10000"> -10000,
   <"electricity", "Pelletizing_wood_10000"> -1000,
   <"inv_cost", "Pelletizing_wood_10000"> -119101,
   <"op_cost", "Pelletizing_wood_10000"> -288588,
   <"pellets", "Pelletizing_wood_10000"> 10000,
# Pelletizing_wood_100000: 10000 dried_woodchips + 1000 electricity + 59692 inv_cost + 288588 op_cost => 10000 pellets
   <"dried_woodchips", "Pelletizing_wood_100000"> -10000,
   <"electricity", "Pelletizing_wood_100000"> -1000,
   <"inv_cost", "Pelletizing_wood_100000"> -59692,
   <"op_cost", "Pelletizing_wood_100000"> -288588,
   <"pellets", "Pelletizing_wood_100000"> 10000,
# Pelletizing_wood_1000000: 10000 dried_woodchips + 1000 electricity + 29917 inv_cost + 288588 op_cost => 10000 pellets
   <"dried_woodchips", "Pelletizing_wood_1000000"> -10000,
   <"electricity", "Pelletizing_wood_1000000"> -1000,
   <"inv_cost", "Pelletizing_wood_1000000"> -29917,
   <"op_cost", "Pelletizing_wood_1000000"> -288588,
   <"pellets", "Pelletizing_wood_1000000"> 10000,
# PV_parks_to_solar_area_PV: 0.0001 solar_area_for_PV_parks => 0.0001 solar_area_for_PV
   <"solar_area_for_PV_parks", "PV_parks_to_solar_area_PV"> -0.0001,
   <"solar_area_for_PV", "PV_parks_to_solar_area_PV"> 0.0001,
# solar_area_for_PV: 0.0001 solar_area_for_thermal_and_PV => 0.0001 solar_area_for_PV
   <"solar_area_for_thermal_and_PV", "solar_area_for_PV"> -0.0001,
   <"solar_area_for_PV", "solar_area_for_PV"> 0.0001,
# PV: 45.926 inv_cost + 0.0001 solar_area_for_PV => 0.131 electricity_PV
   <"inv_cost", "PV"> -45.926,
   <"solar_area_for_PV", "PV"> -0.0001,
   <"electricity_PV", "PV"> 0.131,
# Solar_thermal: 0.0001 solar_area_for_thermal_and_PV + 20.82 inv_cost + 1.63 op_cost => 0.4 individual_heating
   <"solar_area_for_thermal_and_PV", "Solar_thermal"> -0.0001,
   <"inv_cost", "Solar_thermal"> -20.82,
   <"op_cost", "Solar_thermal"> -1.63,
   <"individual_heating", "Solar_thermal"> 0.4,
# Wind_potential: 109861 inv_cost + 0 wind_potential + 33540 op_cost => 1300 electricity_wind
   <"inv_cost", "Wind_potential"> -109861,
   <"wind_potential", "Wind_potential"> -0,
   <"op_cost", "Wind_potential"> -33540,
   <"electricity_wind", "Wind_potential"> 1300,
# district_heating: heat_LT => district_heat_demand
   <"heat_LT", "district_heating"> -1,
   <"district_heat_demand", "district_heating"> 1,
# industrial_heating: heat_HT => industrial_heat_demand
   <"heat_HT", "industrial_heating"> -1,
   <"industrial_heat_demand", "industrial_heating"> 1,
# pellets_individual_heating: pellets => 4.46 individual_heating
   <"pellets", "pellets_individual_heating"> -1,
   <"individual_heating", "pellets_individual_heating"> 4.46,
# split_logs_individual_heating: split_logs => 3.12 individual_heating
   <"split_logs", "split_logs_individual_heating"> -1,
   <"individual_heating", "split_logs_individual_heating"> 3.12,
# Gasification_10000: 10000 dried_woodchips + 462690 inv_cost + 578363 op_cost + 66.9 CaCO3 => 11152 electricity_biomass + 25109.41 heat_LT + 73.22 ash
   <"dried_woodchips", "Gasification_10000"> -10000,
   <"inv_cost", "Gasification_10000"> -462690,
   <"op_cost", "Gasification_10000"> -578363,
   <"CaCO3", "Gasification_10000"> -66.9,
   <"electricity_biomass", "Gasification_10000"> 11152,
   <"heat_LT", "Gasification_10000"> 25109.41,
   <"ash", "Gasification_10000"> 73.22,
# Gasification_100000: 10000 dried_woodchips + 231894 inv_cost + 578363 op_cost + 66.9 CaCO3 => 11152 electricity_biomass + 25087.5 heat_LT + 73.22 ash
   <"dried_woodchips", "Gasification_100000"> -10000,
   <"inv_cost", "Gasification_100000"> -231894,
   <"op_cost", "Gasification_100000"> -578363,
   <"CaCO3", "Gasification_100000"> -66.9,
   <"electricity_biomass", "Gasification_100000"> 11152,
   <"heat_LT", "Gasification_100000"> 25087.5,
   <"ash", "Gasification_100000"> 73.22,
# Gasification_1000000: 10000 dried_woodchips + 116222 inv_cost + 578363 op_cost + 66.9 CaCO3 => 11152 electricity_biomass + 25109.41 heat_LT + 73.22 ash
   <"dried_woodchips", "Gasification_1000000"> -10000,
   <"inv_cost", "Gasification_1000000"> -116222,
   <"op_cost", "Gasification_1000000"> -578363,
   <"CaCO3", "Gasification_1000000"> -66.9,
   <"electricity_biomass", "Gasification_1000000"> 11152,
   <"heat_LT", "Gasification_1000000"> 25109.41,
   <"ash", "Gasification_1000000"> 73.22,
# wood_to_chopper_and_splitter: ip_wood => wood_chopper_and_splitter_input
   <"ip_wood", "wood_to_chopper_and_splitter"> -1,
   <"wood_chopper_and_splitter_input", "wood_to_chopper_and_splitter"> 1,
# shortrotation_to_chopper_and_splitter: shortrotation => wood_chopper_and_splitter_input
   <"shortrotation", "shortrotation_to_chopper_and_splitter"> -1,
   <"wood_chopper_and_splitter_input", "shortrotation_to_chopper_and_splitter"> 1,
# Chopper_7m3: 30800 wood_chopper_and_splitter_input + 0 diesel + 0 inv_cost + 0 op_cost => 30800 woodchips
   <"wood_chopper_and_splitter_input", "Chopper_7m3"> -30800,
   <"diesel", "Chopper_7m3"> -0,
   <"inv_cost", "Chopper_7m3"> -0,
   <"op_cost", "Chopper_7m3"> -0,
   <"woodchips", "Chopper_7m3"> 30800,
# Chopper_22_5m3: 99000 wood_chopper_and_splitter_input + 0 diesel + 0 inv_cost + 0 op_cost => 99000 woodchips
   <"wood_chopper_and_splitter_input", "Chopper_22_5m3"> -99000,
   <"diesel", "Chopper_22_5m3"> -0,
   <"inv_cost", "Chopper_22_5m3"> -0,
   <"op_cost", "Chopper_22_5m3"> -0,
   <"woodchips", "Chopper_22_5m3"> 99000,
# Chopper_90m3: 396000 wood_chopper_and_splitter_input + 0 diesel + 0 inv_cost + 0 op_cost => 396000 woodchips
   <"wood_chopper_and_splitter_input", "Chopper_90m3"> -396000,
   <"diesel", "Chopper_90m3"> -0,
   <"inv_cost", "Chopper_90m3"> -0,
   <"op_cost", "Chopper_90m3"> -0,
   <"woodchips", "Chopper_90m3"> 396000,
# Chopper_Miscanthus: 0 diesel + 0 inv_cost + 0 op_cost + 1966160 Miscanthus => 1966160 Miscanthus_chopped
   <"diesel", "Chopper_Miscanthus"> -0,
   <"inv_cost", "Chopper_Miscanthus"> -0,
   <"op_cost", "Chopper_Miscanthus"> -0,
   <"Miscanthus", "Chopper_Miscanthus"> -1966160,
   <"Miscanthus_chopped", "Chopper_Miscanthus"> 1966160,
# Splitter_wood: 712360 wood_chopper_and_splitter_input + 33.28 diesel + 617 inv_cost + 100000000 op_cost => 712360 split_logs
   <"wood_chopper_and_splitter_input", "Splitter_wood"> -712360,
   <"diesel", "Splitter_wood"> -33.28,
   <"inv_cost", "Splitter_wood"> -617,
   <"op_cost", "Splitter_wood"> -100000000,
   <"split_logs", "Splitter_wood"> 712360,
# heat_LT_to_wastheat: heat_LT => waste_heat
   <"heat_LT", "heat_LT_to_wastheat"> -1,
   <"waste_heat", "heat_LT_to_wastheat"> 1,
# heat_HT_to_wastheat: heat_HT => waste_heat
   <"heat_HT", "heat_HT_to_wastheat"> -1,
   <"waste_heat", "heat_HT_to_wastheat"> 1
   default 0;

var exist[operating_units] binary;
var size[<o> in operating_units] real >= 0 <= capacity_upper_bound[o];
minimize cost:
   (sum<o> in operating_units: size[o]*
      (proportional_cost[o] -
       sum<m> in (raw_materials union products):
          price[m]*material_to_operating_unit_flow_rates[m, o]
      )
   ) +
   (sum<o> in operating_units:
      exist[o] * fix_cost[o]);
subto size_ub:
   forall <o> in operating_units do
      size[o] <= exist[o] * capacity_upper_bound[o];
subto size_lb:
   forall <o> in operating_units do
      size[o] >= exist[o] * capacity_lower_bound[o];
subto raw_lb:
   forall <r> in raw_materials do
      sum<o> in operating_units: -1 * material_to_operating_unit_flow_rates[r, o] * size[o] >= flow_rate_lower_bound[r];
subto inter_lb:
   forall <i> in intermediates do
      sum<o> in operating_units: material_to_operating_unit_flow_rates[i, o] * size[o] >= 0;
subto prod_lb:
   forall <p> in products do
      sum<o> in operating_units: material_to_operating_unit_flow_rates[p, o] * size[o] >= flow_rate_lower_bound[p];
subto raw_ub:
   forall <r> in raw_materials do
      sum<o> in operating_units: -1 * material_to_operating_unit_flow_rates[r, o] * size[o] <= flow_rate_upper_bound[r];
subto inter_ub:
   forall <i> in intermediates do
      sum<o> in operating_units: material_to_operating_unit_flow_rates[i, o] * size[o] <= flow_rate_upper_bound[i];
subto prod_ub:
   forall <p> in products do
      sum<o> in operating_units: material_to_operating_unit_flow_rates[p, o] * size[o] <= flow_rate_upper_bound[p];
