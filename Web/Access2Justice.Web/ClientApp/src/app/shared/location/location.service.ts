import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { DisplayMapLocation, MapLocation } from './location';
declare var Microsoft: any;

@Injectable()

export class LocationService {
  searchManager: any;
  map: any;
  locAddress: any;
  location: any;
  pin: any;
  tempLoc: any;
  mapLocation: MapLocation;
  displayMapLocation: DisplayMapLocation;

  constructor() { }

  getMap() {
    Microsoft.Maps.loadModule(['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], this.loadSearchManager);
  }

  loadSearchManager() {
    let searchManager;
    let map = new Microsoft.Maps.Map('#my-map',
      {
        credentials: environment.bingmap_key
      });

    let manager = new Microsoft.Maps.AutosuggestManager(map);
    manager.attachAutosuggest('#search-box', '#searchbox-container', suggestionSelected);
    searchManager = new Microsoft.Maps.Search.SearchManager(map);
  }

  identifyLocation(searchLocation) {
    let searchRequest = {
      where: searchLocation,
      callback: function (r) {
        if (r && r.results && r.results.length > 0) {
          this.location = r.results[0];
          this.pin = new Microsoft.Maps.Pushpin(this.location.location, {
            icon: '../../assets/images/location/poi_custom.png'
          });

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
          this.mapLocation = { state: '', city: '', county: '', zipCode: '' };
          this.displayMapLocation = { locality: '', address: '' }
          this.mapLocation.state = this.location.address.adminDistrict;
          this.mapLocation.county = this.location.address.district;
          this.mapLocation.city = this.location.address.locality;
          this.mapLocation.zipCode = this.location.address.postalCode;

          sessionStorage.setItem("tempGlobalMapLocation", JSON.stringify(this.mapLocation));

          this.displayMapLocation.locality = this.locAddress;
          this.displayMapLocation.address = this.location.address.adminDistrict;

          sessionStorage.setItem("tempGlobalDisplayMapLocation", JSON.stringify(this.displayMapLocation));

          this.map = new Microsoft.Maps.Map('#my-map',
            {
              credentials: environment.bingmap_key
            });
          this.map.entities.push(this.pin);
          //Determine a bounding box to best view the results.
          let bounds = this.location.bestView;
          this.map.setView({ bounds: bounds, padding: 30 });
        }
      },
      errorCallback: function (e) {
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

  updateLocation(): DisplayMapLocation {
    this.displayMapLocation = JSON.parse(sessionStorage.getItem("tempGlobalDisplayMapLocation"));
    this.mapLocation = JSON.parse(sessionStorage.getItem("tempGlobalMapLocation"));
    
    return this.displayMapLocation;
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
  if (result.address.postalCode === undefined) {
    if (result.address.locality === undefined) {
      if (result.address.district === undefined) {
        this.locAddress = result.address.adminDistrict;
      }
      else {
        this.locAddress = result.address.district;
      }
    }
    else {
      this.locAddress = result.address.locality;
    }
  }
  else {
    this.locAddress = result.address.postalCode;
  }
  this.displayMapLocation = { locality: '', address: '' }
  this.displayMapLocation.locality = this.locAddress;
  this.displayMapLocation.address = result.address.adminDistrict;

  sessionStorage.setItem("tempGlobalDisplayMapLocation", JSON.stringify(this.displayMapLocation));
}
