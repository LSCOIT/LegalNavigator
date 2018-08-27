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
    }
  }

  buildContent(template) {
    let printContents = document.getElementsByTagName(template)[0].innerHTML;
    let popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.write(`${printContents}`);
    this.excludeItems(popupWin);
    this.savePDF(popupWin.document.body);
    popupWin.close();
  };

  savePDF(content) {
    let generatePDFDoc = new jsPDF();
    generatePDFDoc.fromHTML(content, 13, 1, {
      'width': 180
    }, (dispose) => {
      generatePDFDoc.save('PersonalizedPlan.pdf');
    });
  }

  excludeItems(newDoc) {
    let images = newDoc.document.getElementsByClassName('no-print');
    for (let i = 0; i < images.length; i++) {
      images[i].src = '';
      images[i].innerHTML = '';
    }
  }

  ngOnInit() {
  }

}
