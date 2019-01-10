import { HttpClient, HttpParams } from "@angular/common/http";
import { Component, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { Global, UserStatus } from "../../../../../global";
import { ShareView } from "../../share-button/share.model";
import { ShareService } from "../../share-button/share.service";

@Component({
  selector: "app-share-button-route",
  templateUrl: "./share-button-route.component.html",
  styleUrls: ["./share-button-route.component.css"]
})
export class ShareButtonRouteComponent implements OnInit {
  profileData: ShareView = { UserId: "", UserName: "" };
  modalRef: BsModalRef;
  @ViewChild("template") template: TemplateRef<any>;
  agreed: boolean = true;

  constructor(
    private modalService: BsModalService,
    private httpClient: HttpClient,
    private shareService: ShareService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private global: Global
  ) {}

  openModal(template: TemplateRef<any>) {
    let config = {
      ignoreBackdropClick: true,
      keyboard: false
    };
    this.modalRef = this.modalService.show(template, config);
  }

  getResourceLink(): void {
    let params = new HttpParams().set(
      "permaLink",
      this.activeRoute.snapshot.params["id"]
    );
    this.shareService.getResourceLink(params).subscribe(response => {
      if (response != undefined && response.resourceLink != undefined) {
        if (
          response.resourceLink.indexOf("http") === -1 ||
          response.resourceLink.indexOf("//") === -1
        ) {
          if (response.userId && response.userName) {
            this.global.sharedUserId = response.userId;
            this.global.sharedUserName = response.userName;
            this.global.isShared = true;
            this.profileData.UserName = response.userName;
            this.profileData.UserId = response.userId;
            this.openModal(this.template);
          }
          this.global.role = UserStatus.Shared;
          return this.router.navigateByUrl(response.resourceLink, {
            skipLocationChange: true
          });
        } else {
          return (location.href = response.resourceLink);
        }
      }
      return this.router.navigateByUrl("/404");
    });
  }

  onSubmit(agreementForm: NgForm) {
    if (agreementForm.value.agreement) {
      this.modalRef.hide();
    }
  }

  ngOnInit() {
    this.getResourceLink();
  }
}
