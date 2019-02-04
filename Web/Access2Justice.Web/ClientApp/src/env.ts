'use strict';

export interface IEnv {
  production: boolean;
  apiUrl: string;
  bingmap_key: string;
  map_type: boolean;
  internalResourcePagesToShow: number;
  internalResourceRecordsToDisplay: number;
  webResourcePagesToShow: number;
  webResourceRecordsToDisplay: number;
  blobUrl: string;
  virtualEarthUrl: string;
  All: string;
  clientID: string;
  authority: string;
  consentScopes: string[];
  redirectUri: string;
  navigateToLoginRequestUrl: boolean;
  postLogoutRedirectUri: string;
  topicUrl: string;
  userPersonalizedPlanUrl: string;
  apiScope: string;
}

export default window['__ngEnv'] as IEnv;
