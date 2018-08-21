import { Component, OnInit, Input } from '@angular/core';
import { TopicService } from '../shared/topic.service';
import { Topic } from '../shared/topic';
import { MapService } from '../../shared/map/map.service';

@Component({
  selector: 'app-topics',
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.css']
})
export class TopicsComponent implements OnInit {
  topics: Topic;
  @Input() topicLength: number;
  @Input() fullPage: true;
  subscription: any;

  constructor(private topicService: TopicService,
    private mapService: MapService) { }

  getTopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => 
        this.topics = topics
    );
  }

  ngOnInit() {
    this.getTopics();
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.getTopics();
      });
  }

}
