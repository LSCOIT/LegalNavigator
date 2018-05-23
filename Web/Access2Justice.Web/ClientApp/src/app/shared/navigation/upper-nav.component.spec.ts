import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ModalModule } from 'ngx-bootstrap';
import { UpperNavComponent } from './upper-nav.component';

describe('UpperNavComponent', () => {
  let component: UpperNavComponent;
  let fixture: ComponentFixture<UpperNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UpperNavComponent],
      imports: [ModalModule.forRoot()]
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

  it('should render five menu items', () => {
    const menuItems = fixture.debugElement.queryAll(By.css('a'));
    expect(menuItems.length).toBe(5);
  });
});
