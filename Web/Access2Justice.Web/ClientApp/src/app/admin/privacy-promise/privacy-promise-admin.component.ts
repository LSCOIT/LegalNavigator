import { Component, OnInit } from '@angular/core';
import { PrivacyContent} from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-privacy-promise-admin',
  templateUrl: './privacy-promise-admin.component.html',
  styleUrls: ['../admin-styles.css']
})

export class PrivacyPromiseAdminComponent implements OnInit {
  privacyContent: PrivacyContent;
  newPrivacyContent: any;

  constructor(
    private global: Global
    ) { }

  onSubmit(privacyForm: NgForm) {
    this.newPrivacyContent = privacyForm.value;
  }

  ngOnInit() {
    this.privacyContent = this.global.getData().find(x => x.name === 'PrivacyPromisePage');
  }

}
