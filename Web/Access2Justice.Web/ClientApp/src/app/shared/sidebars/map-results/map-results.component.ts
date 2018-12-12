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
            let addressList = this.searchResource.resources[i].address.split('|');
            if (addressList.length == 1) {
              this.addressList.push(addressList);
            } else {
              this.addressList = this.addressList.concat(addressList);
            }
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
    let num = 0;
    for (let index = 0, len = this.addressList.length; index < len; index++) {
      let address = this.addressList[index].toString().replace('\n', ' ').trim();
      if (address.toLowerCase() != 'na') {
        this.mapResultsService.getLocationDetails(address, environment.bingmap_key).subscribe((locationCoordinates) => {
          if (locationCoordinates.resourceSets[0].resources.length == 1) {
            this.latlong = {
              latitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[0],
              longitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[1]
            }
            this.latitudeLongitude.push(this.latlong);
          }
          else {
            this.mapResultsService.getMap();
          }
          if (this.latitudeLongitude.length + num === this.addressList.length) {
            this.mapResultsService.mapResults(this.latitudeLongitude);
          }
        });
      } else
      {
        num++;
      }
    }
  }

  ngOnChanges() {
    this.getAddress();
  }

}



