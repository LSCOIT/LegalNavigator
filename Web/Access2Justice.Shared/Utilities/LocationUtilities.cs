using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.Utilities
{
    public static class LocationUtilities
    {
        public static IEnumerable<Location> GetSearchLocations(Location initialLoation)
        {
            if (initialLoation != null)
            {
                var state = initialLoation.State ?? string.Empty;
                var county = initialLoation.County ?? string.Empty;
                var city = initialLoation.City ?? string.Empty;
                var zipCode = initialLoation.ZipCode ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(zipCode))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                zipCode = string.Empty;

                if (!string.IsNullOrWhiteSpace(initialLoation.City))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                city = string.Empty;

                if (!string.IsNullOrWhiteSpace(initialLoation.County))
                {
                    yield return new Location(state, county, city, zipCode);
                }
                county = string.Empty;

                yield return new Location(state, county, city, zipCode);
            }
            else
            {
                yield return initialLoation;
            }
        }
    }
}
