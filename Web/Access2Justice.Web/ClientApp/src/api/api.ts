import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';
const userProfile = apiUrl + '/user';

export const api = {
  topicUrl: topic + '/gettopics',
  subtopicUrl: topic + '/getsubtopics',
  subtopicDetailUrl: topic + '/getresourcedetails',
  getDocumentUrl: topic + '/getdocument',
  searchUrl: apiUrl + '/search',
  getResourceUrl: apiUrl + '/resources',
  searchOffsetUrl: apiUrl + '/websearch',
  breadcrumbsUrl: topic + '/getbreadcrumbs',
  planUrl: topic + "/getplandetails",
  getPersonalizedResourcesUrl: apiUrl + '/personalizedresources',
  updatePlanUrl: userProfile + "/upsertuserplan",
  getProfileUrl: userProfile + "/getuserprofile",
  userPlanUrl: userProfile + "/upsertuserpersonalizedplan",
  getOrganizationDetailsUrl: topic + '/getorganizationdetails',
  loginUrl: apiUrl + '/login',
  logoutUrl: apiUrl + '/logout',
  questionUrl: apiUrl + '/CuratedExperience/Start',
  saveAndGetNextUrl: apiUrl + '/CuratedExperience/Component/SaveAndGetNext',
  resourceUrl: topic + '/getresource',
  shareUrl: apiUrl + '/Share/GeneratePermaLink',
  unShareUrl: apiUrl + '/Share/RemovePermaLink',
  getResourceLink: apiUrl + '/Share/GetPermalLinkResource',
  checkPermaLink: apiUrl + '/Share/CheckPermaLink'
}
