import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
    
  getCookie(cookieName: string) {    
    let cookieNameEQ = cookieName + "=";
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      let cookie = cookies[i];
      while (cookie.charAt(0) == ' ')
        cookie = cookie.substring(1, cookie.length);
      if (cookie.indexOf(cookieNameEQ) == 0) {
        return cookie.substring(cookieNameEQ.length, cookie.length);
      }
    }
    return null;
  }

  deleteCookie(name: string, value: string, days: number) {
    let expires = "";
    if (days) {
      let date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
  } 

  ngOnInit() {
    let profileData = this.getCookie("profileData");
    if (profileData != undefined) {
      profileData = decodeURIComponent(profileData);
      sessionStorage.setItem("profileData", profileData);      
      this.deleteCookie("profileData", "", -1);      
    }
  }
}
