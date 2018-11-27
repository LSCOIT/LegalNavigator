import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminDashboardComponent } from './admin-dashboard.component';
import { Global } from '../../global';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';

describe('AdminDashboardComponent', () => {
  let component: AdminDashboardComponent;
  let fixture: ComponentFixture<AdminDashboardComponent>;
  let mockGlobal;
  let mockStaticResourceService;

  beforeEach(async(() => {
    mockGlobal = {
      roleInformation: [
        { roleName: "StateAdmin", organizationalUnit: 'Alaska' },
        { roleName: "Developer", organizationalUnit: '' }
      ]
    }
    TestBed.configureTestingModule({
      declarations: [AdminDashboardComponent],
      providers: [
        { provide: Global, useValue: mockGlobal },
        { provide: StaticResourceService, useValue: mockStaticResourceService }
      ],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA,
        NO_ERRORS_SCHEMA
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should check if user is a StateAdmin onInit', () => {
    spyOn(component, 'checkIfStateAdmin');
    component.ngOnInit();
    expect(component.checkIfStateAdmin).toHaveBeenCalledWith(mockGlobal.roleInformation);
    expect(component.isStateAdmin).toBe(true);
    expect(component.stateList).toEqual(['Alaska']);
  });
});
