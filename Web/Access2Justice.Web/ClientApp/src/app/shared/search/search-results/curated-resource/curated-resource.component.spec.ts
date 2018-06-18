import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CuratedResourceComponent } from './curated-resource.component';
import { CuratedResourceService } from './curated-resource.service';

describe('CuratedResourceComponent', () => {
  let component: CuratedResourceComponent;
  let fixture: ComponentFixture<CuratedResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CuratedResourceComponent],
      providers: [
        CuratedResourceService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
