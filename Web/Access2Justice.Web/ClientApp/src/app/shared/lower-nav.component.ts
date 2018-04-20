import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lower-nav',
  templateUrl: './lower-nav.component.html',
  styleUrls: ['./lower-nav.component.css']
})
export class LowerNavComponent implements OnInit {
  public showMenu = false;
  constructor() { }

  ngOnInit() {
  }

  getMenu() {
    if (this.showMenu) {
      return 'block';
    } else {
      return '';
    }
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }
}
