import { Component, OnInit} from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';

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
  savedFrom: string = "subTopicDetails";
  type: string = "Topics";
  showRemoveOption: boolean;

  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService
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
