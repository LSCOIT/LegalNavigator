import { Component, OnInit, Input, AfterViewInit, ElementRef, Renderer } from '@angular/core';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { validateConfig } from '@angular/router/src/config';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-print-button',
  template: `
  <span (click)="print()">Print</span>
 `
})

export class PrintButtonComponent implements OnInit, OnChanges {
  @Input()
  printData: any;
  template: string = '';
  applicationUrl: any = environment.applicationUrl;

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
      this.template = 'app-action-plans'
      if (this.printData == "My Plan") {
        this.template = 'app-action-plans'
        this.printContents(this.template);
      }
      else if (this.printData == "My Saved Resources") {
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
    //popupWin.document.open();
    popupWin.document.write(`
        <html>
          <head>
            <h2><title>Legal Assist https://a2jdevweb.azurewebsites.net/ </title></h2>
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
    //if (this.printData) {
    //  //console.log("User action" + this.printData);
    //}
  }

  ngOnChanges() {
    if (this.printData) {
      console.log("User action" + this.printData);
    }
  }
}
