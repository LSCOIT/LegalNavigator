import { Component, OnInit } from '@angular/core';
import { PrivacyContent} from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";

@Component({
  selector: 'app-privacy-promise-admin',
  templateUrl: './privacy-promise-admin.component.html',
  styleUrls: ['../admin-styles.css']
})

export class PrivacyPromiseAdminComponent implements OnInit {
  privacyContent: PrivacyContent;

  constructor(
    private global: Global
    ) { }

  ngOnInit() {
    this.privacyContent = this.global.getData().find(x => x.name === 'PrivacyPromisePage');
    console.log(this.privacyContent);
  }

}
