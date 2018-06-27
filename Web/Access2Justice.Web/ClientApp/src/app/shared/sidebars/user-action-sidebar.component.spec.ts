import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserActionSidebarComponent } from './user-action-sidebar.component';

describe('UserActionSidebarComponent', () => {
  let component: UserActionSidebarComponent;
  let fixture: ComponentFixture<UserActionSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserActionSidebarComponent ]
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
