import { environment } from '../environments/environment';

const apiUrl = environment.apiUrl;
const topic = apiUrl + '/topics';

export const topicApi = {
  topicUrl: topic + '/gettopics',
  subtopicUrl: topic + '/getsubtopics',
  subtopicDetailUrl: topic + '/getresourcedetails',
  getDocumentUrl: topic + 'api/topics/getdocument'
}

export const searchApi = {
  searchUrl: apiUrl + '/search'
}

export const guidedAssistantApi = {
  questionUrl: 'http://access2justiceapi.azurewebsites.net/api/curatedexperience?surveyId=0b7dfe9b-cec9-4490-b768-c40916d52382'
}
