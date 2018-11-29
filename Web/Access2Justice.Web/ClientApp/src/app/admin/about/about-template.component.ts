import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { About } from '../../about/about';
import { environment } from '../../../environments/environment';
import { NgForm, FormBuilder, FormGroup } from '@angular/forms';
import { MapLocation } from '../../shared/map/map';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-about-template',
  templateUrl: './about-template.component.html',
  styleUrls: ['../admin-styles.css']
})
export class AboutTemplateComponent implements OnInit {
  name: string = 'AboutPage';
  aboutContent: About;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;
  form: FormGroup;
  detailParams: any;
  newAboutContent: any;
  state: string;
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  }
  newsImage;
  @ViewChildren('newsImageUpload') newsImageUpload: QueryList<any>;
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
    private adminService: AdminService
  ) {}

  createForm() {
    this.form = this.fb.group({
      serviceImage: null,
      privacyPromiseImage: null,
      newsImage: null,
      newsImage1: null,
      newsImage2: null,
      sponsorImage: null,
      sponsorImage1: null
    });
  }

  encode(image, index) {
    index = index || '';
    let reader = new FileReader();
    if (event.target["files"] && event.target["files"].length > 0) {
      let file = event.target["files"][0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.get(image+index).setValue({
          filename: file.name,
          filetype: file.type,
          value: reader.result.split(',')[1]
        });
      }
    }
  }

  onSubmit(aboutForm: NgForm) {
    this.newAboutContent = {
      aboutImage: {
        source: this.aboutContent.aboutImage.source,
        altText: this.aboutContent.aboutImage.altText
      },
      contactUs: {
        title: this.aboutContent["contactUs"].title,
        description: aboutForm.value.contactUsDescription || this.aboutContent["contactUs"].description,
        email: aboutForm.value.contactUsEmail || this.aboutContent["contactUs"].email
      },
      inTheNews: {
        title: this.aboutContent["inTheNews"].title,
        description: aboutForm.value.inTheNewsDescription || this.aboutContent["inTheNews"].description,
        news: [
          {
            title: aboutForm.value.newsTitle0 || this.aboutContent["inTheNews"].news[0].title,
            description: aboutForm.value.newsDescription0 || this.aboutContent["inTheNews"].news[0].description,
            image: {
              source: this.form.value.newsImage && this.form.value.newsImage.value || this.aboutContent["inTheNews"].news[0].image.source,
              altText: aboutForm.value.newsImageAltText0 || this.aboutContent["inTheNews"].news[0].image.altText
            },
            url: aboutForm.value.newsUrl0 || this.aboutContent["inTheNews"].news[0].url
          },
          {
            title: aboutForm.value.newsTitle1 || this.aboutContent["inTheNews"].news[1].title,
            description: aboutForm.value.newsDescription1 || this.aboutContent["inTheNews"].news[1].description,
            image: {
              source: this.form.value.newsImage1 && this.form.value.newsImage1.value || this.aboutContent["inTheNews"].news[1].image.source,
              altText: aboutForm.value.newsImageAltText1 || this.aboutContent["inTheNews"].news[1].image.altText
            },
            url: aboutForm.value.newsUrl1 || this.aboutContent["inTheNews"].news[1].url
          },
          {
            title: aboutForm.value.newsTitle2 || this.aboutContent["inTheNews"].news[2].title,
            description: aboutForm.value.newsDescription2 || this.aboutContent["inTheNews"].news[2].description,
            image: {
              source: this.form.value.newsImage2 && this.form.value.newsImage2.value || this.aboutContent["inTheNews"].news[2].image.source,
              altText: aboutForm.value.newsImageAltText2 || this.aboutContent["inTheNews"].news[2].image.altText
            },
            url: aboutForm.value.newsUrl2 || this.aboutContent["inTheNews"].news[2].url
          }
        ]
      },
      location: [this.location],
      mediaInquiries: {
        title: "Media Inquiries",
        description: aboutForm.value.mediaInquiriesDescription || this.aboutContent["mediaInquiries"].description,
        email: aboutForm.value.mediaInquiriesEmail || this.aboutContent["mediaInquiries"].email,
      },
      mission: {
        title: "Our Mission",
        description: aboutForm.value.aboutMission || this.aboutContent["mission"].description,
        sponsors: [
          {
            source: this.form.value.sponsorImage && this.form.value.sponsorImage.value || this.aboutContent["mission"].sponsors[0].source,
            altText: aboutForm.value.sponsorImageAltText0 || this.aboutContent["mission"].sponsors[0].altText
          },
          {
            source: this.form.value.sponsorImage1 && this.form.value.sponsorImage1.value || this.aboutContent["mission"].sponsors[1].source,
            altText: aboutForm.value.sponsorImageAltText1 || this.aboutContent["mission"].sponsors[1].altText
          }
        ]
      },
      name: "AboutPage",
      organizationalUnit: this.location.state,
      privacyPromise: {
        description: aboutForm.value.privacyPromiseDescription || this.aboutContent["privacyPromise"].description,
        image: {
          source: this.form.value.privacyPromiseImage && this.form.value.privacyPromiseImage.value || this.aboutContent["privacyPromise"].image.source,
          altText: aboutForm.value.privacyPromiseImageAltText || this.aboutContent["privacyPromise"].image.altText
        },
        privacyPromiseButton: {
          buttonText: "View our privacy promise",
          buttonAltText: "View our privacy promise",
          buttonLink: "privacy"
        },
        title: "Our Privacy Promise"
      },
      service: {
        description: aboutForm.value.serviceDescription,
        guidedAssistantButton: {
          buttonText: "Use our Guided Assistant",
          buttonAltText: "Use our Guided Assistant",
          buttonLink: "guidedassistant"
        },
        image: {
          source: this.form.value.serviceImage && this.form.value.serviceImage.value || this.aboutContent["service"].image.source,
          altText: aboutForm.value.serviceImageAltText || this.aboutContent["service"].image.altText
        },
        title: "Our Service",
        topicsAndResourcesButton: {
          buttonText: "Browse Topics & Resources",
          buttonAltText: "Browse Topics & Resources",
          buttonLink: "topics"
        }
      }
    }

    this.adminService.saveAboutData(this.newAboutContent).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.toastr.success("Page updated successfully");
        }
      });

  }

  getAboutPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.aboutContent = this.staticContent.find(x => x.name === this.name);
      this.createForm();
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.aboutContent = this.staticContent.find(x => x.name === this.name);
          this.createForm();
        },
        error => this.router.navigateByUrl('error'));
    }
  }

  showNewsUpload(image, index) {
    this[`${image}`].toArray()[index].nativeElement.style.display = 'none' ? 'block' : 'none';
    console.log(image);
  }

  ngOnInit() {
    this.getAboutPageContent();
  }
}
