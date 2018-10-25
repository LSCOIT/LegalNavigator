import { Component, OnInit, Input, QueryList, ViewChildren, HostListener } from '@angular/core';
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
  bottomRowTopics: number;
  @ViewChildren('topicList') topicList: QueryList<any>;
  newTopicList = [];
  windowResizeSubscription;
  windowWidth: number = window.innerWidth;

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.windowWidth = event.target.innerWidth;
    this.addBorder();
    this.findBottomRowTopics();
  }

  constructor(
    private topicService: TopicService,
    private mapService: MapService,
    private global: Global) {}

  getTopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => {
        this.topics = topics;
        this.global.topicsData = topics;
        setTimeout(() => this.findBottomRowTopics(), 0);
      });
  }

  findBottomRowTopics() {
    if (this.windowWidth < 768) {
      this.bottomRowTopics = this.topicList.length % 2;
      if (this.bottomRowTopics === 0) {
        this.setBottomBorder(2);
      } else {
        this.setBottomBorder(this.bottomRowTopics);
      }
    } else if (this.windowWidth >= 768 && window.innerWidth < 992) {
      this.bottomRowTopics = this.topicList.length % 3;
      if (this.bottomRowTopics === 0) {
        this.setBottomBorder(3);
      } else {
        this.setBottomBorder(this.bottomRowTopics);
      }
    } else {
      this.bottomRowTopics = this.topicList.length % 4;
      if (this.bottomRowTopics === 0) {
        this.setBottomBorder( 4);
      } else {
        this.setBottomBorder(this.bottomRowTopics);
      }
    }
  }

  addBorder() {
    this.topicList.toArray().slice(-4).forEach(topic => {
      topic.nativeElement.style.borderBottom = "1px solid #ddd";
    });
  }

  setBottomBorder(lastTopic) {
    this.newTopicList = this.topicList.toArray().slice(-lastTopic);
    this.newTopicList.forEach(topic => {
      topic.nativeElement.style.borderBottom = 'none';
    });
  }


  ngOnInit() {
    if (!this.global.topicsData) {
      this.getTopics();
    } else {
      this.topics = this.global.topicsData;
      setTimeout(() => this.findBottomRowTopics(), 0);
    }

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
