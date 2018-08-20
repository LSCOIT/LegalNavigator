import { Component, OnInit, Input, ElementRef, ViewChild, Output, EventEmitter } from '@angular/core';
import { api } from '../../../api/api';
import { environment } from '../../../environments/environment';
import { Login } from '../navigation/navigation';
import { Router } from '@angular/router';
import { Global, UserStatus } from '../../global';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Input() login: Login;
  userProfile: string;
  isLoggedIn: boolean = false;
  blobUrl: any = environment.blobUrl;
  @ViewChild('dropdownMenu') dropdown: ElementRef;
  @Output() sendProfileOptionClickEvent = new EventEmitter<string>();

  constructor(private router: Router, private global:Global) { }

  onProfileOptionClick() {
    this.sendProfileOptionClickEvent.emit();
  }

  profile() {
    if (this.global.role === UserStatus.Shared
      && location.pathname.indexOf(this.global.shareRouteUrl) >= 0) {
      location.href = this.global.profileRouteUrl;
    }
    else {
      this.router.navigate([this.global.profileRouteUrl]);
    }
  }

  toggleDropdown() {
    this.dropdown.nativeElement.style.display =
      this.dropdown.nativeElement.style.display === 'none' ? 'block' : 'none';
  }

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'POST');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  logout() {
    sessionStorage.removeItem("profileData");
    let form = document.createElement('form');
    form.setAttribute('method', 'POST');
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
    }
  }

}
