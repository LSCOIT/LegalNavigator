import { Component, Input, OnChanges } from '@angular/core';
import { MapResultsService } from './map-results.service';
import { environment } from '../../../../environments/environment';
import { MapLocationResult, LatitudeLongitude } from './map-results';

@Component({
  selector: 'app-map-results',
  templateUrl: './map-results.component.html',
  styleUrls: ['./map-results.component.css']
})
export class MapResultsComponent implements OnChanges {
  addressList: Array<MapLocationResult> = [];
  latitudeLongitude: Array<LatitudeLongitude> = [];
  latlong: LatitudeLongitude;
  @Input() searchResource: any;

  constructor(private mapResultsService: MapResultsService) {
  }

  getAddress() {
    this.addressList = [];
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
    this.latitudeLongitude = [];
    if (this.addressList.length === 0) {
      this.mapResultsService.getMap();
    } else {
      this.displayMapResults();
    }
  }

  displayMapResults() {
    for (let index = 0, len = this.addressList.length; index < len; index++) {
      this.mapResultsService.getLocationDetails(this.addressList[index], environment.bingmap_key).subscribe((locationCoordinates) => {
        this.latlong = {
          latitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[0],
          longitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[1]
        }
        this.latitudeLongitude.push(this.latlong);
        if (this.latitudeLongitude.length === this.addressList.length) {
          this.mapResultsService.mapResults(this.latitudeLongitude);
        }
      });
    }
  }

  ngOnChanges() {
    this.getAddress();
  }
}
