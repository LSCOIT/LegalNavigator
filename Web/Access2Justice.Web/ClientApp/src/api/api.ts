import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';
const userProfile = apiUrl + '/user';
const curatedExperience = apiUrl + '/curated-experience';
const contentUrl = apiUrl + '/static-resource';
const adminUrl = apiUrl + '/admin';

export const api = {
    topicUrl: topic + '/get-topics',
    subtopicUrl: topic + '/get-subtopics',
    subtopicDetailUrl: topic + '/get-resource-details',
    getDocumentUrl: topic + '/get-document',
    searchUrl: apiUrl + '/search',
    getResourceUrl: apiUrl + '/resources',
    searchOffsetUrl: apiUrl + '/web-search',
    breadcrumbsUrl: topic + '/get-breadcrumbs',
    personalizedPlan: apiUrl + '/personalized-plan/generate',
    planUrl: curatedExperience + "/get-plan-details",
    getPersonalizedResourcesUrl: apiUrl + '/personalized-resources',
    updatePlanUrl: userProfile + "/upsert-user-plan",
    getProfileUrl: userProfile + "/get-user-profile",
    userPlanUrl: userProfile + "/upsert-user-personalized-plan",
    upsertUserProfileUrl: userProfile +"/upsert-user-profile",
    getOrganizationDetailsUrl: topic + '/get-organization-details',
    questionUrl: curatedExperience + '/start',
    saveAndGetNextUrl: curatedExperience + '/component/save-and-get-next',
    updateUserPlanUrl: curatedExperience + "/update-plan",
    resourceUrl: topic + '/get-resource',
    getContentsUrl: contentUrl + '/get-static-resources',
    shareUrl: apiUrl + '/share/generate-permalink',
    unShareUrl: apiUrl + '/share/remove-permalink',
    getResourceLink: apiUrl + '/share/get-permalink-resource',
    checkPermaLink: apiUrl + '/share/check-permalink',
    uploadCuratedExperienceTemplateUrl: adminUrl + '/upload-curated-experience-template'    
}
