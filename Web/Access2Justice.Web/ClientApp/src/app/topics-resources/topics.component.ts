import { Component, OnInit, Input } from '@angular/core';
import { SlicePipe } from '@angular/common';
import { TopicService } from './topic.service';
import { Topic } from './topic';
import { TOPICS } from './mock-topics';

@Component({
  selector: 'app-topics',
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.css']
})
export class TopicsComponent implements OnInit {
  @Input() topicLength: number;
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
