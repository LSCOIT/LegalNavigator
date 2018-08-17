import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ShareService } from '../../share-button/share.service';
import { ActivatedRoute, Router } from '@angular/router';
import { api } from '../../../../../../api/api';
import { HttpClient, HttpParams } from '@angular/common/http';
import { window } from 'rxjs/operator/window';
import { ShareView } from '../../share-button/share.model';
import { Global, UserStatus } from '../../../../../global';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-share-button-route',
  templateUrl: './share-button-route.component.html',
  styleUrls: ['./share-button-route.component.css']
})
export class ShareButtonRouteComponent implements OnInit {
  profileData: ShareView = { UserId: '', UserName: '' };
  modalRef: BsModalRef;
  @ViewChild('template') template: TemplateRef<any>;

  constructor(

    private modalService: BsModalService,
    private httpClient: HttpClient,
    private shareService: ShareService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private global: Global) {
    if (global.role === UserStatus.Shared) {
      global.showShare = false;
      global.showRemove = false;
      global.showMarkComplete = false;
    }

  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  close() {
    //need to add validation and prevent from clicking out of modal until i agree has been checked
    //do we need to send this agreement somewhere?
    this.modalRef.hide();
  }

  ngOnInit() {
    this.getResourceLink();
  }

  getResourceLink(): void {
    let params = new HttpParams()
      .set("permaLink", this.activeRoute.snapshot.params['id']);
    this.shareService.getResourceLink(params)
      .subscribe(response => {
        if (response != undefined && response.resourceLink != undefined) {
          if (response.resourceLink.indexOf("http") == -1
            || response.resourceLink.indexOf("//") == -1) {
            if (response.userId && response.userName) {
              this.profileData.UserId = response.userId;
              this.profileData.UserName = response.userName;
              this.profileData.IsShared = true;
              sessionStorage.setItem("profileData", JSON.stringify(this.profileData));
            }
            this.global.role = UserStatus.Shared;
            this.openModal(this.template);
            return this.router.navigateByUrl(response.resourceLink, { skipLocationChange: true });
          }
          else {
            return location.href = response.resourceLink;
          }
        }
        return this.router.navigateByUrl("/404");
      });
  }
}
