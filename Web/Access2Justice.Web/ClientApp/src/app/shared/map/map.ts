export interface MapLocation {
  state?: string;
  city?: string;
  county?: string;
  zipCode?: string;
  locality?: string;
  address?: string;
}

export interface LocationDetails {
  location?: MapLocation;
  country?: string;
  formattedAddress?: string;
}
