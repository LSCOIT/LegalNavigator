import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart } from '@angular/router';
import { BreadcrumbService } from '../shared/breadcrumb.service';
import { Global } from '../../global';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs = [];
  activeTopic: any;
  isResource: boolean = false;
  activeTopicName: string;
  activeTopicId: string;
  isSearch: boolean = false;
  isBookmarked: boolean = false;

  constructor(private breadcrumbService: BreadcrumbService,
    private activeRoute: ActivatedRoute,
    private global: Global,
    private navigateDataService: NavigateDataService,
    private router: Router) {
  }

  ngOnInit() {
    if (this.global.previosUrl.indexOf("/search") !== -1) {
      this.isSearch = true;
      this.getBreadcrumbDetails();
    }
    else {
      this.activeRoute.url
        .subscribe(routeParts => {
          for (let i = 1; i < routeParts.length; i++) {
            this.getBreadcrumbDetails();
          }
        });
    }
  }

  getBreadcrumbDetails() {
    this.activeTopic = this.activeRoute.snapshot.params['topic'] ? this.activeRoute.snapshot.params['topic'] : this.global.activeSubtopicParam;
    if (this.activeTopic != undefined) {
      sessionStorage.setItem("ActiveTopic", this.activeTopic);
    }
    else {
      this.activeTopic = sessionStorage.getItem("ActiveTopic");
    }
    if (this.activeTopic == null) { this.isBookmarked = true; }
    if (this.activeRoute.snapshot.params['id'] != null) {
      this.isResource = true;
    };

    if (!this.isSearch) {
      //Get the data for topic from breadcrumb service
      this.breadcrumbService.getBreadcrumbs(this.activeTopic)
        .subscribe(
          items => {
            this.breadcrumbs = items.response.reverse();
            this.breadcrumbs.unshift({ id: "", name: "All Topics" });
            if (this.isResource) {
              this.breadcrumbs.forEach(item => {
                if (item.id[0] === this.activeTopic) {
                  item.id = item.id[0];
                }
              });
              this.breadcrumbs.push({ id: this.activeRoute.snapshot.params['id'], name: "Resources" });
              this.activeTopic = this.activeRoute.snapshot.params['id'];
            }
            this.breadcrumbs.forEach(item => {
              if (item.id[0] === this.activeTopic) {
                this.activeTopicName = item.name;
                this.activeTopicId = item.id;
              }
            });
          });
    }
    else {
      this.breadcrumbs.unshift({ id: "", name: "Search" });
      if (this.isResource)
        this.breadcrumbs.push({ id: this.activeRoute.snapshot.params['id'], name: "Resources" });
    }
  }
}
