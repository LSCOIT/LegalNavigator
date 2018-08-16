import { Component, OnInit } from '@angular/core';
import { HelpAndFaqs, ImageUrl, Faq, } from '../help-faqs/help-faqs';
import { StaticResourceService } from '../shared/static-resource.service';
import { environment } from '../../environments/environment';

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

  constructor(
    private staticResourceService: StaticResourceService
  ) { }

  filterHelpAndFaqContent(helpAndFaqsContent): void {
    if (helpAndFaqsContent) {
      this.helpAndFaqsContent = helpAndFaqsContent
      this.faqData = helpAndFaqsContent.faqs;
      this.imageData = helpAndFaqsContent.image;
    }
  }

  getHelpFaqPageContent(): void {
    let helpAndFAQPageRequest = { name: this.name };
    if (this.staticResourceService.helpAndFaqsContent && (this.staticResourceService.helpAndFaqsContent.location[0].state == this.staticResourceService.loadStateName())) {
      this.helpAndFaqsContent = this.staticResourceService.helpAndFaqsContent;
      this.filterHelpAndFaqContent(this.staticResourceService.helpAndFaqsContent);
    } else {
      this.staticResourceService.getStaticContent(helpAndFAQPageRequest)
        .subscribe(content => {
          this.helpAndFaqsContent = content[0];
          this.filterHelpAndFaqContent(this.helpAndFaqsContent);
          this.staticResourceService.helpAndFaqsContent = this.helpAndFaqsContent;
        });
    }
  }

  ngOnInit() {
    this.getHelpFaqPageContent();
  }
}
