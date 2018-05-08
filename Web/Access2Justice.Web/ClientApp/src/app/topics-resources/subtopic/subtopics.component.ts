import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute } from '@angular/router';
import { Topic } from '../shared/topic';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})
export class SubtopicsComponent implements OnInit {
  subtopicDetails: Topic;
  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) { }
  getTopicDetails(): void {
    let id = new HttpParams();
    id = id.set('id', this.activeRoute.snapshot.params['id']);
    this.topicService.getTopicDetails(id)
      .subscribe(data => this.subtopicDetails = data);
  }
  ngOnInit() {

    this.getTopicDetails();
  }

}
