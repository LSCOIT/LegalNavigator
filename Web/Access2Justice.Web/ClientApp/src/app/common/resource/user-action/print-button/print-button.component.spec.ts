import { APP_BASE_HREF } from "@angular/common";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute } from "@angular/router";
import { of } from "rxjs";

import { PrintButtonComponent } from "./print-button.component";

describe("PrintButtonComponent", () => {
  let component: PrintButtonComponent;
  let fixture: ComponentFixture<PrintButtonComponent>;
  let activatedRoute: ActivatedRoute;
  let mockTitleAppSearchResults = "app-search-results";
  let mockTitleAppActionPlans = "app-action-plans";
  let mockTitleAppRresourceCardDetail = "app-resource-card-detail";
  let mockTitleAppSubtopicDetail = "app-subtopic-detail";
  let mockTitleTabMySavedResources = "My Saved Resources";
  let mockTitleTabMyPlans = "My Plan";

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrintButtonComponent],
      providers: [
        { provide: APP_BASE_HREF, useValue: "/" },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: { topic: "bd900039-2236-8c2c-8702-d31855c56b0f" }
            },
            url: of([
              { path: "subtopics", params: {} },
              { path: "bd900039-2236-8c2c-8702-d31855c56b0f", params: {} }
            ])
          }
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  // it("should create", () => {
  //   expect(component).toBeTruthy();
  // });

  // it("should display the template name for subtopic: app-subtopic-detail", () => {
  //   component.activeRouteName = "subtopics";
  //   component.template = mockTitleAppSubtopicDetail;
  //   fixture.detectChanges();
  //   expect(component.template).toEqual(mockTitleAppSubtopicDetail);
  // });

  // it("should display the template name for resource: app-resource-card-detail", () => {
  //   component.activeRouteName = "resource";
  //   component.template = mockTitleAppRresourceCardDetail;
  //   fixture.detectChanges();
  //   expect(component.template).toEqual(mockTitleAppRresourceCardDetail);
  // });

  // it("should display the template name for profile: app-action-plans", () => {
  //   component.activeRouteName = "profile";
  //   component.activeTab = mockTitleTabMyPlans;
  //   component.template = mockTitleAppActionPlans;
  //   fixture.detectChanges();
  //   expect(component.activeTab).toEqual(mockTitleTabMyPlans);
  //   expect(component.template).toEqual(mockTitleAppActionPlans);
  // });

  // it("should display the template name for profile: app-search-results", () => {
  //   component.activeRouteName = "profile";
  //   component.activeTab = mockTitleTabMySavedResources;
  //   component.template = mockTitleAppSearchResults;
  //   fixture.detectChanges();
  //   expect(component.activeTab).toEqual(mockTitleTabMySavedResources);
  //   expect(component.template).toEqual(mockTitleAppSearchResults);
  // });
});
