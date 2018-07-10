import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';

export const api = {
  topicUrl: topic + '/gettopics',
  subtopicUrl: topic + '/getsubtopics',
  subtopicDetailUrl: topic + '/getresourcedetails',
  getDocumentUrl: topic + '/getdocument',
  searchUrl: apiUrl + '/search',
  getResourceUrl: apiUrl + '/resources',
  searchOffsetUrl: apiUrl + '/websearch',
  breadcrumbsUrl: topic + '/getbreadcrumbs',
  loginUrl: apiUrl + '/login',
  logoutUrl: apiUrl + '/logout',
  questionUrl: 'http://access2justiceapi.azurewebsites.net/api/curatedexperience?surveyId=0b7dfe9b-cec9-4490-b768-c40916d52382'
}
