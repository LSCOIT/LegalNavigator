export interface MapLocation {
  state?: string;
  city?: string;
  county?: string;
  zipCode?: string;
}

export interface LocationDetails {
  location?: MapLocation;
  displayLocationDetails?: DisplayLocationDetails;
  country?: string;
  formattedAddress?: string;
}

export interface DisplayLocationDetails {
  locality?: string;
  address?: string;
}
