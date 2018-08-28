import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ShareButtonRouteComponent } from './share-button-route.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ShareService } from '../share.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Global } from '../../../../../global';
import { AppComponent } from '../../../../../app.component';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap';

describe('ShareButtonRouteComponent', () => {
  let component: ShareButtonRouteComponent;
  let fixture: ComponentFixture<ShareButtonRouteComponent>;
  class MockRouter {
    navigate = jasmine.createSpy('navigate');
  }
  const mockRouter = new MockRouter();
  const fakeActivatedRoute = {
    snapshot: { data: {} }
  } as ActivatedRoute;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [BrowserModule,
        FormsModule, ModalModule.forRoot(), HttpClientModule,
      RouterModule.forRoot([
          { path: 'share /: id', component: ShareButtonRouteComponent }
      ])],
      declarations: [ ShareButtonRouteComponent],
      providers: [
        BsModalService,
        HttpClient,
        ShareService,
        Router,
        ActivatedRoute,
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: fakeActivatedRoute },
        Global],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
