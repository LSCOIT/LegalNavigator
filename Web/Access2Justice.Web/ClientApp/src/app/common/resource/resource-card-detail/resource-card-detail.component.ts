import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ResourceService } from "../resource.service";

@Component({
  selector: "app-resource-card-detail",
  templateUrl: "./resource-card-detail.component.html",
  styleUrls: ["./resource-card-detail.component.css"]
})
export class ResourceCardDetailComponent implements OnInit {
  resource: any;
  resourceId: string;
  activeSubtopicParam: any;
  topicId: string;

  constructor(
    private resourceService: ResourceService,
    private activeRoute: ActivatedRoute
  ) {}

  getResource() {
    this.resourceId = this.activeRoute.snapshot.params["id"];
    this.activeSubtopicParam = this.activeRoute.snapshot.params["topicid"];
    this.resourceService.getResource(this.resourceId).subscribe(resource => {
      this.resource = resource[0];
    });
  }
  getTopicId(){
    if (sessionStorage.getItem('cacheSearchResults')) {
      const sessionData = JSON.parse(
        sessionStorage.getItem('cacheSearchResults')
      );
      this.topicId = sessionData.topicIds[0];
    }
  }
  ngOnInit() {
    this.getTopicId();
    this.getResource();
  }
}
