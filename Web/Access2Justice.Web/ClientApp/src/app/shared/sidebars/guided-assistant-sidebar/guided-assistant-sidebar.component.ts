import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { PaginationService } from '../../pagination/pagination.service';
import { MapService } from '../../map/map.service';
import { NavigateDataService } from '../../navigate-data.service';
import { MapLocation } from '../../map/map';

@Component({
  selector: 'app-guided-assistant-sidebar',
  templateUrl: './guided-assistant-sidebar.component.html',
  styleUrls: ['./guided-assistant-sidebar.component.css']
})
export class GuidedAssistantSidebarComponent implements OnInit {
  location: MapLocation;
  activeTopic: any;
  @Input() guidedAssistantId: string;
  @Input() showSidebar: boolean;
  resourceFilter: IResourceFilter = {
    ResourceType: '', ContinuationToken: '', TopicIds: [],
    ResourceIds: [], PageNumber: 0, Location: {}, IsResourceCountRequired: false
  };
  topicIds: string[] = [];
  resources: any;
  subscription: any;
  emptyResult: string = "";

  constructor(
    private activeRoute: ActivatedRoute,
    private mapService: MapService,
    private paginationService: PaginationService
  ) { }

  getGuidedAssistantResults() {
    this.topicIds = [];
    this.activeTopic = this.activeRoute.snapshot.params['topic'];
    if (this.activeTopic) {
      this.topicIds.push(this.activeTopic);
      this.resourceFilter = {
        ResourceType: 'Guided Assistant', ContinuationToken: '', TopicIds: this.topicIds,
        ResourceIds: [], PageNumber: 0, Location: this.location,
        IsResourceCountRequired: false
      }
      this.paginationService.getPagedResources(this.resourceFilter).
        subscribe(response => {
          if (response != undefined) {
            this.resources = response["resources"];
            if (this.resources.length > 0) {
              this.guidedAssistantId = this.resources[0].externalUrl;
            }
          }
          else {
            this.guidedAssistantId = this.emptyResult;
          }
        });
    }
    this.guidedAssistantId = this.emptyResult;
  }

  ngOnInit() {
    if (!this.guidedAssistantId) {
      if (sessionStorage.getItem("globalMapLocation")) {
        this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
        this.getGuidedAssistantResults();
      }
      this.subscription = this.mapService.notifyLocation
        .subscribe((value) => {
          this.location = value;
          this.getGuidedAssistantResults();
        });
    }
  }
  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
