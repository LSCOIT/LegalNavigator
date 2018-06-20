import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { LowerNavComponent } from './lower-nav.component';
import { NavigateDataService } from '../navigate-data.service';
import { SearchComponent } from '../search/search.component';
import { SearchService } from '../search/search.service';

describe('LowerNavComponent', () => {
  let component: LowerNavComponent;
  let fixture: ComponentFixture<LowerNavComponent>;
  let router: RouterModule;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        LowerNavComponent, 
        SearchComponent
      ],
      imports: [
        RouterModule.forRoot([ ]),
        HttpClientModule, FormsModule],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        SearchService, 
        NavigateDataService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LowerNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
