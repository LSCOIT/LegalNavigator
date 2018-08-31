import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { UserActionSidebarComponent } from './user-action-sidebar.component';
import { Global } from '../../../global';

describe('UserActionSidebarComponent', () => {
  let component: UserActionSidebarComponent;
  let fixture: ComponentFixture<UserActionSidebarComponent>;
  let mockGlobal;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserActionSidebarComponent ],
      providers: [
        { provide: Global, userValue: mockGlobal }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserActionSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
