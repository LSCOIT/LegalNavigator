import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubtopicDetailComponent } from './subtopic-detail.component';

describe('TopicComponent', () => {
  let component: SubtopicDetailComponent;
  let fixture: ComponentFixture<SubtopicDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SubtopicDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
