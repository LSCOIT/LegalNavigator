import { Component, OnInit } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { About } from '../../about/about';
import { environment } from '../../../environments/environment';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
import Map1 = require("../../shared/map/map");
import MapLocation = Map1.MapLocation;

@Component({
  selector: 'app-about-admin',
  templateUrl: './about-admin.component.html',
  styleUrls: ['../admin-styles.css']
})
export class AboutAdminComponent implements OnInit {
  name: string = 'AboutPage';
  aboutContent: About;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;
  form: FormGroup;
  mapLocation: MapLocation;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private fb: FormBuilder
  ) {
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      serviceImage: null,
      privacyPromiseImage: null
    });
  }

  getAboutPageContent(): void {
    if (this.staticResourceService.aboutContent && (this.staticResourceService.aboutContent.location[0].state == this.staticResourceService.getLocation())) {
      this.aboutContent = this.staticResourceService.aboutContent;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.aboutContent = this.staticContent.find(x => x.name === this.name);
        this.staticResourceService.aboutContent = this.aboutContent;
      }
    }
  }

  encode(image) {
    let reader = new FileReader();
    console.log(event);
    console.log(image);
    if (event.target["files"] && event.target["files"].length > 0) {
      let file = event.target["files"][0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.get(image).setValue({
          filename: file.name,
          filetype: file.type,
          value: reader.result.split(',')[1]
        });
      }
    }
  }

  //onSubmit(aboutForm: NgForm) {
  //  if (sessionStorage.getItem("globalMapLocation")) {
  //    this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
  //  }

  //  let aboutParams = {
  //    contactUs: {
  //      title: this.aboutContent["contactUs"].title,
  //      description: aboutForm.value.contactUsDescription,
  //      email: aboutForm.value.contactUsEmail
  //    },
  //    inTheNews: {
  //      title: this.aboutContent["inTheNews"].title,
  //      description: aboutForm.value.inTheNewsDescription,
  //      news: [] //need to add this
  //    },
  //    location: [this.mapLocation],
  //    mediaInquiries: {
  //      description: aboutForm.value.mediaInquiriesDescription,
  //      email: aboutForm.value.mediaInquiriesEmail,
  //      title: "Media Inquiries"
  //    },
  //    mission: {
  //      title: "Our Mission",
  //      description: "",
  //      sponsors: []
  //    },
  //    name: "AboutPage",
  //    organizationalUnit: "Hawaii",
  //    privacyPromise: {
  //      description: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Repellendus, nulla!",
  //      image: {
  //        source: "",
  //        altText: ""
  //      },
  //      altText: "",
  //      source: "",
  //      privacyPromiseButton: {
  //        buttonText: "View our privacy promise",
  //        buttonAltText: "View our privacy promise",
  //        buttonLink: "privacy"
  //      },
  //      title: "Our Privacy Promise"
  //    },
  //    service: {
  //      description:
  //        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quaerat dicta illo rerum voluptatibus magni molestias officiis totam minima ab quas quae dolorem aut laboriosam expedita atque dignissimos, sed maxime ratione consequatur optio nobis odit repellendus quos dolore! Sunt atque accusantium praesentium, culpa hic enim voluptas suscipit cumque odit deleniti animi reiciendis perspiciatis magnam exercitationem minus? Itaque exercitationem nostrum dignissimos eum tempore magnam, quasi alias voluptas sequi facere voluptate aperiam porro.",
  //      guidedAssistantButton: {
  //        buttonText: "Use our Guided Assistant",
  //        buttonAltText: "Use our Guided Assistant",
  //        buttonLink: "guidedassistant"
  //      },
  //      image: {
  //        source: "",
  //        altText: ""
  //      },
  //      altText: "",
  //      source: "",
  //      title: "Our Service",
  //      topicsAndResourcesButton: {
  //        buttonText: "Browse Topics & Resources",
  //        buttonAltText: "Browse Topics & Resources",
  //        buttonLink: "topics"
  //      }
  //    }
  //  }
  //}

  ngOnInit() {
    this.getAboutPageContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe(() => {
        this.getAboutPageContent();
      });
  }
}

