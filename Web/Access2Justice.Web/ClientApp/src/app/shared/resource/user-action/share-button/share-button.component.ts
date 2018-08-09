import { Component, OnInit, TemplateRef, Input, ViewChild, ElementRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { api } from '../../../../../api/api';
import { HttpClient } from '@angular/common/http';
import { Share, UnShare } from '../share-button/share.model';
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
  @Input() id: string;//resource Id
  @Input() type: string; //resource Type
  @ViewChild('template') public templateref: TemplateRef<any>;
  userId: string;
  sessionKey: string = "showModal";
  shareInput: Share = { ResourceId: '', UserId: '', Url: '' };
  unShareInput: UnShare = { ResourceId: '', UserId: '' };
  shareView: any;
  blank: string = "";
  permaLink: string = "";
  showGenerateLink: boolean = true;
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
    this.unShareInput.ResourceId = this.getActiveParam();
    this.unShareInput.UserId = this.userId;
    this.shareService.removeLink(this.unShareInput)
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

  buildParams() {
    if (this.id) {
      this.shareInput.Url = this.buildUrl() + this.id;
      this.shareInput.ResourceId = this.id;
    }
    else {
      this.shareInput.Url = location.pathname;
      this.shareInput.ResourceId = this.getActiveParam();
    }
    this.shareInput.UserId = this.userId;
  }
  
  buildUrl() {
    if (this.type === 'Topics') {
      return "/topics/";
    }
    if (this.type === 'Guided Assistant') {
      return "/guidedassistant/";
    }
    else {
      return "/resource/";
    }
  }

}
