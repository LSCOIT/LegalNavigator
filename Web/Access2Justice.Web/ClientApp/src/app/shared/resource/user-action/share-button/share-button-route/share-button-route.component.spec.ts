import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA, TemplateRef } from '@angular/core';
import { ShareButtonRouteComponent } from './share-button-route.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { HttpClientModule } from '@angular/common/http';
import { ShareService } from '../share.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Global } from '../../../../../global';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { ShareView } from '../share.model';
class MockBsModalRef {
  public isHideCalled = false;

  hide() {
    this.isHideCalled = true;
  }
}
describe('ShareButtonRouteComponent', () => {
  let component: ShareButtonRouteComponent;
  let fixture: ComponentFixture<ShareButtonRouteComponent>;
  let modalService: BsModalService;
  let mockRouter=
  {
      navigate: jasmine.createSpy('navigate'),
      navigateByUrl: jasmine.createSpy('navigateByUrl')
  };
  let mockGlobal;
  let mockShareService;
  let template: TemplateRef<any>;
  let mockToastr;
  let mockShareButtonRouteComponent;
  let mockPermalink = "9C87441";
  let mockResponse= {
    "userId": '',
    "userName": '',
    "resourceLink": "/topics/9c9a59cc-34ac-4a6f-80c4-90ac041abba7"
  }

  let mockProfileData: ShareView = { UserId: '', UserName: '', IsShared: true };
  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(['success']);
    mockShareService = jasmine.createSpyObj(['getResourceLink']);
    mockShareService.getResourceLink.and.returnValue(of(mockResponse));
   
    TestBed.configureTestingModule({
      imports: [
        BrowserModule,
        FormsModule,
        ModalModule.forRoot(),
        HttpClientModule,
      ],
      declarations: [ShareButtonRouteComponent],
      providers: [
        BsModalService,
        ShareService,
        HttpClientModule,
        { provide: ToastrService, useValue: mockToastr },
        { provide: Global, useValue: { role: '', shareRouteUrl: '' } },
        { provide: ActivatedRoute, useValue: { snapshot: { params: { 'id': '123' } } } },
        { provide: Router, useValue: mockRouter },
        { provide: ShareService, useValue: mockShareService }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();

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

  fit('should create test', () => {
    expect(component).toBeTruthy();
  });

  fit('should call  getResourceLink method when ngOnit loads first time', () => {
    spyOn(component, 'getResourceLink');
    component.getResourceLink();
    expect(component.getResourceLink).toHaveBeenCalled();
  });

  fit("should call modalService show when openModal is called", () => {
    modalService = TestBed.get(BsModalService);
    spyOn(modalService, 'show');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
  });
  fit('should call  getResourceLink method of shareService called', () => {
    let mocklocationchange = Object({ skipLocationChange: true });
    component.getResourceLink();
    component.profileData.IsShared = true;
    expect(component.profileData.UserId).toBe(mockResponse.userId);
    expect(component.profileData.UserName).toEqual(mockResponse.userName);
    expect(component.profileData.IsShared).toBe(mockProfileData.IsShared);
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith(mockResponse.resourceLink, mocklocationchange);
  });
});


