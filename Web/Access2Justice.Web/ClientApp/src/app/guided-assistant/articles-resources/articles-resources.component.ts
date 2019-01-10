import { Component, OnInit } from "@angular/core";
import { NavigateDataService } from "../../shared/services/navigate-data.service";

@Component({
  selector: "app-articles-resources",
  templateUrl: "./articles-resources.component.html",
  styleUrls: ["./articles-resources.component.css"]
})
export class ArticlesResourcesComponent implements OnInit {
  guidedAssistantResults: any;
  constructor(private navigateDataService: NavigateDataService) {}

  ngOnInit() {
    if (this.navigateDataService != undefined) {
      this.guidedAssistantResults = this.navigateDataService.getData();
    }
  }
}
