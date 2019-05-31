export interface Question {
  curatedExperienceId: string;
  answersDocId: string;
  questionsRemaining: number;
  componentId: string;
  name: string;
  text: string;
  learn: string;
  help: string;
  tags: Array<string>;
  buttons: Array<Buttons>;
  buttonsSelected?: Array<Buttons>;
  fieldsSelected?: Array<Fields>;
  fields: Array<string>;
}

export interface Buttons {
  id: string;
  label: string;
  destination: string;
  buttonId?: string;
}

export interface Fields {
  answerNumber: string;
  codeAfter: string;
  codeBefore: string;
  fields: Array<Field>;
}

export interface Field {
  fieldId: string;
  name: string;
  text: string;
  value: string;
}
