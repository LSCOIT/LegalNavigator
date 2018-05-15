import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Topic } from '../shared/topic';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})
export class SubtopicsComponent implements OnInit {
  //subtopics: Topic;
  //activeSubtopic = this.activeRoute.params["value"]["topic"];

  subtopics: Topic;
  activeTopic = this.activeRoute.snapshot.params['topic'];
  activeTopicId: string;

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) {
  }

  getTopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => {
        for (let i = 0; i < topics["result"].length; i++) {
          if (topics["result"][i]["title"].toLowerCase() === this.activeTopic) {
            this.activeTopicId = topics["result"][i]["id"];
            this.topicService.getSubtopics(this.activeTopicId).subscribe(subtopics => this.subtopics = subtopics["result"]);
          }
        }
      });
  };
  
  ngOnInit() {
    this.getTopics();
  }
}



