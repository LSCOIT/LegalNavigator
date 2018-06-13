import { Injectable, Output, EventEmitter } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { MapLocation } from './location';
declare var Microsoft: any;

@Injectable()

export class LocationService {
  searchManager: any;
  map: any;
  locAddress: any;
  location: any;
  pin: any;
  tempLoc: any;
  mapLocation: MapLocation = { locality: '', address: '' };

  @Output() change: EventEmitter<boolean> = new EventEmitter();
  constructor() { }

  getMap() {
    Microsoft.Maps.loadModule(['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], this.loadSearchManager);
  }

  loadSearchManager() {
    let suggestionSelected;
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
          this.locAddress = this.location.address.postalCode;
          if (this.locAddress !== undefined) {
            localStorage.setItem("tempSearchedLocation", this.location.address.postalCode);
          }
          else {
            localStorage.setItem("tempSearchedLocation", this.location.address.locality);
          }
          localStorage.setItem("tempSearchedLocationState", this.location.address.formattedAddress);
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

  updateLocation(): MapLocation {
    this.tempLoc = localStorage.getItem("tempSearchedLocation");
    localStorage.setItem("searchedLocation", this.tempLoc);
    this.tempLoc = localStorage.getItem("tempSearchedLocationState");
    localStorage.setItem("searchedLocationAddress", this.tempLoc);
    this.mapLocation.locality = localStorage.getItem("searchedLocation");
    this.mapLocation.address = localStorage.getItem("searchedLocationAddress");
    this.change.emit();
    return this.mapLocation;
  }

}
