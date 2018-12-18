export class IntakeFormQuestionBase<T> {
  type: string;
  label: string;
  name: string;
  value: Array<any>;
  isRequired: boolean;
  minLength: string;
  maxLength: string;

  constructor(options: {
    type?: string,
    label?: string,
    name?: string,
    value?: Array<any>,
    isRequired?: boolean,
    minLength?: string,
    maxLength?: string
  } = {}) {
    this.type = options.type || '';
    this.label = options.label || '';
    this.name = options.name || '';
    this.value = options.value || [];
    this.isRequired = options.isRequired || false;
    this.minLength = options.minLength || '';
    this.maxLength = options.maxLength || '';
  }
}
