import { ArticlesResourcesComponent } from './articles-resources.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NavigateDataService } from '../../shared/navigate-data.service';

describe('ArticlesResourcesComponent', () => {
  let component: ArticlesResourcesComponent;
  let fixture: ComponentFixture<ArticlesResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArticlesResourcesComponent ],
      providers: [ NavigateDataService ]
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
