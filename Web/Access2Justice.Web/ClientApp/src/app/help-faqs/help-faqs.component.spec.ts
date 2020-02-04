import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Global } from "../global";
import { StateCodeService } from "../common/services/state-code.service";
import { StaticResourceService } from "../common/services/static-resource.service";
import { HelpFaqsComponent } from "./help-faqs.component";
import { SharedModule } from "../shared/shared.module";

describe("HelpFaqsComponent", () => {
  let component: HelpFaqsComponent;
  let fixture: ComponentFixture<HelpFaqsComponent>;
  let mockStaticResourceService;
  let helpAndFaqsContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    helpAndFaqsContent = {
      name: "HelpAndFAQPage",
      location: [{ state: "Default" }]
    };
    globalData = [
      {
        name: "HelpAndFAQPage",
        location: [{ state: "Default" }]
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj([
      "getLocation",
      "getStaticContents"
    ]);
    mockGlobal = jasmine.createSpyObj(["getData"]);
    mockGlobal.getData.and.returnValue(globalData);

    TestBed.configureTestingModule({
      imports: [SharedModule],
      declarations: [HelpFaqsComponent],
      providers: [
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
    fixture = TestBed.createComponent(HelpFaqsComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should set help and faq content to static resource content if it exists", () => {
    mockStaticResourceService.getLocation.and.returnValue("Default");
    mockStaticResourceService.helpAndFaqsContent = helpAndFaqsContent;
    spyOn(component, "filterHelpAndFaqContent");
    component.getHelpFaqPageContent();
    expect(component.helpAndFaqsContent).toEqual(
      mockStaticResourceService.helpAndFaqsContent
    );
    expect(component.filterHelpAndFaqContent).toHaveBeenCalledWith(
      mockStaticResourceService.helpAndFaqsContent
    );
  });
});
