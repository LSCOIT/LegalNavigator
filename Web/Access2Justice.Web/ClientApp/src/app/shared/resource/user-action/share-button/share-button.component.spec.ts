import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ShareButtonComponent } from './share-button.component';
import { BsModalService, ComponentLoaderFactory, PositioningService, BsModalRef } from 'ngx-bootstrap';
import { ArrayUtilityService } from '../../../array-utility.service';
import { HttpClientModule } from '@angular/common/http';
import { ShareService } from './share.service';
import { Observable } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Global } from '../../../../global';
import { Share } from './share.model';
import { expand } from 'rxjs/operators';
import { TemplateRef } from '@angular/core';
import { empty } from 'rxjs/observable/empty';


describe('ShareButtonComponent', () => {

  class ActivateRouteStub {
    params: Observable<any> = Observable.empty();
  }
  class MockBsModalRef {
    public isHideCalled = false;

    hide() {
      this.isHideCalled = true;
    }
  }
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let component: ShareButtonComponent;
  let fixture: ComponentFixture<ShareButtonComponent>;
  let shareService: ShareService;
  let activeRoute: ActivatedRoute;
  let mockUserId = "7BAFDE5D6707167889AE7DBB3133CFF2CB4E06DA356F9E8982B9286F471799F18B631BD6BE3784501DE246AB9FF446E5437B48372C5BFEB170E6D11ACF6AB797";
  let mockSessionKey = "showModal";
  let mockBlank = "";
  let mockURLWithPermaLink = "http://localhost:5150/share/1401C6D";
  let mockPermaLink = "1401C6D";
  let mockShowGenerateLink: boolean = false;
  let mockResourceUrl = "http://localhost:5150/share/";
  let mockShareInput: Share = {
    Url: "/topics/bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    ResourceId: "bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    UserId: "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B9ů64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",

  };
  let mockIstopics = "Topics";
  let mockIsGuidedAssistant = "Guided Assistant";
  let mockId = '6d7dd07a-c454-4b67-b2d8-ed844dadabd9';
  let mockResourceTopics = "/topics/6d7dd07a-c454-4b67-b2d8-ed844dadabd9";
  let mockGuidedAssistance = "/guidedassistant/6d7dd07a-c454-4b67-b2d8-ed844dadabd9";
  const mockResponse = Observable.of(mockPermaLink);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ShareButtonComponent],
      providers: [
        { provide: ActivatedRoute, useValue: ActivateRouteStub },
        BsModalRef,
        BsModalService,
        ComponentLoaderFactory,
        PositioningService,
        ArrayUtilityService,
        ShareService,
        Global
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ShareButtonComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
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
    spyOn(sessionStorage, 'getItem')
      .and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  }));

  it('should create share button component', () => {
    expect(component).toBeTruthy();
  });

  it(' should assign session storage details to when user id is available on ngInit', () => {
    component.userId = mockUserId;
    sessionStorage.setItem("mockGetSession", JSON.stringify(mockSessionKey));
    let mockGetSession = JSON.parse(sessionStorage.getItem("mockGetSession"));
    component.ngOnInit();
    expect(sessionStorage.getItem(this.sessionKey)).toBeNull();
  });

  it('should hide model popup when close method of component called', () => {
    spyOn(modalService, 'hide');
    let modalRefInstance = new MockBsModalRef();
    component.modalRef = modalRefInstance;
    component.close();
    expect(modalRefInstance.isHideCalled).toBeTruthy();
  });

  it('should check permalink is created or not when checklink method of share service called', () => {
    spyOn(shareService, 'checkLink').and.returnValue(Observable.from([]));
    spyOn(modalService, 'show');
    let mockTem = 'template';
    component.shareInput = mockShareInput;
    shareService.checkLink(mockShareInput);
    modalService.show(mockTem);
    expect(modalService.show).toHaveBeenCalled();
  });

  it('should generate perma link when generateLink  method of share service called', () => {
    spyOn(shareService, 'generateLink').and.returnValue(Observable.from([mockPermaLink]));
    spyOn(component, 'generateLink');
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

  it('should remove permalink when removeLink method of component called', () => {
    spyOn(shareService, 'removeLink').and.returnValue(Observable.from([true]));
    spyOn(component, 'removeLink');
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

  it('should call getPermaLink method of component', () => {
    component.shareView = { permaLink: mockPermaLink };
    component.resourceUrl = mockResourceUrl;
    let result = component.getPermaLink();
    expect(component.shareView.permaLink).not.toBeNull();
    expect(component.resourceUrl).toBe(mockResourceUrl);
    expect(result).toEqual(mockURLWithPermaLink);
  });

  it('should call externalLogin method of component when user id is not available', () => {
    spyOn(component, 'externalLogin');
    component.externalLogin();
    expect(component.externalLogin).toHaveBeenCalled();
  });

  it('should build input parameter values when buildParams method of component', () => {
    component.type = mockIstopics;  // topics
    component.id = mockId;
    component.buildParams();
    expect(component.shareInput.Url).toEqual(mockResourceTopics);
  });
  it('should build input parameter values when buildParams method of component', () => {
    component.type = mockIsGuidedAssistant; //guided assistant
    component.id = mockId;
    component.buildParams();
    expect(component.shareInput.Url).toEqual(mockGuidedAssistance);
  });

  it('should return correct url when buildUrl method of component', () => {
    component.type = mockIstopics;  // topics
    component.id = mockId;
    let buidlurl = component.buildUrl();
    expect(buidlurl).toEqual(mockResourceTopics);
  });

});
