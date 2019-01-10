import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SanitizePipe } from "./sanitize.pipe";

@NgModule({
  imports: [CommonModule],
  declarations: [SanitizePipe],
  exports: [SanitizePipe]
})
export class PipeModule {
  static forRoot() {
    return {
      ngModule: PipeModule,
      providers: []
    };
  }
}
