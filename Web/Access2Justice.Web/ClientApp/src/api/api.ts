import ENV from 'env';

const apiUrl = ENV.apiUrl;
const topic = apiUrl + "/topics-resources";
const userProfile = apiUrl + "/user";
const curatedExperience = apiUrl + "/curated-experiences";
const contentUrl = apiUrl + "/static-resources";
const adminUrl = apiUrl + "/admin";
const onboarding = apiUrl + "/onboarding-info";

export const api = {
  topicUrl: topic + "/topics",
  subtopicUrl: topic + "/subtopics",
  subtopicDetailUrl: topic + "/resources/details",
  getDocumentUrl: topic + "/topics/document",
  searchUrl: apiUrl + "/search",
  getResourceUrl: topic + "/paged-resources",
  searchOffsetUrl: apiUrl + "/web-search",
  breadcrumbsUrl: topic + "/topics/breadcrumbs",
  personalizedPlan: apiUrl + "/personalized-plans/generate",
  planUrl: apiUrl + "/personalized-plans",
  getPersonalizedResourcesUrl: topic + "/personalized-resources",
  getProfileUrl: userProfile + "/profile",
  userPlanUrl: userProfile + "/personalized-plan/upsert",
  upsertUserProfileUrl: userProfile + "/profile/upsert",
  getOrganizationDetailsUrl: topic + "/organizations",
  questionUrl: curatedExperience + "/start",
  saveAndGetNextUrl: curatedExperience + "/components/save-and-get-next",
  updateUserPlanUrl: apiUrl + "/personalized-plans/save",
  resourceUrl: topic + "/resources",
  getContentsUrl: contentUrl,
  shareUrl: apiUrl + "/share/permalink/generate",
  unShareUrl: apiUrl + "/share/permalink/remove",
  getResourceLink: apiUrl + "/share/permalink/resource",
  checkPermaLink: apiUrl + "/share/permalink/check",
  updatePrivacyDataUrl: contentUrl + "/privacy/upsert",
  updateAboutDataUrl: contentUrl + "/about/upsert",
  updateHomeDataUrl: contentUrl + "/home/upsert",
  updateHelpAndFaqDataUrl: contentUrl + "/help-and-faq/upsert",
  updatePersonalizedPlanDataUrl: contentUrl + "/personalizedplan/upsert",
  uploadCuratedExperienceTemplateUrl: adminUrl + "/curated-experience",
  getStateCodesUrl: apiUrl + "/StateProvince/state-codes",
  getStateCodeUrl: apiUrl + "/StateProvince/state-code",
  onboardingUrl: onboarding,
  onboardingSubmissionUrl: onboarding + "/eform-submit",
  getTopicDetailsUrl: topic + "/get-topic-details",
  getStateNameUrl: apiUrl + "/StateProvince/state-name",
  getAnswerUrl: apiUrl + "/qnabot/"
};
