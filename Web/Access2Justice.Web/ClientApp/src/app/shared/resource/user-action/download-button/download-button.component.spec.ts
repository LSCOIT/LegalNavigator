import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { DownloadButtonComponent } from "./download-button.component";

describe("DownloadButtonComponent", () => {
  let component: DownloadButtonComponent;
  let fixture: ComponentFixture<DownloadButtonComponent>;
  let mockApplicationUrl = "testUrl";
  let mockTitle = "Download";
  let mockappTemplateName = "personalized-plan";

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DownloadButtonComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should create new component", () => {
    expect(new DownloadButtonComponent()).toBeTruthy();
  });

  it("should accept title and display", () => {
    component.applicationUrl = mockApplicationUrl;
    component.title = mockTitle;
    fixture.detectChanges();
    expect(component.title).toBe(mockTitle);
  });

  it("should accept template and display", () => {
    component.applicationUrl = mockApplicationUrl;
    component.template = mockappTemplateName;
    fixture.detectChanges();
    expect(component.template).toBe(mockappTemplateName);
  });
});
