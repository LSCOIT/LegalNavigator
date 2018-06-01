import { Component, OnInit, Input } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Topic } from '../shared/topic';
import { NavigateDataService } from '../../shared/navigate-data.service';
@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {
  subtopicDetails: any;
  activeSubtopic = this.activeRoute.params["value"]["topic"];
  activeSubtopicParam = this.activeRoute.snapshot.params['subtopic'];
  actionPlanData: any;
  articleData: any;
  videoData: any;
  organizationData: any;
  formData: any;
  subtopics: any;

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute, private router: Router, private navigateDataService: NavigateDataService) {
  }

  filterSubtopicDetail(): void {
    if (this.subtopicDetails) {
      this.actionPlanData = this.subtopicDetails.filter((resource) => resource.resourceType === 'Action Plans');
      this.articleData = this.subtopicDetails.filter((resource) => resource.resourceType === 'Articles');
      this.videoData = this.subtopicDetails.filter((resource) => resource.resourceType === 'Videos');
      this.organizationData = this.subtopicDetails.filter((resource) => resource.resourceType === 'Organizations');
      this.formData = this.subtopicDetails.filter((resource) => resource.resourceType === 'Forms');
    }
  }

  getSubtopicDetail(): void {
   // this.topicService.getSubtopicDetail(this.activeRoute.snapshot.params['subtopic'])
    this.topicService.getSubtopicDetail(this.activeSubtopicParam)
      .subscribe(
      data => {
        this.subtopicDetails = data;
        this.filterSubtopicDetail();
      }
      );

  }


  ngOnInit() {

    this.subtopics = this.navigateDataService.getSubtopics();
    if (this.subtopics == null || this.subtopics == 'undefined') {
      this.topicService.getDocumentData(this.activeSubtopicParam)
        .subscribe(
        data => {
          this.subtopics = data;
        }
        );
    }
    this.getSubtopicDetail();
  }
}
