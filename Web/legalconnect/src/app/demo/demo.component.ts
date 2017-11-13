import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Response } from '@angular/http';
import { SearchService } from '../services/search.service';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'demo',
    templateUrl: './demo.component.html'
})

export class DemoComponent  {
    result: any;
    constructor(private router: Router, private srchServ: SearchService) {
        //http.get('https://api.cognitive.microsoft.com/bing/v5.0/search?q=I am beign evictied&count=10&offset=0&mkt=en-us&safesearch=Moderate')
        // Call map on the response observable to get the parsed people object
        //.map(res => res.json())
        // Subscribe to the observable to get the parsed people object and attach it to the
        // component
        //  .subscribe(people => this.people = people);
    }

}
