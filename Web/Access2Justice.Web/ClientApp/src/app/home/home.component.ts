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
  //heroData: Array<Hero> = [];
  //guidedAssistantData: Array<GuidedAssistant> = [];
  //topicAndResourcesData: Array<TopicAndResources> = [];
  //carouselData: Array<Carousel> = [];
  //informationData: Array<Information> = [];
  //privacyData: Array<Privacy> = [];

  constructor(
    private homeService: HomeService
  ) { }

  //filterHomeContent(): void {
  //  if (this.homeContent) {
  //    this.heroData = this.homeContent[0].hero;
  //    this.guidedAssistantData = this.homeContent[0].guidedAssistant;
  //    this.topicAndResourcesData = this.homeContent[0].topicAndResources;
  //    this.carouselData = this.homeContent[0].carousel;
  //    this.informationData = this.homeContent[0].information;
  //    this.privacyData = this.homeContent[0].privacy;
  //  }
  //}

  getHomePageContent(): void {
    this.homeService.getHomeContent()
      .subscribe(content => {    
        this.homeContent = content;
        console.log(this.homeContent.hero.subHeading);
        //this.filterHomeContent();
      });
  }
  ngOnInit() {
    //this.homePageId = environment.homePageId;
    this.getHomePageContent();
  }
}
