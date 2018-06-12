import { Component, OnInit, Input } from '@angular/core';
import { MapResultsService } from './map-results.service';
import { LocationService } from '../location/location.service';
import { environment } from '../../../environments/environment';
import { MapLocationResult, LatitudeLongitude } from './map-results';
declare var Microsoft: any;
declare var google: any;

@Component({
  selector: 'app-map-results',
  templateUrl: './map-results.component.html',
  styleUrls: ['./map-results.component.css']
})
export class MapResultsComponent implements OnInit {
  organizationsList: Array<MapLocationResult> = [];
  latitudeLongitude: Array<LatitudeLongitude> = [];
  latlong: LatitudeLongitude;
  @Input() searchResource: any;

  constructor(private mapResultsService: MapResultsService) {
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

  ngOnInit() {
    let address;
    //this.getMap();
    if (this.searchResource != null || this.searchResource != undefined) {
      for (let i = 0; i < this.searchResource.resources.length; i++) {
        if (this.searchResource.resources[i].resourceType.toLowerCase() === "Organizations") {
          this.organizationsList.push(this.searchResource.resources[i].address);
          //this.latlong = { latitude: 15, longitude: -15 };
          //this.latitudeLongitude.push(this.latlong);
        }
      }
    }
    
    let map = new Microsoft.Maps.Map('#my-map-results',
      {
        credentials: environment.bingmap_key
      });
    map.getCredentials(this.mapResultsService.callGeocodeService);
    this.latlong = { latitude: 15, longitude: -15 };
    this.latitudeLongitude.push(this.latlong);  
    this.latlong = { latitude: 47.6149, longitude: -122.1941 };
    this.latitudeLongitude.push(this.latlong);
    this.latlong = { latitude: 25, longitude: -15 };
    this.latitudeLongitude.push(this.latlong);
    for (let i = 0, len = this.latitudeLongitude.length; i < len; i++) {
      var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(this.latitudeLongitude[i].latitude, this.latitudeLongitude[i].longitude),  {
        icon: '../../assets/images/location/poi_custom.png'
      });
      map.entities.push(pin);
      let center = map.getCenter();
      map.setView({ center: center, zoom: 2 });
  }
  }

  


}




