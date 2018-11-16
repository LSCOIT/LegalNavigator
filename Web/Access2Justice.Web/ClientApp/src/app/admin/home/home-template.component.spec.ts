import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeTemplateComponent } from './home-template.component';

describe('HomeTemplateComponent', () => {
  let component: HomeTemplateComponent;
  let fixture: ComponentFixture<HomeTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [HomeTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
