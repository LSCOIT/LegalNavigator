import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit {
  @Input() fullPage = false;
  constructor() { }

  ngOnInit() {
  }

}
