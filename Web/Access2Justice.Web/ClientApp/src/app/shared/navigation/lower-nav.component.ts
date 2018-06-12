import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-lower-nav',
  templateUrl: './lower-nav.component.html',
  styleUrls: ['./lower-nav.component.css']
})
export class LowerNavComponent implements OnInit {
  @ViewChild('sidenav') sidenav:ElementRef;
  showSearch = false;
  my_Class = '';

  constructor() { }
  
  openNav() {
    let width = window.innerWidth;
    this.my_Class="dimmer";
    if (width >= 768) {
      this.sidenav.nativeElement.style.width = "33.333%";
      this.sidenav.nativeElement.style.height = "100%";
    } else {
      this.sidenav.nativeElement.style.height = "100%";
      this.sidenav.nativeElement.style.width = "100%";
    }
  }
  
  closeNav() {
    let width = window.innerWidth;
    this.my_Class="";
    if (width >= 768) {
      this.sidenav.nativeElement.style.width = "0";
    } else {
      this.sidenav.nativeElement.style.height = "0";
    }
  }
  
  toggleSearch() {
    this.showSearch = !this.showSearch;
  }
  ngOnInit() {
  }

}
