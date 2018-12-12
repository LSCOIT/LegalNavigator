import { QuestionBase } from "./question-base";

export class TextboxQuestion extends QuestionBase<string> {
  type = 'textbox';
  textboxType: string;

  constructor(options: {} = {}) {
    super(options);
    this.textboxType = options['type'] || '';
  }
}
