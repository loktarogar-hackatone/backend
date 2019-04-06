using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModels
{
	public class HouseDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string MongoId { get; set; }

		public string id { get; set; }
		public string region_id { get; set; }
		public string area_id { get; set; }
		public string city_id { get; set; }
		public string street_id { get; set; }
		public string shortname_region { get; set; }
		public string formalname_region { get; set; }
		public string shortname_area { get; set; }
		public string formalname_area { get; set; }
		public string shortname_city { get; set; }
		public string formalname_city { get; set; }
		public string shortname_street { get; set; }
		public string formalname_street { get; set; }
		public string house_number { get; set; }
		public string building { get; set; }
		public string block { get; set; }
		public string letter { get; set; }
		public string address { get; set; }
		public string houseguid { get; set; }
		public string management_organization_id { get; set; }
		public string built_year { get; set; }
		public string exploitation_start_year { get; set; }
		public string project_type { get; set; }
		public string house_type { get; set; }
		public string is_alarm { get; set; }
		public string method_of_forming_overhaul_fund { get; set; }
		public string floor_count_max { get; set; }
		public string floor_count_min { get; set; }
		public string entrance_count { get; set; }
		public string elevators_count { get; set; }
		public string energy_efficiency { get; set; }
		public string quarters_count { get; set; }
		public string living_quarters_count { get; set; }
		public string unliving_quarters_count { get; set; }
		public string area_total { get; set; }
		public string area_residential { get; set; }
		public string area_non_residential { get; set; }
		public string area_common_property { get; set; }
		public string area_land { get; set; }
		public string parking_square { get; set; }
		public string playground { get; set; }
		public string sportsground { get; set; }
		public string other_beautification { get; set; }
		public string foundation_type { get; set; }
		public string floor_type { get; set; }
		public string wall_material { get; set; }
		public string basement_area { get; set; }
		public string chute_type { get; set; }
		public string chute_count { get; set; }
		public string electrical_type { get; set; }
		public string electrical_entries_count { get; set; }
		public string heating_type { get; set; }
		public string hot_water_type { get; set; }
		public string cold_water_type { get; set; }
		public string sewerage_type { get; set; }
		public string sewerage_cesspools_volume { get; set; }
		public string gas_type { get; set; }
		public string ventilation_type { get; set; }
		public string firefighting_type { get; set; }
		public string drainage_type { get; set; }
	}
}
