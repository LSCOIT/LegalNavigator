import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { api } from '../../../api/api';
import { UpperNavService } from './upper-nav.service';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})

export class UpperNavComponent implements OnInit {


  userProfile: string;
  isLoggedIn: boolean = false;
  constructor(private http: HttpClient, private upperNavService: UpperNavService) {

  }
  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  logout() {
    sessionStorage.removeItem("profileData");
    this.upperNavService.setData = undefined;
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.logoutUrl);
    document.body.appendChild(form);
    form.submit();
  }

  ngOnInit() {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.isLoggedIn = true;
      this.userProfile = profileData["UserName"];
      this.upperNavService.setData = profileData["UserId"];
    }
  }


}
