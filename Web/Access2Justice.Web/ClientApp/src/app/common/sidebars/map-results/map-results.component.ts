import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';

import { LatitudeLongitude } from './map-results';
import { MapResultsService } from './map-results.service';

@Component({
  selector: 'app-map-results',
  templateUrl: './map-results.component.html',
  styleUrls: ['./map-results.component.css']
})
export class MapResultsComponent implements OnChanges {
  latitudeLongitude: Array<LatitudeLongitude> = [];
  latlong: LatitudeLongitude;
  @Input() searchResource: any;
  @Input() addressArr: boolean;
  showMap = false;
  validAddress = [];
  @Output() mapDisplayEvent = new EventEmitter<boolean>();

  constructor(private mapResultsService: MapResultsService) {
  }

  getAddress() {
    let addresses: string[] = [];
    const arrToString = arrAddress => {
      arrAddress.forEach(function (o) {
        addresses = addresses.concat(o.address.split('|'));
      });
    };
    if (this.searchResource) {
      if(this.addressArr === true){
        arrToString(this.searchResource);
      }
      if (this.searchResource.resources) {
        arrToString(this.searchResource.resources);
      }
      this.getMapResults(addresses);
    }
  }

  getMapResults(addresses: string[]) {
    this.latitudeLongitude = [];
    this.validAddress = [];

    if (addresses.length === 0) {
      this.showMap = false;
    } else {
      addresses.forEach(address => {
        if (this.hasNumber(address)) {
          this.checkPoBoxAddresses(address);
        }
      });

      if (this.validAddress.length > 0) {
        this.displayMapResults();
        this.showMap = true;
      } else {
        this.showMap = false;
      }
      this.mapDisplay();
    }
  }

  checkPoBoxAddresses(address) {
    if (!address[0].toLowerCase().includes('p.o.')) {
      this.validAddress.push(address);
    } else {
      let indexOfComma = address[0].indexOf(',') + 2;
      let newAddress = address[0].slice(indexOfComma, -1);
      newAddress = newAddress.trim();
      this.validAddress.push(newAddress);
    }
  }

  hasNumber(myString) {
    return /\d/.test(myString);
  }

  displayMapResults() {
    let num = 0;
    for (let index = 0, len = this.validAddress.length; index < len; index++) {
      const address = this.validAddress[index].toString().trim();
      if (address.toLowerCase() !== 'na' || address.toLowerCase() !== 'n/a') {
        this.mapResultsService
          .getLocationDetails(address)
          .subscribe(locationCoordinates => {
            this.latlong = {
              latitude:
                locationCoordinates.resourceSets[0].resources[0].point
                  .coordinates[0],
              longitude:
                locationCoordinates.resourceSets[0].resources[0].point
                  .coordinates[1]
            };
            this.latitudeLongitude.push(this.latlong);
            if (
              this.latitudeLongitude.length + num ===
              this.validAddress.length
            ) {
              this.mapResultsService.mapResults(this.latitudeLongitude);
            }
          });
      } else {
        num++;
      }
    }
  }

  mapDisplay() {
    this.mapDisplayEvent.emit(this.showMap);
  }

  ngOnChanges() {
    this.getAddress();
  }
}
