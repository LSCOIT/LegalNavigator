import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SanitizePipe } from "./sanitize.pipe";
import { SearchFilterPipe } from "./search-filter.pipe";

@NgModule({
  imports: [CommonModule],
  declarations: [
    SanitizePipe,
    SearchFilterPipe
  ],
  exports: [
    SanitizePipe,
    SearchFilterPipe
  ]
})
export class PipeModule {
}
