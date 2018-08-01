import { Component, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';
import { HomeService } from './home.service';
import { Hero, GuidedAssistant, TopicAndResources, Carousel, Information, Privacy } from './home';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  topicLength = 12;
  slides = [
    { image: '' },
    { image: '' },
    { image: '' }
  ];
  //homePageId: string;
  homeContent: any;
  heroData: Array<Hero> = [];
  guidedAssistantOverviewData: Array<GuidedAssistant> = [];
  topicAndResourcesData: Array<TopicAndResources> = [];
  carouselData: Array<Carousel> = [];
  sponsorOverviewData: Array<Information> = [];
  privacyData: Array<Privacy> = [];

  constructor(
    private homeService: HomeService
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
    this.homeService.getHomeContent()
      .subscribe(content => {    
        this.homeContent = content;
        console.log(this.homeContent.hero.description.textWithLink.url);
        this.filterHomeContent();
      });
  }
  ngOnInit() {
    //this.homePageId = environment.homePageId;
    this.getHomePageContent();
  }
}
