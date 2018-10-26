import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-browser-tab-close',
  templateUrl: './browser-tab-close.component.html',
  styleUrls: ['./browser-tab-close.component.css']
})
export class BrowserTabCloseComponent implements OnInit {
  modalRef: BsModalRef;
  @ViewChild('template') public templateref: TemplateRef<any>;

  constructor(private modalService: BsModalService) { }

  browserTabCloseAlertForm() {

  }

  saveToProfile() {

  }

  close() {
    this.modalRef.hide();
  }

  ngOnInit() {
    console.log(this.templateref);
    this.modalRef = this.modalService.show(this.templateref);    
  }

}
