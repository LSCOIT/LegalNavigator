import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-action-sidebar',
  templateUrl: './user-action-sidebar.component.html',
  styleUrls: ['./user-action-sidebar.component.css']
})
export class UserActionSidebarComponent implements OnInit {
  @Input() mobile = false;

  constructor() { }

  ngOnInit() {
  }

}
