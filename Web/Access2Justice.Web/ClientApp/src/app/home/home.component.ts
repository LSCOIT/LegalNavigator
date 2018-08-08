import { Component, OnInit } from '@angular/core';
import { MapLocation } from '../shared/location/location';
import { LocationService } from '../shared/location/location.service';
import { environment } from '../../environments/environment';

import { Hero, GuidedAssistantOverview, TopicAndResources, Carousel, SponsorOverview, Privacy, Sponsors } from './home';
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
  blobUrl: any = environment.blobUrl;
  pageId: any = 'HomePage';
  homeContent: any;
  heroData: Hero
  guidedAssistantOverviewData: GuidedAssistantOverview
  topicAndResourcesData: TopicAndResources;
  carouselData: Carousel;
  sponsorOverviewData: SponsorOverview;
  privacyData: Privacy;

  constructor(private staticResourceService: StaticResourceService,
    private locationService: LocationService) { }

  filterHomeContent(): void {
    if (this.homeContent) {
      this.heroData = this.homeContent.hero;
      this.guidedAssistantOverviewData = this.homeContent.guidedAssistantOverview;
      this.topicAndResourcesData = this.homeContent.topicAndResources;
      this.sponsorOverviewData = this.homeContent.sponsorOverview;
      this.sponsors = this.homeContent.sponsorOverview.sponsors;
      this.button = this.homeContent.sponsorOverview.button;
      this.carouselData = this.homeContent.carousel;
      this.privacyData = this.homeContent.privacy;
    }
  }
  
  getHomePageContent(): void {    
    let homePageRequest = { name: this.pageId };
    this.staticResourceService.getStaticContent(homePageRequest)
      .subscribe(content => {
        this.homeContent = content[0];
        this.filterHomeContent();
      });
  }

  ngOnInit() {
    this.getHomePageContent();
    this.subscription = this.locationService.notifyLocation
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
