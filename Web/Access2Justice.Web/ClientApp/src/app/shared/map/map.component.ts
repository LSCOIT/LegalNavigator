import { Component, ElementRef, Input, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";

import {ENV} from 'environment';
import { Global } from "../../global";
import { EventUtilityService } from '../services/event-utility.service';
import { StaticResourceService } from '../services/static-resource.service';
import { MapResultsService } from '../sidebars/map-results/map-results.service';
import { Location, LocationNavContent, Navigation } from "../navigation/navigation";
import { DisplayLocationDetails, LocationDetails, MapLocation } from "./map";
import { MapService } from "./map.service";

@Component({
  selector: "app-map",
  templateUrl: "./map.component.html",
  styleUrls: ["./map.component.css"]
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
  locationDetails: LocationDetails;
  displayLocation: DisplayLocationDetails;
  geolocationPosition: any;
  selectedAddress: any;
  @ViewChild("template") public templateref: TemplateRef<any>;
  config: Object;
  locationInputRequired: boolean;
  isError: boolean = false;
  showLocality: boolean = true;
  subscription: any;
  state: string;
  blobUrl: any = ENV.blobUrl;
  navigation: Navigation;
  locationNavContent: LocationNavContent;
  location: Array<Location>;
  detectLocation = false;
  name: string = "Navigation";
  staticContent: any;
  staticContentSubcription: any;
  @Input() displayInitialModal: boolean = false;
  errorSubscription: any;
  locationError: boolean;
  successSubscription: any;
  @ViewChild("changeLocationButton") changeLocationButton: ElementRef;

  constructor(
    private modalService: BsModalService,
    private mapService: MapService,
    private mapResultsService: MapResultsService,
    private staticResourceService: StaticResourceService,
    private global: Global,
    private eventUtilityService: EventUtilityService
  ) {}

  changeLocation(template) {
    this.config = {
      ignoreBackdropClick: false,
      keyboard: true
    };
    this.locationInputRequired = false;
    this.openModal(template);
    this.eventUtilityService.closeSideNav(event);
  }

  openModal(template: TemplateRef<any>) {
    this.isError = false;
    this.modalRef = this.modalService.show(template, this.config);
    this.mapService.getMap(this.mapType);
    document.getElementById("search-box").focus();
  }

  geocode() {
    this.query = document.getElementById("search-box");
    this.searchLocation = this.query["value"];
    this.mapService.identifyLocation(this.searchLocation);
  }

  updateLocation() {
    if (
      this.locationError === false ||
      sessionStorage.getItem("globalSearchMapLocation") ||
      sessionStorage.getItem("localSearchMapLocation")
    ) {
      this.isError = false;
      sessionStorage.removeItem("searchTextResults");
      this.locationError = undefined;
      this.getLocationDetails();
    } else {
      this.geocode();
    }
  }

  getLocationDetails() {
    this.locationDetails = ENV.map_type
      ? JSON.parse(sessionStorage.getItem("globalSearchMapLocation"))
      : JSON.parse(sessionStorage.getItem("localSearchMapLocation"));
    if (this.locationDetails.formattedAddress) {
      if (this.locationDetails.formattedAddress.length < 3) {
        this.mapResultsService
          .getStateFullName(
            this.locationDetails.country,
            this.locationDetails.formattedAddress,
            ENV.bingmap_key
          )
          .subscribe(location => {
            this.locationDetails.displayLocationDetails.address =
              location.resourceSets[0].resources[0].name;
            if (ENV.map_type) {
              this.locationDetails.location.state =
                location.resourceSets[0].resources[0].name;
              this.locationDetails.displayLocationDetails.locality =
                location.resourceSets[0].resources[0].name;
              sessionStorage.setItem(
                "globalSearchMapLocation",
                JSON.stringify(this.locationDetails)
              );
            } else {
              sessionStorage.setItem(
                "localSearchMapLocation",
                JSON.stringify(this.locationDetails)
              );
            }
            this.updateLocationDetails();
          });
      } else {
        this.updateLocationDetails();
      }
    } else {
      this.updateLocationDetails();
    }
  }

  updateLocationDetails() {
    this.locationDetails = this.mapService.updateLocation();
    this.mapLocation = this.locationDetails.location;
    this.displayLocationDetails(this.locationDetails.displayLocationDetails);
    if ((this.modalRef && this.mapLocation) || !this.mapType) {
      this.modalRef.hide();
      this.changeLocationButton.nativeElement.focus();
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

  displayLocationDetails(displayLocation) {
    this.displayLocation = displayLocation;
    if (this.displayLocation && !this.mapType) {
      this.setDisplayLocationDetails();
    } else {
      if (JSON.parse(sessionStorage.getItem("globalMapLocation"))) {
        this.locationDetails = JSON.parse(
          sessionStorage.getItem("globalMapLocation")
        );
        this.displayLocation = this.locationDetails.displayLocationDetails;
        this.setDisplayLocationDetails();
      }
    }
    this.showLocation = false;
  }

  setDisplayLocationDetails() {
    this.address = this.displayLocation.address;
    this.locality = this.displayLocation.locality;
  }

  loadCurrentLocation() {
    this.detectLocation = true;
    if (window.navigator && window.navigator.geolocation) {
      window.navigator.geolocation.getCurrentPosition(
        position => {
          (this.geolocationPosition = position),
            this.mapResultsService
              .getAddressBasedOnPoints(
                this.geolocationPosition.coords.latitude,
                this.geolocationPosition.coords.longitude,
                ENV.bingmap_key
              )
              .subscribe(response => {
                this.selectedAddress = response;
                ENV.map_type = true;
                this.mapResultsService
                  .getStateFullName(
                    this.selectedAddress.resourceSets[0].resources[0].address
                      .countryRegion,
                    this.selectedAddress.resourceSets[0].resources[0].address
                      .adminDistrict,
                    ENV.bingmap_key
                  )
                  .subscribe(stateFullName => {
                    this.selectedAddress.resourceSets[0].resources[0].address.adminDistrict =
                      stateFullName.resourceSets[0].resources[0].name;
                    this.mapService.mapLocationDetails(
                      this.selectedAddress.resourceSets[0].resources[0]
                    );
                    this.mapService.updateLocation();
                    this.locationDetails = JSON.parse(
                      sessionStorage.getItem("globalMapLocation")
                    );
                    this.mapLocation = this.locationDetails.location;
                    this.displayLocationDetails(
                      this.locationDetails.displayLocationDetails
                    );
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
          if (this.displayInitialModal) {
            this.openModal(this.templateref);
          }
        }
      );
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
    if (
      this.staticResourceService.navigation &&
      this.staticResourceService.navigation.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.navigation = this.staticResourceService.navigation;
      this.filterLocationNavigationContent(
        this.staticResourceService.navigation
      );
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.navigation = this.staticContent.find(x => x.name === this.name);
        this.filterLocationNavigationContent(this.navigation);
        this.staticResourceService.navigation = this.navigation;
      }
    }
  }

  setLocalMapLocation() {
    if (!this.mapType && sessionStorage.getItem("searchedLocationMap")) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("searchedLocationMap")
      );
      this.mapLocation = this.locationDetails.location;
      this.displayLocationDetails(this.locationDetails.displayLocationDetails);
    }
  }

  hideSearchPrediction() {
    let searchPredictionContainer = document.getElementById("as_container");
    searchPredictionContainer.style.visibility = "hidden";
  }

  hideLocationError() {
    this.locationError = false;
  }

  ngOnInit() {
    this.getLocationNavigationContent();
    this.showLocality = true;
    this.locationError = undefined;

    this.errorSubscription = this.mapService.notifyLocationError.subscribe(
      value => {
        this.locationError = true;
      }
    );

    this.successSubscription = this.mapService.notifyLocationSuccess.subscribe(
      value => {
        this.locationError = false;
      }
    );

    this.subscription = this.mapService.notifyLocation.subscribe(value => {
      this.locationDetails = value;
      this.displayLocationDetails(this.locationDetails.displayLocationDetails);
    });

    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.getLocationNavigationContent();
      }
    );

    if (location.pathname.indexOf(this.global.shareRouteUrl) !== -1) {
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
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("globalMapLocation")
      );
      this.locationDetails.displayLocationDetails.locality = this.locationDetails.displayLocationDetails.address;
      this.displayLocationDetails(this.locationDetails.displayLocationDetails);
    }
    this.setLocalMapLocation();
    this.subscription = this.global.notifyLocationUpate.subscribe(value => {
      ENV.map_type = true;
      this.updateLocation();
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    if (this.errorSubscription) {
      this.errorSubscription.unsubscribe();
    }

    if (this.successSubscription) {
      this.successSubscription.unsubscribe();
    }

    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
