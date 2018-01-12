import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'doc',
    templateUrl: './doc.component.html'
})
export class DocComponent implements OnInit{

    pdfSrc: string = 'images/Alaska-Eviction-Notice-to-Quit-Form.pdf';
    showDoc: string = "";

    ngOnInit() {
        this.showDoc = localStorage.getItem('hasData');
    }
}

