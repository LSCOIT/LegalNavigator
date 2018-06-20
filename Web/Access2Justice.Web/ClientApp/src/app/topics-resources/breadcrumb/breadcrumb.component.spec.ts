import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BreadcrumbComponent } from './breadcrumb.component';
import { BreadCrumbService } from '../shared/breadcrumb.service';
import { Observable } from 'rxjs/Rx';
import { ActivatedRoute } from '@angular/router/src/router_state';
import { Component } from '@angular/compiler/src/core';
import { api } from '../../../api/api';

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
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.breadCrumbsUrl}/` + id);
      expect(topicid).toEqual(mockBreadcrumb);
      done();
    });
  });
});
