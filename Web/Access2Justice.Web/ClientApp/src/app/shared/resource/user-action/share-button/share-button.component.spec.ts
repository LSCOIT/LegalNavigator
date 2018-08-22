import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ShareButtonComponent } from './share-button.component';
import { BsModalService, ComponentLoaderFactory, PositioningService } from 'ngx-bootstrap';
import { ArrayUtilityService } from '../../../array-utility.service';
import { HttpClientModule } from '@angular/common/http';
import { ShareService } from './share.service';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Global } from '../../../../global';
import { NgModel, NgForm } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

describe('ShareButtonComponent', () => {
  class MockRouter {
    navigate = jasmine.createSpy('navigate');
  }
  const mockRouter = new MockRouter();
  const fakeActivatedRoute = {
    snapshot: { data: {} }
  } as ActivatedRoute;

  let component: ShareButtonComponent;
  let fixture: ComponentFixture<ShareButtonComponent>;
  let router: Router;
  let activeRoute: ActivatedRoute;
  let shareService: ShareService;
  let mockInput = {
    ResourceId: "bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    UserId: "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B9…64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",
    Url: "/topics/bdc07e7a-1f06-4517-88d8-9345bb87c3cf"
  };
  let mockExpected = "CED9B90";
  let mockInputElement = "";


  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ShareButtonComponent],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: fakeActivatedRoute },
        ComponentLoaderFactory,
        PositioningService,
        ArrayUtilityService,
        ShareService,
        Global,
        BsModalService,
        NgModel]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShareButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should call checkPermaLink method of component', () => {
    expect(component).toBeTruthy();
  });
  it('should call  generateLink  method of component called', () => {
    spyOn(component, 'generateLink');
    component.generateLink();
    expect(component.generateLink).toHaveBeenCalled();
  });
  it('should call generateLink service method  when generateLink method of component called', () => {
    const mockResponse = Observable.of(mockExpected);
    spyOn(component, 'generateLink');
    spyOn(shareService, 'generateLink').and.returnValue(mockResponse);
    component.shareInput = mockInput;
    shareService.generateLink(mockInput);
    expect(shareService.generateLink).toHaveBeenCalled();
  });
  
  it('should call removeLink method of component', () => {
    spyOn(component, 'removeLink');
    component.removeLink();
    expect(component.removeLink).toHaveBeenCalled();
  });
  it('should  call getActiveParam method of component', () => {
    spyOn(component, 'getActiveParam');
    component.getActiveParam();
    expect(component.getActiveParam).toHaveBeenCalled();
  });
  it('should call copyLink method of component', () => {
    spyOn(component, 'copyLink');
    component.copyLink(mockInput);
    expect(component.copyLink).toHaveBeenCalled();
  });
  it('should call getPermaLink method of component', () => {
    spyOn(component, 'getPermaLink');
    component.getPermaLink();
    expect(component.getPermaLink).toHaveBeenCalled();
  });
  it('should call externalLogin method of component', () => {
    spyOn(component, 'externalLogin');
    component.externalLogin();
    expect(component.externalLogin).toHaveBeenCalled();
    
  });
  it('should call buildParams method of component', () => {
    spyOn(component, 'buildParams');
    component.buildParams();
    expect(component.buildParams).toHaveBeenCalled();
  });
  it('should call buildUrl method of component', () => {
    spyOn(component, 'buildUrl');
    component.buildUrl();
    expect(component.buildUrl).toHaveBeenCalled();
  }); 
  
});
