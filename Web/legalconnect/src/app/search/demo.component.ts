import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Response } from '@angular/http';
import { SearchService } from '../services/search.service';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'demo',
    templateUrl: './demo.component.html'
})

export class DemoComponent implements OnInit  {
    result: any;
    searchText:string ;
    constructor(private router: Router, private srchServ: SearchService) {
        
    }
    ngOnInit() {
        this.searchText = localStorage.getItem('sentence');
    }
}
