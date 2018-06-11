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
    setTimeout(() => {
      let center = new Microsoft.Maps.Location(15, -15);

      let map = new Microsoft.Maps.Map('#my-map-results',
        {
          credentials: environment.bingmap_key,
          center: new Microsoft.Maps.Location(47.6149, -122.1941)
        });
      center = map.getCenter();

      //Create custom Pushpin
      var pin = new Microsoft.Maps.Pushpin(center, {
        icon: '../../assets/images/location/poi_custom.png'
      });

      //Add the pushpin to the map
      map.entities.push(pin);
      map.setView({ center: center, zoom: 2 });
    });
  }

  //var latlons = [{ lat: 45, lon: -110 }, ....];

  //for (var i = 0, len = latlons.length; i < len; i++) {
  //  var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(latlongs[I].lat, latlongs[I].lon));
  //  map.entities.push(pin);
  //}

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

