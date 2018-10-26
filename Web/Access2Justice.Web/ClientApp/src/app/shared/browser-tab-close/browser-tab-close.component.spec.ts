import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowserTabCloseComponent } from './browser-tab-close.component';

describe('BrowserTabCloseComponent', () => {
  let component: BrowserTabCloseComponent;
  let fixture: ComponentFixture<BrowserTabCloseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrowserTabCloseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowserTabCloseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
