import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ResourceCardComponent } from './resource-card.component';
import { SaveButtonComponent } from '../user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../user-action/share-button/share-button.component';
import { ResourceCardDetailComponent } from '../resource-card-detail/resource-card-detail.component';

describe('ResourceCardComponent', () => {
  let component: ResourceCardComponent;
  let fixture: ComponentFixture<ResourceCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ResourceCardComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        ResourceCardDetailComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'resource/:id', component: ResourceCardDetailComponent }
        ])
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' }
      ]
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
