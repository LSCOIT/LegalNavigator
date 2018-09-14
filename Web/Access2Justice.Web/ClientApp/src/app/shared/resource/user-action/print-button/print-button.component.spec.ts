import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PrintButtonComponent } from './print-button.component';
import { ActivatedRoute, Params, Router, Data } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { APP_BASE_HREF } from '@angular/common';
import { of } from 'rxjs/observable/of';

fdescribe('PrintButtonComponent', () => {
  let component: PrintButtonComponent;
  let fixture: ComponentFixture<PrintButtonComponent>;
  let activatedRoute: ActivatedRoute;
  let routeSpy;

  const routes = [
    { path: 'test/:id', component: PrintButtonComponent }
  ];

  let mockAR: any = {
    url: {
      _value: [{
        path: "subtopics", params: {
          subscribe: function () {
            Observable.of({ id: 'subtopics' });
          }
        }
      }, {
        path: "1d7fc811-dabe-4468-a179-c55075bd22b6", params: {
          subscribe: function () {
            Observable.of({ id: 'subtopics' });
          }
        }
      }, { snapshot: { url: [{ path: 'subtopics' }] } },
      ]
    },

  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrintButtonComponent],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: { 'topic': 'bd900039-2236-8c2c-8702-d31855c56b0f' }
            },
            url: of([
              { path: 'subtopics', params: {} },
              { path: 'bd900039-2236-8c2c-8702-d31855c56b0f', params: {} }
            ])
          }
        },
      ]
    })
      .compileComponents();
  })); 

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should be defined', () => {
    expect(component).toBeDefined();
  });

  it('should create new component', () => {
    expect(new PrintButtonComponent(activatedRoute)).toBeTruthy();
  });

  it('should display the template name for subtopic: app-subtopic-detail', () => {
    component.activeRouteName = 'subtopics';
    component.template = 'app-subtopic-detail';
    fixture.detectChanges();
    expect(component.template).toEqual('app-subtopic-detail');
  });

  it('should display the template name for resource: app-resource-card-detail', () => {
    component.activeRouteName = 'resource';
    component.template = 'app-resource-card-detail';
    fixture.detectChanges();
    expect(component.template).toEqual('app-resource-card-detail');
  });

  it('should display the template name for profile: app-action-plans', () => {
    component.activeRouteName = 'profile';
    component.activeTab = 'My Plan';
    component.template = 'app-action-plans';
    fixture.detectChanges();
    expect(component.activeTab).toEqual('My Plan');
    expect(component.template).toEqual('app-action-plans');
  });

  it('should display the template name for profile: app-search-results', () => {
    component.activeRouteName = 'profile';
    component.activeTab = 'My Saved Resources';
    component.template = 'app-search-results'
    fixture.detectChanges();
    expect(component.activeTab).toEqual('My Saved Resources');
    expect(component.template).toEqual('app-search-results');
  });

});
