import { Component, OnInit, Input } from '@angular/core';
import { TopicService } from '../shared/topic.service';
import { Topic } from '../shared/topic';

@Component({
  selector: 'app-topics',
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.css']
})
export class TopicsComponent implements OnInit
{
  @Input() topicLength: number;
  topics: Topic;

  getTopics(): void {
    this.topicService.getTopics()
        .subscribe(topics => this.topics = topics["result"]);
  }

  constructor(private topicService: TopicService) { }

  ngOnInit() {
    this.getTopics();
  }

}
