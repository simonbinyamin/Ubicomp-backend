using System.Text.Json.Serialization;

namespace Ubicomp_backend.APIModels
{
        public class solTemperature {

            public solLocation solLocation { get; set; }
            public string terrestrial_date { get; set; }
            public string sol { get; set; }
            public string min_temp	{ get; set; }
            public string max_temp { get; set; }
            public string pressure	{ get; set; }

            //public string id { get; set; }
            // public string pressure_string { get; set; }
            // public string abs_humidity { get; set; }
            // public string wind_speed { get; set; }
            // public string wind_direction { get; set; }
            // public string atmo_opacity { get; set; }
            // public string sunrise { get; set; }
            // public string sunset { get; set; }
            // public string local_uv_irradiance_index	{ get; set; }
            // public string min_gts_temp { get; set; }
            //public string ls { get; set; }
            //public string season { get; set; }


        }
}