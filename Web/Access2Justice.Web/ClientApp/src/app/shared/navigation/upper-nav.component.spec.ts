import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { By } from '@angular/platform-browser';
import { ModalModule } from 'ngx-bootstrap';
import { UpperNavComponent } from './upper-nav.component';
import { HttpClientModule } from '@angular/common/http';

describe('UpperNavComponent', () => {
  let component: UpperNavComponent;
  let fixture: ComponentFixture<UpperNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UpperNavComponent],
      imports: [ModalModule.forRoot(), HttpClientModule],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpperNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render four menu items', () => {
    const menuItems = fixture.debugElement.queryAll(By.css('a'));
    expect(menuItems.length).toBe(3);
  });
});
