import { inject } from "@angular/core/testing";
import { DomSanitizer } from "@angular/platform-browser";
import { SanitizePipe } from "./sanitize.pipe";
import {SanitizeHtmlPipe} from "./sanitizeHtml.pipe";

describe("SanitizeHtmlPipe", () => {
  it("create an instance", inject(
    [DomSanitizer],
    (domSanitizer: DomSanitizer) => {
      const pipe = new SanitizeHtmlPipe(domSanitizer);
      expect(pipe).toBeTruthy();
    }
  ));
});
