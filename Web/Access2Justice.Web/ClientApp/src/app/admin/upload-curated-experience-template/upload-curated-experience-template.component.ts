import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http'
import { api } from '../../../api/api';
import { NgForm } from '@angular/forms';
import { Response } from '@angular/http';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-upload-curated-experience-template',
  templateUrl: './upload-curated-experience-template.component.html',
  styleUrls: ['./upload-curated-experience-template.component.css']
})
export class UploadCuratedExperienceTemplateComponent implements OnInit {
  message: string;
  @ViewChild('file') file: ElementRef;
  
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit(uploadForm: NgForm) {
    if (uploadForm.form.value) {
      let formValue = uploadForm.form.value;
      let file = this.file.nativeElement.files[0];
      let formData = new FormData();
      if (file.size > 0) {
        formData.append("templateFile", file);
        formData.append('name', formValue.name);
        formData.append('description', formValue.description);
      }

      //formValue.file = this.file.nativeElement.files[0];

      let params = new HttpParams();
      params.append('Content-Type', 'multipart/form-data;');
      const options = {
        params: params, 
        reportProgress: true,
      };

      const uploadReq = new HttpRequest('POST', api.uploadCuratedExperienceTemplateUrl, formData,options);

      this.http.request(uploadReq).subscribe(event => {
        console.log(event.type + HttpEventType.Response);
        if (event.type === HttpEventType.Response)
          this.message = event.body.toString();
      });
    }
  }
}
