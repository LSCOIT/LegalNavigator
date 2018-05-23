import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
declare var Microsoft: any;

@Injectable()
export class Globals {
  readonly mapGlobal = new Microsoft.Maps.Map('#myMap',
    {
      credentials: environment.bingmap_key
    });
}

