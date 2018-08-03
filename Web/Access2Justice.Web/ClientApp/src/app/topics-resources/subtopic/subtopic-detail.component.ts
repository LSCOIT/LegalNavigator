import { Component, OnInit} from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router'; 
import { NavigateDataService } from '../../shared/navigate-data.service';
import { IResourceFilter, ILuisInput } from "../../shared/search/search-results/search-results.model";
import { SearchService } from '../../shared/search/search.service';
import { PaginationService } from "../../shared/search/pagination.service";

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
  subtopics: any;
  subtopic: any;
  topIntent: string; 
  savedFrom: string = "subTopicDetails";
  type: string = "Topics";
  showRemoveOption: boolean;
  luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '', LuisTopScoringIntent: '' };
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: { "state": "", "county": "", "city": "", "zipCode": "" }, IsResourceCountRequired: true };

  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private searchService: SearchService,
    private router: Router,
    private paginationService: PaginationService 
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
    }
  }

  getDataOnReload() {
    this.activeSubtopicParam = this.activeRoute.snapshot.params['topic'];
    // Getting the sub topic name while routing from subtopic page.
    this.subtopics = this.navigateDataService.getData();
    if (this.subtopics) {
      this.topicService.getDocumentData(this.activeSubtopicParam)
        .subscribe(
        data => {
          this.subtopics = data[0];
          this.topIntent = data[0].name;
        }); 
    }
    this.getSubtopicDetail();
  }

  getSubtopicDetail(): void {
    this.topicService.getSubtopicDetail(this.activeSubtopicParam)
      .subscribe(
        data => {
          this.subtopicDetails = data;
          console.log(this.subtopicDetails);
          this.filterSubtopicDetail();
        }
      );
  }

  clickShowMore(resourceType: string) {
    this.activeSubtopicParam = this.activeRoute.snapshot.params['topic'];
    this.resourceFilter.ResourceType = resourceType;
    this.resourceFilter.TopicIds.push(this.activeSubtopicParam);
    this.resourceFilter.Location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.resourceFilter.IsResourceCountRequired = true;
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.searchResults = response;
        this.searchResults.topIntent = this.topIntent;
        this.searchResults.resourceType = resourceType;
        this.searchResults.isItFromTopicPage = true;
        this.navigateDataService.setData(this.searchResults);
        this.router.navigate(['/search']);
      }
    });
  }

  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getDataOnReload();
        }
      });
    this.showRemoveOption = false;
  }
}
