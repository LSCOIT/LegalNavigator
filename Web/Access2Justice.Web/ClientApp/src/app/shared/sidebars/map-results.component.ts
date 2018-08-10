import { Component, OnInit, Input } from '@angular/core';
import { MapResultsService } from './map-results.service';
import { environment } from '../../../environments/environment';
import { MapLocationResult, LatitudeLongitude } from './map-results';
import { ArrayUtilityService } from '../array-utility.service';

@Component({
  selector: 'app-map-results',
  templateUrl: './map-results.component.html',
  styleUrls: ['./map-results.component.css']
})
export class MapResultsComponent implements OnInit {
  addressList: Array<MapLocationResult> = [];
  latitudeLongitude: Array<LatitudeLongitude> = [];
  latlong: LatitudeLongitude;
  @Input() searchResource: any;
  tempResourceStorage: any = [];
  isObjectExists: boolean = false;

  constructor(private mapResultsService: MapResultsService,
    private arrayUtilityService: ArrayUtilityService) {
  }

  getAddress() {
    sessionStorage.removeItem("cacheMapResults");
    if (this.searchResource) {
      if (this.searchResource.resources) {
        for (let i = 0; i < this.searchResource.resources.length; i++) {
          if (this.searchResource.resources[i].address) {
            this.addressList.push(this.searchResource.resources[i].address);
          }
        }
      }
      this.getMapResults(this.addressList);
    }
  }

  getMapResults(address) {
    this.addressList = address;
    this.mapResultsService.getMap();
    if (this.addressList.length > 0) {
      for (let i = 0, len = this.addressList.length; i < len; i++) {
        this.mapResultsService.getLocationDetails(this.addressList[i], environment.bingmap_key).subscribe((locationCoordinates) => {
          this.latlong = {
            latitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[0],
            longitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[1]
          }
          this.latitudeLongitude.push(this.latlong);
          if (this.latitudeLongitude.length == this.addressList.length) {
            sessionStorage.setItem("cacheMapResults", JSON.stringify(this.latitudeLongitude));
            this.mapResultsService.mapResults(this.latitudeLongitude);
          }
        });
      }
    }
  }

  getCacheMapResults() {
    if (sessionStorage.getItem("cacheSearchResults") && JSON.parse(sessionStorage.getItem("cacheMapResults"))) {
      this.tempResourceStorage = JSON.parse(sessionStorage.getItem("cacheSearchResults"));
      if (this.tempResourceStorage && this.tempResourceStorage.resources.length > 0
        && this.tempResourceStorage.resources.length === this.searchResource.resources.length) {
        if (this.tempResourceStorage.resources.length > 0) {
          for (let index = 0; index < this.tempResourceStorage.resources.length; index++) {
            this.isObjectExists = this.arrayUtilityService.checkObjectItemIdExistInArray(this.searchResource.resources, this.tempResourceStorage.resources[index].id);
            if (!this.isObjectExists) {
              return this.isObjectExists;
            }
          }
        }
      }
    }
  }

  ngOnInit() {
    this.getCacheMapResults();
    if (!this.isObjectExists) {
      this.getAddress();
    } else {
      this.mapResultsService.mapResults(JSON.parse(sessionStorage.getItem("cacheMapResults")));
    }
  }
}




