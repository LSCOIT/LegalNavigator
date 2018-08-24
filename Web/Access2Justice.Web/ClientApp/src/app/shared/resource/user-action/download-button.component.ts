import { Component, OnInit, InjectionToken } from '@angular/core';
import * as jsPDF from 'jspdf';

@Component({
  selector: 'app-download-button',
  template: `
 <span id="download" (click)="downloadFile_PDF()">
 <img src="./assets/images/small-icons/download.svg" class="nav-icon" aria-hidden="true" />
  Download
</span>
 `,
})
export class DownloadButtonComponent implements OnInit {
  template: string = '';
  applicationUrl: any = window.location.origin;
  title: any = document.title;
  content: any;
  mainHTML: any;
  constructor() { }

  downloadFile_PDF(): void {
    if (location.pathname.indexOf("/plan") >= 0) {
      this.template = 'app-personalized-plan';
      this.buildContent(this.template);
      this.savePDF();
    }

  }

  buildContent(template) {

    this.excludeItems();

    let printContents = document.getElementsByTagName(template)[0].innerHTML;

    this.mainHTML = `
        <html>
          <head>
            <title> ${this.title + ' - ' + this.applicationUrl}</title>
            <style>
                @media print {
                .no-print, .no-print * {
                  visibility: hidden;
                  height: 0;
                }
                .print-only * {
                   display: block;
                }
                .collapse {
                    display: block !important;
                    height: 0 !important;
                }
                .hours li span {
                    margin-right: 10px;
                }
              }
            </style>
          </head>
      <body >${printContents}.</body>
        </html>`

  };

  savePDF() {

    var specialElementHandlers = {
      '#editor': function (element, renderer) {
        return true;
      }
    };

    var generatePDFDoc = new jsPDF();
    var imgRegex = new RegExp('<img[^>]*?>', 'g');
    var content = this.mainHTML.replace(this.mainHTML.match(imgRegex), "");

    generatePDFDoc.fromHTML(content, 15, 15, {
      'width': 170,
      'elementHandlers': specialElementHandlers
    }, function (dispose) {
      generatePDFDoc.save('PersonalizedPlan.pdf');
    });

  }

  excludeItems() {
    for (var i = document.images.length; i-- > 0;) {
      document.images[i].parentNode.removeChild(document.images[i]);
    }
    document.getElementsByTagName('app-share-button')[0].innerHTML = "";
    document.getElementsByTagName('app-user-action-sidebar')[0].innerHTML = "";
    document.getElementsByTagName('app-print-button')[0].innerHTML = "";
    document.getElementsByTagName('app-download-button')[0].innerHTML = "";
  }

  ngOnInit() {
  }

}
