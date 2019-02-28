import { Component, OnInit } from "@angular/core";
import { ShowMoreService } from "../common/sidebars/show-more/show-more.service";

@Component({
  selector: "app-topics-resources",
  templateUrl: "./topics-resources.component.html",
  styleUrls: ["./topics-resources.component.css"]
})
export class TopicsResourcesComponent implements OnInit {
  constructor(private showMoreService: ShowMoreService) {}

  clickSeeMoreOrganizationsFromTopic(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(
      resourceType,
      undefined,
      undefined
    );
  }

  ngOnInit() {}
}
