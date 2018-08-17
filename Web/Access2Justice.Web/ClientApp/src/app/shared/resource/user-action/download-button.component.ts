import { Component, OnInit, Inject } from '@angular/core';
import * as jsPDF from 'jspdf'

@Component({
  selector: 'app-download-button',
  template: `
 <span id="download" (click)="downloadToPDF()">
 <img src="./assets/images/small-icons/download.svg" class="nav-icon" aria-hidden="true" />
  Download
</span>
 //`,
 // providers: [
 //   { provide: 'Window', useValue: window }
 // ]
})
export class DownloadButtonComponent implements OnInit {

  constructor() {
  //@Inject('Window') private window: Window;
    }

  downloadToPDF() {
   
    var doc = new jsPDF();
    var elementHandler = {
      '#ignorePDF': function (element, renderer) {
        return true;
      }
    };
    var source = window.document.getElementsByTagName("app-personalized-plan")[0];
    doc.fromHTML(
      source,
      15,
      15,
      {
        'width': 180, 'elementHandlers': elementHandler
      });

    doc.output("dataurlnewwindow");
  }
  
  ngOnInit() {
  }


}
