import { ActionReducerMap, MetaReducer } from '@ngrx/store';

import { ENV } from 'environment';
import { State } from './state';

export const reducers: ActionReducerMap<State> = {
  savedResources: () => null
};

export const metaReducers: MetaReducer<State>[] = !ENV.production ? [] : [];
