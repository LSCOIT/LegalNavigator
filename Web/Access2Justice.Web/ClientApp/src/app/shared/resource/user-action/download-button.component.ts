import { Component, OnInit, Inject, ElementRef, ViewChild, AfterContentInit } from '@angular/core';
import * as jsPDF from 'jspdf'
import { viewParentEl } from '@angular/core/src/view/util';

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
 
    //@ViewChild('pdfContent') content: ElementRef;
  
 

  
  constructor() {
  }

  downloadFile_PDF() {

    //let doc = new jsPDF();
    //let specialElementHandlers = {
    //  '#editor': function (element, renderer) {
    //    return true;
    //  }
    //};
    ////let content = this.content.nativeElement;
    //let content = document.getElementsByClassName("pdfContent")[0].innerHTML;
    //var test = "<html><body>" + content + "</body></html>";
    //var test1 = "<html><body>Test</body></html>";
    ////doc.fromHTML(content, 15, 15, {
    ////  'width': 198,
    ////  'elementHandlers': specialElementHandlers
    ////});

    //doc.fromHTML(content, 0, 0, {
    //  'width': 100, // max width of content on PDF
    //  'elementHandlers': specialElementHandlers
    //},function (bla) { doc.save('saveInCallback.pdf'); },10);

    //doc.save("test.pdf");
    //let content = document.getElementsByClassName("pdfContent")[0].outerHTML;

    var specialElementHandlers = {
      '#editor': function (element, renderer) {
        return true;
      }
    };
    
    
    let content = document.getElementsByClassName("pdfContent")[0].innerHTML;
    var doc = new jsPDF();//('p', 'pt', 'a4', true);
    doc.fromHTML(content, 15, 15, {
      'width': 170,
      'elementHandlers': specialElementHandlers
    }, function (dispose) {
      doc.save('thisMotion.pdf');
    });

    //var pdf_Object = new filePDF('p', 'pt', 'letter');
    //var specialElementHandlers = {
    //  '#editor': function (element, renderer) {
    //    return true;
    //  },
    //  '.controls': function (element, renderer) {
    //    return true;
    //  }
    //};

    //var options = {
    //  background: '#fff',
    //  format: 'PNG',
    //  margin: {
    //    top: 0,
    //    bottom: 0
    //  },
    //  pagesplit: true,
    //  'elementHandlers': specialElementHandlers,
    //};

    //pdf_Object.internal.scaleFactor = 2.25;
    //pdf_Object.addHTML(document.body, options, function () {
    //  pdf_Object.save('PersonalisedPlan-' + new Date() + '.pdf');
    //});
  }

  ngOnInit() {
  }

}
