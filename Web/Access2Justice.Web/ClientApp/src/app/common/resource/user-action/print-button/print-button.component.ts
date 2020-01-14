import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TopicService } from "../../../../topics-resources/shared/topic.service";
import { HttpParams } from "@angular/common/http";

@Component({
  selector: "app-print-button",
  template:`
    <button
      (click)="print()"
      class="user-button"
      id="print"
      [ngClass]="{ link: addLinkClass, '': !addLinkClass }"
    >
      <img
        *ngIf="showIcon"
        src="./assets/images/small-icons/print.svg"
        class="nav-icon"
        aria-hidden="true"
      />
      Print
    </button>
  `
})
export class PrintButtonComponent implements OnInit {
  template: string = "";
  applicationUrl: any = window.location.origin;
  title: any = document.title;
  activeTab: string = "";
  activeRouteName: string = "";
  activeTopic: any;
  topic: any;
  logoSrc: any;
  @Input() showIcon: boolean = true;
  @Input() addLinkClass: boolean = false;

  constructor(private activeRoute: ActivatedRoute, 
    private topicService: TopicService) {
  }

  getOrganizationalUnit(): string {
    var locationDetails = JSON.parse(
      sessionStorage.getItem("globalMapLocation")
    );
    return locationDetails.location.state;
  }

  print(): void {
    this.activeRoute.url.subscribe(routeParts => {
      if(routeParts.length > 1){
        this.activeTopic = routeParts[routeParts.length-1].path;
      }
      for (let i = 0; i < routeParts.length; i++) {
        this.activeRouteName = routeParts[i].path;
        break;
      }
    });
    if (this.activeRouteName === "subtopics") {
      this.template = "app-subtopic-detail";
    } else if (this.activeRouteName === "plan") {
      this.template = "app-personalized-plan";
    } else if (this.activeRouteName === "resource") {
      this.template = "app-resource-card-detail";
    } else if (this.activeRouteName === "profile") {
      this.activeTab = document.getElementsByClassName(
        "nav-link active"
      )[0].firstElementChild.textContent;
      if (this.activeTab === "My Plan") {
        this.template = "app-action-plans";
      } else if (this.activeTab === "My Saved Resources") {
        this.template = "app-saved-resources";
      }else if (this.activeTab === "Incoming Resources") {
        this.template = "app-incoming-resources";
      }
    } else {
      this.template = "body";
    }
   

    if(this.template === "app-resource-card-detail"){
          if (sessionStorage.getItem('globalMapLocation')) {
            let location = JSON.parse(
              sessionStorage.getItem('globalMapLocation')
            );
            let state = location.location.state;
            let params = new HttpParams()
            .set("organizationalUnit", state);    

            this.topicService.getLogo(params).subscribe(logo => {
              this.logoSrc = logo ? logo.source : '';
              this.printContents(this.template, this.logoSrc);
            });
          }
    }
    else if(this.template === "app-personalized-plan"){
      this.printContents(this.template);
    }
else if(this.activeTopic){
    this.topicService.getDocumentDataWithShared(this.activeTopic, true).subscribe(topic => {
      if(topic[0]) {
        this.topic = topic[0];
        let params = new HttpParams()
          .set("organizationalUnit", this.topic.organizationalUnit);
          
        this.topicService.getLogo(params).subscribe(logo => {
          this.logoSrc = logo ? logo.source : '';
          this.printContents(this.template, this.logoSrc);
        });
      }
    });
  } else if(this.template === "app-action-plans"){
    var organizationalUnit = this.getOrganizationalUnit();
    let params = new HttpParams()
    .set("organizationalUnit", organizationalUnit);
    
    this.topicService.getLogo(params).subscribe(logo => {
      this.logoSrc = logo ? logo.source : '';
      this.printContents(this.template, this.logoSrc);
    });
  }
  else{
    this.printContents(this.template);
  }
}

  printContents(template, logoSrc = '') {
    var imageElement = '';
    if(logoSrc){ 
      imageElement = '<img src="data:image/gif;base64,' + logoSrc +' " width="100" height="100">';  
    }

    let printContents = document.getElementsByTagName(template)[0].innerHTML;

    let popupWin = window.open(
      "",
      "_blank",
      "top=0,left=0,height=100%,width=auto"
    );
    popupWin.document.write(`
        <html>
          <head>
            <title> ${this.title + " - " + this.applicationUrl}</title>
            <style>
                @media print {
                .no-print, .no-print * {
                  display: none !important;
                }
                .row {
                  margin: 10px auto;
                  width: 100%;
                }
                .print-only * {
                   display: block;
                }
                .collapse {
                    display: block !important;
                    height: auto !important;
                }
                li {
                  margin-bottom: 10px;
                }
                .hours li span {
                  margin-right: 10px;
                }
                .subtopic-heading {
                  display: inline-block;
                }
                a {
                  text-decoration: none;
                }
              }
            </style>
          </head>
      <body onload="window.print();window.close()">${imageElement} ${printContents}</body>
        </html>`);
    popupWin.document.close();
  }

  ngOnInit() {}
}
