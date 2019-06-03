import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { BreadcrumbService } from "../shared/breadcrumb.service";

@Component({
  selector: "app-breadcrumb",
  templateUrl: "./breadcrumb.component.html",
  styleUrls: ["./breadcrumb.component.css"]
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs = [];
  activeTopic: any;
  isResource: boolean = false;
  activeTopicName: string;
  activeTopicId: string;
  @Input() resourceTopic: string;
  @Input() resourceType: string;

  constructor(
    private breadcrumbService: BreadcrumbService,
    private activeRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.activeRoute.url.subscribe(routeParts => {
      for (let i = 1; i < routeParts.length; i++) {
        this.getBreadcrumbDetails();
      }
    });
  }

  getBreadcrumbDetails() {
    var me = this;
    if (this.activeRoute.snapshot.params["topic"] != null) {
      this.activeTopic = this.activeRoute.snapshot.params["topic"];
    } else if (this.resourceTopic) {
      this.activeTopic = this.resourceTopic;
    }

    if (this.activeRoute.snapshot.params["id"] != null) {
      this.isResource = true;
    }
    this.breadcrumbService.getBreadcrumbs(this.activeTopic).subscribe(items => {
      this.breadcrumbs = items.response.map(item => {
        if (typeof item.id !== "string") {
          return {
            id: item.id[0],
            name: item.name
          };
        } else {
          return item;
        }
      });
      this.breadcrumbs.reverse();
      this.breadcrumbs.unshift({ id: "", name: "All Topics" });
      this.breadcrumbs.forEach(item => {
        if (item.id === this.activeTopic) {
          me.activeTopicName = item.name;
          me.activeTopicId = item.id;
        }
      });
      if (this.isResource) {
        if (this.resourceType.slice(-1) === "s") {
          this.resourceType = this.resourceType.slice(
            0,
            this.resourceType.length - 1
          );
        }
        this.breadcrumbs.push({
          id: this.resourceTopic,
          name: this.resourceType
        });
      }
    });
  }
}
