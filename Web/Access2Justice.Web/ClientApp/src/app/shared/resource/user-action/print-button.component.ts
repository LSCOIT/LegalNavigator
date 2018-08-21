import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-print-button',
  template: `
  <span (click)="print()">Print</span>
 `
})

export class PrintButtonComponent implements OnInit {
  template: string = '';
  applicationUrl: any = window.location.host;
  title: any = document.title;
  activeTab: string = '';
  constructor() { }

  print(): void {
    if (location.pathname.indexOf("/topics") >= 0) {
      this.template = 'app-subtopic-detail';
      this.printContents(this.template)
    }

    else if (location.pathname.indexOf("/plan") >= 0) {
      this.template = 'app-personalized-plan'
      this.printContents(this.template)
    }

    else if (location.pathname.indexOf("/resource") >= 0) {
      this.template = 'app-resource-card-detail'
      this.printContents(this.template)
    }

    else if (location.pathname.indexOf("/profile") >= 0) {
      this.activeTab = document.getElementsByClassName("nav-link active")[0].innerText;

      if (this.activeTab == "My Plan") {
        this.template = 'app-action-plans'
        this.printContents(this.template);
      }
      else if (this.activeTab == "My Saved Resources") {
        this.template = 'app-search-results';
        this.printContents(this.template)        
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
            <title>Access to justice - https://a2jdevweb.azurewebsites.net/</title>
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
