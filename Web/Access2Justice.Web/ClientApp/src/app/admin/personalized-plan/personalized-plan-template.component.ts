import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { StaticResourceService } from '../../shared/services/static-resource.service';
import { Global } from '../../global';
import { About } from '../../about/about';
import { environment } from '../../../environments/environment';
import { NgForm, FormBuilder, FormGroup } from '@angular/forms';
import { MapLocation } from '../../shared/map/map';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavigateDataService } from '../../shared/services/navigate-data.service';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../admin.service';
import { PersonalizedPlan } from '../../guided-assistant/personalized-plan/personalized-plan';

@Component({
  selector: 'app-personalized-plan-template',
  templateUrl: './personalized-plan-template.component.html',
  styleUrls: ['../admin-styles.css']
})
export class PersonalizedPlanTemplateComponent implements OnInit {
  name: string = 'PersonalizedActionPlanPage';
  personalizedPlanContent: PersonalizedPlan;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;
  form: FormGroup;
  detailParams: any;
  newPersonalizedPlanContent: any;
  state: string;
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  }
  @ViewChildren('sponsorImageUpload') sponsorImageUpload: QueryList<any>;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private fb: FormBuilder,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService,
    private adminService: AdminService) { }

  createForm() {
    this.form = this.fb.group({
      sponsorImage: null
    });
  }

  encode(image, index) {
    index = index || '';
    let reader = new FileReader();
    if (event.target["files"] && event.target["files"].length > 0) {
      let file = event.target["files"][0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.get(image + index).setValue({
          filename: file.name,
          filetype: file.type,
          value: reader.result.split(',')[1]
        });
      }
    }
  }

  createAboutParams(personalizedPlanForm) {
    this.newPersonalizedPlanContent = {
      name: this.name,
      location: [this.location],
      organizationalUnit: this.location.state,
      description: personalizedPlanForm.value.personalizedPlanDescription || this.personalizedPlanContent["description"],
      sponsors: [
        {
          source: this.form.value.sponsorImage && this.form.value.sponsorImage.value || this.personalizedPlanContent["sponsors"][0].source || '',
          altText: personalizedPlanForm.value.sponsorImageAltText0 || this.personalizedPlanContent["sponsors"][0].altText
        }
      ]
    }
  }

  onSubmit(personalizedPlanForm: NgForm) {
    this.createAboutParams(personalizedPlanForm);
    this.adminService.savePersonalizedPlanData(this.newPersonalizedPlanContent).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.toastr.success("Page updated successfully");
        }
      },
      error => {
        console.log(error);
        this.toastr.warning("Page was not updated. Please check the console");
      });
  }

  getPersonalizedPlanPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.personalizedPlanContent = this.staticContent.find(x => x.name === this.name);
      this.createForm();
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.personalizedPlanContent = this.staticContent.find(x => x.name === this.name);
          this.createForm();
        },
        error => this.router.navigateByUrl('error'));
    }
  }

  showNewsUpload(image, index) {
    this[`${image}`].toArray()[index].nativeElement.style.display = 'none' ? 'block' : 'none';
  }

  ngOnInit() {
    this.getPersonalizedPlanPageContent();
  }
}
