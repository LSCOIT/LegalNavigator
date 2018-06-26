import { Component, OnInit } from '@angular/core';
import { Resources } from '../personalized-plan/personalized-plan';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  resources: Resources;

  constructor() { }

  getSavedResources(): void {
    this.resources = JSON.parse(sessionStorage.getItem("bookmarkedResource"));
  }

  ngOnInit() {
    this.getSavedResources();
  }

}
