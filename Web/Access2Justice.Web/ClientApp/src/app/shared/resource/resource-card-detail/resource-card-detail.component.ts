import { Component, OnInit } from '@angular/core';
import { RouteDataService } from '../../../shared/route-data.service';
import { isNullOrUndefined } from 'util';


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
        if (!isNullOrUndefined(this.data) && !isNullOrUndefined(this.data.subtopic)) {
            this.subtopic = this.data.subtopic[0];
            this.topic = this.data.topicName;
        }
    }


}
