import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BreadcrumbComponent } from './breadcrumb.component';
import { BreadCrumbService } from '../shared/breadcrumb.service';
import { Observable } from 'rxjs/Rx';
import { ActivatedRoute } from '@angular/router/src/router_state';
import { Component } from '@angular/compiler/src/core';

describe('BreadCrumb Component', () => {
  let service: BreadCrumbService;
  let breadcrumbComponent: BreadcrumbComponent;
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  let id: string = '19a02209-ca38-4b74-bd67-6ea941d41518';
  let activatedRoute: ActivatedRoute

  //Testing whether breadcrumb component is created
  beforeEach(() => {
    breadcrumbComponent = new BreadcrumbComponent(httpSpy, activatedRoute);
  });

  it('should create Breadcrumb component', () => {
    expect(breadcrumbComponent).toBeTruthy();
  });

  //Testing whether breadcrumb service is created
  beforeEach(() => {
    service = new BreadCrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should create Breadcrumb Service', () => {
    expect(service).toBeTruthy();

  });

  //Testing whether breadcrumb items are retriving via breadcrumb service
  beforeEach(() => {
    service = new BreadCrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should retrieve breadcrumbs from the API via Get', (done) => {
    let mockBreadcrumb = [
      { "id": "19a02209-ca38-4b74-bd67-6ea941d41518" }
    ];

    const mockResponse = Observable.of(mockBreadcrumb);

    httpSpy.get.and.returnValue(mockResponse);

    service.getBreadCrumbs(id).subscribe(topicid => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${service.getBreadCrumbsUrl}/` + id);
      expect(topicid).toEqual(mockBreadcrumb);
      done();
    });
  });

});


//describe('Breadcrumb Component', () => {
//  let breadCrumbService: BreadCrumbService;
//  let breadcrumbComponent: BreadcrumbComponent;
//  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
//  let activatedRoute: ActivatedRoute
//  let breadcrumbs = [];
//  let mockResponse = Observable.of(breadcrumbs)
//  let activeTopic: "testtopicguid";
//  let mainTopic: any;


//  //Verify Breadcrumb service whether created
//  beforeEach(() => {
//    breadCrumbService = new BreadCrumbService(httpSpy);
//    httpSpy.get.calls.reset();
//  });

//  it('Breadcrumb Service should create', () => {
//    expect(breadCrumbService).toBeTruthy();
//  });

//  //Verify Breadcrumb component whether created
//  beforeEach(() => {
//    breadcrumbComponent = new BreadcrumbComponent(breadCrumbService, activatedRoute); //new BreadcrumbComponent(httpSpy);
//  });

//  it('Breadcrumb Component should create', () => {
//    expect(breadcrumbComponent).toBeTruthy();
//  });


//  //Verify Breadcrumb - topic doesn't have parent topic
//  beforeEach(() => {

//    //Get the data for topic from breadcrumb service
//    breadCrumbService.getBreadCrumbs(activeTopic)
//      .subscribe(
//      items => {
//        breadcrumbs = items.response;
//      });
//  });
//  it('Breadcrumb-topic does not contain parent', () => {
//    expect(breadcrumbs.length == 1).toBe(true);
//  });

//  //Verify Breadcrumb - topic has parent topic
//  beforeEach(() => {
//    //Get the data for topic from breadcrumb service
//    breadCrumbService.getBreadCrumbs(activeTopic)
//      .subscribe(
//      items => {
//        breadcrumbs = items.response;
//      });
//  });
//  it('Breadcrumb- topic has parent topics', () => {
//    expect(breadcrumbs.length > 1).toBe(true)
//  });

//})
