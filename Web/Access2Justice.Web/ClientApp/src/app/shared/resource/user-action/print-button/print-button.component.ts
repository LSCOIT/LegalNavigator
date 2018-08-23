import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-print-button',
  template:`
  <span (click)="print()" [ngClass]="{'link': addLinkClass, '': !addLinkClass}">
      <img *ngIf="showIcon" src="./assets/images/small-icons/print.svg" class="nav-icon" aria-hidden="true" />
      Print
</span>
 `
})

export class PrintButtonComponent implements OnInit {
  template: string = '';
  applicationUrl: any = window.location.origin;
  title: any = document.title;
  activeTab: string = '';
  activeRouteName: string = '';
  @Input() showIcon: boolean = true;
  @Input() addLinkClass: boolean = false;

  constructor(private activeRoute: ActivatedRoute) { }

  print(): void {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 0; i < routeParts.length; i++) {
          this.activeRouteName = routeParts[i].path;
          break;
        }
      });

    if (this.activeRouteName === "subtopics") {
      this.template = 'app-subtopic-detail';
      this.printContents(this.template);
    }

    else if (this.activeRouteName === "plan") {
      this.template = 'app-personalized-plan';
      this.printContents(this.template);
    }

    else if (this.activeRouteName === "resource") {
      this.template = 'app-resource-card-detail';
      this.printContents(this.template);
    }

    else if (this.activeRouteName === "profile") {
      this.activeTab = document.getElementsByClassName("nav-link active")[0].firstElementChild.textContent;

      if (this.activeTab == "My Plan") {
        this.template = 'app-action-plans';
        this.printContents(this.template);
      }
      else if (this.activeTab == "My Saved Resources") {
        this.template = 'app-search-results';
        this.printContents(this.template);
      }
    }

    else {
      window.print();
    }
  }

  printContents(template) {
    let printContents, popupWin;
    printContents = document.getElementsByTagName(template)[0].innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.write(`
        <html>
          <head>
            <title> ${this.title + ' - ' + this.applicationUrl}</title>
            <style>
                @media print {
                .no-print, .no-print * {
                  display: none !important;
                }
                .print-only * {
                   display: block;
                }
                .collapse {
                    display: block !important;
                    height: auto !important;
                }
                .hours li span {
                    margin-right: 10px;
                }
              }
            </style>
          </head>
      <body onload="window.print();window.close()">${printContents}</body>
        </html>`
    );
    popupWin.document.close();
  }

  ngOnInit() {
  } 
}
