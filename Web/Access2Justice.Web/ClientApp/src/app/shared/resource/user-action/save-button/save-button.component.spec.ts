import { ActivatedRoute, Router } from '@angular/router';
import { ArrayUtilityService } from '../../../array-utility.service';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BsModalService } from 'ngx-bootstrap';
import { EventUtilityService } from '../../../event-utility.service';
import { Global } from '../../../../global';
import { HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ProfileComponent } from '../../../../profile/profile.component';
import { SaveButtonComponent } from './save-button.component';
import { ToastrService } from 'ngx-toastr';

describe('SaveButtonComponent', () => {
  let component: SaveButtonComponent;
  let fixture: ComponentFixture<SaveButtonComponent>;
  let mockToastr;
  let mockGlobal;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [SaveButtonComponent],
      providers: [
        PersonalizedPlanService, 
        ArrayUtilityService, 
        ProfileComponent, 
        EventUtilityService, 
        PersonalizedPlanComponent, 
        BsModalService,
        { provide: ToastrService, useValue: mockToastr },
        { provide: Global, useValue: mockGlobal },
        { provide: ActivatedRoute,
          useValue: {snapshot: {params: {'id': '123'}}}
        },
        { provide: Router, useValue: mockRouter }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaveButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
