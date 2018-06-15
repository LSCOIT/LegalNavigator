import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationService } from './location.service';
import { MapLocation, DisplayMapLocation } from './location';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  @Input() mapType: string;
  modalRef: BsModalRef;
  locality: string;
  address: any;
  showLocation: boolean = true;
  query: any;
  searchLocation: string;
  displayMapLocation: DisplayMapLocation;

  constructor(private modalService: BsModalService, private locationService: LocationService) {
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
    this.locationService.getMap();
  }

  geocode() {
    this.query = document.getElementById('search-box');
    this.searchLocation = this.query["value"];
    this.locationService.identifyLocation(this.searchLocation, this.mapType);
  }

  updateLocation() {
    this.displayMapLocation = this.locationService.updateLocation(this.mapType);
    //if (this.displayMapLocation.locality !== "" && this.displayMapLocation.address !== "") {
    //  this.address = this.displayMapLocation.address;
    //  this.locality = this.displayMapLocation.locality;
    //  this.showLocation = false;
    //}
    this.displayLocationDetails(this.displayMapLocation);
    if (this.modalRef) {
      this.modalRef.hide();
    }
  }

  displayLocationDetails(displayMapLocation) {
    this.displayMapLocation = displayMapLocation;
    if (this.displayMapLocation) {
      this.address = this.displayMapLocation.address;
      this.locality = this.displayMapLocation.locality;
      this.showLocation = false;
    }
  }

  ngOnInit() {
    if (sessionStorage.getItem("globalDisplayMapLocation")) {
      if (this.mapType === "global") {
        this.displayMapLocation = JSON.parse(sessionStorage.getItem("globalDisplayMapLocation"));
      }
      if (this.mapType === "searchResultsMap") {
        this.displayMapLocation = JSON.parse(sessionStorage.getItem("searchresultsDisplayMapLocation"));
      }
      this.displayLocationDetails(this.displayMapLocation);
    }
  }
}
