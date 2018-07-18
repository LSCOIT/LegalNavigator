import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticlesResourcesComponent } from './articles-resources.component';

describe('ArticlesResourcesComponent', () => {
  let component: ArticlesResourcesComponent;
  let fixture: ComponentFixture<ArticlesResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArticlesResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticlesResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
