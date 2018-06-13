import { Injectable, Output, EventEmitter } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { MapLocation, DisplayMapLocation } from './location';
declare var Microsoft: any;

@Injectable()

export class LocationService {
  searchManager: any;
  map: any;
  locAddress: any;
  location: any;
  pin: any;
  tempLoc: any;
  mapLocation: MapLocation = { state: '', city: '', county: '', zipCode: '' };
  displayMapLocation: DisplayMapLocation = { locality: '', address: '' }

  @Output() change: EventEmitter<boolean> = new EventEmitter();

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

          localStorage.setItem("tempGlobalState", this.location.address.adminDistrict);
          localStorage.setItem("tempGlobalCounty", this.location.address.district);
          localStorage.setItem("tempGlobalCity", this.location.address.locality);
          localStorage.setItem("tempGlobalZipCode", this.location.address.postalCode);

          localStorage.setItem("tempGlobalSearchedLocation", this.locAddress);
          localStorage.setItem("tempGlobalSearchedLocationState", this.location.address.adminDistrict);
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
    this.tempLoc = localStorage.getItem("tempGlobalSearchedLocation");
    localStorage.setItem("globalSearchedLocation", this.tempLoc);
    this.tempLoc = localStorage.getItem("tempGlobalSearchedLocationState");
    localStorage.setItem("globalSearchedLocationAddress", this.tempLoc);

    this.mapLocation.state = localStorage.getItem("tempGlobalState");
    this.mapLocation.county = localStorage.getItem("tempGlobalCounty");
    this.mapLocation.city = localStorage.getItem("tempGlobalCity");
    this.mapLocation.zipCode = localStorage.getItem("tempGlobalZipCode");

    this.displayMapLocation.locality = localStorage.getItem("globalSearchedLocation");
    this.displayMapLocation.address = localStorage.getItem("globalSearchedLocationAddress");
    this.change.emit();
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

  localStorage.setItem("tempGlobalSearchedLocation", this.locAddress);
  localStorage.setItem("tempGlobalSearchedLocationState", result.address.adminDistrict);
}
