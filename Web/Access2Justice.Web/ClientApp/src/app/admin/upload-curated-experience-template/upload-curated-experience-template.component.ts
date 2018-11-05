import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MsalService } from '@azure/msal-angular';
import { api } from '../../../api/api';
import { environment } from '../../../environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-upload-curated-experience-template',
  templateUrl: './upload-curated-experience-template.component.html',
  styleUrls: ['./upload-curated-experience-template.component.css']
})
export class UploadCuratedExperienceTemplateComponent implements OnInit {
  successMessage: string;
  errorMessage: string;
  @ViewChild('file') file: ElementRef;

  constructor(private http: HttpClient, private msalService: MsalService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    if (!this.msalService.getUser()) {
      this.msalService.loginRedirect(environment.consentScopes);
    }
  }

  onSubmit(uploadForm: NgForm) {
    this.spinner.show();
    if (uploadForm.form.value) {
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

      this.http.post(api.uploadCuratedExperienceTemplateUrl, formData, { params, responseType: 'text'})
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
  }
}
