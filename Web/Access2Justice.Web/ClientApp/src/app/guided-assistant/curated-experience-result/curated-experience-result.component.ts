import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-curated-experience-result',
  templateUrl: './curated-experience-result.component.html',
  styleUrls: ['./curated-experience-result.component.css']
})
export class CuratedExperienceResultComponent implements OnInit {
  @Input() guidedAssistantResults;
  relevantIntents: Array<string>;

  constructor(private navigateDataService: NavigateDataService) { }

  filterIntent() {
    this.relevantIntents = this.guidedAssistantResults.relevantIntents
      .filter((resource) => resource.resourceType !== 'None');
  }

  ngOnInit() {
    this.guidedAssistantResults = this.navigateDataService.getData();
    this.filterIntent();
    console.log(this.guidedAssistantResults);
  }

}
