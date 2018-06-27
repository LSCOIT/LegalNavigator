import { Component, OnInit } from '@angular/core';
import { Resources } from '../profile/personalized-plan/personalized-plan';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  resources: Resources;

  constructor() { }

  getSavedResources(): void {
    this.resources = JSON.parse(sessionStorage.getItem("bookmarkedResource"));
  }

  ngOnInit() {
    this.getSavedResources();
  }

}
