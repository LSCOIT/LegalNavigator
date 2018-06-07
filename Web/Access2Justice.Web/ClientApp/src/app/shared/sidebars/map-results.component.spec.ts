import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapResultsComponent } from './map-results.component';

describe('MapResultsComponent', () => {
  let component: MapResultsComponent;
  let fixture: ComponentFixture<MapResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapResultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
