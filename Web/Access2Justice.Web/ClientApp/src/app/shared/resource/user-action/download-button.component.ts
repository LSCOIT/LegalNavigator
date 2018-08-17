import { Component, OnInit, Inject } from '@angular/core';
import * as jsPDF from 'jspdf'

@Component({
  selector: 'app-download-button',
  template: `
 <span id="download" (click)="downloadToPDF()">
 <img src="./assets/images/small-icons/download.svg" class="nav-icon" aria-hidden="true" />
  Download
</span>
 `,

})
export class DownloadButtonComponent implements OnInit {

  constructor() {  
  }

  downloadToPDF() {

    var doc = new jsPDF();
    var elementHandler = {
      '#ignorePDF': function (element, renderer) {
        return true;
      }
    };
  
    var pdf = new jsPDF();
    pdf.addHTML(document.body, function () {
      pdf.save('PersonalisedPlan.pdf');
    });

  }

  ngOnInit() {
  }

}
