import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadButtonComponent } from './download-button.component';
import { inject } from '@angular/core/src/render3';
import { Window } from 'selenium-webdriver';
import { elementClass } from '@angular/core/src/render3/instructions';
import { DebugElement } from '@angular/core';
import { HttpModule } from '@angular/http';
import { By } from '@angular/platform-browser';

describe('DownloadButtonComponent', () => {
  let component: DownloadButtonComponent;
  let fixture: ComponentFixture<DownloadButtonComponent>;
  let mockDownloadButtonComponent: DownloadButtonComponent[]

  let mockApplicationUrl = 'testUrl';
  let mockTitle = 'Download';
  let mockContent = 'TestContent';
  let mockappTemplateName = 'personalized-plan';

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DownloadButtonComponent],
      imports: [HttpModule],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create new component', () => {
    expect(new DownloadButtonComponent()).toBeTruthy();
  });

  it('should accept title and display', () => {
    component.applicationUrl = mockApplicationUrl;
    component.title = mockTitle;
    fixture.detectChanges();
    expect(component.title).toBe(mockTitle);
  });

  it('should accept template and display', () => {
    component.applicationUrl = mockApplicationUrl;
    component.template = mockappTemplateName;
    fixture.detectChanges();
    expect(component.template).toBe(mockappTemplateName);

  });

});
