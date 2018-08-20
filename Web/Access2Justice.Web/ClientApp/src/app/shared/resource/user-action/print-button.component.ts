import { Component, OnInit, Input } from '@angular/core';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { validateConfig } from '@angular/router/src/config';

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


  constructor() { }

  print(): void {

    if (location.pathname.indexOf("/topics") >= 0) {
      this.template = 'app-subtopic-detail';
      window.print();
    }

    else if (location.pathname.indexOf("/plan") >= 0) {
      this.template = 'app-personalized-plan'
      this.printContents(this.template)
    }

    else if (location.pathname.indexOf("/profile") >= 0) {
      this.template = 'app-action-plans';
      this.printContents(this.template);
    }

    else {
      window.print();
    }
  }

  printContents(template) {
    let printContents, popupWin;

  printContents = document.getElementsByTagName(template)[0].innerHTML;
  popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
  popupWin.document.open();
  popupWin.document.write(`
      <html>
        <head>
          <style>
              @media print {
              .no-print, .no-print * {
                display: none !important;
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



  //print() {
  //  this.toggle();

  //  window.print();

  //  //var mywindow = window.open('', 'PRINT', 'height=400,width=600');
  //  //let list = document.getElementsByTagName("selectorInput")[0].innerHTML;
  //  //mywindow.document.write(document.getElementsByTagName("app-action-plans")[0].innerHTML);
  //  //mywindow.print();
  //  //mywindow.close();
  //  //return true;

  //  //mywindow.document.write('<html><head><title>' + document.title + '</title>');
  //  //mywindow.document.write('</head><body >');
  //  //mywindow.document.write('<h1>' + document.title + '</h1>');
  //  //mywindow.document.write(document.getElementsByTagName("app-action-plans")[0].innerHTML);

  //  //mywindow.document.write(document.getElementsByTagName("app-search-results")[0].innerHTML);
  //  //mywindow.document.write('</body></html>');

  //  //mywindow.document.close(); // necessary for IE >= 10
  //  //mywindow.focus(); // necessary for IE >= 10*/


  //}

  printSection(): void {
    let data = document.getElementsByClassName("Items-Container")[0].innerHTML;
    let newWindow = window.open("data:text/html," + encodeURIComponent(data),
      "_blank");
    newWindow.focus();
  }

  ngOnInit() {
    if (this.printData) {
      console.log("User acrtion" + this.printData);
    }
  }

  ngOnChanges() {
    if (this.printData) {
      console.log("User acrtion" + this.printData);
    }
  }
}
