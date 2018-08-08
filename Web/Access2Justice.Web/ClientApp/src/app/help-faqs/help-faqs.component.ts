import { Component, OnInit } from '@angular/core';
import { HelpAndFaqs, ImageUrl, Faq, } from '../help-faqs/help-faqs';
import { StaticResourceService } from '../shared/static-resource.service';
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
  constructor(
    private staticResourceService: StaticResourceService
  ) { }

  filterHelpAndFaqContent(): void {
    if (this.helpAndFaqsContent) {
      this.faqData = this.helpAndFaqsContent.faqs;
      this.imageData = this.helpAndFaqsContent.image;
    }
  }

  getHelpFaqPageContent(id): void {
    let helpAndFAQPageRequest = { name: this.name };
    this.staticResourceService.getStaticContent(helpAndFAQPageRequest)
      .subscribe(content => {
        this.helpAndFaqsContent = content[0];
        this.filterHelpAndFaqContent();
      });
  }
  ngOnInit() {
    this.getHelpFaqPageContent(this.id);
  }
}
