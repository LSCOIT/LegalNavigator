import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-curated-experience-result',
  templateUrl: './curated-experience-result.component.html',
  styleUrls: ['./curated-experience-result.component.css']
})
export class CuratedExperienceResultComponent implements OnInit {
  @Input() guidedAssistantResults;
  relevantIntents: Array<string>;

  constructor(
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService
  ) { }

  saveForLater() {
    this.toastr.success("Topics added. You can view them once you've completed the guided assistant.");
  }

  filterIntent() {
    if (this.guidedAssistantResults.relevantIntents) {
      this.relevantIntents = this.guidedAssistantResults.relevantIntents
        .filter(resource => resource !== 'None');
    }
  }

  ngOnInit() {
    this.guidedAssistantResults = this.navigateDataService.getData();
    this.filterIntent();
  }

}
