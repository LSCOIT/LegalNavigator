import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute,Router } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ShowMoreService } from "../../shared/sidebars/show-more/show-more.service";
import { ISubtopicGuidedInput } from "../shared/topic";
import { Input } from "@angular/core/src/metadata/directives";

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})

export class SubtopicsComponent implements OnInit {
  subtopics: any[];
  activeTopic: any;
  activeTopicId: string;
  subtopicName: string;
  topic: any;
  icon: any;
  guidedInput: ISubtopicGuidedInput = { activeId: '', name: '' };
  constructor(
    private topicService: TopicService,    
    private activeRoute: ActivatedRoute,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private showMoreService: ShowMoreService
  ) {}


  getSubtopics(): void {
    this.activeTopic = this.activeRoute.snapshot.params['topic'];
    this.topicService.getDocumentData(this.activeTopic)
      .subscribe(
      topic => {
        this.topic = topic[0];
        this.icon = topic[0].icon;
        this.guidedInput = { activeId: this.activeTopic, name: this.topic.name };
        });
    this.topicService.getSubtopics(this.activeTopic)
      .subscribe(
        subtopics => {
          this.subtopics = subtopics;
          this.navigateDataService.setData(this.subtopics);
          if (this.subtopics.length === 0) { 
            this.router.navigateByUrl('/subtopics/' + this.activeTopic, { skipLocationChange: true });
          }
      });    
  }

  clickSeeMoreOrganizationsFromSubtopic(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(resourceType, this.activeTopic);
  }
  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getSubtopics();
        }
      });
  }
}
