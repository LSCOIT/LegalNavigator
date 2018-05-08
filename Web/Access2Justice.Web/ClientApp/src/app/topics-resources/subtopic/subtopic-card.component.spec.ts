import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubtopicCardComponent } from './subtopic-card.component';

describe('SubtopicCardComponent', () => {
  let component: SubtopicCardComponent;
  let fixture: ComponentFixture<SubtopicCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubtopicCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
