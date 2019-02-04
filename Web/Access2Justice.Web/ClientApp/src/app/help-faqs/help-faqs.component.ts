import { Component, OnDestroy, OnInit } from '@angular/core';

import ENV from 'env';
import { Global } from "../global";
import { Faq, HelpAndFaqs, ImageUrl } from './help-faqs';
import { StaticResourceService } from "../shared/services/static-resource.service";

@Component({
  selector: "app-help-faqs",
  templateUrl: "./help-faqs.component.html",
  styleUrls: ["./help-faqs.component.css"]
})
export class HelpFaqsComponent implements OnInit, OnDestroy {
  helpAndFaqsContent: HelpAndFaqs;
  faqData: Array<Faq> = [];
  imageData: ImageUrl;
  name = "HelpAndFAQPage";
  blobUrl: string = ENV.blobUrl;
  staticContent: any;
  staticContentSubcription: any;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global
  ) {}

  filterHelpAndFaqContent(helpAndFaqsContent): void {
    if (helpAndFaqsContent) {
      this.helpAndFaqsContent = helpAndFaqsContent;
      this.faqData = helpAndFaqsContent.faqs;
      this.imageData = helpAndFaqsContent.image;
    }
  }

  getHelpFaqPageContent(): void {
    if (
      this.staticResourceService.helpAndFaqsContent &&
      this.staticResourceService.helpAndFaqsContent.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.helpAndFaqsContent = this.staticResourceService.helpAndFaqsContent;
      this.filterHelpAndFaqContent(
        this.staticResourceService.helpAndFaqsContent
      );
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.helpAndFaqsContent = this.staticContent.find(
          x => x.name === this.name
        );
        this.filterHelpAndFaqContent(this.helpAndFaqsContent);
        this.staticResourceService.helpAndFaqsContent = this.helpAndFaqsContent;
      }
    }
  }

  ngOnInit() {
    this.getHelpFaqPageContent();
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.getHelpFaqPageContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
