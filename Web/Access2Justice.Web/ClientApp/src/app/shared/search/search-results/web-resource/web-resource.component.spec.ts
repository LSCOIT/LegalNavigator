import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { SaveButtonComponent } from "../../../resource/user-action/save-button/save-button.component";
import { ShareButtonComponent } from "../../../resource/user-action/share-button/share-button.component";
import { WebResourceComponent } from "./web-resource.component";

describe("WebResourceComponent", () => {
  let component: WebResourceComponent;
  let fixture: ComponentFixture<WebResourceComponent>;
  let mockWebResource = {
    webResources: {
      _type: "SearchResponse",
      queryContext: {
        originalQuery: "i need volunteers"
      },
      webPages: {
        webSearchUrl: "https://www.bing.com/search?q=i+need+volunteers",
        webSearchUrlPingSuffix: "DevEx,5365.1",
        totalEstimatedMatches: 46700,
        value: [
          {
            id: "https://api.cognitive.microsoft.com/api/v7/#WebPages.0",
            name: "Pro Bono Net - Official Site",
            url: "http://www.probono.net/",
            urlPingSuffix: "DevEx,5104.1",
            about: [
              {
                name: "Pro Bono Net"
              },
              {
                name: "Pro Bono Net"
              }
            ],
            isFamilyFriendly: true,
            displayUrl: "www.probono.net",
            snippet:
              "In honor of National Volunteer ... Pro Bono Net would like to extend our gratitude to the thousands of volunteer lawyers who make a huge difference for those in need ...",
            deepLinks: [
              {
                name: "New York",
                url: "https://www.probono.net/ny/",
                urlPingSuffix: "DevEx,5082.1",
                snippet:
                  "NYC Bar Association Seeks Volunteers for the upcoming Monday Night Law Clinic session beginning this October. Volunteers must apply by August 31st, 2018 to mondaynightlaw@gmail.com, and must attend two training sessions on September 17th and September 24th, 2018 from 5:45 to 9:00 pm. Click here for application and more information."
              },
              {
                name: "About Pro Bono Net",
                url: "http://www.probono.net/about/",
                urlPingSuffix: "DevEx,5089.1",
                snippet:
                  "Pro Bono Net is a national nonprofit, ... Pro Bono Net transforms the way legal help reaches those in need. ... Mobilize volunteers to expand help available."
              }
            ],
            dateLastCrawled: "2018-08-21T21:52:00Z",
            fixedPosition: false,
            language: "en",
            isNavigational: true
          },
          {
            id: "https://api.cognitive.microsoft.com/api/v7/#WebPages.8",
            name: "National Pro Bono Opportunities Guide - Pro Bono Net",
            url: "https://www.probono.net/oppsguide/",
            urlPingSuffix: "DevEx,5235.1",
            about: [
              {
                name: "Pro Bono Net"
              }
            ],
            isFamilyFriendly: true,
            displayUrl: "https://www.probono.net/oppsguide",
            snippet:
              "Volunteer Week 2018. In honor of National Volunteer Week 2018, which took place April 15-21, Pro Bono Net would like to extend our gratitude to the thousands of volunteer lawyers who make a huge difference for those in need.",
            dateLastCrawled: "2018-08-23T10:50:00Z",
            fixedPosition: false,
            language: "en",
            isNavigational: false
          }
        ]
      }
    }
  };
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [
        WebResourceComponent,
        SaveButtonComponent,
        ShareButtonComponent
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebResourceComponent);
    component = fixture.componentInstance;
    component.webResult = mockWebResource;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
