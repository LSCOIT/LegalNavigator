import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, ModalModule } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';

import { Global } from '../../../../../global';
import { ShareView } from '../share.model';
import { ShareService } from '../share.service';
import { ShareButtonRouteComponent } from './share-button-route.component';

describe('ShareButtonRouteComponent', () => {
  let component: ShareButtonRouteComponent;
  let fixture: ComponentFixture<ShareButtonRouteComponent>;
  let modalService: BsModalService;
  let mockRouter = {
    navigate: jasmine.createSpy('navigate'),
    navigateByUrl: jasmine.createSpy('navigateByUrl')
  };
  let mockShareService;
  let mockToastr;
  let mockResponse = {
    userId: '',
    userName: '',
    resourceLink: '/topics/9c9a59cc-34ac-4a6f-80c4-90ac041abba7'
  };
  let mockProfileData: ShareView = {
    UserId: '',
    UserName: '',
    IsShared: true
  };

  beforeEach(async(() => {
    mockShareService = jasmine.createSpyObj(['getResourceLink']);
    mockToastr = jasmine.createSpyObj(['success']);
    mockShareService.getResourceLink.and.returnValue(of(mockResponse));

    TestBed.configureTestingModule({
      imports: [
        BrowserModule,
        FormsModule,
        ModalModule.forRoot(),
        HttpClientModule
      ],
      declarations: [ShareButtonRouteComponent],
      providers: [
        BsModalService,
        ShareService,
        HttpClientModule,
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: Global,
          useValue: {
            role: '',
            shareRouteUrl: ''
          }
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: '123'
              }
            }
          }
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: ShareService,
          useValue: mockShareService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    spyOn(component, 'getResourceLink');
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

    spyOn(sessionStorage, 'getItem').and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, 'setItem').and.callFake(mockSessionStorage.setItem);
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});
