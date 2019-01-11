export class IntakeFormQuestionBase<T> {
  name: string;
  value: string;

  constructor(
    options: {
      name?: string;
      value?: any;
    } = {}
  ) {
    this.name = options.name || "";
    this.value = options.value || [];
  }
}
