import { Component, OnInit, Input } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute, Router  } from '@angular/router';
import { Topic } from '../shared/topic';

@Component({
  selector: 'app-subtopic-detail',
  templateUrl: './subtopic-detail.component.html',
  styleUrls: ['./subtopic-detail.component.css']
})

export class SubtopicDetailComponent implements OnInit {
  subtopicDetails: Topic;
  activeSubtopic = this.activeRoute.params["value"]["topic"];

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute, private router: Router) {
  }

  getSubtopicDetail(): void {
    this.topicService.getSubtopicDetail(this.activeRoute.snapshot.params['subtopic'])
      .subscribe(
        data => this.subtopicDetails = data[0]
      );
  }

  ngOnInit() {
    this.getSubtopicDetail();
  }
}
