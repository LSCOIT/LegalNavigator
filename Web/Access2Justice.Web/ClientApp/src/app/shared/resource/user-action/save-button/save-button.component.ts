import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, CreatePlan, StepTag, PlanTag, Steps, ProfileResources, SavedResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { environment } from '../../../../../environments/environment';
import { UpperNavService } from '../../../navigation/upper-nav.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
})
export class SaveButtonComponent implements OnInit {
  resources: Resources;
  savedResources: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  @Input() resourceId: string;
  @Input() resourceType: string;
  userId: string;
  planId: string;
  type: string;
  createPlan: CreatePlan = { planId: '', oId: '', type: '', planTags: [] };
  planDetails: any;
  updatedSteps: Array<StepTag>;
  planTags: Array<PlanTag> = [];
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  steps: Array<Steps>;
  planTag: PlanTag = { topicId: '', stepTags: this.steps };
  isSavedPlan: boolean = false;
  notifySavePlan: Subject<boolean> = new Subject<boolean>();
  modalRef: BsModalRef;

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService,
    private upperNavService: UpperNavService) {
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

  savePlanResources(template: TemplateRef<any>): void {
    this.resources = { url: '', itemId: '' };
    this.resources.url = this.router.url;
    if (this.activeRoute.snapshot.params['topic']) {
      this.resources.itemId = this.activeRoute.snapshot.params['topic'];
    } else if (this.activeRoute.snapshot.params['id']) {
      this.resources.itemId = this.activeRoute.snapshot.params['id'];
    }
    if (!this.userId) {
      this.personalizedPlanService.saveResourcesToSession(this.resources);
      this.externalLogin();
    }
    else {
      if (this.resources.url.startsWith("/plan")) {
        this.planId = this.resources.itemId;
        this.savePlanToProfile(template);
      }
      if (this.resources.url.startsWith("/subtopics") ||
        this.resources.url.startsWith("/search")) {
        this.type = "Topic";
        if (this.resourceId != undefined) {
          this.resources.itemId = this.resourceId;
          this.type = this.resourceType;
        }
        this.profileResources.resourceTags = [];
        this.savedResources = { itemId: '', resourceType: '' };
        this.personalizedPlanService.getUserPlanId(this.userId)
          .subscribe(response => {
            if (response != undefined) {
              response.forEach(property => {
                if (property.resourceTags) {
                  property.resourceTags.forEach(resource => {
                    this.profileResources.resourceTags.push(resource);
                  })
                }
              });
            }
            this.savedResources = { itemId: this.resources.itemId, resourceType: this.type };
            if (this.profileResources.resourceTags != undefined && this.profileResources.resourceTags.indexOf(this.savedResources) == -1) {
              this.profileResources.resourceTags.push(this.savedResources);
              this.saveResourceToProfile(this.profileResources.resourceTags, template);
            }
          });
      }
    }
  }

  savePlanToProfile(template) {
    this.planTags = [];
    this.personalizedPlanService.getActionPlanConditions(this.planId)
      .subscribe(planDetails => {
        planDetails.planTags.forEach(plan => {
          this.updatedSteps = [];
          plan.stepTags.forEach(step => {
            this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted }
            this.updatedSteps.push(this.updatedStep);
          })
          this.planTag = { topicId: plan.id[0].id, stepTags: this.updatedSteps };
          this.planTags.push(this.planTag);
        });
        this.createPlan = { planId: planDetails.id, oId: this.userId, type: planDetails.type, planTags: this.planTags }
        this.personalizedPlanService.userPlan(this.createPlan)
          .subscribe(() => {
            console.log("Saved Successfully");
            this.isSavedPlan = true;
            this.modalRef = this.modalService.show(template);
          });
      });
  }

  close() {
    this.modalRef.hide();
    this.router.navigate(['profile']);
  }

  saveResourceToProfile(resourceTags, template) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.userPlan(this.profileResources)
      .subscribe(() => {
        this.isSavedPlan = true;
        this.modalRef = this.modalService.show(template);
      });
  }

  ngOnInit() {
  }

}

