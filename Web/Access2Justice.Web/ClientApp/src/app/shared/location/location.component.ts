import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { } from '@types/bingmaps';
import { environment } from '../../../environments/environment';
declare var Microsoft: any;
/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  showPosition: any;
  map: any;
  modalRef: BsModalRef;
  searchManager: any;
  locAddress: any;
  anchorage: string;
  tempLoc: any;
  anchorageAddress: any;
  showLocation: boolean = true;
  geolocationPosition: any;
  
  constructor(private modalService: BsModalService) {
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
    this.getMap();
  }

  getMap() {
    Microsoft.Maps.loadModule(['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], this.loadSearchManager);
  }


  loadSearchManager() {
    let suggestionSelected;
    let searchManager;
    let map = new Microsoft.Maps.Map('#my-map',
      {
        credentials: environment.bingmap_key
      });

    let manager = new Microsoft.Maps.AutosuggestManager(map);
    manager.attachAutosuggest('#search-box', '#searchbox-container', suggestionSelected);
    searchManager = new Microsoft.Maps.Search.SearchManager(map);
  }

  suggestionSelected(result) {
    //Remove previously results from the map.
    this.map.entities.clear();
    //Show the suggestion as a pushpin and center map over it.
    let pin = new Microsoft.Maps.Pushpin(result.location);
    this.map.entities.push(pin);
    this.map.setView({ bounds: result.bestView });
  }


  geocode() {
    //Get the users query and geocode it.
    let query = document.getElementById('search-box');
    let loc;
    let searchRequest = {
      where: query["value"],
      callback: function (r) {
        if (r && r.results && r.results.length > 0) {
          let pin, pins = [], locs = [];
          //Add a pushpin for each result to the map and create a list to display.
          //Create a pushpin for each result.
          pin = new Microsoft.Maps.Pushpin(r.results[0].location, {
            icon: '../../assets/images/location/poi_custom.png'
          });
          loc = r.results[0];
          pins.push(pin);
          locs.push(loc.location);
          this.locAddress = loc.address.postalCode;
          if (this.locAddress !== undefined) {
            localStorage.setItem("tempSearchedLocation", loc.address.postalCode);
          }
          else {
            localStorage.setItem("tempSearchedLocation", loc.address.locality);
          }
          localStorage.setItem("tempSearchedLocationState", loc.address.formattedAddress);
          this.map = new Microsoft.Maps.Map('#my-map',
            {
              credentials: environment.bingmap_key
            });
          //Add the pins to the map
          this.map.entities.push(pins);
          //Determine a bounding box to best view the results.
          let bounds = loc.bestView;
          this.map.setView({ bounds: bounds, padding: 30 });
        }
      },
      errorCallback: function (e) {
      }
    };
    //Make the geocode request.
    let map = new Microsoft.Maps.Map('#my-map',
      {
        credentials: environment.bingmap_key
      });
    this.searchManager = new Microsoft.Maps.Search.SearchManager(map);
    this.searchManager.geocode(searchRequest);
  }

  updateLocation() {
    this.tempLoc = localStorage.getItem("tempSearchedLocation");
    localStorage.setItem("searchedLocation", this.tempLoc);
    this.tempLoc = localStorage.getItem("tempSearchedLocationState");
    localStorage.setItem("searchedLocationAddress", this.tempLoc);
    this.anchorage = localStorage.getItem("searchedLocation");
    this.anchorageAddress = localStorage.getItem("searchedLocationAddress");
    if (this.anchorage !== "" && this.anchorageAddress !== "") {
      this.showLocation = false;
    }
    this.modalRef.hide();
  }


  ngOnInit() {
  }
}
