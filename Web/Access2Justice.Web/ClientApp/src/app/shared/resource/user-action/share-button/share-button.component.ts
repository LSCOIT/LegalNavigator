import { Component, OnInit, TemplateRef, Input, ViewChild } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { api } from '../../../../../api/api';
import { HttpClient } from '@angular/common/http';
import { Share, UnShare, ShareView } from '../share-button/share.model';
import { ShareService } from '../share-button/share.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-share-button',
  templateUrl: './share-button.component.html',
  styleUrls: ['./share-button.component.css']
})
export class ShareButtonComponent implements OnInit {
  @Input() showIcon = true;
  modalRef: BsModalRef;
  @ViewChild('template') public templateref: TemplateRef<any>;
  userId: string;
  sessionKey: string = "showModal";
  shareInput: Share = { ResourceId: '', UserId: '', Url: '' };
  unShareInput: UnShare = { ResourceId: '', UserId: '' };
  shareView: ShareView = { PermaLink: '', IsGenerated: true };
  permaLink: any;
  hideGenerateLink: boolean = false;
  resourceUrl: string = window.location.protocol + "//" + window.location.host + "/share/";
  
  constructor(private modalService: BsModalService,
    private arrayUtilityService: ArrayUtilityService,
    private httpClient: HttpClient,
    private shareService: ShareService,
    private activeRoute: ActivatedRoute) {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
  }

  openModal(template: TemplateRef<any>) {
    if (!this.userId) {
      sessionStorage.setItem(this.sessionKey, "true");
      this.externalLogin();
    } else {
      this.checkPermaLink();
      this.modalRef = this.modalService.show(template);
    }
  }

  close() {
    this.modalRef.hide();
  }

  checkPermaLink() {
    this.shareInput.ResourceId = this.getActiveParam();
    this.shareInput.Url = location.pathname;
    this.shareInput.UserId = this.userId;
    this.shareService.checkLink(this.shareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.shareView = response;
        }
      });
  }

  generateLink() {
    this.shareInput.ResourceId = this.getActiveParam();
    this.shareInput.Url = location.pathname;
    this.shareInput.UserId = this.userId;
    this.shareService.generateLink(this.shareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.shareView = response;
        }
      });
  }

  removeLink(): void {
    this.unShareInput.ResourceId = this.getActiveParam();
    this.unShareInput.UserId = this.userId;
    this.shareService.removeLink(this.unShareInput)
      .subscribe(response => {
        if (response != undefined) {
          this.close();
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

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  ngOnInit() {
    if (this.userId) {
      let hasLoggedIn = sessionStorage.getItem(this.sessionKey);
      if (hasLoggedIn) {
        sessionStorage.removeItem(this.sessionKey);
        this.openModal(this.templateref);
      }
    }

  }

}
