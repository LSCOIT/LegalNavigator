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
  providers: [
    { provide: 'Window', useValue: window }
  ]
})
export class DownloadButtonComponent implements OnInit {

  constructor() {
  @Inject('Window') private window: Window;
    }

  downloadToPDF() {
   
    var doc = new jsPDF();
    doc.text(20, 20, 'Personalized Plan');
    doc.text(20, 30, 'This is first page.');
    doc.addPage();
    doc.text(20, 20, 'This is second page.');

    doc.save('PersonalizedPlan.pdf');
  }
  
  ngOnInit() {
  }


}
