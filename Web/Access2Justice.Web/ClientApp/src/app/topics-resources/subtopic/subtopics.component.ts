import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute,Router } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-subtopics',
  templateUrl: './subtopics.component.html',
  styleUrls: ['./subtopics.component.css']
})

export class SubtopicsComponent implements OnInit {
  subtopics: any[];
  activeTopic: any;
  activeTopicId: string;
  subtopicName: string;

  constructor(
    private topicService: TopicService,    
    private activeRoute: ActivatedRoute,
    private router: Router,
    private navigateDataService: NavigateDataService
  ) {}

  getSubtopics(): void {
    this.activeTopic = this.activeRoute.snapshot.params['topic'];
    this.topicService.getSubtopics(this.activeTopic)
      .subscribe(
        subtopics => {
          this.subtopics = subtopics;
          this.navigateDataService.setData(this.subtopics);
          if (this.subtopics.length === 0) {
            this.router.navigateByUrl('/subtopics/' + this.activeTopic, { skipLocationChange: true });
          }
        });
  }

  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getSubtopics();
        }
      });
  }
}
