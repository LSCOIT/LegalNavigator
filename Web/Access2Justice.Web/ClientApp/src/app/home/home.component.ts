import { Component, OnInit } from '@angular/core';
import { MapLocation } from '../shared/location/location';
import { LocationService } from '../shared/location/location.service';
import { environment } from '../../environments/environment';

import { Hero, GuidedAssistantOverview, TopicAndResources, Carousel, SponsorOverview, Privacy } from './home';
import { StaticResourceService } from '../shared/static-resource.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  topicLength = 12;
  mapLocation: MapLocation;
  state: string;
  subscription: any;
  slides = [
    { image: '' },
    { image: '' },
    { image: '' }
  ];
  contentUrl: any = environment.blobUrl;
  pageId = 'HomePage';
  homeContent: any;
  heroData: Hero
  guidedAssistantOverviewData: GuidedAssistantOverview
  topicAndResourcesData: TopicAndResources;
  carouselData: Carousel;
  sponsorOverviewData: SponsorOverview;
  privacyData: Privacy;

  
  loadStateName() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.state = this.mapLocation.locality;
    }
  }
  constructor(
      private staticResourceService: StaticResourceService, private locationService: LocationService
  ) { }

  filterHomeContent(): void {
    if (this.homeContent) {
      this.heroData = this.homeContent.hero;
      this.guidedAssistantOverviewData = this.homeContent.guidedAssistantOverview;
      this.topicAndResourcesData = this.homeContent.topicAndResources;
      this.carouselData = this.homeContent.carousel;
      this.sponsorOverviewData = this.homeContent.sponsorOverview;
      this.privacyData = this.homeContent.privacy;
    }
  }

  getHomePageContent(): void {
    this.staticResourceService.getStaticContents(this.pageId)
      .subscribe(content => {
        this.homeContent = content[0];
        this.filterHomeContent();
      });
  }
  ngOnInit() {
    this.loadStateName();
    this.subscription = this.locationService.notifyLocation
      .subscribe((value) => {
        this.loadStateName();
      });
  }
    this.getHomePageContent();
  }
}

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
