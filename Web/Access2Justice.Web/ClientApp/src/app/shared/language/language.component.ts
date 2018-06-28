import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {
  items: string[] = [
    'Afrikaans', 'Arabic', 'Bangla', 'Bosnian (Latin)', 'Bulgarian', 'Cantonese (Traditional)', 'Catalan', 'Chinese SimplifiedCS', 'Chinese TraditionalCS', 'Croatian', 'Czech', 'Danish', 'Dutch', 'English', 'Estonian', 'Fijian', 'Filipino', 'Finnish', 'French', 'German', 'German', 'Greek', 'Haitian Creole', 'Hebrew', 'Hindi', 'Hmong Daw', 'Hungarian', 'Indonesian', 'Italian', 'Japanese', 'Kiswahili', 'Klingon', 'Klingon (plqaD)', 'Korean', 'Latvian', 'Lithuanian', 'Malagasy', 'Malay', 'Maltese', 'Norwegian', 'Persian', 'Polish', 'Portuguese', 'Queretaro Otomi', 'Romanian', 'Russian', 'Samoan', 'Serbian (Cyrillic)', 'Serbian (Latin)', 'Slovak', 'Slovenian', 'Spanish', 'Swedish', 'Tahitian', 'Tamil', 'Thai', 'Tongan', 'Turkish', 'Ukrainian', 'Urdu', 'Vietnamese', 'Welsh', 'Yucatec Maya'];

  constructor() { }

  ngOnInit() {
  }

}
