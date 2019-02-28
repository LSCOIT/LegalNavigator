import { HttpParams } from "@angular/common/http";
import { Component, OnInit, TemplateRef } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { QuestionControlService } from "../question-control.service";
import { IntakeFormQuestionBase } from "./intake-form-question-base";
import { IntakeQuestionService } from "./intake-question-service/intake-question.service";

@Component({
  selector: "app-intake-form",
  templateUrl: "./intake-form.component.html",
  styleUrls: ["./intake-form.component.css"]
})
export class IntakeFormComponent implements OnInit {
  modalRef: BsModalRef;
  intakeQuestions: IntakeFormQuestionBase<any>[];
  form: FormGroup;
  payLoad;

  constructor(
    private modalService: BsModalService,
    private qcs: QuestionControlService,
    private intakeQuestionService: IntakeQuestionService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private activeRoute: ActivatedRoute
  ) {
    this.form = this.qcs.toFormGroup([]);
  }

  getIntakeQuestions() {
    let param = new HttpParams().set(
      "organizationId",
      this.activeRoute.snapshot.params["id"]
    );
    this.intakeQuestionService.getIntakeQuestions(param).subscribe(response => {
      this.payLoad = response;
      this.intakeQuestions = response["userFields"];
      this.form = this.qcs.toFormGroup(this.intakeQuestions);
    });
  }

  onSubmit() {
    Object.keys(this.form.value).map(key => {
      this.payLoad.userFields.forEach(field => {
        if (field.name === key) {
          field.value = this.form.value[key];
        }
      });
    });
    this.spinner.show();
    this.intakeQuestionService
      .sendIntakeResponse(this.payLoad)
      .subscribe(response => {
        this.modalRef.hide();
        this.spinner.hide();
        this.toastr.success("Form submitted successfully");
      });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this.getIntakeQuestions();
  }
}
