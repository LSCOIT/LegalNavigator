/// <reference path="../../../../node_modules/bingmaps/types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { ENV } from 'environment';
import { StateCodeService } from '../services/state-code.service';
import { DisplayLocationDetails, LocationDetails, MapLocation } from './map';

@Injectable()
export class MapService {
  searchManager: any;
  map: any;
  locAddress: any;
  location: any;
  pin: any;
  mapLocation: MapLocation = {
    state: '',
    city: '',
    county: '',
    zipCode: ''
  };
  displayLocation: DisplayLocationDetails = {
    locality: '',
    address: ''
  };
  notifyLocalLocation: Subject<MapLocation> = new Subject<MapLocation>();
  notifyLocation: Subject<LocationDetails> = new Subject<LocationDetails>();
  notifyLocationError: Subject<any> = new Subject<any>();
  notifyLocationSuccess: Subject<any> = new Subject<any>();
  state: string;
  locationDetails: LocationDetails = {
    location: this.mapLocation,
    displayLocationDetails: this.displayLocation,
    country: '',
    formattedAddress: ''
  };
  stateCode: any = '';

  constructor(private stateCodeService: StateCodeService) {
  }

  getMap(mapType) {
    ENV.map_type = mapType;
    Microsoft.Maps.loadModule(
      ['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], () => {
        let searchManager;
        const bingMap = new Microsoft.Maps.Map('#my-map', {
          credentials: ENV.bingmap_key
        });
        const manager = new Microsoft.Maps.AutosuggestManager({map: bingMap});
        manager.attachAutosuggest(
          '#search-box',
          '#searchbox-container',
          suggestionSelected
        );
        searchManager = new Microsoft.Maps.Search.SearchManager(bingMap);
      }
    );
  }

  identifyLocation(searchLocation) {
    const searchRequest: Microsoft.Maps.Search.IGeocodeRequestOptions = {
      where: searchLocation,
      callback: result => this.onGeoCodeSuccess(result),
      errorCallback: e => {
        this.onGeocodeError(e);
      }
    };

    const bingMap = new Microsoft.Maps.Map('#my-map', {
      credentials: ENV.bingmap_key
    });
    this.searchManager = new Microsoft.Maps.Search.SearchManager(bingMap);
    this.searchManager.geocode(searchRequest);
  }

  onGeoCodeSuccess(result: Microsoft.Maps.Search.IGeocodeResult) {
    if (result && result.results && result.results.length > 0) {
      this.location = result.results[0];
      this.pin = new Microsoft.Maps.Pushpin(this.location.location, {
        icon: '../../assets/images/location/poi_custom.png'
      });

      const mapService = new MapService(null);
      this.location.address.adminDistrict = this.location.address.locality
        ? this.location.address.locality
        : this.location.address.formattedAddress;
      mapService.mapLocationDetails(this.location);

      this.map = new Microsoft.Maps.Map('#my-map', {
        credentials: ENV.bingmap_key
      });
      this.map.entities.push(this.pin);
      this.map.setView({bounds: this.location.bestView, padding: 30});
      this.notifyLocationSuccess.next('success');
    }
  }

  onGeocodeError(event) {
    this.notifyLocationError.next(event);
  }

