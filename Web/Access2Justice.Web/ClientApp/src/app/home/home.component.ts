import { Component, OnDestroy, OnInit } from '@angular/core';

import ENV from 'environment';
import { Global } from "../global";
import { LocationDetails } from "../shared/map/map";
import { MapService } from "../shared/map/map.service";
import { StaticResourceService } from "../shared/services/static-resource.service";
import { Carousel, GuidedAssistantOverview, Hero, Home, Privacy, SponsorOverview, Sponsors, TopicAndResources } from "./home";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit, OnDestroy {
  topicLength = 12;
  locationDetails: LocationDetails;
  state: string;
  subscription: any;
  slides = [];
  sponsors: Array<Sponsors>;
  button: { buttonText: ""; buttonAltText: ""; buttonLink: "" };
  blobUrl: string = ENV.blobUrl;
  name = "HomePage";
  homeContent: Home;
  heroData: Hero;
  guidedAssistantOverviewData: GuidedAssistantOverview;
  topicAndResourcesData: TopicAndResources;
  carouselData: Carousel;
  sponsorOverviewData: SponsorOverview;
  privacyData: Privacy;
  staticContent: any;
  staticContentSubcription: any;

  constructor(
    private staticResourceService: StaticResourceService,
    private mapService: MapService,
    private global: Global
  ) {}

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
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("globalMapLocation")
      );
      this.state = this.locationDetails.displayLocationDetails.address;
    }
  }

  createSlideContent(slides) {
    this.slides = slides.filter(
      slide => typeof slide.quote !== "undefined" && slide.quote.length > 0
    );
  }

  getHomePageContent(): void {
    if (
      this.staticResourceService.homeContent &&
      this.staticResourceService.homeContent.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.homeContent = this.staticResourceService.homeContent;
      this.filterHomeContent(this.staticResourceService.homeContent);
      this.createSlideContent(this.homeContent.carousel.slides);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.homeContent = this.staticContent.find(x => x.name === this.name);
        this.filterHomeContent(this.homeContent);
        this.createSlideContent(this.homeContent.carousel.slides);
        this.staticResourceService.homeContent = this.homeContent;
      }
    }
  }

  ngOnInit() {
    this.loadStateName();
    this.getHomePageContent();
    this.subscription = this.mapService.notifyLocation.subscribe(value => {
      this.loadStateName();
      this.getHomePageContent();
    });
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.loadStateName();
        this.getHomePageContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
