import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationService } from './location.service';
import { MapLocation } from './location';

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

  constructor(private modalService: BsModalService, private locationService: LocationService) {
  }

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

  ngOnInit() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.displayLocationDetails(this.mapLocation);
    }
  }
}
