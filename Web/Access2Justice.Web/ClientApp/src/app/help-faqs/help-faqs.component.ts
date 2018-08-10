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

  filterHelpAndFaqContent(): void {
    if (this.helpAndFaqsContent) {
      this.faqData = this.helpAndFaqsContent.faqs;
      this.imageData = this.helpAndFaqsContent.image;
    }
  }

  getHelpFaqPageContent(): void {
    let helpAndFAQPageRequest = { name: this.name };
    this.staticResourceService.getStaticContent(helpAndFAQPageRequest)
      .subscribe(content => {
        this.helpAndFaqsContent = content[0];
        this.filterHelpAndFaqContent();
      });
  }
  ngOnInit() {
    this.getHelpFaqPageContent();
  }
}
