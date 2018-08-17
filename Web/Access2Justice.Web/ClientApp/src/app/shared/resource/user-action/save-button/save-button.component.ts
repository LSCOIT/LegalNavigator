import { Component, OnInit, Input, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, ProfileResources, SavedResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
})
export class SaveButtonComponent implements OnInit {
  @Input() showIcon = true;
  resources: Resources;
  savedResources: SavedResources;
  resourceTags: Array<SavedResources> = [];
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  @Input() id: string;
  @Input() type: string;
  @Input() resourceDetails: any = {};
  userId: string;
  planId: string;
  sessionKey: string = "bookmarkedResource";
  planSessionKey: string = "bookmarkPlanId";
  resourceStorage: any;
  planStorage: any;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private arrayUtilityService: ArrayUtilityService
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
      sessionStorage.setItem(this.planSessionKey, JSON.stringify(this.id));
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      sessionStorage.setItem(this.sessionKey, JSON.stringify(this.savedResources));
    }
  }

  savePlanResourcesPostLogin() {
    if (this.type === "Plan") {
      this.planId = this.id;
      this.savePlanToProfile();
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.personalizedPlanService.saveResourcesToProfile(this.savedResources);
    }
  }

  savePlanToProfile() {
    let params = new HttpParams()
      .set("oId", this.userId)
      .set("planId", this.planId);
    this.personalizedPlanService.savePersonalizedPlanToProfile(params)
      .subscribe(() => {
        this.personalizedPlanService.showSuccess('Plan saved to profile');
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
    }
  }

  ngOnInit() {
    this.saveBookmarkedResource();
    if (this.userId) {
      this.saveBookmarkedPlan();
    }
  }
}
