import { HttpClient } from "@angular/common/http";
import { Component, Input, OnInit, ViewChildren } from "@angular/core";
import { BsModalRef, BsModalService } from "ngx-bootstrap";

@Component({
  selector: "app-inner-question",
  templateUrl: "./inner-question.component.html",
  styleUrls: ["./inner-question.component.scss"],
})
export class InnerQuestionComponent implements OnInit {
  @Input() inputData: any;
  @ViewChildren("caseExampleTemplate") template;
  errorMessage: string;
  caseObject: any;
  modalRef: BsModalRef;
  caseId: any;
  constructor(
    private modalService: BsModalService,
    private httpClient: HttpClient
  ) {}

  lookup(): void {
    this.errorMessage = "";
    this.caseObject = null;
    this.httpClient
      .get<any>(
        "https://j8p7pj61n8.execute-api.us-west-2.amazonaws.com/default/hawaiiCaseLookup" +
          "?id=" +
          this.caseId
      )
      .subscribe(
        (response) => {
          var responseApiCode = response.responseCode;
          if (responseApiCode === 503) {
            this.errorMessage = response.message;
          } else if (responseApiCode == 100) {
            this.caseObject = new CaseObject(
              response.caseId,
              response.caseStatus,
              response.caseTitle
            );
          } else if (responseApiCode == 404) {
            this.errorMessage = "case with ID " + this.caseId + " is not found";
          }
        },
        (error) => {
          console.log(error);
        }
      );
  }

  ngOnInit() {}

  openModal(event: any) {
    if (event.target.id === "case-lookup") {
      var temp = this.template.first;
      this.modalRef = this.modalService.show(temp, { class: "modal-sm" });
    }
  }
}
export class CaseObject {
  constructor(caseId, caseStatus, caseTitle) {
    this.caseId = caseId;
    this.caseStatus = caseStatus;
    this.caseTitle = caseTitle;
  }
  caseId: string;
  caseStatus: string;
  caseTitle: string;
}
