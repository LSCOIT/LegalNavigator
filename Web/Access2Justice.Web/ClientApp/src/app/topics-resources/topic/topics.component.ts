import { Component, OnInit, Input } from '@angular/core';
import { TopicService } from '../shared/topic.service';
import { Topic } from '../shared/topic';
import { MapService } from '../../shared/map/map.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { Global } from '../../global';

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

  constructor(
    private topicService: TopicService,
    private mapService: MapService,
    private global: Global) { }

  getTopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => {
        this.topics = topics;
        this.global.topicsData = topics;
      });
  }

  ngOnInit() {
    if (!this.global.topicsData) {
      this.getTopics();
    } else { this.topics = this.global.topicsData; }
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.global.topicsData = null;
        this.getTopics();
      });
  }
  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  } 

}
