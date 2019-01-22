// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.
export const environment = {
  production: false,
  apiUrl: "http://localhost:4200/api",
  bingmap_key:
    "AlyLqEyg06sSOQJUwYsqcsm69zWmVKYgmaGvqdBPISKCJ59IQqqfCCaQ42KgqTyG",
  map_type: false,
  internalResourcePagesToShow: 2,
  internalResourceRecordsToDisplay: 10,
  webResourcePagesToShow: 10,
  webResourceRecordsToDisplay: 10,
  blobUrl: "https://cs4892808efec24x447cx944.blob.core.windows.net",
  virtualEarthUrl: "https://dev.virtualearth.net/REST/v1/Locations/",
  All: "All",
  clientID: "d528c88c-301c-4a5c-8967-96d571a5be96",
  authority: "https://login.microsoftonline.com/common/",
  consentScopes: ["api://d528c88c-301c-4a5c-8967-96d571a5be96/access_as_user"],
  redirectUri: "http://localhost:5150/",
  navigateToLoginRequestUrl: true,
  postLogoutRedirectUri: "http://localhost:5150/",
  topicUrl: "http://localhost:4200/api/topics",
  userPersonalizedPlanUrl:
    "http://localhost:4200/api/user/upsertuserpersonalizedplan",
  apiScope: "api://d528c88c-301c-4a5c-8967-96d571a5be96/access_as_user"
};
