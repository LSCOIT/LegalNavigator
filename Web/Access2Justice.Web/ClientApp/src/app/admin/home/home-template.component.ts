import { Component, OnInit, QueryList, ViewChildren } from "@angular/core";
import { FormBuilder, FormGroup, NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";

import ENV from 'env';
import { MapLocation } from "../../shared/map/map";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { AdminService } from "../admin.service";

@Component({
  selector: "admin-home",
  templateUrl: "./home-template.component.html",
  styleUrls: ["../admin-styles.css"]
})
export class HomeTemplateComponent implements OnInit {
  homeContent: any;
  staticContent: any;
  name = "HomePage";
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  };
  blobUrl: string = ENV().blobUrl;
  regionalIllustrations = [
    {
      name: " Beachy(No Island)",
      blobLocation: "/static-resource/regional-images/beachy-no-island.svg"
    },
    {
      name: "Beachy(With Island)",
      blobLocation: "/static-resource/regional-images/beachy-with-island.svg"
    },
    {
      name: "Forested Hills(Deciduous Trees) 1)",
      blobLocation:
        "/static-resource/regional-images/forested-hills-deciduous-trees-1.svg"
    },
    {
      name: "Forested Hills(Deciduous Trees) 2",
      blobLocation:
        "/static-resource/regional-images/forested-hills-deciduous-trees-2.svg"
    },
    {
      name: "Forested Hills(Pine Trees)",
      blobLocation:
        "/static-resource/regional-images/forested-hills-pine-trees.svg"
    },
    {
      name: "Mountainous",
      blobLocation: "/static-resource/regional-images/moutainous.svg"
    },
    {
      name: "Prairie",
      blobLocation: "/static-resource/regional-images/prairie.svg"
    },
    {
      name: "River / Lake",
      blobLocation: "/static-resource/regional-images/river-lake.svg"
    },
    {
      name: "SouthWest",
      blobLocation: "/static-resource/regional-images/southwest.svg"
    },
    {
      name: "Urban",
      blobLocation: "/static-resource/regional-images/urban.svg"
    }
  ];
  heroImageSelected: string;
  newHomeContent;
  form: FormGroup;
  @ViewChildren("slideImageUpload") slideImageUpload: QueryList<any>;
  @ViewChildren("sponsorImageUpload") sponsorImageUpload: QueryList<any>;

  constructor(
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {}

  createForm() {
    this.form = this.fb.group({
      guidedAssistantImage: null,
      privacyImage: null,
      slideImage: null,
      slideImage1: null,
      slideImage2: null,
      sponsorImage: null,
      sponsorImage1: null
    });
  }

  encode(image, index) {
    index = index || "";
    const reader = new FileReader();
    if (event.target["files"] && event.target["files"].length > 0) {
      const file = event.target["files"][0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.get(image + index).setValue({
          filename: file.name,
          filetype: file.type,
          // cast based on readAsDataURL call
          value: (reader.result as string).split(",")[1]
        });
      };
    }
  }

  getHomePageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.homeContent = this.staticContent.find(x => x.name === this.name);
      this.createForm();
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.homeContent = this.staticContent.find(x => x.name === this.name);
          this.createForm();
        },
        error => this.router.navigateByUrl("error")
      );
    }
  }

  createHomeParams(homeForm) {
    this.newHomeContent = {
      name: this.name,
      location: [this.location],
      organizationalUnit: this.location.state,
      hero: {
        heading: homeForm.value.heroHeading || this.homeContent.hero.heading,
        description: {
          text:
            homeForm.value.heroDescriptionText ||
            this.homeContent.hero.description.text,
          textWithLink: {
            urlText:
              homeForm.value.heroDescriptionUrlText ||
              this.homeContent.hero.description.textWithLink.urlText,
            url:
              homeForm.value.heroDescriptionUrl ||
              this.homeContent.hero.description.textWithLink.url
          }
        },
        image: {
          source: this.heroImageSelected || this.homeContent.hero.image.source,
          altText:
            homeForm.value.heroImageAltText ||
            this.homeContent.hero.image.altText
        }
      },
      guidedAssistantOverview: {
        heading:
          homeForm.value.guidedAssistantHeading ||
          this.homeContent.guidedAssistantOverview.heading,
        description: {
          steps: [
            {
              order: "1",
              description:
                homeForm.value.guidedAssistantSteps1 ||
                this.homeContent.guidedAssistantOverview.description.steps[0][
                  "description"
                ]
            },
            {
              order: "2",
              description:
                homeForm.value.guidedAssistantSteps2 ||
                this.homeContent.guidedAssistantOverview.description.steps[1][
                  "description"
                ]
            },
            {
              order: "3",
              description:
                homeForm.value.guidedAssistantSteps3 ||
                this.homeContent.guidedAssistantOverview.description.steps[2][
                  "description"
                ]
            }
          ],
          text:
            homeForm.value.gidedAssistantDescriptionText ||
            this.homeContent.guidedAssistantOverview.description.text,
          textWithLink: {
            urlText:
              homeForm.value.homeGuidedAssistantDescriptionUrlText ||
              this.homeContent.guidedAssistantOverview.description.textWithLink
                .urlText,
            url:
              homeForm.value.guidedAssistantDescriptionUrl ||
              this.homeContent.guidedAssistantOverview.description.textWithLink
                .url
          }
        },
        button: {
          buttonText:
            homeForm.value.guidedAssistantButtonText ||
            this.homeContent.guidedAssistantOverview.button.buttonText,
          buttonLink:
            homeForm.value.guidedAssistantButtonLink ||
            this.homeContent.guidedAssistantOverview.button.buttonLink
        },
        image: {
          source:
            (this.form.value.guidedAssistantImage &&
              this.form.value.guidedAssistantImage.value) ||
            this.homeContent.guidedAssistantOverview.image.source,
          altText:
            homeForm.value.guidedAssistantImageAltText ||
            this.homeContent.guidedAssistantOverview.image.altText
        }
      },
      topicAndResources: {
        heading:
          homeForm.value.topicResourcesHeading ||
          this.homeContent.topicAndResources.heading,
        button: {
          buttonText:
            homeForm.value.topicResourcesButtonText ||
            this.homeContent.topicAndResources.button.buttonText,
          buttonLink:
            homeForm.value.topicResourcesButtonLink ||
            this.homeContent.topicAndResources.button.buttonLink
        }
      },
      carousel: {
        slides: [
          {
            quote:
              homeForm.value.slideQuote0 ||
              this.homeContent.carousel.slides[0]["quote"],
            author:
              homeForm.value.slideAuthor0 ||
              this.homeContent.carousel.slides[0]["author"],
            location:
              homeForm.value.slideLocation0 ||
              this.homeContent.carousel.slides[0]["location"],
            image: {
              source:
                (this.form.value.slideImage &&
                  this.form.value.slideImage.value) ||
                this.homeContent.carousel.slides[0].image.source,
              altText:
                homeForm.value.slideImageAltText0 ||
                this.homeContent.carousel.slides[0].image.altText
            }
          },
          {
            quote:
              homeForm.value.slideQuote1 ||
              this.homeContent.carousel.slides[1]["quote"],
            author:
              homeForm.value.slideAuthor1 ||
              this.homeContent.carousel.slides[1]["author"],
            location:
              homeForm.value.slideLocation1 ||
              this.homeContent.carousel.slides[1]["location"],
            image: {
              source:
                (this.form.value.slideImage1 &&
                  this.form.value.slideImage1.value) ||
                this.homeContent.carousel.slides[1].image.source,
              altText:
                homeForm.value.slideImageAltText1 ||
                this.homeContent.carousel.slides[1].image.altText
            }
          },
          {
            quote:
              homeForm.value.slideQuote2 ||
              this.homeContent.carousel.slides[2]["quote"],
            author:
              homeForm.value.slideAuthor2 ||
              this.homeContent.carousel.slides[2]["author"],
            location:
              homeForm.value.slideLocation2 ||
              this.homeContent.carousel.slides[2]["location"],
            image: {
              source:
                (this.form.value.slideImage2 &&
                  this.form.value.slideImage2.value) ||
                this.homeContent.carousel.slides[2].image.source,
              altText:
                homeForm.value.slideImageAltText2 ||
                this.homeContent.carousel.slides[2].image.altText
            }
          }
        ]
      },
      sponsorOverview: {
        heading:
          homeForm.value.sponsorOverviewHeading ||
          this.homeContent.sponsorOverview.heading,
        description:
          homeForm.value.sponsorOverviewDescription ||
          this.homeContent.sponsorOverview.description,
        sponsors: [
          {
            source:
              (this.form.value.sponsorImage &&
                this.form.value.sponsorImage.value) ||
              this.homeContent.sponsorOverview.sponsors[0]["source"],
            altText:
              homeForm.value.sponsorImageAltText0 ||
              this.homeContent.sponsorOverview.sponsors[0]["altText"]
          },
          {
            source:
              (this.form.value.sponsorImage1 &&
                this.form.value.sponsorImage1.value) ||
              this.homeContent.sponsorOverview.sponsors[1]["source"],
            altText:
              homeForm.value.sponsorImageAltText1 ||
              this.homeContent.sponsorOverview.sponsors[1]["altText"]
          }
        ],
        button: {
          buttonText:
            homeForm.value.sponsorButtonText ||
            this.homeContent.sponsorOverview.button.buttonText,
          buttonLink:
            homeForm.value.sponsorButtonLink ||
            this.homeContent.sponsorOverview.button.buttonLink
        }
      },
      privacy: {
        heading:
          homeForm.value.privacyHeading || this.homeContent.privacy.heading,
        description:
          homeForm.value.privacyDescription ||
          this.homeContent.privacy.description,
        button: {
          buttonText:
            homeForm.value.privacyButtonText ||
            this.homeContent.privacy.button.buttonText,
          buttonLink:
            homeForm.value.privacyButtonLink ||
            this.homeContent.privacy.button.buttonLink
        },
        image: {
          source:
            (this.form.value.privacyImage &&
              this.form.value.privacyImage.value) ||
            this.homeContent.privacy.image.source,
          altText:
            homeForm.value.guidedAssistantImageAltText ||
            this.homeContent.privacy.image.altText
        }
      },
      helpText: {
        beginningText:
          homeForm.value.helpTextBeginningText ||
          this.homeContent.helpText["beginningText"],
        phoneNumber:
          homeForm.value.helpTextPhone ||
          this.homeContent.helpText["phoneNumber"],
        endingText:
          homeForm.value.helpTextEndingText ||
          this.homeContent.helpText["endingText"]
      }
    };
  }

  onSubmit(homeForm: NgForm) {
    this.spinner.show();
    this.createHomeParams(homeForm);
    this.adminService.saveHomeData(this.newHomeContent).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.homeContent = response;
          this.toastr.success("Page updated successfully");
        }
      },
      error => {
        console.log(error);
        this.toastr.warning("Page was not updated. Check console for error");
      }
    );
  }

  previewImage(event) {
    this.heroImageSelected = event.target.value;
  }

  showImageUpload(image, index) {
    this[`${image}`].toArray()[index].nativeElement.style.display = "none"
      ? "block"
      : "none";
  }

  ngOnInit() {
    this.getHomePageContent();
  }
}
