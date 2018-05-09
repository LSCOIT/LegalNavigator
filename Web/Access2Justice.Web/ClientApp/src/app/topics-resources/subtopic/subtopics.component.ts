import { Component, OnInit, Input } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Topic } from '../shared/topic';

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})
export class SubtopicsComponent implements OnInit {
  topics: Topic;
  activeSubtopic = this.activeRoute.params["value"]["topic"];

  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute, private router: Router) {
  }

  getTopics(): void {
    this.topicService.getTopics()
      .subscribe(topics => this.topics = topics["result"]);
  }

  ngOnInit() {
    this.getTopics();
  }

}

