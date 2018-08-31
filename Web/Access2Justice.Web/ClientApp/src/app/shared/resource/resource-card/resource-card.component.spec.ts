import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ResourceCardComponent } from './resource-card.component';
import { SaveButtonComponent } from '../user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../user-action/share-button/share-button.component';
import { ResourceCardDetailComponent } from '../resource-card-detail/resource-card-detail.component';
import { Global } from '../../../global';

describe('ResourceCardComponent', () => {
  let component: ResourceCardComponent;
  let fixture: ComponentFixture<ResourceCardComponent>;
  let mockGlobal;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ResourceCardComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        ResourceCardDetailComponent
      ],
      imports: [],
      providers: [
        { provide: Global, useValue: mockGlobal }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
