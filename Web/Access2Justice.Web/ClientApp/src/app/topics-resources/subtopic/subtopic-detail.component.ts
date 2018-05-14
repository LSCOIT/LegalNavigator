import { Component, OnInit, Input } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Topic } from '../shared/topic';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {

  subtopicDetail: Topic;
  activeTopic = this.activeRoute.snapshot.params['topic'];
  activeTopicId: string;
  activeSubtopic = this.activeRoute.snapshot.params['subtopic'];
  activeSubtopicId: string;

  getSubtopicDetail(): void {
    this.topicService.getTopics()
      .subscribe(topics => {
        for (let i = 0; i < topics["result"].length; i++) {
          if (topics["result"][i]["title"].toLowerCase() === this.activeTopic) {
            this.activeTopicId = topics["result"][i]["id"];
            this.topicService.getSubtopics(this.activeTopicId)
              .subscribe(subtopics => {
                for (let j = 0; j < subtopics["result"].length; j++) {
                  if (subtopics["result"][j]["title"].toLowerCase() === this.activeSubtopic) {
                    this.activeSubtopicId = subtopics["result"][j]["id"];
                    this.topicService.getSubtopicDetail(this.activeSubtopicId)
                      .subscribe(subtopics => this.subtopicDetail = subtopics);
                  }
                };
              });
          }
        }
      });
  }

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    this.getSubtopicDetail();
  }

}
