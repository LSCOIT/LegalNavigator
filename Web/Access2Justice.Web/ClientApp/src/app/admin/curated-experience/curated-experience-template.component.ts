import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { api } from '../../../api/api';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-curated-experience-template',
  templateUrl: './curated-experience-template.component.html',
  styleUrls: ['../admin-styles.css']
})
export class CuratedExperienceTemplateComponent implements OnInit {
  successMessage: string;
  errorMessage: string;
  @ViewChild('file') file: ElementRef;

  constructor(private http: HttpClient,
    private spinner: NgxSpinnerService) { }

  ngOnInit() { }

  onSubmit(uploadForm: NgForm) {
    if (uploadForm.valid) {
      this.spinner.show();
      this.successMessage = "";
      this.errorMessage = "";
      let formValue = uploadForm.form.value;
      let file = this.file.nativeElement.files[0];
      let formData = new FormData();
      if (file != null && file.size > 0) {
        formData.append("templateFile", file);
        formData.append('name', formValue.name);
        formData.append('description', formValue.description);
      }

      let params = new HttpParams();
      params.append('Content-Type', 'multipart/form-data;');
      const options = {
        params: params
      };

      this.http.post(api.uploadCuratedExperienceTemplateUrl, formData, { params, responseType: 'text' })
        .subscribe(
          response => {
            this.spinner.hide();
            uploadForm.reset();
            this.successMessage = response;
          },
          error => {
            this.spinner.hide();
            if (error.error) {
              this.errorMessage = error.error;
            }
            else if (error.statusText) {
              this.errorMessage = error.statusText;
            }
            else {
              this.errorMessage = error
            }
            console.log(error);
          });

    }
    else {
      this.errorMessage = "Please provide the required fields.";
    }
  }
}
