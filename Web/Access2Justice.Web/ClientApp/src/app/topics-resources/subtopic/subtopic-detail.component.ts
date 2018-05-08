import { Component, OnInit } from "@angular/core";
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
