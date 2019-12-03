import { Location } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { By } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Global } from "../../global";
import { MapService } from "../../common/map/map.service";
import { ArrayUtilityService } from "../../common/services/array-utility.service";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StateCodeService } from "../../common/services/state-code.service";
import { PersonalizedPlanService } from "../personalized-plan/personalized-plan.service";
import { CuratedExperienceResultComponent } from "./curated-experience-result.component";

describe("CuratedExperienceResultComponent", () => {
  let component: CuratedExperienceResultComponent;
  let fixture: ComponentFixture<CuratedExperienceResultComponent>;
  let mockToastr;
  let mockNavigateDataService;
  let mockGuidedAssistantResults;
  let mockRouter;
  let mockArrayUtilityService;
  let mockGlobal;
  let mockPersonalizedPlanService;
  let mockMapLocation = {
    state: "California",
    city: "Riverside County",
    county: "Indio",
    zipCode: "92201"
  };
  let mockDisplayLocationDetails = {
    locality: "Indio",
    address: "92201"
  };
  let mockLocationDetails = {
    location: mockMapLocation,
    displayLocationDetails: mockDisplayLocationDetails,
    country: "United States",
    formattedAddress: "Hjorth St, Indio, California 92201, United States"
  };
  let mockSavedTopics = ["Divorce", "ChildCustody"];
  let mockLocation = {
    subscribe: () => {}
  };

  beforeEach(async(() => {
    mockNavigateDataService = jasmine.createSpyObj(["getData"]);
    mockToastr = jasmine.createSpyObj(["success"]);
    mockRouter = jasmine.createSpyObj(["navigateByUrl"]);
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "saveTopicsFromGuidedAssistantToProfile"
    ]);
    mockGuidedAssistantResults = {
      topIntent: "Divorce",
      relevantIntents: ["Domestic Violence", "Tenant's rights", "None"],
      topicIds: ["e1fdbbc6-d66a-4275-9cd2-2be84d303e12"],
      guidedAssistantId: "9a6a6131-657d-467d-b09b-c570b7dad242"
    };
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [CuratedExperienceResultComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        MapService,
        StateCodeService,
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: ArrayUtilityService,
          useValue: mockArrayUtilityService
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: Location,
          useValue: mockLocation
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceResultComponent);
    component = fixture.componentInstance;
    mockNavigateDataService.getData.and.returnValue(mockGuidedAssistantResults);
    component.ngOnInit();
    fixture.detectChanges();

    mockGlobal = {
      userId: "UserId",
      topicsSessionKey: "test"
    };
    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      },
      clear: () => {
        store = {};
      }
    };
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  // it("should filter out relevant intents that are None", () => {
  //   component.filterIntent();
  //   expect(component.relevantIntents).toEqual([
  //     "Domestic Violence",
  //     "Tenant's rights"
  //   ]);
  // });

  it("should call toastr when Save for Later button is clicked", () => {
    spyOn(component, "saveForLater");
    component.relevantIntents = ["Domestic Violence", "Tenant's rights"];
    fixture.debugElement
      .query(By.css(".btn-secondary"))
      .triggerEventHandler("click", { stopPropogration: () => {} });
    const button = fixture.debugElement.query(By.css(".btn-secondary"));
    button.triggerEventHandler("click", { stopPropogration: () => {} });
    expect(component.saveForLater).toHaveBeenCalled();
  });

  it("should call saveTopicsFromGuidedAssistantToProfile if user is logged in", () => {
    let mockIntentInput = {
      location: mockMapLocation,
      intents: mockSavedTopics
    };
    component.savedTopics = mockSavedTopics;
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify(mockLocationDetails)
    );
    component.saveForLater();
    expect(component.locationDetails).toEqual(mockLocationDetails);
    expect(component.intentInput).toEqual(mockIntentInput);
    expect(
      mockPersonalizedPlanService.saveTopicsFromGuidedAssistantToProfile
    ).toHaveBeenCalledWith(mockIntentInput, true);
  });
});
