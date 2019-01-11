import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Router } from "@angular/router";
import { BsDropdownModule } from "ngx-bootstrap";
import { BsModalService } from "ngx-bootstrap/modal";
import { Global } from "../../../global";
import { StateCodeService } from "../../../shared/services/state-code.service";
import { PipeModule } from "../../pipe/pipe.module";
import { ResourceCardDetailComponent } from "../resource-card-detail/resource-card-detail.component";
import { SaveButtonComponent } from "../user-action/save-button/save-button.component";
import { ShareButtonComponent } from "../user-action/share-button/share-button.component";
import { ResourceCardComponent } from "./resource-card.component";

describe("ResourceCardComponent", () => {
  let component: ResourceCardComponent;
  let fixture: ComponentFixture<ResourceCardComponent>;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ResourceCardComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        ResourceCardDetailComponent
      ],
      imports: [
        PipeModule.forRoot(),
        BsDropdownModule.forRoot(),
        HttpClientModule
      ],
      providers: [
        BsModalService,
        StateCodeService,
        {
          provide: Global,
          useValue: {
            role: "",
            shareRouteUrl: ""
          }
        },
        {
          provide: ResourceCardComponent,
          useValue: {
            id: "",
            resources: [
              {
                itemId: "",
                resourceType: "",
                resourceDetails: {}
              }
            ]
          }
        },
        {
          provide: Router,
          useValue: mockRouter
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(ResourceCardComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  }));

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
