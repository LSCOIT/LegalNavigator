import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ReadMoreComponent } from "./read-more.component";

describe("ReadMoreComponent", () => {
  let component: ReadMoreComponent;
  let fixture: ComponentFixture<ReadMoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReadMoreComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadMoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should call determineView on ngOnChanges", () => {
    spyOn(component, "determineView");
    component.text = "test";
    component.ngOnChanges();
    expect(component.determineView).toHaveBeenCalled();
  });

  it("should hide toggle show full text when text is less than maxLength", () => {
    component.text = "File this when finishing the custody case";
    component.maxLength = 200;
    component.determineView();
    expect(component.isCollapsed).toBe(false);
    expect(component.hideToggle).toBe(true);
  });

  it("should show toggle and collapsed text when text is more than maxLength", () => {
    component.text = "File this when finishing the custody case";
    component.maxLength = 10;
    component.determineView();
    expect(component.isCollapsed).toBe(true);
    expect(component.hideToggle).toBe(false);
  });
});
