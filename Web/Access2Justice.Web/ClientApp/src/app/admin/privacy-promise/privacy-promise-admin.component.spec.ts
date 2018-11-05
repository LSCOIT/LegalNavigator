import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { PrivacyPromiseAdminComponent } from './privacy-promise-admin.component';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { AdminService } from '../admin.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

describe('PrivacyPromiseComponent', () => {
  let component: PrivacyPromiseAdminComponent;
  let fixture: ComponentFixture<PrivacyPromiseAdminComponent>;
  let mockStaticResourceService;
  let mockGlobal;
  let mockAdminService;
  let mockNgxSpinnerService;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseAdminComponent],
      imports: [FormsModule],
      providers: [
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal },
        { provide: AdminService, useValue: mockAdminService },
        { provide: NgxSpinnerService, useValue: mockNgxSpinnerService },
        { provide: Router, useValue: mockRouter }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyPromiseAdminComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


});
