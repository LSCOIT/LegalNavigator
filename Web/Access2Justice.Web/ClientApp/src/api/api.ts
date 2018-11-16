import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics-resources';
const userProfile = apiUrl + '/user';
const curatedExperience = apiUrl + '/curated-experiences';
const contentUrl = apiUrl + '/static-resources';
const adminUrl = apiUrl + '/admin';

export const api = {
    topicUrl: topic + '/topics',
    subtopicUrl: topic + '/subtopics',
    subtopicDetailUrl: topic + '/resources/details',
    getDocumentUrl: topic + '/topics/document',
    searchUrl: apiUrl + '/search',
    getResourceUrl: topic + '/paged-resources',
    searchOffsetUrl: apiUrl + '/web-search',
    breadcrumbsUrl: topic + '/topics/breadcrumbs',
    personalizedPlan: apiUrl + '/personalized-plans/generate',
    planUrl: curatedExperience + "/get-plan-details",//this route doesn't exist in any controller
    getPersonalizedResourcesUrl: topic + '/personalized-resources',
    updatePlanUrl: userProfile + "/upsert-user-plan",//this route doesn't exist in any controller
    getProfileUrl: userProfile + "/profile",
    userPlanUrl: userProfile + "/personalized-plan/upsert",
    upsertUserProfileUrl: userProfile +"/profile/upsert",
    getOrganizationDetailsUrl: topic + '/organizations',
    questionUrl: curatedExperience + '/start',
    saveAndGetNextUrl: curatedExperience + '/components/save-and-get-next',
    updateUserPlanUrl: curatedExperience + "/update-plan",//this route doesn't exist in any controller
    resourceUrl: topic + '/resources',
    getContentsUrl: contentUrl,
    shareUrl: apiUrl + '/share/permalink/generate',
    unShareUrl: apiUrl + '/share/permalink/remove',
    getResourceLink: apiUrl + '/share/permalink/resource',
    checkPermaLink: apiUrl + '/share/permalink/check',
    updatePrivacyDataUrl: contentUrl + '/privacy/upsert',
    updateAboutDataUrl: contentUrl + '/about/upsert',
    uploadCuratedExperienceTemplateUrl: adminUrl + '/curated-experience'    
}
