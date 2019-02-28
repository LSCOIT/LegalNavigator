import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedResourcesComponent } from './saved-resources.component';

describe('SavedResourcesComponent', () => {
  let component: SavedResourcesComponent;
  let fixture: ComponentFixture<SavedResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SavedResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
