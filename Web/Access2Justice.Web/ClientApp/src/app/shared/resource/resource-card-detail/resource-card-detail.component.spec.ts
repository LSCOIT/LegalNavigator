import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceCardDetailComponent } from './resource-card-detail.component';

describe('ResourceCardDetailComponent', () => {
  let component: ResourceCardDetailComponent;
  let fixture: ComponentFixture<ResourceCardDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ResourceCardDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceCardDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
