import { BreadCrumbService } from './breadcrumb.service';

describe('BreadCrumb Service:', () => {
  let service: BreadCrumbService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);

  beforeEach(() => {
    service = new BreadCrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('Breadcrumb Service should be created', () => {
    expect(service).toBeTruthy();

  });

  it('should have no breadcrumbs to start', () => {
    service = new BreadCrumbService(httpSpy);
    expect(service.getBreadCrumbs.length).toBe(1);
  });

  it('should retrieve root parent breadcrumb', () => {
    service = new BreadCrumbService(httpSpy);
    service.getBreadCrumbs("addf41e9-1a27-4aeb-bcbb-7959f95094ba")
    expect(service.getBreadCrumbs.length).toBe(1);
  });

});
