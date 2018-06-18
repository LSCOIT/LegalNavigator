import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';

@Component({
  selector: 'app-lower-nav',
  templateUrl: './lower-nav.component.html',
  styleUrls: ['./lower-nav.component.css']
})
export class LowerNavComponent implements OnInit {
  width = 0;
  showSearch = false;
  showMenu = false;
  my_Class = '';

  @ViewChild('sidenav') sidenav:ElementRef;
  @HostListener('window:resize')
    onResize() {
    this.width = window.innerWidth;
  }

  constructor() { }
  
  openNav() {
    this.my_Class="dimmer";
    if (this.width >= 768) {
      this.sidenav.nativeElement.style.width = "33.333%";
      this.sidenav.nativeElement.style.height = "100%";
    } else {
      this.sidenav.nativeElement.style.height = "100%";
      this.sidenav.nativeElement.style.width = "100%";
    }
  }
  
  closeNav() {
    this.my_Class="";
    if (this.width >= 768) {
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
