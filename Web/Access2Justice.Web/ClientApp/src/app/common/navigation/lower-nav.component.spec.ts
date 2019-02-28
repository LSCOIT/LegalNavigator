import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Global } from "../../global";
import { EventUtilityService } from "../services/event-utility.service";
import { StateCodeService } from "../services/state-code.service";
import { StaticResourceService } from "../services/static-resource.service";
import { MapService } from "../map/map.service";
import { LowerNavComponent } from "./lower-nav.component";

describe("LowerNavComponent", () => {
  let component: LowerNavComponent;
  let fixture: ComponentFixture<LowerNavComponent>;
  let mockStaticResourceService;
  let navigation;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    navigation = {
      name: "Navigation",
      location: [{ state: "Default" }]
    };
    globalData = [
      {
        name: "Navigation",
        location: [
          {
            state: "Default"
          }
        ]
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj([
      "getLocation",
      "getStaticContents"
    ]);
    mockGlobal = jasmine.createSpyObj(["getData"]);
    mockGlobal.getData.and.returnValue(globalData);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [LowerNavComponent],
      providers: [
        MapService,
        EventUtilityService,
        StateCodeService,
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LowerNavComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should set about content to static resource about content if it exists", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.navigation = navigation;
    spyOn(component, "filterNavigationContent");
    component.getNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
    expect(component.filterNavigationContent).toHaveBeenCalledWith(
      mockStaticResourceService.navigation
    );
  });
});
