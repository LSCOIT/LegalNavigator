import { Component, OnInit, Input } from '@angular/core';
import { SearchService } from '../../search.service';

@Component({
  selector: 'app-web-resource',
  templateUrl: './web-resource.component.html',
  styleUrls: ['./web-resource.component.css']
})
export class WebResourceComponent implements OnInit {

  @Input()
  searchResults: any;
  @Input()
  webResult: any;
  type: string = "WebResources";
  
  constructor(private searchService: SearchService) { }

  ngOnInit() {
    if (this.searchResults != undefined && this.searchResults.webResources != undefined) { 
      this.searchResults = this.searchResults.webResources.webPages;

    }
  }


}
