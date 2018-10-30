import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { api } from '../../../api/api';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-upload-curated-experience-template',
  templateUrl: './upload-curated-experience-template.component.html',
  styleUrls: ['./upload-curated-experience-template.component.css']
})
export class UploadCuratedExperienceTemplateComponent implements OnInit {
  successMessage: string;
  errorMessage: string;
  @ViewChild('file') file: ElementRef;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit(uploadForm: NgForm) {
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

      this.http.post(api.uploadCuratedExperienceTemplateUrl, formData, options)
        .subscribe(
          response => {
            uploadForm.reset();
            this.successMessage = "Upload successfull.";
          },
          error => {
            uploadForm.reset();
            this.errorMessage = error.error == undefined ? error : error.error;
            console.log(error);
          }
        );
    }
  }
}
