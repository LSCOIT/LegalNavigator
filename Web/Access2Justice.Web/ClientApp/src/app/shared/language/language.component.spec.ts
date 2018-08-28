import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguageComponent } from './language.component';
import { StaticResourceService } from '../static-resource.service';
import { HttpClientModule } from '@angular/common/http';


describe('LanguageComponent', () => {
  let component: LanguageComponent;
  let fixture: ComponentFixture<LanguageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [LanguageComponent],
      providers: [StaticResourceService]
      })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LanguageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
