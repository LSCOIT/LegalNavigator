import { Injectable, Input, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
declare var Microsoft: any;

@Injectable()

export class MapResultsService implements OnInit {
  searchResults: any;
  map: any;
  mapResults: any;

  //@Input() searchResource: any;

  constructor() {
  }

  getMap() {
    Microsoft.Maps.loadModule(['Microsoft.Maps.AutoSuggest', 'Microsoft.Maps.Search'], this.loadSearchManager);
  }

  loadSearchManager() {
    let searchManager;
    let map = new Microsoft.Maps.Map('#my-map-results',
      {
        credentials: environment.bingmap_key
      });
  }
  ngOnInit() {
    //if (this.searchResource != null || this.searchResource != undefined) {
    //  if (this.searchResults.resources != null) {
    //    for (let i = 0; i < this.searchResults.resources.length; i++) {
    //      if (this.searchResults.resources[i].resourceType == "Organizations")
    //    }
    //  }
    //  this.mapResults = this.searchResource;
    //} else {
    //  this.mapResults = this.mapResults;
    //}
  }

}

