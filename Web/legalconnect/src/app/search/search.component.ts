import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from "@angular/platform-browser";
import { SearchService } from '../services/search.service';

@Component({
    selector: 'search',
    templateUrl: './search.component.html',
    styles: [`
    ng2-auto-complete, input {
      display: block; border: 1px solid #ccc; width: 300px;
    }
  `]
})
export class SearchComponent implements OnInit {
    searchText: string;
    arrayOfStrings = ["I am being evicted",
        "How to fight for eviction",
        "what to do when you get an eviction notice",
        "can an eviction be reversed",
        "how can i prolong an eviction",
        "how to drag out an eviction",
        "getting evicted nowhere to go",
        "getting evicted for not paying rent",
        "stop eviction process",
        "how an eviction works",
        "how long does it take to evict someone",
        "how an eviction affects you",
        "apartment eviction process",
    "my husband makes me feel unsafe",];

    constructor(private router: Router, private srchServ: SearchService) { }


    ngOnInit() {
        localStorage.setItem('linkName', "");
        this.srchServ.getLocation().subscribe(
            (resCountry) => {
                localStorage.setItem('geoState', resCountry.region);
            });
    }

    redirect() {
        localStorage.setItem('returnFlag', 'false');
        localStorage.setItem('sentence', this.searchText );
        this.router.navigateByUrl("demo");
    }

    valueChanged(newVal: any) {
        this.searchText = newVal;
        
    }
}

