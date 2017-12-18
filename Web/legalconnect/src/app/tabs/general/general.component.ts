import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'general',
    templateUrl: './general.component.html'
})
export class GeneralComponent implements OnInit {
    //  pageHeader: string = "Bing";

    ngOnInit() {
        localStorage.setItem('linkName', "");
        console.log('general');
    }
}

