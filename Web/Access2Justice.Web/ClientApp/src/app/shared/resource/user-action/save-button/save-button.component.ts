import { Component, OnInit, Input, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, ProfileResources, SavedResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';
import { HttpParams } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
})
export class SaveButtonComponent implements OnInit {
  @Input() showIcon = true;
  resources: Resources;
  savedResources: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  @Input() id: string;
  @Input() type: string;
  @Input() resourceDetails: any = {};
  userId: string;
  planId: string;
  planDetails: any;
  isSavedPlan: boolean = false;
  modalRef: BsModalRef;
  @ViewChild('template') public templateref: TemplateRef<any>;
  sessionKey: string = "bookmarkedResource";
  planSessionKey: string= "bookmarkPlanId";
  resourceStorage: any;
  planStorage: any;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService,
    private arrayUtilityService: ArrayUtilityService,
    private toastr: ToastrService
  ) {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
  }

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  savePlanResources(): void {
    if (!this.userId) {
      this.savePlanResourcesPreLogin();
      this.externalLogin();
    } else {
      this.savePlanResourcesPostLogin();
    }
  }

  savePlanResourcesPreLogin() {
    if (this.type === "Plan") {
      this.personalizedPlanService.savePlanToSession(this.id);
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.personalizedPlanService.saveResourcesToSession(this.savedResources);
    }
  }

  savePlanResourcesPostLogin() {
    if (this.type === "Plan") {
      this.planId = this.id;
      this.savePlanToProfile();
    } else {
      this.saveResourcesToProfile();
    }
  }
  
  savePlanToProfile() {
    let params = new HttpParams()
      .set("oId", this.userId)
      .set("planId", this.planId);
    this.personalizedPlanService.savePersonalizedPlanToProfile(params)
      .subscribe(() => {
        this.isSavedPlan = true;
        this.showSuccess('Plan saved to profile');
      });
  }

  showSuccess(message) {
    this.toastr.success(message);
  }

  saveResourcesToProfile() {
    this.profileResources.resourceTags = [];
    this.savedResources = { itemId: '', resourceType: '', resourceDetails: {} };
    this.personalizedPlanService.getUserSavedResources(this.userId)
      .subscribe(response => {
        if (response != undefined) {
          response.forEach(property => {
            if (property.resourceTags) {
              property.resourceTags.forEach(resource => {
                this.profileResources.resourceTags.push(resource);
              });
            }
          });
        }
        this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
        if (!this.arrayUtilityService.checkObjectExistInArray(this.profileResources.resourceTags, this.savedResources)) {
          this.profileResources.resourceTags.push(this.savedResources);
          this.saveResourceToProfile(this.profileResources.resourceTags);
        }
      });
  }

  saveResourceToProfile(resourceTags) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.saveResources(this.profileResources)
      .subscribe(() => {
        this.isSavedPlan = true;
        this.showSuccess('Resource saved to profile');
      });
  }
  
  saveBookmarkedPlan() {
    this.planStorage = sessionStorage.getItem(this.planSessionKey);
    if (this.planStorage) {
      this.savePlanResources();
      sessionStorage.removeItem(this.planSessionKey);
    }
  }

  saveBookmarkedResource() {
    this.resourceStorage = sessionStorage.getItem(this.sessionKey);
    if (this.resourceStorage && this.resourceStorage.length > 0) {
      this.resourceStorage = JSON.parse(this.resourceStorage);
      this.id = this.resourceStorage[0].itemId;
      this.type = this.resourceStorage[0].resourceType;
      this.resourceDetails = this.resourceStorage[0].resourceDetails;
      this.savePlanResources();
      sessionStorage.removeItem(this.sessionKey);
    }
  }

  ngOnInit() {
    this.saveBookmarkedResource();
    if (this.userId) {
      this.saveBookmarkedPlan();
    }
  }
}
