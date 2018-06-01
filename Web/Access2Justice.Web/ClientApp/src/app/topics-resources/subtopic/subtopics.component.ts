import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})

export class SubtopicsComponent implements OnInit {
  subtopics: any[];
  activeTopic = this.activeRoute.snapshot.params['topic'];
  activeTopicId: string;
  subtopicName: string;

  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService
  ) {}

  getSubtopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => {
        for (let i = 0; i < topics.length; i++) {
          if (topics[i]["name"].toLowerCase() === this.activeTopic) {
            this.activeTopicId = topics[i]["id"];
            this.topicService.getSubtopics(this.activeTopicId)
              .subscribe(
                subtopics => this.subtopics = subtopics
               );
            this.topicService.getSubtopics(this.activeTopicId)
              .subscribe(
                subtopics => {
                  this.subtopics = subtopics;
                  this.navigateDataService.setData(this.subtopics);
            });
          }
        }
      });
  };

  ngOnInit() {
    this.getSubtopics();
  }
}
