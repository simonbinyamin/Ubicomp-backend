public record solTemperature(solLocation solLocation, string terrestrial_date, string sol, string min_temp, string max_temp, string pressure)
{
    public solLocation solLocation { get; init; } = solLocation;
}