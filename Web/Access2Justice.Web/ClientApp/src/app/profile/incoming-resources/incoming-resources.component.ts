import { Component, OnInit, Optional } from '@angular/core';
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';


@Component({
  selector: 'app-incoming-resources',
  templateUrl: './incoming-resources.component.html',
  styleUrls: ['./incoming-resources.component.scss']
})
export class IncomingResourcesComponent implements OnInit {

  private incomingResources = [];

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
  ) { }

  private getIncomingResources(): void {
    this.personalizedPlanService.getPersonalizedResources('incoming-resources').subscribe(incomingResources => {
      for (let key in incomingResources) {
        incomingResources[key].forEach(i => {this.incomingResources.push(i)});
      }
    });
  }

  ngOnInit() {
    if (this.tab) {
      this.tab.select.subscribe(() => this.getIncomingResources());
    } else {
      this.getIncomingResources();
    }
  }

}
