using System.Text.Json.Serialization;

namespace Ubicomp_backend.APIModels
{
    public record solTemperature(solLocation solLocation, string terrestrial_date, string sol, string min_temp, string max_temp, string pressure)
    {
        public solLocation solLocation { get; init; } = solLocation;
    }

    //string id
    //string pressure_string
    //string abs_humidity
    //string wind_speed
    //string wind_direction
    //string atmo_opacity
    //string sunrise
    //string sunset
    //string local_uv_irradiance_index
    //string min_gts_temp
    //string ls
    //string season

}