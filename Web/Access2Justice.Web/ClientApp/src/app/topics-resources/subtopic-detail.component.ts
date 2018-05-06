import { Component, OnInit } from "@angular/core";
import { TopicService } from '../topics-resources/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Topic } from '../topics-resources/topic';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {
  subtopicDetail: Topic;

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) {}

  getTopicDetail(): void {
    let name = new HttpParams();
    name = name.set('name', this.activeRoute.snapshot.params['title']);
    this.topicService.getTopicDetail(name)
      .subscribe(data => this.subtopicDetail = data);
  }
  ngOnInit() {
    this.getTopicDetail();
  }

}
