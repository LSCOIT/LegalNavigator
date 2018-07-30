export interface Question {
  curatedExperienceId : string,
  answersDocId: string,
  questionsRemaining: number,
  componentId: string,
  name: string,
  text: string,
  learn: string,
  help: string,
  tags: Array<string>,
  buttons: Array<Buttons>,
  fields: Array<string>
}

export interface Buttons {
  id: string,
  label: string,
  destination: string
}
