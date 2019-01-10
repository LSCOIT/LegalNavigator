import { Component, OnInit, QueryList, ViewChildren } from "@angular/core";
import { environment } from "../../environments/environment";
import { Global } from "../global";
import { Faq, HelpAndFaqs, ImageUrl } from "../help-faqs/help-faqs";
import { StaticResourceService } from "../shared/services/static-resource.service";

@Component({
  selector: "app-help-faqs",
  templateUrl: "./help-faqs.component.html",
  styleUrls: ["./help-faqs.component.css"]
})
export class HelpFaqsComponent implements OnInit {
  helpAndFaqsContent: HelpAndFaqs;
  faqData: Array<Faq> = [];
  imageData: ImageUrl;
  name: string = "HelpAndFAQPage";
  blobUrl: string = environment.blobUrl;
  staticContent: any;
  staticContentSubcription: any;
  @ViewChildren("contentAnswer") contentAnswer: QueryList<any>;

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

  listenToExpandOnEnter(e) {
    let indexOfAccordion = e.target.id.substr(-1);
    let currentAnswer = this.contentAnswer
      .toArray()
      .find(
        element => element.nativeElement.id.substr(-1) === indexOfAccordion
      );
    if (
      currentAnswer.nativeElement.parentElement.parentElement.style.display ===
      "none"
    ) {
      currentAnswer.nativeElement.parentElement.parentElement.style.display =
        "block";
      document
        .getElementsByTagName("accordion-group")
        [indexOfAccordion].classList.add("panel-open");
    } else {
      currentAnswer.nativeElement.parentElement.parentElement.style.display =
        "none";
      document
        .getElementsByTagName("accordion-group")
        [indexOfAccordion].classList.remove("panel-open");
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