  updateLocation(): Observable<LocationDetails> {
    if (ENV.map_type) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem('globalSearchMapLocation')
      );
      if (this.locationDetails.location.state) {
        return this.stateCodeService.getStateCode(this.locationDetails.location.state).pipe(map(response => {
          this.stateCode = response ? response : 'Default';
          this.locationDetails.location.state = this.stateCode;
          this.setGlobalMapLocationDetails();

          return this.locationDetails;
        }));
      } else {
        this.setGlobalMapLocationDetails();
      }
    } else {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem('localSearchMapLocation')
      );
      if (this.locationDetails.location.state) {
        this.stateCodeService
          .getStateCode(this.locationDetails.location.state)
          .subscribe(response => {
            this.stateCode = response;
            this.locationDetails.location.state = this.stateCode;
            this.setLocalMapLocationDetails();
          });
      } else {
        this.setLocalMapLocationDetails();
      }
    }
    return of(this.locationDetails);
  }

  setGlobalMapLocationDetails() {
    sessionStorage.setItem(
      'globalMapLocation',
      JSON.stringify(this.locationDetails)
    );
    sessionStorage.removeItem('globalSearchMapLocation');
    this.notifyLocation.next(this.locationDetails);
  }

  setLocalMapLocationDetails() {
    this.mapLocation = this.locationDetails.location;
    this.notifyLocalLocation.next(this.mapLocation);
  }

  mapLocationDetails(location) {
    this.location = location;
    this.mapLocation = {state: '', city: '', county: '', zipCode: ''};
    this.displayLocation = {locality: '', address: ''};
    this.location.address.adminDistrict = this.location.address.adminDistrict
      ? this.location.address.adminDistrict
      : this.location.address.formattedAddress;
    this.mapLocation.state = this.location.address.adminDistrict;
    this.mapLocation.county = this.location.address.district;
    this.mapLocation.city = this.location.address.locality;
    this.mapLocation.zipCode = this.location.address.postalCode;
    let country: string;
    sessionStorage.setItem(
      'mapAddress',
      JSON.stringify({
        state: location.address.adminDistrict,
        county: location.address.district,
        city: location.address.locality,
        zipCode: location.address.postalCode
      })
    );
    const fullStateName = this.location.address.formattedAddress;
    if (fullStateName.indexOf(',') > 0) {
      country = this.location.address.countryRegion;
      this.state = fullStateName.slice(
        fullStateName.indexOf(',') + 2,
        fullStateName.length
      );
      this.locationDetails = {country: country, formattedAddress: this.state};
    }
    if (this.location.address.postalCode === undefined) {
      if (this.location.address.locality === undefined) {
        if (this.location.address.district === undefined) {
          this.locAddress = this.location.address.adminDistrict;
        } else {
          this.locAddress = this.location.address.district;
        }
      } else {
        this.locAddress = this.location.address.locality;
      }
    } else {
      this.locAddress = this.location.address.postalCode;
    }
    this.displayLocation.locality = this.locAddress;
    this.displayLocation.address = this.location.address.adminDistrict;
    this.locationDetails = {
      location: this.mapLocation,
      displayLocationDetails: this.displayLocation,
      country: country,
      formattedAddress: this.state
    };
    if (ENV.map_type) {
      this.locationDetails.location.city = '';
      this.locationDetails.location.county = '';
      this.locationDetails.location.zipCode = '';
      sessionStorage.setItem(
        'globalSearchMapLocation',
        JSON.stringify(this.locationDetails)
      );
    } else {
      if (
        (this.location.entitySubType && this.location.entitySubType.indexOf('Postcode') !== -1) ||
        (this.location.entityType && this.location.entityType.indexOf('Postcode') !== -1)
      ) {
        this.mapLocation.state = '';
        this.mapLocation.county = '';
        this.mapLocation.city = '';
      } else if (
        (this.location.entitySubType && this.location.entitySubType.indexOf('AdminDivision1') !== -1) ||
        (this.location.entityType && this.location.entityType.indexOf('AdminDivision1') !== -1)
      ) {
        this.mapLocation.county = '';
        this.mapLocation.city = '';
        this.mapLocation.zipCode = '';
      } else if (
        (this.location.entitySubType && this.location.entitySubType.indexOf('AdminDivision2') !== -1) ||
        (this.location.entityType && this.location.entityType.indexOf('AdminDivision2') !== -1)
      ) {
        this.mapLocation.state = '';
        this.mapLocation.city = '';
        this.mapLocation.zipCode = '';
      } else if (
        (this.location.entitySubType && this.location.entitySubType.indexOf('PopulatedPlace') !== -1) ||
        (this.location.entityType && this.location.entityType.indexOf('PopulatedPlace') !== -1)
      ) {
        this.mapLocation.state = '';
        this.mapLocation.county = '';
        this.mapLocation.zipCode = '';
      }
      this.locationDetails.location = this.mapLocation;
      sessionStorage.setItem(
        'localSearchMapLocation',
        JSON.stringify(this.locationDetails)
      );
    }
  }
}

function suggestionSelected(result) {
  const bingMap = new Microsoft.Maps.Map('#my-map', {
    credentials: ENV.bingmap_key
  });
  bingMap.entities.clear();
  const pin = new Microsoft.Maps.Pushpin(result.location, {
    icon: '../../assets/images/location/poi_custom.png'
  });
  bingMap.entities.push(pin);
  bingMap.setView({bounds: result.bestView, padding: 30});
  const mapService = new MapService(null);
  mapService.mapLocationDetails(result);
}
