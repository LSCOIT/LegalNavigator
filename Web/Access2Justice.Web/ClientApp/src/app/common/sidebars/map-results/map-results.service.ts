import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ENV } from 'environment';
import { LatitudeLongitude } from './map-results';

declare var Microsoft: any;

const virtualEarthUrl = ENV.virtualEarthUrl;
const bingMapKey = ENV.bingmap_key;

@Injectable()
export class MapResultsService {
  latitudeLongitude: Array<LatitudeLongitude> = [];

  constructor(private http: HttpClient) {
  }

  getLocationPoints(address): any {
    var requestUrl = virtualEarthUrl + "/US" + encodeURIComponent(address.replace(/(\r\n|\n|\r)/g, ' ')) + "?key=" + bingMapKey + "&jsonp=JSONP_CALLBACK";
    return this.http.jsonp(requestUrl, 'JSONP_CALLBACK');
  }

  getLocationDetails(address): any {
    return this.http.get(virtualEarthUrl + encodeURIComponent(address.replace(/(\r\n|\n|\r)/g, ' ')), {
      params: {
        output: 'json',
        key: bingMapKey
      }
    });
  }

  getAddressBasedOnPoints(latitude, longitude): any {
    return this.http.get(virtualEarthUrl + encodeURIComponent(`${latitude},${longitude}`), {params: {key: bingMapKey}});
  }

  getStateFullName(countryRegion, state): any {
    return this.http.get(virtualEarthUrl, {
      params: {
        CountryRegion: countryRegion,
        adminDistrict: state,
        key: bingMapKey
      }
    });
  }

  getMap() {
    const map = new Microsoft.Maps.Map('#my-map-results', {
      credentials: bingMapKey
    });
  }

  mapResults(locationCoordinates) {
    this.latitudeLongitude = locationCoordinates;
    if (this.latitudeLongitude.length === 1) {
      const latitude = this.latitudeLongitude[0].latitude;
      const longitude = this.latitudeLongitude[0].longitude;
      const streetMap = new Microsoft.Maps.Map('#my-map-results', {
        credentials: ENV.bingmap_key,
        mapTypeId: Microsoft.Maps.MapTypeId.streetside,
        zoom: 14,
        center: new Microsoft.Maps.Location(latitude, longitude)
      });
      const pin = new Microsoft.Maps.Pushpin(
        new Microsoft.Maps.Location(latitude, longitude),
        {
          icon: '../../assets/images/location/poi_custom.png'
        }
      );
      streetMap.entities.push(pin);
      streetMap.setView({mapTypeId: Microsoft.Maps.MapTypeId.streetside});
    } else {
      const map = new Microsoft.Maps.Map('#my-map-results', {
        credentials: ENV.bingmap_key
      });
      for (let i = 0, len = this.latitudeLongitude.length; i < len; i++) {
        const pin = new Microsoft.Maps.Pushpin(
          new Microsoft.Maps.Location(
            this.latitudeLongitude[i].latitude,
            this.latitudeLongitude[i].longitude
          ),
          {
            icon: '../../assets/images/location/poi_custom.png'
          }
        );
        map.entities.push(pin);
        const bestview = Microsoft.Maps.LocationRect.fromLocations(
          this.latitudeLongitude
        );
        map.setView({bounds: bestview, zoom: 5});
      }
    }
  }
}
