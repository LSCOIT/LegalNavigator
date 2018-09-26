import { Component, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';
import { Global } from '../global';
import { StaticResourceService } from '../shared/static-resource.service';
import { HelpAndFaqs, ImageUrl, Faq, } from '../help-faqs/help-faqs';

@Component({
  selector: 'app-help-faqs',
  templateUrl: './help-faqs.component.html',
  styleUrls: ['./help-faqs.component.css']
})
export class HelpFaqsComponent implements OnInit {

  helpAndFaqsContent: HelpAndFaqs;
  faqData: Array<Faq> = [];
  imageData: ImageUrl;
  name: string = 'HelpAndFAQPage';
  blobUrl: string = environment.blobUrl;
  staticContent: any;
  staticContentSubcription: any;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global
  ) { }

  filterHelpAndFaqContent(helpAndFaqsContent): void {
    if (helpAndFaqsContent) {
      this.helpAndFaqsContent = helpAndFaqsContent
      this.faqData = helpAndFaqsContent.faqs;
      this.imageData = helpAndFaqsContent.image;
    }
  }

  getHelpFaqPageContent(): void {
    if (this.staticResourceService.helpAndFaqsContent && (this.staticResourceService.helpAndFaqsContent.location[0].state == this.staticResourceService.getLocation())) {
      this.helpAndFaqsContent = this.staticResourceService.helpAndFaqsContent;
      this.filterHelpAndFaqContent(this.staticResourceService.helpAndFaqsContent);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.helpAndFaqsContent = this.staticContent.find(x => x.name === this.name);
        this.filterHelpAndFaqContent(this.helpAndFaqsContent);
        this.staticResourceService.helpAndFaqsContent = this.helpAndFaqsContent;
      }
    }
  }

  ngOnInit() {
    this.getHelpFaqPageContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe((value) => {
        this.getHelpFaqPageContent();
      });
  }
}
