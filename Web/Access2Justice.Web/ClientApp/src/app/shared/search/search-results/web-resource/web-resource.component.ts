import { Component, OnInit, Input } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'app-web-resource',
    templateUrl: './web-resource.component.html',
    styleUrls: ['./web-resource.component.css']
})
export class WebResourceComponent implements OnInit {

    @Input() searchResults: any;
    @Input() webResult: any;

    constructor() { }

    ngOnInit() {
        this.bindingData();
    }

    bindingData() {

        if (!isNullOrUndefined(this.searchResults) && !isNullOrUndefined(this.searchResults.webResources)) {
            this.searchResults = this.searchResults.webResources.webPages;
        }

    }

}
