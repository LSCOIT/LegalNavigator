import { HttpClientModule } from "@angular/common/http";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { BsModalRef, BsModalService, ComponentLoaderFactory, PositioningService } from "ngx-bootstrap";
import { from } from "rxjs";
import { ReactiveFormsModule } from '@angular/forms';

import { Global } from "../../../../global";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ArrayUtilityService } from "../../../services/array-utility.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { ShareButtonComponent } from "./share-button.component";
import { Share } from "./share.model";
import { ShareService } from "./share.service";

describe("ShareButtonComponent", () => {
  let modalService: BsModalService;
  let component: ShareButtonComponent;
  let fixture: ComponentFixture<ShareButtonComponent>;
  let shareService: ShareService;
  let personalizedPlanService;
  let navigateDataService;
  let global;
  const mockUserId =
    "7BAFDE5D6707167889AE7DBB3133CFF2CB4E06DA356F9E8982B9286F471799F18B631BD6BE3784501DE246AB9FF446E5437B48372C5BFEB170E6D11ACF6AB797";
  let mockSessionKey = "showModal";
  let mockBlank = "";
  let mockURLWithPermaLink = "http://localhost:5150/share/1401C6D";
  let mockPermaLink = "1401C6D";
  let mockShowGenerateLink: boolean = false;
  let mockResourceUrl = "http://localhost:5150/share/";
  let mockShareInput: Share = {
    Url: "/topics/bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    ResourceId: "bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    UserId:
      "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B9ů64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",
    Location: null
  };
  let mockIstopics = "Topics";
  let mockIsGuidedAssistant = "Guided Assistant";
  let mockId = "6d7dd07a-c454-4b67-b2d8-ed844dadabd9";
  let mockResourceTopics = "/topics/6d7dd07a-c454-4b67-b2d8-ed844dadabd9";
  let mockGuidedAssistance =
    "/guidedassistant/6d7dd07a-c454-4b67-b2d8-ed844dadabd9";
  let msalService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        ReactiveFormsModule
      ],
      declarations: [ShareButtonComponent],
      providers: [
        BsModalRef,
        BsModalService,
        ComponentLoaderFactory,
        PositioningService,
        ArrayUtilityService,
        ShareService,
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: "123"
              }
            }
          }
        },
        {
          provide: Global,
          useValue: {
            global,
            userId: "UserId"
          }
        },
        {
          provide: NavigateDataService,
          useValue: navigateDataService
        },
        {
          provide: Router,
          useValue: {
            url: "/plan/id"
          }
        },
        {
          provide: PersonalizedPlanService,
          useValue: personalizedPlanService
        },
        {
          provide: MsalService,
          useValue: msalService
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ShareButtonComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    shareService = TestBed.get(ShareService);
    modalService = TestBed.get(BsModalService);

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
    spyOn(sessionStorage, "getItem").and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, "removeItem").and.callFake(
      mockSessionStorage.removeItem
    );
    spyOn(sessionStorage, "clear").and.callFake(mockSessionStorage.clear);
  }));

  it("should create share button component", () => {
    expect(component).toBeTruthy();
  });

  it(" should assign session storage details to when user id is available on ngInit", () => {
    component.userId = mockUserId;
    sessionStorage.setItem("mockGetSession", JSON.stringify(mockSessionKey));
    component.ngOnInit();
    expect(sessionStorage.getItem(this.sessionKey)).toBeNull();
  });

  it("should hide model popup when close method of component called", () => {
    spyOn(modalService, "hide");
    const modalRefInstance: jasmine.SpyObj<BsModalRef> = jasmine.createSpyObj<BsModalRef>('BsModalRef', ['hide']);
    component.modalRef = modalRefInstance;
    component.close();
    expect(modalRefInstance.hide).toHaveBeenCalled();
  });

  it("should check permalink is created or not when checklink method of share service called", () => {
    spyOn(shareService, "checkLink").and.returnValue(from([]));
    spyOn(modalService, "show");
    let mockTem = "template";
    component.shareInput = mockShareInput;
    shareService.checkLink(mockShareInput);
    modalService.show(mockTem);
    expect(modalService.show).toHaveBeenCalled();
  });

  it("should generate perma link when generateLink  method of share service called", () => {
    spyOn(shareService, "generateLink").and.returnValue(
      from([mockPermaLink])
    );
    spyOn(component, "generateLink");
    component.generateLink();
    component.shareView = mockPermaLink;
    component.showGenerateLink = mockShowGenerateLink;
    shareService.generateLink(mockShareInput);
    expect(shareService.generateLink).toHaveBeenCalledWith(mockShareInput);
    expect(shareService.generateLink(mockShareInput)).not.toBeNull();
    expect(component.shareView).toEqual(mockPermaLink);
    expect(component.showGenerateLink).toBe(mockShowGenerateLink);
    expect(component.generateLink).toHaveBeenCalled();
  });

  it("should remove permalink when removeLink method of component called", () => {
    spyOn(shareService, "removeLink").and.returnValue(from([true]));
    spyOn(component, "removeLink");
    component.removeLink();
    component.permaLink = mockBlank;
    component.showGenerateLink = mockShowGenerateLink;
    shareService.removeLink(mockShareInput);
    expect(shareService.removeLink).toHaveBeenCalledWith(mockShareInput);
    expect(shareService.removeLink(mockShareInput)).not.toBeNull();
    expect(component.permaLink).toEqual(mockBlank);
    expect(component.showGenerateLink).toBe(mockShowGenerateLink);
    expect(component.removeLink).toHaveBeenCalled();
  });

  it("should call getPermaLink method of component", () => {
    component.shareView = { permaLink: mockPermaLink };
    component.resourceUrl = mockResourceUrl;
    let result = component.getPermaLink();
    expect(component.shareView.permaLink).not.toBeNull();
    expect(component.resourceUrl).toBe(mockResourceUrl);
    expect(result).toEqual(mockURLWithPermaLink);
  });

  it("should call externalLogin method of component when user id is not available", () => {
    spyOn(component, "externalLogin");
    component.externalLogin();
    expect(component.externalLogin).toHaveBeenCalled();
  });

  it("should return correct url when buildUrl method of component", () => {
    component.type = mockIstopics;
    component.id = mockId;
    let buidlurl = component.buildUrl();
    expect(buidlurl).toEqual(mockResourceTopics);
  });
});
