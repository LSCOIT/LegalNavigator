import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminComponent } from './admin.component';
import { Global } from '../../global';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';

describe('AdminComponent', () => {
  let component: AdminComponent;
  let fixture: ComponentFixture<AdminComponent>;
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
      declarations: [AdminComponent],
      providers: [
        { provide: Global, useValue: mockGlobal },
        { provide: StaticResourceService, useValue: mockStaticResourceService }
      ],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminComponent);
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
