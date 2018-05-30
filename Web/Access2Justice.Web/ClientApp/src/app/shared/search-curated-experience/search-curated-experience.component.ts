import { Component, OnInit } from '@angular/core';
import { CuratedExp } from './mock-curated-experience';
import { CuratedExperience } from './curatedexperience'
import { CuratedExperienceService } from './curatedexperience.service';

@Component({
  selector: 'app-search-curated-experience',
  templateUrl: './search-curated-experience.component.html',
  styleUrls: ['./search-curated-experience.component.css']
})
export class SearchCuratedExperienceComponent implements OnInit {
  curatedExperience: CuratedExperience[];

  constructor(private curatedExpService: CuratedExperienceService) { }

  getSearchCuratedExperience(): void {
    this.curatedExpService.getSearchCuratedExperience()
      .subscribe(curatedExperience => this.curatedExperience = curatedExperience);
  }

  ngOnInit() {
    this.getSearchCuratedExperience();
  }

}
