import { Component, OnInit, Input } from '@angular/core';
import { MapResultsService } from './map-results.service';
import { environment } from '../../../environments/environment';
import { MapLocationResult, LatitudeLongitude } from './map-results';
declare var Microsoft: any;

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

  getAddress() {
    if (this.searchResource != null || this.searchResource != undefined) {
      if (this.searchResource.resources != null || this.searchResource.resources != undefined) {
        for (let i = 0; i < this.searchResource.resources.length; i++) {
          if (this.searchResource.resources[i].resourceType.toLowerCase() === "organizations") {
            this.organizationsList.push(this.searchResource.resources[i].address);
          }
        }
      }
      this.getMapResults(this.organizationsList);
    }
  }

  getMapResults(organizations) {
    this.organizationsList = organizations;
    this.mapResultsService.getMap();
    if (this.organizationsList.length > 0) {
      for (let i = 0, len = this.organizationsList.length; i < len; i++) {
        this.mapResultsService.getLocationDetails(this.organizationsList[i], environment.bingmap_key).subscribe((locationCoordinates) => {
          this.latlong = {
            latitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[0],
            longitude: locationCoordinates.resourceSets[0].resources[0].point.coordinates[1]
          }
          this.latitudeLongitude.push(this.latlong);
          if (this.latitudeLongitude.length == this.organizationsList.length) {
            this.mapResultsService.mapResults(this.latitudeLongitude);
          }
        });
      }
    }
  }

  ngOnInit() {
    this.getAddress();
  }
}




