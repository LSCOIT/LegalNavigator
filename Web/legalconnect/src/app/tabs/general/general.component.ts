import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'general',
    templateUrl: './general.component.html'
})
export class GeneralComponent implements OnInit {
    //  pageHeader: string = "Bing";
    userMessage: string = "";
    ngOnInit() {
        localStorage.setItem('linkName', "");
        
        var hasData = localStorage.getItem('hasData');
        if (hasData == "true")
            this.userMessage = " ";
        else
            this.userMessage = "Please go to chat";
    }
}

