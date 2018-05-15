import { Component, OnInit, Input } from "@angular/core";
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

  subTopicDetails: any;

  getTopicDetails(): void {
    console.log(this.activeRoute);
    //let subtopic = new HttpParams();
    //subtopic = subtopic.set('title', this.activeRoute.snapshot.params['subtopic']);
    //  this.topicService.getTopicDetails(this.activeRoute.snapshot.params['subtopic'])
    //    .subscribe(
    //    data => this.subtopicDetails = data);
  }
  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    this.getTopicDetails();
  }

}
