import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-curated-experience-result',
  templateUrl: './curated-experience-result.component.html',
  styleUrls: ['./curated-experience-result.component.css']
})
export class CuratedExperienceResultComponent implements OnInit {
  @Input() guidedAssistantResults;
  constructor(private navigateDataService: NavigateDataService) { }

  ngOnInit() {
    this.guidedAssistantResults = this.navigateDataService.getData();
    console.log(this.guidedAssistantResults);
  }

}
