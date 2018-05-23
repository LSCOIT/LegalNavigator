import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { LocationComponent } from '../shared/location/location.component';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})
export class UpperNavComponent implements OnInit {
  items: string[] = [
    'Afrikaans', 'Arabic', 'Bangla', 'Bosnian (Latin)', 'Bulgarian', 'Cantonese (Traditional)', 'Catalan', 'Chinese SimplifiedCS', 'Chinese TraditionalCS', 'Croatian', 'Czech', 'Danish', 'Dutch', 'English', 'Estonian', 'Fijian', 'Filipino', 'Finnish', 'French', 'German', 'German', 'Greek', 'Haitian Creole', 'Hebrew', 'Hindi', 'Hmong Daw', 'Hungarian', 'Indonesian', 'Italian', 'Japanese','Kiswahili', 'Klingon', 'Klingon (plqaD)', 'Korean', 'Latvian', 'Lithuanian', 'Malagasy', 'Malay', 'Maltese', 'Norwegian', 'Persian', 'Polish', 'Portuguese', 'Queretaro Otomi', 'Romanian', 'Russian', 'Samoan', 'Serbian (Cyrillic)', 'Serbian (Latin)', 'Slovak', 'Slovenian', 'Spanish', 'Swedish', 'Tahitian', 'Tamil', 'Thai', 'Tongan', 'Turkish', 'Ukrainian', 'Urdu', 'Vietnamese', 'Welsh', 'Yucatec Maya'];

  modalRef: BsModalRef;

  constructor(private modalService: BsModalService) {}
 
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
  }
}
