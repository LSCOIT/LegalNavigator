import { Component, OnInit } from '@angular/core';
import { MapLocation } from '../shared/map/map';
import { MapService } from '../shared/map/map.service';
import { environment } from '../../environments/environment';

import { Home, Hero, GuidedAssistantOverview, TopicAndResources, Carousel, SponsorOverview, Privacy, Sponsors } from './home';
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
  sponsors: Array<Sponsors>;
  button: { buttonText: '', buttonAltText: '', buttonLink: '' }
  blobUrl: string = environment.blobUrl;
  name: string = 'HomePage';
  homeContent: Home;
  heroData: Hero;
  guidedAssistantOverviewData: GuidedAssistantOverview
  topicAndResourcesData: TopicAndResources;
  carouselData: Carousel;
  sponsorOverviewData: SponsorOverview;
  privacyData: Privacy;

  constructor(private staticResourceService: StaticResourceService,
    private mapService: MapService) { }

  filterHomeContent(homeContent): void {
    if (homeContent) {
      this.heroData = homeContent.hero;
      this.guidedAssistantOverviewData = homeContent.guidedAssistantOverview;
      this.topicAndResourcesData = homeContent.topicAndResources;
      this.sponsorOverviewData = homeContent.sponsorOverview;
      this.sponsors = homeContent.sponsorOverview.sponsors;
      this.carouselData = homeContent.carousel;
      this.privacyData = homeContent.privacy;
    }
  }
  loadStateName() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.state = this.mapLocation.address;
    }
  }

  getHomePageContent(): void {
    let homePageRequest = { name: this.name };
    if (!this.staticResourceService.homeContent) {
      this.staticResourceService.getStaticContent(homePageRequest)
        .subscribe(content => {
          this.homeContent = content[0];
          this.filterHomeContent(this.homeContent);
          this.staticResourceService.homeContent = this.homeContent;
        });
    }
    else {
      this.homeContent = this.staticResourceService.homeContent;
      this.filterHomeContent(this.staticResourceService.homeContent);
    }
  }

  ngOnInit() {
    this.loadStateName();
    this.getHomePageContent();
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.loadStateName();
        this.getHomePageContent();
      });
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
