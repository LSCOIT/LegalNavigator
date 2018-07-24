import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-action-sidebar',
  templateUrl: './user-action-sidebar.component.html',
  styleUrls: ['./user-action-sidebar.component.css']
})
export class UserActionSidebarComponent implements OnInit {
  @Input() mobile = false;
  @Input() showSave = true;
  @Input() showPrint = true;
  @Input() showDownload = false;
  @Input() showSetting = false;
  @Input() id: string = "";
  @Input() type: string = "";
  resourceId: string;
  resourceType: string;

  constructor() { }

  ngOnInit() {
    this.resourceId = this.id;
    this.resourceType = this.type;
  }

}
