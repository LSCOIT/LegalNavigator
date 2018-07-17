import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DidYouKnowComponent } from './did-you-know.component';

describe('DidYouKnowComponent', () => {
  let component: DidYouKnowComponent;
  let fixture: ComponentFixture<DidYouKnowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DidYouKnowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DidYouKnowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
