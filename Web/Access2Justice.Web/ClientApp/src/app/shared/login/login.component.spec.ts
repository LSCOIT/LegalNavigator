import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Global } from '../../global';
import { LoginComponent } from './login.component';
import { Router } from '@angular/router';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockRouter;
  let mockGlobal;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginComponent ],
      providers: [
          { provide: Router, useValue: mockRouter },
          { provide: Global, useValue: mockGlobal }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
