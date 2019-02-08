import { Component, ElementRef, HostListener, OnInit, ViewChild } from "@angular/core";

import {ENV} from 'environment';
import { Global } from "../../global";
import { StaticResourceService } from '../services/static-resource.service';
import { MapService } from "../map/map.service";
import { HelpAndFAQ, Language, Location, Login, Navigation, PrivacyPromise } from "./navigation";

@Component({
  selector: "app-upper-nav",
  templateUrl: "./upper-nav.component.html",
  styleUrls: ["./upper-nav.component.css"]
})
export class UpperNavComponent implements OnInit {
  blobUrl: any = ENV.blobUrl;
  navigation: Navigation;
  name: string = "Navigation";
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  subscription: any;
  staticContent: any;
  staticContentSubcription: any;
  @ViewChild("upperNav") upperNav: ElementRef;
  @HostListener("window:scroll", ["$event"])
  onScroll(e) {
    if (window.pageYOffset > 100) {
      this.upperNav.nativeElement.classList.add("box-shadow");
    } else {
      this.upperNav.nativeElement.classList.remove("box-shadow");
    }
  }

  constructor(
    private staticResourceService: StaticResourceService,
    private mapService: MapService,
    private global: Global
  ) {}

  filterUpperNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.privacyPromise = navigation.privacyPromise;
      this.helpAndFAQ = navigation.helpAndFAQ;
      this.login = navigation.login;
    }
  }

  getUpperNavigationContent(): void {
    if (
      this.staticResourceService.navigation &&
      this.staticResourceService.navigation.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.navigation = this.staticResourceService.navigation;
      this.filterUpperNavigationContent(this.navigation);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.navigation = this.staticContent.find(x => x.name === this.name);
        this.filterUpperNavigationContent(this.navigation);
        this.staticResourceService.navigation = this.navigation;
      }
    }
  }

  ngOnInit() {
    this.getUpperNavigationContent();
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.getUpperNavigationContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
