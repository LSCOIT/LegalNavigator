import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LocationService } from '../location/location.service'
import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';
import { ServiceOrgService } from './service-org.service'
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
describe('ServiceOrgSidebarComponent', () => {
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  let locationService: LocationService;
  let serviceorgservice: ServiceOrgService;
  beforeEach(async(() => {
    TestBed.configureTestingModule
      ({
        declarations: [ServiceOrgSidebarComponent],
        providers: [
          ServiceOrgService,
          LocationService
        ], imports: [HttpClientModule]
      })
    TestBed.compileComponents();
    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    locationService = TestBed.get(LocationService);
    fixture.detectChanges();
  }));
  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

});
