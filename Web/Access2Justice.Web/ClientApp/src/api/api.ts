import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';
const userProfile = apiUrl + '/user';
const curatedExperience = apiUrl + '/curatedexperience';
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
    personalizedPlan: curatedExperience + '/personalizedplan',
    planUrl: curatedExperience + "/getplandetails",
    getPersonalizedResourcesUrl: apiUrl + '/personalizedresources',
    updatePlanUrl: userProfile + "/upsertuserplan",
    getProfileUrl: userProfile + "/getuserprofile",
    userPlanUrl: userProfile + "/upsertuserpersonalizedplan",
    getOrganizationDetailsUrl: topic + '/getorganizationdetails',
    loginUrl: '/signIn',
    logoutUrl: '/signOut',
    questionUrl: curatedExperience + '/start',
    saveAndGetNextUrl: curatedExperience + '/component/saveandgetnext',
    updateUserPlanUrl: curatedExperience + "/updateplan",
    resourceUrl: topic + '/getresource',
    getContentsUrl: contentUrl + '/getstaticresources',
    shareUrl: apiUrl + '/share/generatepermalink',
    unShareUrl: apiUrl + '/share/removepermalink',
    getResourceLink: apiUrl + '/share/getpermalinkresource',
    checkPermaLink: apiUrl + '/share/checkpermalink'
}
