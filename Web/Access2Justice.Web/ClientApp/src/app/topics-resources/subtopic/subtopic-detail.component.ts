import { Component, OnInit} from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { PersonalizedPlanService } from "../../profile/personalized-plan/personalized-plan.service";
import { Resources } from "../../profile/personalized-plan/personalized-plan";

@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {
  subtopicDetails: any;
  activeSubtopicParam = this.activeRoute.snapshot.params['topic'];
  actionPlanData: any;
  articleData: any;
  videoData: any;
  organizationData: any;
  formData: any;
  subtopics: any;
  subtopic: any;
  resources: Resources;

  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private router: Router,
    private personalizedPlanService: PersonalizedPlanService
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
          data => this.subtopics = data
        );
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

  savePlanResources(): void {
    this.resources = { url: '', resourceType: '', itemId:'' };
    this.resources.url = this.router.url;
    this.resources.resourceType = "Sub Topics";
    this.resources.itemId = this.activeRoute.snapshot.params['topic'];
    this.personalizedPlanService.saveResourcesToSession(this.resources);
    console.log("Book marked!!!");
  }

  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getDataOnReload();
        }
      });
  }
}
