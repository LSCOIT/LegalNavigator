import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { environment } from '../../../environments/environment';
import { MsalService } from '@azure/msal-angular';
import { Global } from '../../global';

@Component({
  selector: 'app-browser-tab-close',
  templateUrl: './browser-tab-close.component.html',
  styleUrls: ['./browser-tab-close.component.css']
})
export class BrowserTabCloseComponent implements OnInit {
  modalRef: BsModalRef;
  @ViewChild('template') public templateref: TemplateRef<any>;

  constructor(private modalService: BsModalService,
    private msalService: MsalService,
    private global: Global) { }

  saveToProfile() {
    if (sessionStorage.getItem(this.global.sessionKey)
      || sessionStorage.getItem(this.global.planSessionKey)
      || sessionStorage.getItem(this.global.topicsSessionKey)) {
      this.global.isLoginRedirect = true;
      this.msalService.loginRedirect(environment.consentScopes);
    }
  }

  close() {
    sessionStorage.removeItem(this.global.sessionKey);
    sessionStorage.removeItem(this.global.planSessionKey);
    this.modalRef.hide();
  }

  ngOnInit() {
    if (sessionStorage.getItem(this.global.sessionKey)
      || sessionStorage.getItem(this.global.planSessionKey)
      || sessionStorage.getItem(this.global.topicsSessionKey)) {
      this.modalRef = this.modalService.show(this.templateref);
    }
  }

}
