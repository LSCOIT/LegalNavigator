import { inject, TestBed } from "@angular/core/testing";
import { of } from "rxjs/observable/of";
import { Global } from "../../../../global";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ProfileComponent } from "../../../../profile/profile.component";
import { SaveButtonService } from "./save-button.service";

describe("SaveButtonService", () => {
  let service: SaveButtonService;
  let mockPersonalizedPlanService;
  let global: Global;
  let mockProfileComponent;
  let mockPlanDetails = {
    id: "29250697-8d22-4f9d-bbf8-96c1b5b72e54",
    isShared: false,
    topics: [
      {
        topicId: "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        name: "Protective Order",
        icon: null,
        additionalReadings: [
          {
            text: "Domestic Violence - What it is",
            url: "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            text: "Safety Planning Tips",
            url: "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        steps: [
          {
            description: "Short Term",
            isComplete: false,
            order: 1,
            resources: [],
            stepId: "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            title: "Short Term Protective Order"
          },
          {
            description: "Long Term",
            isComplete: false,
            order: 2,
            resources: [],
            stepId: "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            title: "Long Term Protective Order"
          }
        ]
      },
      {
        topicId: "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
        name: "Divorce",
        icon: null,
        additionalReadings: [
          {
            text: "Domestic Violence - What it is",
            url: "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          }
        ],
        steps: [
          {
            description: "Step Description",
            isComplete: false,
            order: 1,
            resources: [],
            stepId: "df822558-73c2-4ac8-8259-fabe2334eb71",
            title: "Step Title"
          }
        ]
      }
    ]
  };

  beforeEach(() => {
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "userPlan",
      "showSuccess"
    ]);
    mockPersonalizedPlanService.userPlan.and.returnValue(of(mockPlanDetails));
    mockProfileComponent = jasmine.createSpyObj(["getPersonalizedPlan"]);
    service = new SaveButtonService(
      mockPersonalizedPlanService,
      global,
      mockProfileComponent
    );

    TestBed.configureTestingModule({
      providers: [
        SaveButtonService,
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: Global,
          useValue: {
            global,
            userId: "userId",
            planSessionKey: "planKey"
          }
        },
        {
          provide: ProfileComponent,
          useValue: mockProfileComponent
        }
      ]
    });
    global = TestBed.get(Global);
  });

  beforeEach(() => {
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

    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, "removeItem").and.callFake(
      mockSessionStorage.removeItem
    );
    spyOn(sessionStorage, "clear").and.callFake(mockSessionStorage.clear);
  });

  it("should be created", inject(
    [SaveButtonService],
    (service: SaveButtonService) => {
      expect(service).toBeTruthy();
    }
  ));

  it("should call service methods and profilecomponent if userId exists in savePlanToUserProfile method", () => {
    global.userId = "UserId";
    service.savePlanToUserProfile(mockPlanDetails);
    expect(service.personalizedPlan).toEqual(mockPlanDetails);
    expect(mockPersonalizedPlanService.userPlan).toHaveBeenCalled();
    expect(mockPersonalizedPlanService.showSuccess).toHaveBeenCalled();
    expect(sessionStorage.removeItem).toHaveBeenCalled();
    expect(mockProfileComponent.getPersonalizedPlan).toHaveBeenCalled();
  });
});
