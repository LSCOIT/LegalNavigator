import { Component, Input, OnInit } from "@angular/core";
import { Global } from "../../../../global";
import { ShowMoreService } from "../../../sidebars/show-more/show-more.service";

@Component({
  selector: "app-articles",
  templateUrl: "./articles.component.html",
  styleUrls: ["./articles.component.css"]
})
export class ArticlesComponent implements OnInit {
  @Input() resource;
  activeResource: any;
  replacedContents: any;

  constructor(
    private showMoreService: ShowMoreService,
    private global: Global
  ) {}

  clickSeeMoreOrganizationsFromArticles(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(
      resourceType,
      this.global.activeSubtopicParam,
      this.global.topIntent
    );
  }

  displayResourceUrlData() {
    if (this.resource.contents) {
      for (
        let iterator = 0;
        iterator < this.resource.contents.length;
        iterator++
      ) {
        var contentData = this.resource.contents[iterator].content;
        var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/gi;
        this.replacedContents = contentData.replace(exp, "<a href='$1'>$1</a>");
        this.resource.contents[iterator].content = this.replacedContents;
      }
    }
  }

  ngOnInit() {
    this.displayResourceUrlData();
  }
}
