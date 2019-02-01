import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { api } from "../../../api/api";
import { BreadcrumbService } from "../shared/breadcrumb.service";
import { BreadcrumbComponent } from "./breadcrumb.component";

describe("Breadcrumb Component", () => {
  let service: BreadcrumbService;
  let breadcrumbComponent: BreadcrumbComponent;
  const httpSpy = jasmine.createSpyObj("http", ["get"]);
  const id = "19a02209-ca38-4b74-bd67-6ea941d41518";
  let activatedRoute: ActivatedRoute;

  beforeEach(() => {
    breadcrumbComponent = new BreadcrumbComponent(httpSpy, activatedRoute);
  });

  it("should create Breadcrumb component", () => {
    expect(breadcrumbComponent).toBeTruthy();
  });

  beforeEach(() => {
    service = new BreadcrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it("should create Breadcrumb Service", () => {
    expect(service).toBeTruthy();
  });

  beforeEach(() => {
    service = new BreadcrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it("should retrieve breadcrumbs from the API via Get", done => {
    let mockBreadcrumb = [{ id: "19a02209-ca38-4b74-bd67-6ea941d41518" }];
    const mockResponse = Observable.of(mockBreadcrumb);
    httpSpy.get.and.returnValue(mockResponse);
    service.getBreadcrumbs(id).subscribe(topicid => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.breadcrumbsUrl}/` + id);
      expect(topicid).toEqual(mockBreadcrumb);
      done();
    });
  });
});
