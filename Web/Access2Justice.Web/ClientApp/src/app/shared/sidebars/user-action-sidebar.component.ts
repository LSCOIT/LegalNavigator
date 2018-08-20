import { Component, OnInit, Input } from '@angular/core';
import { Global, UserStatus } from '../../global';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-user-action-sidebar',
  templateUrl: './user-action-sidebar.component.html',
  styleUrls: ['./user-action-sidebar.component.css']
})
export class UserActionSidebarComponent implements OnInit, OnChanges {
  @Input() mobile = false;
  @Input() showSave = true;
  @Input() showPrint = true;
  @Input() showDownload = false;
  @Input() showSetting = false;
  @Input() id: string = "";
  @Input() type: string = "";
  @Input() activeTabData: any;
  resourceId: string;
  resourceType: string;
  printData: any;

  constructor(private global: Global) {
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showShare = false;
      global.showSetting = false;
    }
    else {
      global.showShare = true;
    }
  }

  ngOnInit() {
    this.resourceId = this.id;
    this.resourceType = this.type;
  }

  ngOnChanges() {
    if (this.activeTabData) {
      console.log("User acrtion" + this.activeTabData);
      this.printData = this.activeTabData;
      console.log("print data" + this.printData);
    }
  }
}
