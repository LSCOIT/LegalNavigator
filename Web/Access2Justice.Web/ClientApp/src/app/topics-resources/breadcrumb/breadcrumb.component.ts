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
  activeTopicName: any;

  constructor(private breadCrumbService: BreadCrumbService,
    private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activeRoute.url
      .subscribe(routeParts => {
        for (let i = 1; i < routeParts.length; i++) {
          this.getBreadcrumbDetails();
        }
      });
  }

  getBreadcrumbDetails() {
    if (this.activeRoute.snapshot.params['topic'] != null) {
      this.activeTopic = this.activeRoute.snapshot.params['topic'];
    };
    if (this.activeRoute.snapshot.params['id'] != null) {
      this.isResource = true;
    };

    //Get the data for topic from breadcrumb service
    this.breadCrumbService.getBreadCrumbs(this.activeTopic)
      .subscribe(
        items => {
          this.breadcrumbs = items.response.reverse();

          this.breadcrumbs.forEach(item => {
            if (item.id == this.activeTopic) {
              this.activeTopicName = item.name;
            }
          });
        });

  }
}
