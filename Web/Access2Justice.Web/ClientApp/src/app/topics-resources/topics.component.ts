import { Component, OnInit } from '@angular/core';
import { TopicService } from './topic.service';
import { Topic } from './topic';
import { TOPICS } from './mock-topics';

@Component({
  selector: 'app-topics',
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.css']
})
export class TopicsComponent implements OnInit {

  topics: Topic[];

  getTopics(): void {
    this.topicService.getTopics()
        .subscribe(topics => this.topics = topics);
  }

  constructor(private topicService: TopicService) { }

  ngOnInit() {
    this.getTopics();
  }

}
