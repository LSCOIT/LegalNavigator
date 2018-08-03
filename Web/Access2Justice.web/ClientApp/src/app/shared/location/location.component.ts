import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationService } from './location.service';
import { MapLocation } from './location';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from '../../shared/navigation/navigation';
import { environment } from '../../../environments/environment';

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
  contentUrl: any = environment.blobUrl;
  navigation: Navigation = {
    id: '',
    language: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } },
    location: { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    privacyPromise: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    helpAndFAQ: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    login: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    logo: { firstLogo: '', secondLogo: '', link: '' },
    home: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    guidedAssistant: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    topicAndResources: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    about: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    search: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } }
  }
  id: string = 'Navigation';
  location: Location = { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  
  constructor(private modalService: BsModalService, private locationService: LocationService, private staticResourceService: StaticResourceService) {
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

  filterLocationNavigationContent(): void {
    if (this.navigation) {
      this.id = this.navigation.id;
      this.location = this.navigation.location;
      this.location.text = this.navigation.location.text;
    }
  }

  getLocationNavigationContent(): void {
    this.staticResourceService.getStaticContents(this.id)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterLocationNavigationContent();
      });
  }

  ngOnInit() {
    this.getLocationNavigationContent();
    if (sessionStorage.getItem("globalMapLocation")) {      
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.displayLocationDetails(this.mapLocation);
    }
  }
}
