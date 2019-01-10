import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Global } from "../../global";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { FooterComponent } from "./footer.component";

describe("FooterComponent", () => {
  let component: FooterComponent;
  let fixture: ComponentFixture<FooterComponent>;
  let mockStaticResourceService;
  let navigation;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    navigation = {
      name: "Navigation",
      location: [
        {
          state: "Default"
        }
      ]
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
      declarations: [FooterComponent],
      providers: [
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
    fixture = TestBed.createComponent(FooterComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should set about content to static resource if about context exists", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.navigation = navigation;
    spyOn(component, "filterNavigationContent");
    component.getNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
    expect(component.filterNavigationContent).toHaveBeenCalledWith(
      mockStaticResourceService.navigation
    );
  });

  it("should get content if navigated", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.navigation = navigation;
    component.getNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
  });

  it("should get content by :global.getData", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.navigation = "testnode";
    component.getNavigationContent();
    expect(component.navigation.name).toContain(globalData[0].name);
  });
});
