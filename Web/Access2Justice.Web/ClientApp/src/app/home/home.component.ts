import { Component, OnInit } from '@angular/core';
import { MapLocation } from '../shared/location/location';
import { LocationService } from '../shared/location/location.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  topicLength = 12;
  mapLocation: MapLocation;
  state: string;
  subscription: any;
  slides = [
    { image: '' },
    { image: '' },
    { image: '' }
  ];

  constructor(private locationService: LocationService) { }

  loadStateName() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.state = this.mapLocation.locality;
    }
  }

  ngOnInit() {
    this.loadStateName();
    this.subscription = this.locationService.notifyLocation
      .subscribe((value) => {
        this.loadStateName();
      });
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
