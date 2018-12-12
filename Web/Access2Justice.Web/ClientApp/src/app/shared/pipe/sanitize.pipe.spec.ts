import { SanitizePipe } from './sanitize.pipe';
import { DomSanitizer } from '@angular/platform-browser';
import { inject } from '@angular/core/testing';

describe('SanitizePipe', () => {
  it('create an instance', inject([DomSanitizer], (domSanitizer: DomSanitizer) => {
    const pipe = new SanitizePipe(domSanitizer);
    expect(pipe).toBeTruthy();
  }));
});
