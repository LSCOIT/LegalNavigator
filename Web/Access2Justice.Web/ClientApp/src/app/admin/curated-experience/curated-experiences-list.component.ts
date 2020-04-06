import { OnInit, Component, TemplateRef } from "@angular/core";
import { CuratedExperience } from "../../topics-resources/shared/curatedExperience";
import { CuratedExperienceService } from "../../topics-resources/shared/curatedExperience.service";
import { PageChangedEvent } from "ngx-bootstrap/pagination";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";

@Component({
  selector: "app-curated-experience-list",
  templateUrl: "./curated-experiences-list.component.html",
  styleUrls: ["./curated-experiences-list.component.css"],
})
export class CuratedExperienceListComponent implements OnInit {
  curatedExperiences: CuratedExperience[];
  returnedArray: CuratedExperience[];
  isHawaii: boolean;
  isAlaska: boolean;
  modalRef: BsModalRef;

  constructor(
    private curatedExperienceService: CuratedExperienceService,
    private modalService: BsModalService
  ) {}

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { class: "modal-sm" });
  }

  confirm(id: string, title: string): void {
    this.curatedExperienceService
      .deleteCuratedExperience(id, title)
      .subscribe((response) => {
        this.refresh();
      });
    this.modalRef.hide();
  }

  decline(): void {
    this.modalRef.hide();
  }

  pageChanged(event: PageChangedEvent): void {
    const startItem = (event.page - 1) * event.itemsPerPage;
    const endItem = event.page * event.itemsPerPage;
    this.returnedArray = this.curatedExperiences.slice(startItem, endItem);
  }

  refresh() {
    if (this.isHawaii) {
      this.getCuratedExperiences("Hawaii");
    } else if (this.isAlaska) {
      this.getCuratedExperiences("Alaska");
    } else {
      this.getCuratedExperiences();
    }
  }

  checkHawaii() {
    if (this.isHawaii) {
      this.getCuratedExperiences("Hawaii");
    } else {
      this.getCuratedExperiences();
    }
  }
  checkAlaska() {
    if (this.isAlaska) {
      this.getCuratedExperiences("Alaska");
    } else {
      this.getCuratedExperiences();
    }
  }

  getCuratedExperiences(location?: string) {
    this.curatedExperienceService
      .getCuratedExperiences(location)
      .subscribe((response) => {
        if (response != undefined) {
          this.curatedExperiences = response;
          this.returnedArray = this.curatedExperiences.slice(0, 10);
        }
      });
  }
  ngOnInit(): void {
    this.getCuratedExperiences();
  }
}
