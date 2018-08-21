import { Component, OnInit, Inject } from '@angular/core';
import * as filePDF from 'jspdf'

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

  constructor() {
  }

  downloadFile_PDF() {

    var pdf_Object = new filePDF('p', 'pt', 'letter');
    var specialElementHandlers = {
      '#editor': function (element, renderer) {
        return true;
      },
      '.controls': function (element, renderer) {
        return true;
      }
    };

    var options = {
      background: '#fff',
      format: 'PNG',      
      margin: {
        top: 0,
        bottom: 0
      },
      pagesplit: true,
      'elementHandlers': specialElementHandlers,      
    };
    pdf_Object.internal.scaleFactor = 2.25;
    pdf_Object.addHTML(document.body, options, function () {
      pdf_Object.save('PersonalisedPlan-' + new Date() + '.pdf');
    });
  }

  ngOnInit() {
  }

}
