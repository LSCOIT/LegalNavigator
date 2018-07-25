import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PersonalizedPlanComponent } from './personalized-plan.component';
import { PersonalizedPlanService } from './personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { ArrayUtilityService } from '../../shared/array-utility.service';


describe('PersonalizedPlanComponent', () => {
  let component: PersonalizedPlanComponent;
  let fixture: ComponentFixture<PersonalizedPlanComponent>;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([
          { path: 'plan /: id', component: PersonalizedPlanComponent }
        ]),
        HttpClientModule],
      declarations: [PersonalizedPlanComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        PersonalizedPlanService, ArrayUtilityService
       ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalizedPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
