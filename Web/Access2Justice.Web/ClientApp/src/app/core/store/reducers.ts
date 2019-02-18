import { ActionReducerMap, MetaReducer } from '@ngrx/store';

import { ENV } from 'environment';
import { State } from './state';
import { ActionTypes, AppAction } from './actions';
import { MapLocation } from '../../shared/map/map';

export function locationReducer(state: MapLocation = null, action: AppAction): MapLocation {
  switch (action.type) {
    case ActionTypes.SetLocation: {
      return action.payload;
    }

    default: {
      return state;
    }
  }
}

export const reducers: ActionReducerMap<State> = {
  location: locationReducer
};

export const metaReducers: MetaReducer<State>[] = !ENV.production ? [] : [];
