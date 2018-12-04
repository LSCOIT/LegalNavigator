import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormArray, NgForm, FormGroup } from '@angular/forms';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { HelpAndFaqs } from '../../help-faqs/help-faqs';
import { StaticResourceService } from '../../shared/static-resource.service';
import { MapLocation } from '../../shared/map/map';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from '../admin.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'admin-help-faqs-template',
  templateUrl: './help-faqs-template.component.html',
  styleUrls: ['../admin-styles.css']
})
export class HelpFaqsTemplateComponent implements OnInit {
  faqForm: FormGroup;
  helpAndFaqsContent: any;
  staticContent: any;
  name: string = 'HelpAndFAQPage';
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  }
  newHelpAndFaqsContent: any;

  constructor(
    private fb: FormBuilder,
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) { }

  get faqs() {
    return this.faqForm.get('faqs') as FormArray;
  }

  addFAQs() {
    this.faqs.push(this.createFAQs());
  }

  createFAQs(): FormGroup {
    return this.fb.group({
      question:'',
      answer: ''
    });
  }

  createForm() {
    this.faqForm = this.fb.group({
      faqs: this.fb.array([this.createFAQs()])
    });
  }

  getHelpFaqPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.helpAndFaqsContent = this.staticContent.find(x => x.name === this.name);
      this.createForm();
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.helpAndFaqsContent = this.staticContent.find(x => x.name === this.name);
          this.createForm();
        },
        error => this.router.navigateByUrl('error'));
    }
  }

  onSubmit(helpFaqForm: NgForm) {
    this.newHelpAndFaqsContent = {
      name: this.name,
      location: [this.location],
      organizationalUnit: this.location.state,
      description: helpFaqForm.value.helpPageDescription || this.helpAndFaqsContent.description,
      faqs: [
        {
          question: helpFaqForm.value.question0,
          answer: helpFaqForm.value.answer0
        },
        {
          question: helpFaqForm.value.question1,
          answer: helpFaqForm.value.answer1
        }
      ],
      image: {
        source: this.helpAndFaqsContent.image.source,
        altText: this.helpAndFaqsContent.image.altText
      }
    }

    this.adminService.saveHelpAndFaqData(this.newHelpAndFaqsContent).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.helpAndFaqsContent = response;
          this.toastr.success("Page updated successfully");
        }
      });
  }

  ngOnInit() {
    this.getHelpFaqPageContent();
  }

}
