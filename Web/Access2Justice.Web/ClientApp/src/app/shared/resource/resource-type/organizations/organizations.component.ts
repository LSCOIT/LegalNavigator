import { Component, OnInit, TemplateRef, Input } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-organizations',
  templateUrl: './organizations.component.html',
  styleUrls: ['./organizations.component.css']
})
export class OrganizationsComponent implements OnInit {
  currentImage: string;
  currentModalImage: string;
  modalRef: BsModalRef;
  @Input() resource;
  @Input()
  organizationResource: any;
  searchResource: any = { resources: [] };

  constructor(private modalService: BsModalService) { }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  changeImage(image) {
    console.log(image.target.src);
    this.currentImage = image.target.src;
  }

  changeModalImage(image) {
    console.log(image.target.src);
    this.currentModalImage = image.target.src;
  }

  ngOnInit() {
    this.organizationResource = this.organizationResource;
    this.searchResource.resources.push(this.organizationResource);
  }

}
