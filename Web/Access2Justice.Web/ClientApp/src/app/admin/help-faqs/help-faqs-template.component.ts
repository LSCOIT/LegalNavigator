import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormArray, NgForm, FormGroup } from '@angular/forms';

@Component({
  selector: 'admin-help-faqs-template',
  templateUrl: './help-faqs-template.component.html',
  styleUrls: ['./help-faqs-template.component.css']
})
export class HelpFaqsTemplateComponent implements OnInit {
  public faqForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  get faqs() {
    return this.faqForm.get('faqs') as FormArray;
  }

  createFAQs(): FormGroup {
    return this.fb.group({
      question:'',
      answer: ''
    });
  }

  addFAQs() {
    this.faqs.push(this.createFAQs());
  }

  onSubmit(homeForm: NgForm) {
    console.log(this.faqForm);
  }

  ngOnInit() {
    this.faqForm = this.fb.group({
      faqs: this.fb.array([this.createFAQs()])
    });
  }

}
