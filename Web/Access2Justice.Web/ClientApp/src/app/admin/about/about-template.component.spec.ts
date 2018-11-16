import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AboutTemplateComponent } from './about-template.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AboutAdminComponent', () => {
  let component: AboutTemplateComponent;
  let fixture: ComponentFixture<AboutTemplateComponent>;
  let mockStaticResource;
  let mockGlobal;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [AboutTemplateComponent],
      providers: [
        { provide: StaticResourceService, useValue: mockStaticResource },
        { provide: Global, useValue: mockGlobal }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutTemplateComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
