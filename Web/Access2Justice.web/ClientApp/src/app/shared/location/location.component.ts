import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationService } from './location.service';
import { MapLocation } from './location';
import { environment } from '../../../environments/environment';
import { MapResultsService } from '../../shared/sidebars/map-results.service';
import { Navigation, Location, LocationNavContent } from '../navigation/navigation';
import { StaticResourceService } from '../../shared/static-resource.service';

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
  config: Object;
  locationInputRequired: boolean;
  isError: boolean = false;
  showLocality: boolean = true;
  subscription: any;
  state: string;
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  locationNavContent: LocationNavContent;
  location: Array<Location>// = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } };

  name: string = 'Navigation';

  constructor(private modalService: BsModalService, private locationService: LocationService,
    private mapResultsService: MapResultsService, private staticResourceService: StaticResourceService) { }

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
    this.locationService.getMap(this.mapType);
  }

  geocode() {
    this.query = document.getElementById('search-box');
    this.searchLocation = this.query["value"];
    this.locationService.identifyLocation(this.searchLocation);
  }

  updateLocation() {
    this.isError = false;
    this.mapLocation = this.locationService.updateLocation();
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
          this.config = {
            ignoreBackdropClick: true,
            keyboard: false
          };
          this.locationInputRequired = true;
          this.openModal(this.templateref);
        });
    }
  }
  
  filterLocationNavigationContent(): void {
    if (this.navigation) {
      this.name = this.navigation.name;
      this.location = this.navigation.location;
      this.locationNavContent = this.navigation.locationNavContent;
    }
  }

  getLoationNavigationContent(): void {
    let homePageRequest = { name: this.name};
    this.staticResourceService.getStaticContent(homePageRequest)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterLocationNavigationContent();
      });
  }

  ngOnInit() {
    this.getLoationNavigationContent();
    this.showLocality = true;
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
    this.subscription = this.locationService.notifyLocation
      .subscribe((value) => {
        this.displayLocationDetails(this.mapLocation);
      });
  }
}
