import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { LowerNavComponent } from './lower-nav.component';
import { SearchComponent } from '../search/search.component';

describe('LowerNavComponent', () => {
  let component: LowerNavComponent;
  let fixture: ComponentFixture<LowerNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        LowerNavComponent,
        SearchComponent
      ],
      imports: [FormsModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LowerNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
