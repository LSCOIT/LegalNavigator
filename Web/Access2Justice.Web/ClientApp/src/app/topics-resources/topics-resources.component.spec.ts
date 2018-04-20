import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicsResourcesComponent } from './topics-resources.component';

describe('TopicsResourcesComponent', () => {
  let component: TopicsResourcesComponent;
  let fixture: ComponentFixture<TopicsResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopicsResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
