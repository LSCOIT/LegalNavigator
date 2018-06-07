import { Component, OnInit } from '@angular/core';
import { MapResultsService } from './map-results.service';
import { LocationService } from '../location/location.service';

@Component({
  selector: 'app-map-results',
  templateUrl: './map-results.component.html',
  styleUrls: ['./map-results.component.css']
})
export class MapResultsComponent implements OnInit {

  constructor(private mapResultsService: MapResultsService) {
    //this.mapResultsService.getMap();
  }

  ngOnInit() {
  }

}
