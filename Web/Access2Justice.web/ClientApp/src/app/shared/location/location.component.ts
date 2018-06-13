import { Component, OnInit, TemplateRef } from '@angular/core';
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
    this.locationService.identifyLocation(this.searchLocation);
  }

  updateLocation() {
    this.displayMapLocation = this.locationService.updateLocation();
    if (this.displayMapLocation.locality !== "" && this.displayMapLocation.address !== "") {
      this.address = this.displayMapLocation.address;
      this.locality = this.displayMapLocation.locality;
      this.showLocation = false;
    }
    if (this.modalRef) {
      this.modalRef.hide();
    }
  }

  ngOnInit() {
  }
}
