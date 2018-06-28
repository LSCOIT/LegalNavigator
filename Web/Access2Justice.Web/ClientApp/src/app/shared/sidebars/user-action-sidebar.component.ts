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

  constructor() { }

  ngOnInit() {
  }

}
