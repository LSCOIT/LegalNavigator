import { Component, ViewChildren } from "@angular/core";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { Global } from "../../../global";

@Component({
  selector: "app-warning-question",
  templateUrl: "./auth-warning.component.html",
  styleUrls: ["./auth-warning.component.css"],
})
export class AuthWarningComponent {
  modalRef: BsModalRef;
  @ViewChildren("authWarningPopup") template;
  externalCeUrl: string;
  linkTarget: string;
  constructor(
    private modalService: BsModalService,
    private global: Global
  ) { }

  showAuthWarning(event) {
    this.linkTarget = event.target.target;
    if (!this.global.isLoggedIn) {
      this.externalCeUrl = event.target.href;
      var temp = this.template.first;
      this.modalRef = this.modalService.show(temp, { class: "modal-sm" });
    }
    else {
      this.externalCeUrl = event.target.href.replace('#page1', '&oid=' + this.global.userId + '#page1');
      window.open(this.externalCeUrl, this.linkTarget);
    }
  }

  openExternalGuidedInterview(): void {
    window.open(this.externalCeUrl, this.linkTarget);
    this.modalRef.hide();
  }
}
