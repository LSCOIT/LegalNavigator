export class Question {

  constructor(
    public questionId: string,
    public title: string,
    public questionType: string,
    public userAnswer: string,
    public choices: any = []) { }
}
