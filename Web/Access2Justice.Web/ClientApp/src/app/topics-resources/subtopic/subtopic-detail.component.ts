import { Component, OnInit} from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router'; 
import { NavigateDataService } from '../../shared/navigate-data.service';
import { IResourceFilter, ILuisInput } from "../../shared/search/search-results/search-results.model";
import { SearchService } from '../../shared/search/search.service';
import { PaginationService } from "../../shared/pagination/pagination.service";
import { ShowMoreService } from "../../shared/sidebars/show-more/show-more.service";
import { ISubtopicGuidedInput } from "../shared/topic";
import { MapService } from "../../shared/map/map.service";
import { Global } from "../../global";

@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {
  searchResults: any; 
  subtopicDetails: any;
  activeSubtopicParam = this.activeRoute.snapshot.params['topic'];
  actionPlanData: any;
  articleData: any;
  videoData: any;
  organizationData: any;
  formData: any;
  relatedLinksData: any;
  subtopics: any;
  subtopic: any;
  topIntent: string; 
  savedFrom: string = "subTopicDetails";
  type: string = "Topics";
  showRemoveOption: boolean;
  guidedSutopicDetailsInput: ISubtopicGuidedInput = { activeId: '', name: '' };
  luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '', LuisTopScoringIntent: '' };
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: { "state": "", "county": "", "city": "", "zipCode": "" }, IsResourceCountRequired: true };
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
  ) { }

  filterSubtopicDetail(): void {
    if (this.subtopicDetails) {
      this.actionPlanData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Action Plans');
      this.articleData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Articles');
      this.videoData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Videos');
      this.organizationData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Organizations');
      this.formData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Forms');
      this.relatedLinksData = this.subtopicDetails
        .filter((resource) => resource.resourceType === 'Additional Readings');
    }
  }

  getDataOnReload() {
    this.activeSubtopicParam = this.activeRoute.snapshot.params['topic'];
    this.subtopics = this.navigateDataService.getData();
    if (this.subtopics) {
      this.topicService.getDocumentData(this.activeSubtopicParam)
        .subscribe(
        data => {
          this.subtopics = data[0];
          this.topIntent = data[0].name;
          this.guidedSutopicDetailsInput = { activeId: this.activeSubtopicParam, name: this.subtopics.name };
        }); 
    }
    this.getSubtopicDetail();
  }

  getSubtopicDetail(): void {
    this.topicService.getSubtopicDetail(this.activeSubtopicParam)
      .subscribe(
        data => {
          this.subtopicDetails = data;
          this.filterSubtopicDetail();
        }
      );
  }
  clickSeeMoreOrganizationsFromSubtopicDetails(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(resourceType, this.activeSubtopicParam, this.topIntent);
  }

  ngOnInit() {
    this.displayResources = this.global.displayResources;
    this.global.activeSubtopicParam = this.activeSubtopicParam;
    this.global.topIntent = this.topIntent;
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getDataOnReload();
        }
      });
    this.showRemoveOption = false;
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.topicService.getTopics().subscribe(response => {
        if (response != undefined) {
        this.global.topicsData = response;
        if (this.router.url.startsWith('/topics') || this.router.url.startsWith('/subtopics')) {
          this.router.navigateByUrl('/topics');
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
