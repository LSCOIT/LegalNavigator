import { Action } from '@ngrx/store';

import { MapLocation } from '../../shared/map/map';

export enum ActionTypes {
  SetLocation = '[App] Set Location',
}

export class SetLocation implements Action {
  readonly type = ActionTypes.SetLocation;

  constructor(public payload: MapLocation) {}
}

export type AppAction = SetLocation;
