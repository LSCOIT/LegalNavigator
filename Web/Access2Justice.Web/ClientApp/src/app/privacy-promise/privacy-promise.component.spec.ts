import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Global } from "../global";
import { MapService } from "../shared/map/map.service";
import { StaticResourceService } from "../shared/services/static-resource.service";
import { PrivacyPromiseComponent } from "./privacy-promise.component";

describe("PrivacyPromiseComponent", () => {
  let component: PrivacyPromiseComponent;
  let fixture: ComponentFixture<PrivacyPromiseComponent>;
  let mockStaticResourceService;
  let privacyContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    (privacyContent = {
      name: "PrivacyPromisePage",
      location: [{ state: "Default" }]
    }),
      (globalData = [
        {
          name: "PrivacyPromisePage",
          location: [{ state: "Default" }]
        }
      ]);
    mockStaticResourceService = jasmine.createSpyObj([
      "getLocation",
      "getStaticContents"
    ]);
    mockGlobal = jasmine.createSpyObj(["getData"]);
    mockGlobal.getData.and.returnValue(globalData);

    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseComponent],
      providers: [
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        MapService
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyPromiseComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should set privacy promise content to static resource content if it exists", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.helpAndFaqsContent = privacyContent;
    component.getPrivacyPageContent();
    expect(component.privacyContent).toEqual(
      mockStaticResourceService.helpAndFaqsContent
    );
  });
});
