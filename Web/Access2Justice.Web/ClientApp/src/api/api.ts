import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';
const userProfile = apiUrl + '/user';
const curatedExperience = apiUrl + '/CuratedExperience';
const contentUrl = apiUrl + '/staticresource';

export const api = {
    topicUrl: topic + '/gettopics',
    subtopicUrl: topic + '/getsubtopics',
    subtopicDetailUrl: topic + '/getresourcedetails',
    getDocumentUrl: topic + '/getdocument',
    searchUrl: apiUrl + '/search',
    getResourceUrl: apiUrl + '/resources',
    searchOffsetUrl: apiUrl + '/websearch',
    breadcrumbsUrl: topic + '/getbreadcrumbs',
    personalizedPlan: curatedExperience + '/PersonalizedPlan',
    planUrl: curatedExperience + "/getplandetails",
    getPersonalizedResourcesUrl: apiUrl + '/personalizedresources',
    updatePlanUrl: userProfile + "/upsertuserplan",
    getProfileUrl: userProfile + "/getuserprofile",
    userPlanUrl: userProfile + "/upsertuserpersonalizedplan",
    getOrganizationDetailsUrl: topic + '/getorganizationdetails',
    loginUrl: '/signIn',
    logoutUrl: '/signOut',
    questionUrl: curatedExperience + '/Start',
    saveAndGetNextUrl: curatedExperience + '/Component/SaveAndGetNext',
    updateUserPlanUrl: curatedExperience + "/updateplan",
    resourceUrl: topic + '/getresource',
    getContentUrl: contentUrl + '/getstaticresource',
    shareUrl: apiUrl + '/Share/GeneratePermaLink',
    unShareUrl: apiUrl + '/Share/RemovePermaLink',
    getResourceLink: apiUrl + '/Share/GetPermalLinkResource',
    checkPermaLink: apiUrl + '/Share/CheckPermaLink'
}
