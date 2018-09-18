// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.
export const environment = {
  production: false,
  apiUrl: 'http://localhost:4200/api',
  bingmap_key: 'AlyLqEyg06sSOQJUwYsqcsm69zWmVKYgmaGvqdBPISKCJ59IQqqfCCaQ42KgqTyG',
  map_type: false,
  internalResourcePagesToShow: 2,
  internalResourceRecordsToDisplay: 10,
  webResourcePagesToShow: 10,
  webResourceRecordsToDisplay: 10,
  blobUrl: 'https://cs4892808efec24x447cx944.blob.core.windows.net',
  clientID: 'f0d077e6-f293-4c01-9cfb-b8327735533d',
  authority: 'https://login.microsoftonline.com/common/',
  consentScopes: ["user.read", 'api://f0d077e6-f293-4c01-9cfb-b8327735533d/access_as_user'],
  redirectUri: 'http://localhost:5150/',
  navigateToLoginRequestUrl: true,
  postLogoutRedirectUri: 'http://localhost:5150/',
  All: 'All'
};
