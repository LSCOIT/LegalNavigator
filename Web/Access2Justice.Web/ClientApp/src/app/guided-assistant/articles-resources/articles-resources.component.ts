import { Component, OnInit } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-articles-resources',
  templateUrl: './articles-resources.component.html',
  styleUrls: ['./articles-resources.component.css']
})
export class ArticlesResourcesComponent implements OnInit {
  guidedAssistantResults: any;
  constructor(private navigateDataService: NavigateDataService) { }

  ngOnInit() {
    //Todo - When user come from other pages need to pass the respective resource data & topic name.
    if (this.navigateDataService != undefined) {
      this.guidedAssistantResults = this.navigateDataService.getData();
    }
  }
}
