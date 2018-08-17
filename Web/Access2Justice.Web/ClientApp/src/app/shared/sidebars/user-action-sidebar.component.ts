import { Component, OnInit, Input } from '@angular/core';
import { Global, UserStatus } from '../../global';

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

  constructor(private global: Global) {
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showShare = false;
    }
    else {
      global.showShare = true;
    }
  }

  ngOnInit() {
    this.resourceId = this.id;
    this.resourceType = this.type;
  }

}
