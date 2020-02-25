import { Component, HostListener, Input, OnInit, QueryList, ViewChildren } from "@angular/core";
import { Global } from "../../global";
import { MapService } from "../../common/map/map.service";
import { Topic } from "../shared/topic";
import { TopicService } from "../shared/topic.service";


@Component({
  selector: "app-topics",
  templateUrl: "./topics.component.html",
  styleUrls: ["./topics.component.css"]
})
export class TopicsComponent implements OnInit {
  topics: Topic[];
  @Input() topicLength: number;
  @Input() fullPage: true;
  subscription: any;
  bottomRowTopics: number;
  @ViewChildren("topicList") topicList: QueryList<any>;
  newTopicList = [];
  windowResizeSubscription;
  windowWidth: number = window.innerWidth;

  @HostListener("window:resize", ["$event"])
  onResize(event) {
    this.windowWidth = event.target.innerWidth;
    this.addBorder();
    this.findBottomRowTopics();
  }

  constructor(
    private topicService: TopicService,
    private mapService: MapService,
    private global: Global
  ) {}

  getTopics(): void {
    this.topicService.getTopics().subscribe(topics => {
      if(topics){
        var rankingData = [];
        rankingData = topics.filter(x => x.ranking == 1);
        if(rankingData.length > 1){
          rankingData = rankingData.sort((a, b) => a.name === b.name ? 0 : (a.name < b.name ? -1 : 1));
        }
        var newTopicArray = [];
        newTopicArray = topics.filter(x => !rankingData.includes(x));
        if(newTopicArray.length > 0){
          newTopicArray = rankingData.concat(newTopicArray);
        } else{
          newTopicArray = rankingData;
        }
        this.topics = newTopicArray;
  
        this.global.topicsData = newTopicArray;
        setTimeout(() => this.findBottomRowTopics(), 0);
      }
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
    } else if (
      (this.windowWidth >= 768 && window.innerWidth < 992) ||
      (!this.fullPage && this.windowWidth >= 768)
    ) {
      this.bottomRowTopics = this.topicList.length % 3;
      if (this.bottomRowTopics === 0) {
        this.setBottomBorder(3);
      } else {
        this.setBottomBorder(this.bottomRowTopics);
      }
    } else if (this.windowWidth > 992 && this.fullPage) {
      this.bottomRowTopics = this.topicList.length % 4;
      if (this.bottomRowTopics === 0) {
        this.setBottomBorder(4);
      } else {
        this.setBottomBorder(this.bottomRowTopics);
      }
    }
  }

  addBorder() {
    this.topicList
      .toArray()
      .slice(-4)
      .forEach(topic => {
        topic.nativeElement.style.borderBottom = "1px solid #ddd";
      });
  }

  setBottomBorder(lastTopic) {
    this.newTopicList = this.topicList.toArray().slice(-lastTopic);
    this.newTopicList.forEach(topic => {
      topic.nativeElement.style.borderBottom = "none";
    });
  }

  ngOnInit() {
    if (!this.global.topicsData) {
      this.getTopics();
    } else {
      this.topics = this.global.topicsData;
      setTimeout(() => this.findBottomRowTopics(), 0);
    }
    this.subscription = this.mapService.notifyLocation.subscribe(() => {
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
