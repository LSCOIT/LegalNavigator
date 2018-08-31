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
import { RemoveButtonComponent } from './remove-button.component';
import { ToastrService } from 'ngx-toastr';

describe('RemoveButtonComponent', () => {
  let component: RemoveButtonComponent;
  let fixture: ComponentFixture<RemoveButtonComponent>;
  let mockToastr;
  let mockGlobal;
  let mockRouter;

  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(['success']);
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      declarations: [RemoveButtonComponent],
      providers: [
        PersonalizedPlanService, 
        ArrayUtilityService, 
        ProfileComponent, 
        EventUtilityService, 
        BsModalService,
        PersonalizedPlanComponent,
        { provide: ToastrService, useValue: mockToastr },
        { provide: Global, useValue: mockGlobal },
        { provide: ActivatedRoute,
          useValue: {snapshot: {params: {'id': '123'}}}
        },
        { provide: Router, useValue: mockRouter }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });
});
