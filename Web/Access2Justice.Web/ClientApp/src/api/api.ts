import { ENV } from "environment";

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
  printSubtopicUrl: topic + "/resources/print",
  getDocumentUrl: topic + "/topics/document",
  getPrintLogo: topic + "/topics/image",
  printTopicUrl: topic + "/topics/print",
  searchUrl: apiUrl + "/search",
  getResourceUrl: topic + "/paged-resources",
  searchOffsetUrl: apiUrl + "/web-search",
  breadcrumbsUrl: topic + "/topics/breadcrumbs",
  personalizedPlan: apiUrl + "/personalized-plans/generate",
  printPersonalizedPlan: apiUrl + "/personalized-plans/generate-print",
  printPersonalizedPlanById: apiUrl + "/personalized-plans/plan/print",
  planUrl: apiUrl + "/personalized-plans",
  getPersonalizedResourcesUrl: topic + "/personalized-resources",
  getProfileUrl: userProfile + "/profile",
  userPlanUrl: userProfile + "/personalized-plan/upsert",
  removeSharedResources: userProfile + "/resources/delete",
  // unshareSharedToResources: userProfile + '/share/permalink/remove',
  unshareSharedToResources: apiUrl + "/share/link/unshare",
  upsertUserProfileUrl: userProfile + "/profile/upsert",
  getOrganizationDetailsUrl: topic + "/organizations",
  questionUrl: curatedExperience + "/start",
  questionBackUrl: curatedExperience + "/components/back",
  saveAndGetNextUrl: curatedExperience + "/components/save-and-get-next",
  curatedExperienceUrl: curatedExperience,
  updateUserPlanUrl: apiUrl + "/personalized-plans/save",
  getActionPlanIds: apiUrl + "/personalized-plans/action-plans",
  resourceUrl: topic + "/resources",
  getContentsUrl: contentUrl,
  shareUrl: apiUrl + "/share/permalink/generate",
  shareLinkToUser: apiUrl + "/share/link/send",
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
  getAnswerUrl: apiUrl + "/qnabot/",
};
