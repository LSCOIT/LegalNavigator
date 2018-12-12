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
  showMap: boolean = false;
  validAddress = [];

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
      this.addressList.forEach(address => {
        if (this.hasNumber(address)) {
          this.checkPoBoxAddresses(address);
        };
      });

      if (this.validAddress.length > 0) {
        this.displayMapResults();
        this.showMap = true;
      } else {
        this.showMap = false;
      }
    }
  }

  checkPoBoxAddresses(address) {
    if (!address[0].toLowerCase().includes("p.o.")) {
      this.validAddress.push(address);
    } else {
      let indexOfComma = address[0].indexOf(",") + 2;
      let newAddress = address[0].slice(indexOfComma, -1);
      if (newAddress[0] === " ") {
        newAddress = newAddress.slice(1, -1);
        this.validAddress.push([newAddress]);
      } else {
        this.validAddress.push([newAddress]);
      }
    }
  }

  hasNumber(myString) {
    return /\d/.test(myString);
  }

  displayMapResults() {
    let num = 0;
    for (let index = 0, len = this.validAddress.length; index < len; index++) {
      let address = this.validAddress[index].toString().replace('\n', ' ').trim();
      if (address.toLowerCase() != 'na') {
        this.mapResultsService.getLocationDetails(address, environment.bingmap_key).subscribe((locationCoordinates) => {
          this.latlong = {
            latitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[0],
            longitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[1]
          }
          this.latitudeLongitude.push(this.latlong);
          if (this.latitudeLongitude.length + num === this.validAddress.length) {
            this.mapResultsService.mapResults(this.latitudeLongitude);
          }
        });
      } else {
        num++;
      }
    }
  }

  ngOnChanges() {
    this.getAddress();
  }

}



