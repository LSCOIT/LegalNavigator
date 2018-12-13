import { Component, OnInit } from '@angular/core';
import { SearchService } from '../shared/search/search.service';
import { NavigateDataService } from '../shared/navigate-data.service';
import { ILuisInput } from '../shared/search/search-results/search-results.model';
import { Router } from '@angular/router';
import { LocationDetails } from '../shared/map/map';
import { GuidedAssistant } from './guided-assistant';
import { environment } from '../../environments/environment';
import { StaticResourceService } from '../shared/static-resource.service';
import { Global } from '../global';

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


  name: string = 'GuidedAssistantPrivacyPage';
  GuidedAssistantPageContent: GuidedAssistant;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;
  description: string = '';


  constructor(
    private searchService: SearchService,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private global: Global) { }

  onSubmit(guidedAssistantForm) {
    this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.luisInput["Sentence"] = guidedAssistantForm.value.searchText;
    this.luisInput["Location"] = this.locationDetails.location;
    this.searchService.search(this.luisInput)
      .subscribe(response => {
        this.guidedAssistantResults = response;
        this.navigateDataService.setData(this.guidedAssistantResults);
        if (this.guidedAssistantResults.guidedAssistantId && this.guidedAssistantResults.topicIds.length > 0)
        sessionStorage.setItem("searchTextResults", JSON.stringify(response));
        this.router.navigateByUrl('/guidedassistantSearch', { skipLocationChange: true });
      });
  }

  getGuidedAssistantContent(): void {
    if (this.staticResourceService.GuidedAssistantPageContent && (this.staticResourceService.GuidedAssistantPageContent.location[0].state == this.staticResourceService.getLocation())) {
      this.GuidedAssistantPageContent = this.staticResourceService.GuidedAssistantPageContent;
      this.description = this.GuidedAssistantPageContent.description;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.GuidedAssistantPageContent = this.staticContent.find(x => x.name === this.name);
        this.staticResourceService.GuidedAssistantPageContent = this.GuidedAssistantPageContent;
        this.description = this.GuidedAssistantPageContent.description;
      }
    }
  }
  ngOnInit() {
    if (JSON.parse(sessionStorage.getItem("searchTextResults")) != null) {
      this.navigateDataService.setData(JSON.parse(sessionStorage.getItem("searchTextResults")));
      this.router.navigateByUrl('/guidedassistantSearch', { skipLocationChange: true });
    }
    else {
      this.router.navigateByUrl('/guidedassistant');
    }

    this.getGuidedAssistantContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe(() => {
        this.getGuidedAssistantContent();
      });
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
