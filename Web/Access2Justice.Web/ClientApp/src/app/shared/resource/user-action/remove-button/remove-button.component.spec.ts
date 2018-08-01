import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RemoveButtonComponent } from './remove-button.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule, HttpClient, HttpHandler } from '@angular/common/http';
import { ArrayUtilityService } from '../../../array-utility.service';
import { ProfileComponent } from '../../../../profile/profile.component';
import { EventUtilityService } from '../../../event-utility.service';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ModalModule } from 'ngx-bootstrap';

describe('RemoveButtonComponent', () => {
  let component: RemoveButtonComponent;
  let fixture: ComponentFixture<RemoveButtonComponent>;

  //const fakeActivatedRoute = {
  //  snapshot: { data: { ... } }
  //} as ActivatedRoute;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, RouterTestingModule, ModalModule.forRoot()],
      declarations: [RemoveButtonComponent],
      providers: [PersonalizedPlanService, HttpClientModule, HttpClient, HttpHandler, ArrayUtilityService, ProfileComponent, EventUtilityService, PersonalizedPlanComponent, ActivatedRoute]
      //, useValue: fakeActivatedRoute 
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
