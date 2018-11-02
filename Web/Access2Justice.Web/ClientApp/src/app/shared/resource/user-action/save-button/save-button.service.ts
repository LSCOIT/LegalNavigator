import { Injectable } from '@angular/core';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpParams } from '@angular/common/http';
import { PlanStep, PlanTopic, PersonalizedPlan } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { Global } from '../../../../global';
import { ArrayUtilityService } from '../../../array-utility.service';

@Injectable()
export class SaveButtonService {
  personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] };
  personalizedPlanSteps: Array<PlanStep>;
  planTopic: PlanTopic = { topicId: '', steps: this.personalizedPlanSteps };
  planTopics: Array<PlanTopic>;
  planStepCount: number = 0;
  planId: string;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  planTopicIds: Array<string>;
  topicIds: Array<string>;
  stepIds: Array<string>;
  planStepIds: Array<string>;

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private arrayUtilityService: ArrayUtilityService) { }

  getPlan(planId) {
    this.planId = planId;
    let params = new HttpParams()
      .set("personalizedPlanId", planId);
    this.personalizedPlanService.getActionPlanConditions(params)
      .subscribe((plan) => {
        this.planTopics = [];
        this.personalizedPlanSteps = [];
        this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
        plan.topics.forEach(item => {
          this.personalizedPlanSteps = this.getPersonalizedPlanSteps(item.steps);
          this.planTopic = { topicId: item.topicId, steps: this.personalizedPlanSteps };
          this.planTopics.push(this.planTopic);
        });
        this.savePlanToUserProfile(this.planTopics);
      });
  }

  getPersonalizedPlanSteps(steps): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: this.personalizedPlanService.getResourceIds(step.resources), topicIds: []
      };
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    return this.personalizedPlanSteps;
  }

  savePlanToUserProfile(plan) {
    this.planStepCount = 0;
    let params = new HttpParams()
      .set("oid", this.global.userId)
      .set("type", "plan");
    this.personalizedPlanService.getUserSavedResources(params)
      .subscribe(response => {
        if (response && response.length > 0) {
          this.BuildUserPlan(response, plan);
          this.addRemainingTopicsToPlan(plan);
          this.planId = response[0].id;
        }
        this.personalizedPlan = { id: plan.id, topics: plan.topics, isShared: plan.isShared };
        const params = {
          "id": this.personalizedPlan.id,
          "topics": this.personalizedPlan.topics,
          "isShared": this.personalizedPlan.isShared
        }
        this.personalizedPlanService.userPlan(params)
          .subscribe(response => {
            if (response) {
              if (this.planStepCount > 0) {
                this.personalizedPlanService.showSuccess('Plan saved to profile');
              } else {
                this.personalizedPlanService.showWarning('Plan already saved to profile');
              }
            }
          });
      });
  }

  BuildUserPlan(response, plan) {
    response.forEach(property => {
      if (property.topics) {
        this.planTopics = [];
        this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
        this.planTopicIds = this.getTopicIds(plan);
        this.BuildUserPlanWithExistingTopics(property.topics, plan);
        this.topicIds = this.getTopicIds(property.topics);
        this.planTopicIds.forEach(topicId => {
          if (!this.arrayUtilityService.checkObjectExistInArray(this.topicIds, topicId)) {
            this.planStepCount++;
          }
        });
      }
    });
  }

  BuildUserPlanWithExistingTopics(topics, plan) {
    topics.forEach(topic => {
      plan.forEach(planTopic => {
        this.personalizedPlanSteps = [];
        this.stepIds = [];
        this.BuildPlanStepsForExistingTopicId(planTopic, topic);
      });
      if (this.arrayUtilityService.checkObjectExistInArray(this.planTopicIds, topic.topicId)) {
        this.topicTagsForMatchingTopicId(topic);
      } else {
        this.topicTagsForNonMatchingTopicId(topic);
      }
      this.planTopic = { topicId: topic.topicId, steps: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
  }

  BuildPlanStepsForExistingTopicId(planTopic, topic) {
    if (planTopic.topicId === topic.topicId) {
      planTopic.steps.forEach(step => {
        this.personalizedPlanStep = {
          stepId: step.stepId, title: step.title,
          description: step.description, order: step.order, isComplete: step.isComplete,
          resources: step.resources, topicIds: []
        };
        this.stepIds.push(step.stepId);
        this.personalizedPlanSteps.push(this.personalizedPlanStep);
      });
    }
  }

  topicTagsForMatchingTopicId(topic) {
    this.planStepIds = [];
    this.planStepCount = 0;
    topic.steps.forEach(step => {
      this.planStepIds.push(step.stepId);
      if (!this.arrayUtilityService.checkObjectExistInArray(this.stepIds, step.stepId)) {
        this.buildPlanSteps(step);
      }
    });
    this.stepIds.forEach(stepId => {
      if (!this.arrayUtilityService.checkObjectExistInArray(this.planStepIds, stepId)) {
        this.planStepCount++;
      }
    });
  }

  topicTagsForNonMatchingTopicId(topic) {
    topic.steps.forEach(step => {
      this.buildPlanSteps(step);
    });
  }

  buildPlanSteps(step) {
    this.personalizedPlanStep = {
      stepId: step.stepId, title: step.title,
      description: step.description, order: step.order, isComplete: step.isComplete,
      resources: step.resources, topicIds: []
    };
    this.personalizedPlanSteps.push(this.personalizedPlanStep);
  }

  getTopicIds(topics): Array<string> {
    this.topicIds = [];
    topics.forEach(topic => {
      this.topicIds.push(topic.topicId);
    });
    return this.topicIds;
  }

  addRemainingTopicsToPlan(plan) {
    plan.forEach(topic => {
      if (!this.arrayUtilityService.checkObjectExistInArray(this.topicIds, topic.topicId)) {
        this.personalizedPlanSteps = [];
        this.personalizedPlanSteps = topic.steps;
        this.planTopic = { topicId: topic.topicId, steps: this.personalizedPlanSteps };
        this.planTopics.push(this.planTopic);
      }
    });
  }

}
