import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { MapLocation } from "../../shared/map/map";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { AdminService } from "../admin.service";

@Component({
  selector: "admin-help-faqs-template",
  templateUrl: "./help-faqs-template.component.html",
  styleUrls: ["../admin-styles.css"]
})
export class HelpFaqsTemplateComponent implements OnInit {
  faqForm: FormGroup;
  helpAndFaqsContent: any;
  staticContent: any;
  name: string = "HelpAndFAQPage";
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  };
  newHelpAndFaqsContent: any;
  faqParams: Array<Object>;

  constructor(
    private fb: FormBuilder,
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {}

  get faqs() {
    return this.faqForm.get("faqs") as FormArray;
  }

  addFaqs() {
    this.faqs.push(this.createFaqs());
  }

  createFaqs(): FormGroup {
    return this.fb.group({
      question: "",
      answer: ""
    });
  }

  createForm() {
    this.faqForm = this.fb.group({
      faqs: this.fb.array([this.createFaqs()])
    });
  }

  getHelpFaqPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.helpAndFaqsContent = this.staticContent.find(
        x => x.name === this.name
      );
      this.createForm();
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.helpAndFaqsContent = this.staticContent.find(
            x => x.name === this.name
          );
          this.createForm();
        },
        error => this.router.navigateByUrl("error")
      );
    }
  }

  mapSectionDescription(form) {
    let numOfTitles = Object.keys(form).filter(
      key => key.indexOf("question") > -1
    );
    this.faqParams = numOfTitles.map((key, i) => {
      return {
        question: form[`question${i}`],
        answer: form[`answer${i}`]
      };
    });
  }

  createHelpAndFaqsParams(form) {
    this.newHelpAndFaqsContent = {
      name: this.name,
      location: [this.location],
      organizationalUnit: this.location.state,
      description:
        form.value.helpPageDescription || this.helpAndFaqsContent.description,
      faqs: this.faqParams,
      image: {
        source: this.helpAndFaqsContent.image.source,
        altText: this.helpAndFaqsContent.image.altText
      }
    };
  }

  onSubmit(helpFaqForm: NgForm) {
    this.mapSectionDescription(helpFaqForm.value);
    this.faqForm.value.faqs.forEach(faq => {
      if (faq.question && faq.answer) {
        this.faqParams.push(faq);
      }
    });
    this.createHelpAndFaqsParams(helpFaqForm);
    this.adminService.saveHelpAndFaqData(this.newHelpAndFaqsContent).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.toastr.success("Page updated successfully");
          this.faqParams = [];
        }
      },
      error => {
        console.log(error);
        this.toastr.warning("Page was not updated. Check console for error");
      }
    );
  }

  ngOnInit() {
    this.getHelpFaqPageContent();
  }
}
