import { Component, OnInit, Input } from '@angular/core';
import { IResourceFilter } from '../search/search-results/search-results.model';
import { ActivatedRoute } from '@angular/router';
import { PaginationService } from '../search/pagination.service';

@Component({
  selector: 'app-guided-assistant-sidebar',
  templateUrl: './guided-assistant-sidebar.component.html',
  styleUrls: ['./guided-assistant-sidebar.component.css']
})
export class GuidedAssistantSidebarComponent implements OnInit {
  location: any;
  resourceFilter: IResourceFilter;
  activeTopic: any;
  guidedAssistantId: string;

  constructor(private activeRoute: ActivatedRoute, private paginationService: PaginationService) { }

  getGuidedAssistant() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    } else {
      this.location = "";
    }
    this.activeTopic = this.activeRoute.snapshot.params['topic'];
    this.resourceFilter = {
      ResourceType: 'Guided Assistant',
      TopicIds: [this.activeTopic],
      Location: this.location,
      PageNumber: 0,
      ContinuationToken: '',
      IsResourceCountRequired: true,
      ResourceIds: []
    }
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response["resources"][0] != undefined) {
        this.guidedAssistantId = response["resources"][0]["externalUrl"];
      }
    });
  }

  ngOnInit() {
    this.getGuidedAssistant();
  }

}
