import { Component, Input, OnInit, TemplateRef } from "@angular/core";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";

@Component({
  selector: "app-organizations",
  templateUrl: "./organizations.component.html",
  styleUrls: ["./organizations.component.css"]
})
export class OrganizationsComponent implements OnInit {
  currentImage: string;
  currentModalImage: string;
  urlOrigin: string;
  modalRef: BsModalRef;
  @Input() resource;
  @Input() searchResource: any = { resources: [] };

  constructor(private modalService: BsModalService) {}

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  changeImage(image) {
    this.currentImage = image.target.src;
  }

  changeModalImage(image) {
    this.currentModalImage = image.target.src;
  }

  ngOnInit() {
    this.searchResource.resources.push(this.resource);
    try {
      this.urlOrigin = new URL(this.resource.url).origin;
    } catch (e) {
      this.urlOrigin = this.resource.url;
    }
  }
}
