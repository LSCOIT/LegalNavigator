import { Component, OnInit } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { About } from '../../about/about';
import { environment } from '../../../environments/environment';
import { NgForm, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MapLocation } from '../../shared/map/map';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';

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

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private fb: FormBuilder,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService,
  ) {
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      serviceImage: null,
      privacyPromiseImage: null,
      newsImage1: null,
      newsImage2: null,
      newsImage3: null
    });
  }

  encode(image, index) {
    index = index || '';
    console.log(index);
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
      contactUs: {
        title: this.aboutContent["contactUs"].title,
        description: aboutForm.value.contactUsDescription,
        email: aboutForm.value.contactUsEmail
      },
      inTheNews: {
        title: this.aboutContent["inTheNews"].title,
        description: aboutForm.value.inTheNewsDescription,
        news: [
          {
            title: aboutForm.value.newsTitle1,
            description: aboutForm.value.newsDescription1,
            image: {
              source: this.form.value.newsImage1.value,
              altText: aboutForm.value.newsImageAltText1
            },
            url: aboutForm.value.newsUrl1
          },
          {
            title: aboutForm.value.newsTitle2,
            description: aboutForm.value.newsDescription2,
            image: {
              source: this.form.value.newsImage2.value,
              altText: aboutForm.value.newsImageAltText2
            },
            url: aboutForm.value.newsUrl2
          },
          {
            title: aboutForm.value.newsTitle3,
            description: aboutForm.value.newsDescription3,
            image: {
              source: this.form.value.newsImage3.value,
              altText: aboutForm.value.newsImageAltText3
            },
            url: aboutForm.value.newsUrl3
          }
        ]
      },
      location: [this.location],
      mediaInquiries: {
        title: "Media Inquiries",
        description: aboutForm.value.mediaInquiriesDescription,
        email: aboutForm.value.mediaInquiriesEmail,
      },
      mission: {
        title: "Our Mission",
        description: aboutForm.value.aboutMission,
        sponsors: []
      },
      name: "AboutPage",
      organizationalUnit: this.location,
      privacyPromise: {
        description: aboutForm.value.privacyPromiseDescription,
        image: {
          source: this.form.value.privacyPromiseImage.value,
          altText: aboutForm.value.privacyPromiseImageAltText
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
          source: this.form.value.serviceImage.value,
          altText: aboutForm.value.serviceImageAltText
        },
        title: "Our Service",
        topicsAndResourcesButton: {
          buttonText: "Browse Topics & Resources",
          buttonAltText: "Browse Topics & Resources",
          buttonLink: "topics"
        }
      }
    }
  }


  getAboutPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.aboutContent = this.staticContent.find(x => x.name === this.name);
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.aboutContent = this.staticContent.find(x => x.name === this.name);
        },
        error => this.router.navigateByUrl('error'));
    }
  }

  ngOnInit() {
    this.getAboutPageContent();
  }
}
