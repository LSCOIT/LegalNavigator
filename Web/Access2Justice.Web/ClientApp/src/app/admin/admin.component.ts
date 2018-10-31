import { Component, OnInit } from '@angular/core';
import { Global } from '../global';
import { StaticResourceService } from "../shared/static-resource.service";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin-styles.css']
})
export class AdminComponent implements OnInit {
  roleInformationSubscription;
  isStateAdmin: boolean = false;
  stateList: Array<string> = [];
  staticResource: any;
  showStaticContentPage: boolean = false;

  constructor(
    private global: Global,
    private staticResourceService: StaticResourceService
  ) { }

  checkIfStateAdmin(roleInformation) {
    roleInformation.forEach(role => {
      if (role.roleName === 'StateAdmin') {
        console.log("check if state admin");
        this.isStateAdmin = true;
        this.stateList.push(role.organizationalUnit);
      }
    });
  }

  displayStatePage(event) {
    let location = { state: event.target.innerText };
    this.staticResourceService.getStaticContents(location).subscribe(response => {
      this.staticResource = response;
      this.showStaticContentPage = true;
    });
  }

  ngOnInit() {
    this.checkIfStateAdmin(this.global.roleInformation);
  }

}
