export const environment = {
  production: true,
  apiUrl: 'https://access2justiceapi.azurewebsites.net/api',
  bingmap_key: 'AlyLqEyg06sSOQJUwYsqcsm69zWmVKYgmaGvqdBPISKCJ59IQqqfCCaQ42KgqTyG',
  map_type: false,
  internalResourcePagesToShow: 2,
  internalResourceRecordsToDisplay: 10,
  webResourcePagesToShow: 10,
  webResourceRecordsToDisplay: 10,
  blobUrl: 'https://cs4892808efec24x447cx944.blob.core.windows.net',
  All: 'All',
  clientID: '15c4cbff-86ef-4c76-9b08-874ce0b55c8a',
  authority: 'https://login.microsoftonline.com/common/',
  consentScopes: ["api://15c4cbff-86ef-4c76-9b08-874ce0b55c8a/access_as_user"],
  redirectUri: 'http://localhost:5150/',
  navigateToLoginRequestUrl: true,
  postLogoutRedirectUri: 'http://localhost:5150/',
  userPersonalizedPlanUrl: 'http://localhost:4200/api/user/upsertuserpersonalizedplan',
  apiScope: 'api://15c4cbff-86ef-4c76-9b08-874ce0b55c8a/access_as_user'
};
