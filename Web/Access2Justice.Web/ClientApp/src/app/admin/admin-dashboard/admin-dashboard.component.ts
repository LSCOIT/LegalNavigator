import { Component, OnInit } from '@angular/core';
import { Global } from '../../global';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Router } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['../admin-styles.css']
})
export class AdminDashboardComponent implements OnInit {
  roleInformationSubscription;
  isStateAdmin: boolean = false;
  isPortalAdmin: boolean = false;
  stateList: Array<string> = [];
  staticResource: any;
  showStaticContentPage: boolean = false;
  location;

  constructor(
    private global: Global,
    private staticResourceService: StaticResourceService,
    private router: Router,
    private navigateDataService: NavigateDataService
  ) { }

  checkIfStateAdmin(roleInformation) {
    roleInformation.forEach(role => {
      if (role.roleName === 'StateAdmin') {
        this.isStateAdmin = true;
        this.stateList.push(role.organizationalUnit);
      }
      if (role.roleName === 'PortalAdmin') {
        this.isPortalAdmin = true;
      }
    });
  }

  displayStatePage(event) {
    this.location = { state: event.target.innerText };
    this.staticResourceService.getStaticContents(this.location).subscribe(response => {
      this.staticResource = response;
      this.showStaticContentPage = true;
    });
  }

  navigateToPage(event) {
    let pageName = event.target.innerText;
    this.navigateDataService.setData(this.staticResource);
    switch (pageName) {
      case 'HomePage':
        this.router.navigate(['/admin/home', this.location.state]);
        break;
      case 'PrivacyPromisePage':
        this.router.navigate(['/admin/privacy'], { queryParams: { state: this.location.state } });
        break;
      case 'AboutPage':
        this.router.navigateByUrl('/admin/about');
        break;
      case 'HelpAndFAQPage':
        this.router.navigateByUrl('/admin/help');
        break;
      case 'Navigation':
        this.router.navigateByUrl('/admin/navigation');
        break;
    }
  }
    
  ngOnInit() {
    this.checkIfStateAdmin(this.global.roleInformation);
  }

}
