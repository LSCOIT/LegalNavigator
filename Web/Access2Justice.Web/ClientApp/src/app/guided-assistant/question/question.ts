export interface Question {
  curatedExperienceId : string,
  answersDocId: string,
  stepRemained: number,
  componentId: string,
  name: string,
  text: string,
  learn: string,
  help: string,
  tags: Array<string>,
  buttons: [{
    id: string,
    label: string,
    destination: string,
  }],
  fields: Array<string>,
}
