import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BreadCrumbService } from '../shared/breadcrumb.service';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs = [];
  activeTopic: any;
  isResource: boolean = false;
  mainTopic: any;
  
  constructor(private breadCrumbService: BreadCrumbService,
    private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    if (this.activeRoute.snapshot.params['subtopic'] != null) {
      this.activeTopic = this.activeRoute.snapshot.params['subtopic'];
    };
    if (this.activeRoute.snapshot.params['id'] != null) {
      this.isResource = true;
    };

    //Get the data for topic from breadcrumb service
    this.breadCrumbService.getBreadCrumbs(this.activeTopic)
      .subscribe(
      items => {
        this.breadcrumbs = items.response.reverse();
        for (var i = 0; i < this.breadcrumbs.length; i++) {
          this.mainTopic = this.breadcrumbs[i].name;
          break;
        }
      });
  }
}
