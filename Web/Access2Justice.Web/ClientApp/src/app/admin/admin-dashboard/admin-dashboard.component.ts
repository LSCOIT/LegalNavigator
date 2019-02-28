import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { Global } from "../../global";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StaticResourceService } from "../../common/services/static-resource.service";

@Component({
  selector: "app-admin-dashboard",
  templateUrl: "./admin-dashboard.component.html",
  styleUrls: ["../admin-styles.css"]
})
export class AdminDashboardComponent implements OnInit {
  roleInformationSubscription;
  isStateAdmin: boolean = false;
  isPortalAdmin: boolean = false;
  stateList: Array<string> = [];
  staticResource: any;
  showStaticContentPage: boolean = false;
  location;
  stateDropdown = "Select State";

  constructor(
    private global: Global,
    private staticResourceService: StaticResourceService,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private spinner: NgxSpinnerService
  ) {}

  checkIfStateAdmin(roleInformation) {
    roleInformation.forEach(role => {
      if (role.roleName === "StateAdmin") {
        this.isStateAdmin = true;
        this.stateList.push(role.organizationalUnit);
      }
      if (role.roleName === "PortalAdmin") {
        this.isPortalAdmin = true;
      }
    });
  }

  displayStatePage(event) {
    this.stateDropdown = event.target.innerText;
    this.location = { state: event.target.innerText };
    this.spinner.show();
    this.staticResourceService.getStaticContents(this.location).subscribe(
      response => {
        this.spinner.hide();
        this.staticResource = response;
        this.showStaticContentPage = true;
      },
      error => this.router.navigate(["/error"])
    );
  }

  navigateToPage(event) {
    let pageName = event.target.innerText;
    this.navigateDataService.setData(this.staticResource);
    switch (pageName) {
      case "HomePage":
        this.router.navigate(["/admin/home"], {
          queryParams: { state: this.location.state }
        });
        break;
      case "PrivacyPromisePage":
        this.router.navigate(["/admin/privacy"], {
          queryParams: { state: this.location.state }
        });
        break;
      case "AboutPage":
        this.router.navigate(["/admin/about"], {
          queryParams: { state: this.location.state }
        });
        break;
      case "HelpAndFAQPage":
        this.router.navigate(["/admin/help"], {
          queryParams: { state: this.location.state }
        });
        break;
      case "PersonalizedActionPlanPage":
        this.router.navigate(["/admin/plan"], {
          queryParams: { state: this.location.state }
        });
        break;
    }
  }

  ngOnInit() {
    this.checkIfStateAdmin(this.global.roleInformation);
  }
}
