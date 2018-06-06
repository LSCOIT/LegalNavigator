import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebResourceComponent } from './web-resource.component';

describe('WebResourceComponent', () => {
  let component: WebResourceComponent;
  let fixture: ComponentFixture<WebResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
