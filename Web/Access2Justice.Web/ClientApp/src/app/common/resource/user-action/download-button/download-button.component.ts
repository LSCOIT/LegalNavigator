import * as jsPDF from "jspdf";
import { Component, Input, OnInit } from "@angular/core";
import { HttpParams } from "@angular/common/http";
import { Global, UserStatus } from '../../../../global';

import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { TopicService } from "../../../../topics-resources/shared/topic.service";

@Component({
  selector: "app-download-button",
  template: `
    <button id="download" class="user-button" (click)="downloadPDF()">
      <img
        src="./assets/images/small-icons/download.svg"
        class="nav-icon"
        aria-hidden="true"
      />
      Download
    </button>
    <div class="popover-content">
      <p class="popover-message">
        Are you using a public computer? Consider using the print option
        instead.
      </p>
    </div>
  `,
  providers: [
    PersonalizedPlanComponent
  ]
})
export class DownloadButtonComponent implements OnInit {
  template: string = "";
  applicationUrl: any = window.location.origin;
  title: any = document.title;
  content: any;
  mainHTML: any;
  activeTab: string = "";

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private personalizedPlanComponent: PersonalizedPlanComponent,
    private topicService: TopicService,
    private global: Global,
  ) {
    global.showRemove = !(global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0);
  }

  downloadPDF() {
    if (location.pathname.indexOf("/topics") >= 0) {
      let address, location;
      // this.template = "app-subtopic-detail";
      if (sessionStorage.getItem("globalMapLocation")) {
        address = JSON.parse(
          sessionStorage.getItem("globalMapLocation")
        );
        location = address.location;
      }
      let params = new HttpParams()
      .set("topicId", this.global.activeSubtopicParam)
      .set("state", location.state || '')
      .set("county", location.country || '')
      .set("city", location.city || '')
      .set("zipCode", location.zipCode || '');
      
      this.topicService.printTopic(params).subscribe(response => {
        if(response) {
          var file = new Blob([response], {type: 'application/pdf'});
          var fileURL = URL.createObjectURL(file);
          window.open(fileURL, '_blank', '');
        }
      });
    } else if (location.pathname.indexOf("/plan") >= 0) {
      //this.template = "app-personalized-plan";
      let params = new HttpParams()
      .set("curatedExperienceId", this.personalizedPlanService.planDetails.curatedExperienceId)
      .set("answersDocId", this.personalizedPlanService.planDetails.answersDocId);
      this.personalizedPlanService.printUserPlan(params).subscribe(response => {
        if(response) {
          var file = new Blob([response], {type: 'application/pdf'});
          var fileURL = URL.createObjectURL(file);
          window.open(fileURL, '_blank', '');
        }
      });
    } else if (location.pathname.indexOf("/resource") >= 0) {
      // this.template = "app-resource-card-detail";
      let params = new HttpParams()
      .set("resourceId", this.global.activeSubtopicParam);
      
      this.topicService.printResourceTopic(params).subscribe(response => {
        if(response) {
          var file = new Blob([response], {type: 'application/pdf'});
          var fileURL = URL.createObjectURL(file);
          window.open(fileURL, '_blank', '');
        }
      });
    } else if (location.pathname.indexOf("/profile") >= 0) {
      this.activeTab = document.getElementsByClassName(
        "nav-link active"
      )[0].firstElementChild.textContent;
      if (this.activeTab === "My Plan") {
        this.template = "app-action-plans";
      } else if (this.activeTab === "My Saved Resources") {
        this.template = "app-search-results";
      }      
      this.buildContent(this.template);
    } else {
      this.template = "body";
      this.buildContent(this.template);
    }
  }

  // downloadPDF(): void {
  //   if (location.pathname.indexOf("/topics") >= 0) {
  //     this.template = "app-subtopic-detail";
  //   } else if (location.pathname.indexOf("/plan") >= 0) {
  //     this.template = "app-personalized-plan";
  //   } else if (location.pathname.indexOf("/resource") >= 0) {
  //     this.template = "app-resource-card-detail";
  //   } else if (location.pathname.indexOf("/profile") >= 0) {
  //     this.activeTab = document.getElementsByClassName(
  //       "nav-link active"
  //     )[0].firstElementChild.textContent;
  //     if (this.activeTab === "My Plan") {
  //       this.template = "app-action-plans";
  //     } else if (this.activeTab === "My Saved Resources") {
  //       this.template = "app-search-results";
  //     }
  //   } else {
  //     this.template = "body";
  //   }
  //   this.buildContent(this.template);
  // }

  buildContent(template) {
    let printContents = document.getElementsByTagName(template)[0].innerHTML;
    let popupWin = window.open(
      "",
      "_blank",
      "top=0,left=0,height=100%,width=auto"
    );
    popupWin.document.write(`${printContents}`);
    this.excludeItems(popupWin);
    this.savePDF(popupWin);
  }

  savePDF(content) {
    let generatePDFDoc = new jsPDF();
    generatePDFDoc.fromHTML(
      content.document.body,
      13,
      1,
      {
        width: 180
      },
      dispose => {
        generatePDFDoc.save("Document.pdf");
      }
    );
    setTimeout(() => content.close(), 1000);
  }

  excludeItems(newDoc) {
    let images = newDoc.document.getElementsByClassName("no-print");
    for (let i = 0; i < images.length; i++) {
      images[i].src = "";
      images[i].innerHTML = "";
    }
  }

  ngOnInit() {}
}
