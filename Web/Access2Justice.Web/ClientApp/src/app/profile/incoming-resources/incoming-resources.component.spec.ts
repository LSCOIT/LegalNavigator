import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingResourcesComponent } from './incoming-resources.component';

describe('IncomingResourcesComponent', () => {
  let component: IncomingResourcesComponent;
  let fixture: ComponentFixture<IncomingResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
