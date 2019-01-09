import { Component, OnInit } from "@angular/core";
import { TopicService } from '../shared/topic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NavigateDataService } from '../../shared/services/navigate-data.service';
import { ShowMoreService } from "../../shared/sidebars/show-more/show-more.service";
import { ISubtopicGuidedInput } from "../shared/topic";
import { MapService } from "../../shared/map/map.service";
import { Global } from '../../global';

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
  topic: any;
  icon: any;
  topicIntent: string;
  guidedInput: ISubtopicGuidedInput = { activeId: '', name: '' };
  subscription: any;
  constructor(
    private topicService: TopicService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private showMoreService: ShowMoreService,
    private mapService: MapService,
    private global: Global
   ) { }


  getSubtopics(): void {
    this.activeTopic = this.activeRoute.snapshot.params['topic'];
    this.topicService.getDocumentData(this.activeTopic)
      .subscribe(
      topic => {
        this.topic = topic[0];
        this.icon = topic[0].icon == "" ? "../../../assets/images/categories/topic.svg" : topic[0].icon;
        this.guidedInput = { activeId: this.activeTopic, name: this.topic.name };
      });
    this.topicService.getSubtopics(this.activeTopic)
      .subscribe(
      subtopics => {
        this.sortAlphabetically(subtopics);
        this.subtopics = subtopics;
        this.navigateDataService.setData(this.subtopics);
        if (this.subtopics.length === 0) {
          this.router.navigateByUrl('/subtopics/' + this.activeTopic, { skipLocationChange: true });
        }
      });
  }

  clickSeeMoreOrganizationsFromSubtopic(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(resourceType, this.activeTopic, this.topicIntent);
  }

  sortAlphabetically(subtopics) {
    subtopics.sort((a, b) =>
        a.name.toLowerCase() !== b.name.toLowerCase() ?
          a.name.toLowerCase() < b.name.toLowerCase() ?
            -1 : 1 : 0
    );
  }

  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getSubtopics();
        }
      });
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.topicService.getTopics().subscribe(response => {
          if (response != undefined) {
            this.global.topicsData = response;
            if (this.router.url.startsWith('/topics') || this.router.url.startsWith('/subtopics')) {
              this.router.navigateByUrl('/topics');
            }
          }
        });
      });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  } 
}
