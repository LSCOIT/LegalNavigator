import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AboutAdminComponent } from './about-admin.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AboutAdminComponent', () => {
  let component: AboutAdminComponent;
  let fixture: ComponentFixture<AboutAdminComponent>;
  let mockStaticResource;
  let mockGlobal;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [AboutAdminComponent],
      providers: [
        { provide: StaticResourceService, useValue: mockStaticResource },
        { provide: Global, useValue: mockGlobal }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
