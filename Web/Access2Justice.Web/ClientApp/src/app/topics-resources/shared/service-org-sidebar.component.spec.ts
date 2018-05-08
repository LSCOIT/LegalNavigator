import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';

describe('ServiceOrgSidebarComponent', () => {
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServiceOrgSidebarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
