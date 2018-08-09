import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';
const userProfile = apiUrl + '/user';
const curatedExperience = apiUrl + '/CuratedExperience';

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
  loginUrl: apiUrl + '/login',
  logoutUrl: apiUrl + '/logout',
  questionUrl: curatedExperience + '/Start',
  saveAndGetNextUrl: curatedExperience + '/Component/SaveAndGetNext',
  updateUserProfileUrl: userProfile + '/updateuserprofile',
  getUserProfileUrl: userProfile + "/getuserprofiledata",
  updateUserPlanUrl: curatedExperience + "/updateplan",
  resourceUrl: topic + '/getresource',
  shareUrl: apiUrl + '/Share/GeneratePermaLink',
  unShareUrl: apiUrl + '/Share/RemovePermaLink',
  getResourceLink: apiUrl + '/Share/GetPermalLinkResource',
  checkPermaLink: apiUrl + '/Share/CheckPermaLink'
}
