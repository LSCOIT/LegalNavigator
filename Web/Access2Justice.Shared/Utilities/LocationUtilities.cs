using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.Utilities
{
    public static class LocationUtilities
    {
        public static IEnumerable<Location> GetSearchLocations(Location initialLocation)
        {
            if (initialLocation != null)
            {
                var state = initialLocation.State ?? string.Empty;
                var county = initialLocation.County ?? string.Empty;
                var city = initialLocation.City ?? string.Empty;
                var zipCode = initialLocation.ZipCode ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(zipCode))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                zipCode = string.Empty;

                if (!string.IsNullOrWhiteSpace(initialLocation.City))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                city = string.Empty;

                if (!string.IsNullOrWhiteSpace(initialLocation.County))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                county = string.Empty;

                yield return new Location(state, county, city, zipCode);
            }
            else
            {
                yield return initialLocation;
            }
        }

        public static Location GetNotNullLocation(Location initialLocation)
        {
            var state = initialLocation?.State ?? string.Empty;
            var county = initialLocation?.County ?? string.Empty;
            var city = initialLocation?.City ?? string.Empty;
            var zipCode = initialLocation?.ZipCode ?? string.Empty;

            return new Location(state, county, city, zipCode);
        }
    }
}
