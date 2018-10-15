import { Component, OnInit, TemplateRef, Input, ViewChild, ElementRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { api } from '../../../../../api/api';
import { HttpClient } from '@angular/common/http';
import { Share } from '../share-button/share.model';
import { ShareService } from '../share-button/share.service';
import { ActivatedRoute } from '@angular/router';
import { Global, UserStatus } from '../../../../global';
import { MsalService } from '@azure/msal-angular';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-share-button',
  templateUrl: './share-button.component.html',
  styleUrls: ['./share-button.component.css']
})
export class ShareButtonComponent implements OnInit {
  @Input() showIcon = true;
  modalRef: BsModalRef;
  @Input() id: string;//resource Id
  @Input() type: string; //resource Type
  @Input() url: string; //resource Url
  @Input() webResourceUrl: string;//web resource Url
  @ViewChild('template') public templateref: TemplateRef<any>;
  userId: string;
  sessionKey: string = "showModal";
  shareInput: Share = { ResourceId: '', UserId: '', Url: '' };
  shareView: any;
  blank: string = "";
  permaLink: string = "";
  showGenerateLink: boolean = true;
  resourceUrl: string = window.location.protocol + "//" + window.location.host + "/share/";
  emptyId: string = "{00000000-0000-0000-0000-000000000000}";
  @Input() addLinkClass: boolean = false;

  constructor(private modalService: BsModalService,
    private arrayUtilityService: ArrayUtilityService,
    private httpClient: HttpClient,
    private shareService: ShareService,
    private activeRoute: ActivatedRoute,
    public global: Global,
    private msalService: MsalService) {    
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showShare = false;
    }
    else {
      global.showShare = true;
    }
  }

  openModal(template: TemplateRef<any>) {    
    if (!this.global.userId) {
      sessionStorage.setItem(this.sessionKey, "true");
      this.externalLogin();
    } else {
      this.checkPermaLink(template);
    }
  }

  close() {
    this.modalRef.hide();
  }

  checkPermaLink(template) {
    this.buildParams();
    this.shareService.checkLink(this.shareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.shareView = response;
          if (this.shareView.permaLink) {
            this.showGenerateLink = false;
            this.permaLink = this.getPermaLink();
          }
        }
        this.modalRef = this.modalService.show(template);
      });
  }

  generateLink() {
    this.buildParams();
    this.shareService.generateLink(this.shareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.shareView = response;
          this.permaLink = this.getPermaLink();
          this.showGenerateLink = false;
        }
      });
  }

  removeLink(): void {
    this.buildParams();
    this.shareService.removeLink(this.shareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.close();
          this.showGenerateLink = true;
          this.permaLink = this.blank;
        }
      });
  }

  getActiveParam() {
    if (this.activeRoute.snapshot.params['topic'] != null) {
      return this.activeRoute.snapshot.params['topic'];
    };
    if (this.activeRoute.snapshot.params['id'] != null) {
      return this.activeRoute.snapshot.params['topic'];
    };
  }

  copyLink(inputElement) {
    inputElement.select();
    document.execCommand('copy');
    inputElement.setSelectionRange(0, 0);
  }

  getPermaLink() {
    if (this.shareView.permaLink)
      return this.resourceUrl + this.shareView.permaLink;
    else
      return this.blank;
  }

  externalLogin() {
    this.msalService.loginRedirect(environment.consentScopes);
  }

  ngOnInit() {
    if (this.global.userId) {
      let hasLoggedIn = sessionStorage.getItem(this.sessionKey);
      if (hasLoggedIn) {
        sessionStorage.removeItem(this.sessionKey);
        this.openModal(this.templateref);
      }
    }
  }

  buildParams() {
    if (this.id || this.type === 'WebResources') {
      this.shareInput.Url = this.buildUrl();
      this.shareInput.ResourceId = this.id;
    }
    else {
      this.shareInput.Url = location.pathname;
      this.shareInput.ResourceId = this.getActiveParam();
    }
    this.shareInput.UserId = this.global.userId;
  }

  buildUrl() {
    if (this.type === 'Topics') {
      return "/topics/" + this.id;
    }
    if (this.type === 'Guided Assistant') {
      return "/guidedassistant/" + this.id;
    }
    if (this.type === 'Forms' || this.type === 'Essential Readings') {
      return this.url;
    }
    if (this.type === 'WebResources') {
      this.id = this.emptyId;
      if (this.url) {
        return this.url;
      } else if (this.webResourceUrl) {
        return this.webResourceUrl
      }
    }
    else {
      return "/resource/" + this.id;
    }
  }

}
