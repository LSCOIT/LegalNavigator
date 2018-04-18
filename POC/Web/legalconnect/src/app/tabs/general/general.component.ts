import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'general',
    templateUrl: './general.component.html'
})
export class GeneralComponent implements OnInit {
    
    ngOnInit() {
        localStorage.setItem('linkName', "");
        localStorage.setItem("play", "false");
        
       
       
    }
}

