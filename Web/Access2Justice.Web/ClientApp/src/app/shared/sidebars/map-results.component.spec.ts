import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MapResultsComponent } from './map-results.component';
import { MapResultsService } from './map-results.service';
import { DebugElement } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

describe('MapResultsComponent', () => {
  let component: MapResultsComponent;
  let fixture: ComponentFixture<MapResultsComponent>;
  let service: MapResultsService;
  let element: DebugElement;

  beforeEach(
    () => {
      TestBed.configureTestingModule({
        imports: [HttpClientModule],
        declarations: [MapResultsComponent],
        providers: [MapResultsService]
      });
      TestBed.compileComponents();
      fixture = TestBed.createComponent(MapResultsComponent);
      component = fixture.componentInstance;
      service = TestBed.get(MapResultsService);
    });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });
});
