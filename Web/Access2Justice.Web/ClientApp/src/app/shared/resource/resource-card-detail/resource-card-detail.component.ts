import { Component, OnInit } from '@angular/core';
import { RouteDataService } from '../../../shared/route-data.service';


@Component({
    selector: 'app-resource-card-detail',
    templateUrl: './resource-card-detail.component.html',
    styleUrls: ['./resource-card-detail.component.css']
})
export class ResourceCardDetailComponent implements OnInit {
    topic: any;
    subtopic: any;
    data: any;
    constructor(
        private routeDataService: RouteDataService
    ) { }

    ngOnInit() {
        this.data = this.routeDataService.getData();
      if (this.data != undefined && this.data.subtopic != undefined) {
            this.subtopic = this.data.subtopic[0];
            this.topic = this.data.topicName;
        }
    }


}
