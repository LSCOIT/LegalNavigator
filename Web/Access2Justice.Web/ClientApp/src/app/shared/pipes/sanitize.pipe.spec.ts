import { inject } from "@angular/core/testing";
import { DomSanitizer } from "@angular/platform-browser";
import { SanitizePipe } from "./sanitize.pipe";

describe("SanitizePipe", () => {
  it("create an instance", inject(
    [DomSanitizer],
    (domSanitizer: DomSanitizer) => {
      const pipe = new SanitizePipe(domSanitizer);
      expect(pipe).toBeTruthy();
    }
  ));
});
