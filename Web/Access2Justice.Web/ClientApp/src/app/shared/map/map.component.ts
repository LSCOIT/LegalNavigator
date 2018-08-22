import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { MapService } from './map.service';
import { MapLocation } from './map';
import { environment } from '../../../environments/environment';
import { MapResultsService } from '../../shared/sidebars/map-results.service';
import { Navigation, Location, LocationNavContent } from '../navigation/navigation';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { StaticContentDataService } from '../static-content-data.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
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
  config: Object;
  locationInputRequired: boolean;
  isError: boolean = false;
  showLocality: boolean = true;
  subscription: any;
  state: string;
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  locationNavContent: LocationNavContent;
  location: Array<Location>;
  detectLocation = false;
  name: string = 'Navigation';
  staticContent: any;

  constructor(private modalService: BsModalService,
    private mapService: MapService,
    private mapResultsService: MapResultsService,
    private staticResourceService: StaticResourceService,
    private global: Global,
    private staticContentDataService: StaticContentDataService) { }

  changeLocation(template) {
    this.config = {
      ignoreBackdropClick: false,
      keyboard: true
    };
    this.locationInputRequired = false;
    this.openModal(template);
  }

  openModal(template: TemplateRef<any>) {
    this.isError = false;
    this.modalRef = this.modalService.show(template, this.config);
    this.mapService.getMap(this.mapType);
  }

  geocode() {
    this.query = document.getElementById('search-box');
    this.searchLocation = this.query["value"];
    this.mapService.identifyLocation(this.searchLocation);
  }

  updateLocation() {
    this.isError = false;
    this.mapLocation = this.mapService.updateLocation();
    this.displayLocationDetails(this.mapLocation);
    if ((this.modalRef && this.mapLocation) || !this.mapType) {
      this.modalRef.hide();
    } else {
      this.isError = true;
    }
    if (!this.mapType) {
      this.showLocality = false;
    }
  }

  onSearchChange() {
    this.isError = false;
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
    this.detectLocation = true;
    if (window.navigator && window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        position => {
          this.geolocationPosition = position,
            this.mapResultsService.getAddressBasedOnPoints(this.geolocationPosition.coords.latitude,
              this.geolocationPosition.coords.longitude, environment.bingmap_key).subscribe(response => {
                this.selectedAddress = response;
                environment.map_type = true;
                this.mapResultsService.getStateFullName(this.selectedAddress.resourceSets[0].resources[0].address.countryRegion,
                  this.selectedAddress.resourceSets[0].resources[0].address.adminDistrict, environment.bingmap_key)
                  .subscribe(stateFullName => {
                    this.selectedAddress.resourceSets[0].resources[0].address.adminDistrict = stateFullName.resourceSets[0].resources[0].name;
                    this.mapService.mapLocationDetails(this.selectedAddress.resourceSets[0].resources[0]);
                    this.mapService.updateLocation();
                    this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
                    this.displayLocationDetails(this.mapLocation);
                  });
              });
          this.detectLocation = false;
        },
        error => {
          this.config = {
            ignoreBackdropClick: true,
            keyboard: false
          };
          this.locationInputRequired = true;
          this.detectLocation = false;
          this.openModal(this.templateref);
        });
    }
  }

  filterLocationNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.location = navigation.location;
      this.locationNavContent = navigation.locationNavContent;
    }
  }

  getLocationNavigationContent(): void {
    let homePageRequest = { name: this.name };
    if (this.staticResourceService.navigation && (this.staticResourceService.navigation.location[0].state == this.staticResourceService.getLocation())) {
      this.navigation = this.staticResourceService.navigation;
      this.filterLocationNavigationContent(this.staticResourceService.navigation);
    } else {
      //this.staticResourceService.getStaticContent(homePageRequest)
      //  .subscribe(content => {
      //    this.navigation = content[0];
      //    this.filterLocationNavigationContent(this.navigation);
      //    this.staticResourceService.navigation = this.navigation;
      //  });
      if (this.staticContentDataService.getData()) {
        this.staticContent = this.staticContentDataService.getData();
        this.staticContent.forEach(content => {
          if (content.name === this.name) {
            this.navigation = content;
            this.filterLocationNavigationContent(this.navigation);
            this.staticResourceService.navigation = this.navigation;
          }
        });
      }
    }
  }

  setLocalMapLocation() {
    if (!this.mapType && sessionStorage.getItem("searchedLocationMap")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("searchedLocationMap"));      
      this.displayLocationDetails(this.mapLocation);
    }
  }

  ngOnInit() {
    this.getLocationNavigationContent();
    this.showLocality = true;
    if (location.pathname.indexOf(this.global.shareRouteUrl) != -1) {
      return;
    }
    if (this.mapType) {
      if (!sessionStorage.getItem("globalMapLocation")) {
        this.loadCurrentLocation();
      }
    } else {      
      this.showLocality = false;
    }
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.displayLocationDetails(this.mapLocation);
    }
    this.setLocalMapLocation();
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.displayLocationDetails(this.mapLocation);
      });
  }
}
