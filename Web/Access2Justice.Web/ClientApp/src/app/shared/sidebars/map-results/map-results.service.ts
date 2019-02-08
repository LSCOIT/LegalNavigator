import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import {ENV} from 'environment';
import { LatitudeLongitude } from "./map-results";

declare var Microsoft: any;

const virtualEarthUrl = ENV.virtualEarthUrl;
@Injectable()
export class MapResultsService {
  latitudeLongitude: Array<LatitudeLongitude> = [];

  constructor(private http: HttpClient) {}
  
  getLocationDetails(address, credentials): any {
    let cleanAddress = encodeURI(address).replace("%0A", "%20");
    let searchRequest =
      virtualEarthUrl + cleanAddress + "?output=json&key=" + credentials;
    return this.http.get(searchRequest);
  }

  getAddressBasedOnPoints(latitude, longitude, credentials): any {
    let searchRequest =
      virtualEarthUrl +
      encodeURI(latitude) +
      "," +
      encodeURI(longitude) +
      "?key=" +
      credentials;
    return this.http.get(searchRequest);
  }

  getStateFullName(countryRegion, state, credentials): any {
    let searchRequest =
      virtualEarthUrl +
      "?CountryRegion=" +
      encodeURI(countryRegion) +
      "&adminDistrict=" +
      encodeURI(state) +
      "&key=" +
      credentials;
    return this.http.get(searchRequest);
  }

  getMap() {
    let map = new Microsoft.Maps.Map("#my-map-results", {
      credentials: ENV.bingmap_key
    });
  }

  mapResults(locationCoordinates) {
    this.latitudeLongitude = locationCoordinates;
    if (this.latitudeLongitude.length === 1) {
      let latitude = this.latitudeLongitude[0].latitude;
      let longitude = this.latitudeLongitude[0].longitude;
      let streetMap = new Microsoft.Maps.Map("#my-map-results", {
        credentials: ENV.bingmap_key,
        mapTypeId: Microsoft.Maps.MapTypeId.streetside,
        zoom: 14,
        center: new Microsoft.Maps.Location(latitude, longitude)
      });
      var pin = new Microsoft.Maps.Pushpin(
        new Microsoft.Maps.Location(latitude, longitude),
        {
          icon: "../../assets/images/location/poi_custom.png"
        }
      );
      streetMap.entities.push(pin);
      streetMap.setView({ mapTypeId: Microsoft.Maps.MapTypeId.streetside });
    } else {
      let map = new Microsoft.Maps.Map("#my-map-results", {
        credentials: ENV.bingmap_key
      });
      for (let i = 0, len = this.latitudeLongitude.length; i < len; i++) {
        var pin = new Microsoft.Maps.Pushpin(
          new Microsoft.Maps.Location(
            this.latitudeLongitude[i].latitude,
            this.latitudeLongitude[i].longitude
          ),
          {
            icon: "../../assets/images/location/poi_custom.png"
          }
        );
        map.entities.push(pin);
        let bestview = Microsoft.Maps.LocationRect.fromLocations(
          this.latitudeLongitude
        );
        map.setView({ bounds: bestview, zoom: 5 });
      }
    }
  }
}
