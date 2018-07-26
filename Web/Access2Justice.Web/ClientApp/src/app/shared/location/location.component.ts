import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationService } from './location.service';
import { MapLocation } from './location';
import { environment } from '../../../environments/environment';
import { MapResultsService } from '../../shared/sidebars/map-results.service';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  @Input() mapType: boolean;
  modalRef: BsModalRef;
  locality: string;
  address: any;
  showLocation: boolean = true;
  query: any;
  searchLocation: string;
  mapLocation: MapLocation;
  geolocationPosition: any;
  selectedAddress: any;
  @ViewChild('template') public templateref: TemplateRef<any>;

  constructor(private modalService: BsModalService, private locationService: LocationService,
              private mapResultsService: MapResultsService) {  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
    this.locationService.getMap(this.mapType);
  }

  geocode() {
    this.query = document.getElementById('search-box');
    this.searchLocation = this.query["value"];
    this.locationService.identifyLocation(this.searchLocation);
  }

  updateLocation() {
    this.mapLocation = this.locationService.updateLocation();
    this.displayLocationDetails(this.mapLocation);
    if (this.modalRef) {
      this.modalRef.hide();
    }
  }

  displayLocationDetails(mapLocation) {
    this.mapLocation = mapLocation;
    if (this.mapLocation) {
      this.address = this.mapLocation.address;
      this.locality = this.mapLocation.locality;
      this.showLocation = false;
    }
  }

  loadCurrentLocation() {
    if (window.navigator && window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        position => {
          this.geolocationPosition = position,
            this.mapResultsService.getAddressBasedOnPoints(this.geolocationPosition.coords.latitude,
              this.geolocationPosition.coords.longitude, environment.bingmap_key).subscribe(response => {
                this.selectedAddress = response;
                environment.map_type = true;
                this.locationService.mapLocationDetails(this.selectedAddress.resourceSets[0].resources[0]);
                this.locationService.updateLocation();
                this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
                this.displayLocationDetails(this.mapLocation);
              });
        },
        error => {
          this.openModal(this.templateref);
        });
    }
  }

  ngOnInit() {
    this.loadCurrentLocation();
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.displayLocationDetails(this.mapLocation);
    }
  }
}
