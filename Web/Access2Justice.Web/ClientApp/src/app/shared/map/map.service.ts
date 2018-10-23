import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MapLocation, LocationDetails } from './map';
import { Subject } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { api } from '../../../api/api';
declare var Microsoft: any;

@Injectable()
export class MapService {
  searchManager: any;
  map: any;
  locAddress: any;
  location: any;
  pin: any;
  mapLocation: MapLocation = { state: '', city: '', county: '', zipCode: '', locality: '', address: '' };
  notifyLocalLocation: Subject<MapLocation> = new Subject<MapLocation>();
  notifyLocation: Subject<MapLocation> = new Subject<MapLocation>();
  notifyLocationError: Subject<any> = new Subject<any>();
  notifyLocationSuccess: Subject<any> = new Subject<any>();

  state: string;
  locationDetails: LocationDetails = { location: this.mapLocation, country: '', formattedAddress: '' }
  constructor() { }

  getMap(mapType) {
    environment.map_type = mapType;
    Microsoft.Maps.loadModule(['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], this.loadSearchManager);
  }

  loadSearchManager() {
    let searchManager;
    let map = new Microsoft.Maps.Map('#my-map',
      {
        credentials: environment.bingmap_key,
      });
    let manager = new Microsoft.Maps.AutosuggestManager(map);
    manager.attachAutosuggest('#search-box', '#searchbox-container', suggestionSelected);
    searchManager = new Microsoft.Maps.Search.SearchManager(map);
  }

  identifyLocation(searchLocation) {
    let searchRequest = {
      where: searchLocation,
      callback: r => this.onGeoCodeSuccess(r),
      errorCallback: e => {
        this.onGeocodeError(e);
      }
    };

    //Make the geocode request.
    let map = new Microsoft.Maps.Map('#my-map',
      {
        credentials: environment.bingmap_key
      });
    this.searchManager = new Microsoft.Maps.Search.SearchManager(map);
    this.searchManager.geocode(searchRequest);
  }

  onGeoCodeSuccess(r) {
    if (r && r.results && r.results.length > 0) {
      this.location = r.results[0];
      this.pin = new Microsoft.Maps.Pushpin(this.location.location, {
        icon: '../../assets/images/location/poi_custom.png'
      });

      let mapService = new MapService();
      this.location.address.adminDistrict = (this.location.address.locality ? this.location.address.locality : this.location.address.formattedAddress);
      mapService.mapLocationDetails(this.location);

      this.map = new Microsoft.Maps.Map('#my-map',
        {
          credentials: environment.bingmap_key
        });
      this.map.entities.push(this.pin);
      //Determine a bounding box to best view the results.
      let bounds = this.location.bestView;
      this.map.setView({ bounds: bounds, padding: 30 });
      this.notifyLocationSuccess.next("success");
    }
  }

  onGeocodeError(event) {
    this.notifyLocationError.next(event);
  }

  updateLocation(): LocationDetails {
    if (environment.map_type) {
      this.locationDetails = JSON.parse(sessionStorage.getItem("globalSearchMapLocation"));
      this.mapLocation = this.locationDetails.location;
      sessionStorage.setItem("globalMapLocation", JSON.stringify(this.mapLocation));
      sessionStorage.removeItem('globalSearchMapLocation');
      this.notifyLocation.next(this.mapLocation);
    }
    else {
      this.locationDetails = JSON.parse(sessionStorage.getItem("localSearchMapLocation"));
      this.mapLocation = this.locationDetails.location;
      this.notifyLocalLocation.next(this.mapLocation);
    }
    return this.locationDetails;
  }

  mapLocationDetails(location) {
    this.location = location;
    this.mapLocation = { state: '', city: '', county: '', zipCode: '', locality: '', address: '' };
    this.location.address.adminDistrict = (this.location.address.adminDistrict ? this.location.address.adminDistrict : this.location.address.formattedAddress);
    this.mapLocation.state = this.location.address.adminDistrict;
    this.mapLocation.county = this.location.address.district;
    this.mapLocation.city = this.location.address.locality;
    this.mapLocation.zipCode = this.location.address.postalCode;
    if ((this.location.entitySubType != undefined &&
      this.location.entitySubType.indexOf("Postcode") != -1)
      || (this.location.entityType != undefined &&
        this.location.entityType.indexOf("PostalAddress") != -1)) {
      //this.mapLocation.state = "";
      this.mapLocation.county = "";
      this.mapLocation.city = "";
    }
    //State
    else if ((this.location.entitySubType != undefined &&
      this.location.entitySubType.indexOf("AdminDivision1") != -1)
      || (this.location.entityType != undefined &&
        this.location.entityType.indexOf("AdminDivision1") != -1)) {
      this.mapLocation.county = "";
      this.mapLocation.city = "";
      this.mapLocation.zipCode = "";
    }
    //County
    else if (this.location.entitySubType != undefined &&
      this.location.entitySubType.indexOf("AdminDivision2") != -1) {
      //this.mapLocation.state = "";
      this.mapLocation.city = "";
      this.mapLocation.zipCode = "";
    }
    //City
    else if ((this.location.entitySubType != undefined &&
      this.location.entitySubType.indexOf("PopulatedPlace") != -1)
      || (this.location.entityType != undefined &&
        this.location.entityType.indexOf("PopulatedPlace") != -1)) {
      //this.mapLocation.state = "";
      this.mapLocation.county = "";
      this.mapLocation.zipCode = "";
    }
    let country: string;
    let fullStateName = this.location.address.formattedAddress;
    if (fullStateName.indexOf(',') > 0) {
      country = this.location.address.countryRegion;
      this.state = fullStateName.slice(fullStateName.indexOf(',') + 2, fullStateName.length);
      this.locationDetails = { country: country, formattedAddress: this.state };
    }

    if (this.location.address.postalCode === undefined) {
      if (this.location.address.locality === undefined) {
        if (this.location.address.district === undefined) {
          this.locAddress = this.location.address.adminDistrict;
        }
        else {
          this.locAddress = this.location.address.district;
        }
      }
      else {
        this.locAddress = this.location.address.locality;
      }
    }
    else {
      this.locAddress = this.location.address.postalCode;
    }
    this.mapLocation.locality = this.locAddress;
    this.mapLocation.address = this.location.address.adminDistrict;
    this.locationDetails = { location: this.mapLocation, country: country, formattedAddress: this.state }
    if (environment.map_type) {
      sessionStorage.setItem("globalSearchMapLocation", JSON.stringify(this.locationDetails));
    }
    else {
      //Zipcode
      if ((this.location.entitySubType != undefined &&
        this.location.entitySubType.indexOf("Postcode") != -1)
        || (this.location.entityType != undefined &&
          this.location.entityType.indexOf("PostalAddress") != -1)) {
        this.mapLocation.state = "";
        this.mapLocation.county = "";
        this.mapLocation.city = "";
      }
      //State
      else if ((this.location.entitySubType != undefined &&
        this.location.entitySubType.indexOf("AdminDivision1") != -1)
        || (this.location.entityType != undefined &&
        this.location.entityType.indexOf("AdminDivision1") != -1)) {
        this.mapLocation.county = "";
        this.mapLocation.city = "";
        this.mapLocation.zipCode = "";
      }
      //County
      else if (this.location.entitySubType != undefined &&
        this.location.entitySubType.indexOf("AdminDivision2") != -1) {
        this.mapLocation.state = "";
        this.mapLocation.city = "";
        this.mapLocation.zipCode = "";
      }
      //City
      else if ((this.location.entitySubType != undefined &&
        this.location.entitySubType.indexOf("PopulatedPlace") != -1)
        || (this.location.entityType != undefined &&
        this.location.entityType.indexOf("PopulatedPlace") != -1)) {
        this.mapLocation.state = "";
        this.mapLocation.county = "";
        this.mapLocation.zipCode = "";
      }
      this.locationDetails.location = this.mapLocation;
      sessionStorage.setItem("localSearchMapLocation", JSON.stringify(this.locationDetails));
    }
  }

}

function suggestionSelected(result) {
  let map = new Microsoft.Maps.Map('#my-map',
    {
      credentials: environment.bingmap_key
    });
  //Remove previously selected suggestions from the map.
  map.entities.clear();
  //Show the suggestion as a pushpin and center map over it.
  var pin = new Microsoft.Maps.Pushpin(result.location, {
    icon: '../../assets/images/location/poi_custom.png'
  });
  map.entities.push(pin);
  map.setView({ bounds: result.bestView, padding: 30 });
  let mapService = new MapService();
  mapService.mapLocationDetails(result);
}
