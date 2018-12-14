import { Component, OnInit } from '@angular/core';
import { SearchService } from '../shared/search/search.service';
import { NavigateDataService } from '../shared/navigate-data.service';
import { ILuisInput } from '../shared/search/search-results/search-results.model';
import { Router } from '@angular/router';
import { LocationDetails } from '../shared/map/map';

@Component({
  selector: 'app-guided-assistant',
  templateUrl: './guided-assistant.component.html',
  styleUrls: ['./guided-assistant.component.css']
})
export class GuidedAssistantComponent implements OnInit {
  searchText: string;
  topicLength = 12;
  luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '', LuisTopScoringIntent: '' };
  guidedAssistantResults: any;
  locationDetails: LocationDetails;
  previousSearchText: string;

  constructor(
    private searchService: SearchService,
    private router: Router,
    private navigateDataService: NavigateDataService) { }

  onSubmit(guidedAssistantForm) {
    this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    if (guidedAssistantForm.value.searchText) {
      this.luisInput["Sentence"] = guidedAssistantForm.value.searchText;
      this.luisInput["Location"] = this.locationDetails.location;
      this.searchService.search(this.luisInput)
        .subscribe(response => {
          this.guidedAssistantResults = response;
          this.navigateDataService.setData(this.guidedAssistantResults);
          if (this.guidedAssistantResults.guidedAssistantId && this.guidedAssistantResults.topicIds.length > 0) {
            sessionStorage.setItem("searchTextResults", JSON.stringify(response));
            sessionStorage.setItem("searchText", guidedAssistantForm.value.searchText);
            this.router.navigateByUrl('/guidedassistant/search');
          }
        });
    }
  }
  
  ngOnInit() {
    if (JSON.parse(sessionStorage.getItem("searchTextResults")) != null) {
      this.navigateDataService.setData(JSON.parse(sessionStorage.getItem("searchTextResults")));
      this.router.navigateByUrl('/guidedassistant/search');
    } else {
      if (sessionStorage.getItem("searchText")) {
        this.previousSearchText = sessionStorage.getItem("searchText");
      };
    }
  }  

}
