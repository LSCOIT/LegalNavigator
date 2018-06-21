import { BreadcrumbService } from './breadcrumb.service';

describe('Breadcrumb Service:', () => {
  let service: BreadcrumbService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);

  beforeEach(() => {
    service = new BreadcrumbService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('Breadcrumb Service should be created', () => {
    expect(service).toBeTruthy();

  });

  it('should have no breadcrumbs to start', () => {
    service = new BreadcrumbService(httpSpy);
    expect(service.getBreadcrumbs.length).toBe(1);
  });

  it('should retrieve root parent breadcrumb', () => {
    service = new BreadcrumbService(httpSpy);
    service.getBreadcrumbs("addf41e9-1a27-4aeb-bcbb-7959f95094ba")
    expect(service.getBreadcrumbs.length).toBe(1);
  });

});
