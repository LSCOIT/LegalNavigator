import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Global } from "../../global";
import { MapService } from "../../common/map/map.service";
import { ILuisInput, IResourceFilter } from "../../common/search/search-results/search-results.model";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { ShowMoreService } from "../../common/sidebars/show-more/show-more.service";
import { ISubtopicGuidedInput } from "../shared/topic";
import { TopicService } from "../shared/topic.service";
import { ParamsChange } from "../../common/search/search-filter/search-filter.component";

@Component({
  selector: "app-subtopic-detail",
  templateUrl: "./subtopic-detail.component.html",
  styleUrls: ["./subtopic-detail.component.css"]
})
export class SubtopicDetailComponent implements OnInit {
  searchResults: any;
  subtopicDetails: any;
  isInternalResource: boolean;
  activeSubtopicParam = this.activeRoute.snapshot.params["topic"];
  actionPlanData: any;
  articleData: any;
  videoData: any;
  organizationData: any;
  formData: any;
  relatedLinksData: any;
  subtopics: any;
  subtopic: any;
  isSortingDisabled: boolean;
  topIntent: string;
  savedFrom: string = "subTopicDetails";
  type: string = "Topics";
  showRemoveOption: boolean;
  guidedSutopicDetailsInput: ISubtopicGuidedInput = {
    activeId: "",
    name: ""
  };
  locationDetails: any;
  location: any;
  resourceResults: any;
  luisInput: ILuisInput = {
    Sentence: "",
    Location: "",
    TranslateFrom: "",
    TranslateTo: "",
    LuisTopScoringIntent: ""
  };
  resourceFilter: IResourceFilter = {
    ResourceType: "",
    ContinuationToken: "",
    TopicIds: [],
    ResourceIds: [],
    PageNumber: 0,
    Location: {
      state: "",
      county: "",
      city: "",
      zipCode: ""
    },
    IsResourceCountRequired: true,
    IsOrder: false,
    OrderByField: "",
    OrderBy: "",
    url: '',
    sharedTo: []
  };
  subscription: any;
  displayResources: number;

  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private showMoreService: ShowMoreService,
    private mapService: MapService,
    private global: Global,
    private router: Router
  ) {}

  filterSubtopicDetail(): void {
    if (this.subtopicDetails) {
      this.actionPlanData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Action Plans"
      );
      this.articleData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Articles"
      );
      this.videoData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Videos"
      );
      this.organizationData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Organizations"
      );
      this.formData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Forms"
      );
      this.relatedLinksData = this.subtopicDetails.filter(
        resource => resource.resourceType === "Additional Readings" || resource.resourceType === "Related Links"
      );
    }
  }

  getDataOnReload() {
    this.activeSubtopicParam = this.activeRoute.snapshot.params["topic"];
    this.subtopics = this.navigateDataService.getData();
    if (this.subtopics) {
      this.topicService
        .getDocumentData(this.activeSubtopicParam)
        .subscribe(data => {
          if(data && data.length > 0){
            this.subtopics = data[0];
            this.topIntent = data[0].name;
            this.guidedSutopicDetailsInput = {
              activeId: this.activeSubtopicParam,
              name: this.subtopics.name
            };
            this.getSubtopicDetail();
          }
        });   
    }

  }

  getSubtopicDetail(): void {
    this.topicService
      .getSubtopicDetail(this.activeSubtopicParam, this.subtopics.name)
      .subscribe(data => {
        var rankingData = [];
        rankingData = data.filter(x => x.ranking == 1);
        var newArray = [];
        if(rankingData.length > 1){
          rankingData = rankingData.sort((a, b) => a.name === b.name ? 0 : (a.name < b.name ? -1 : 1));
        }
        newArray = data.filter(x => !rankingData.includes(x));
        if(newArray.length > 0){
          newArray = rankingData.concat(newArray);
        } else{
          newArray = rankingData;
        }
        this.subtopicDetails = newArray;
        this.filterSubtopicDetail();
      });

      this.topicService
        .getDocumentData(this.activeSubtopicParam)
        .subscribe(data => {
          if(data.length > 0){
            this.subtopics = data[0];
          }
        });
  }

  clickSeeMoreOrganizationsFromSubtopicDetails(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(
      resourceType,
      this.activeSubtopicParam,
      this.topIntent
    );
  }

  loadStatisticFilter() {
    var obj = this;
    this.showMoreService.loadStatisticFilter('All', this.activeSubtopicParam, function(response){
      obj.searchResults = response;
      obj.resourceResults = response.resourceTypeFilter;
      obj.isInternalResource = true;
      obj.isSortingDisabled = true;
    });
  }

  enableFilter(event: ParamsChange){
    var obj = event;
    this.clickSeeMoreOrganizationsFromSubtopicDetails(obj.filterParam);
  }

  ngOnInit() {
    this.loadStatisticFilter();
    this.displayResources = this.global.displayResources;
    this.global.activeSubtopicParam = this.activeSubtopicParam;
    this.global.topIntent = this.topIntent;
    this.activeRoute.url.subscribe(routeParts => {
      for (let i = 1; i < routeParts.length; i++) {
        this.getDataOnReload();
      }
    });
    this.showRemoveOption = false;
    this.subscription = this.mapService.notifyLocation.subscribe(value => {
      this.topicService.getTopics().subscribe(response => {
        if (response != undefined) {
          this.global.topicsData = response;
          if (
            this.router.url.startsWith("/topics") ||
            this.router.url.startsWith("/subtopics")
          ) {
            this.router.navigateByUrl("/topics");
          }
        }
      });
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
