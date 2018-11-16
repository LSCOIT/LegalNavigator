import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { PrivacyPromiseTemplateComponent } from './privacy-promise-template.component';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { AdminService } from '../admin.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

describe('PrivacyPromiseTemplateComponent', () => {
  let component: PrivacyPromiseTemplateComponent;
  let fixture: ComponentFixture<PrivacyPromiseTemplateComponent>;
  let mockStaticResourceService;
  let mockGlobal;
  let mockAdminService;
  let mockNgxSpinnerService;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseTemplateComponent],
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
    fixture = TestBed.createComponent(PrivacyPromiseTemplateComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


});
