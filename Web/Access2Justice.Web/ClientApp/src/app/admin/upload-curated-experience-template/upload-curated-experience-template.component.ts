import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http'
import { api } from '../../../api/api';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-upload-curated-experience-template',
  templateUrl: './upload-curated-experience-template.component.html',
  styleUrls: ['./upload-curated-experience-template.component.css']
})
export class UploadCuratedExperienceTemplateComponent implements OnInit {
  progress: number;
  message: string;
  @ViewChild('file') file: ElementRef;
  
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit(uploadForm: NgForm) {
    if (uploadForm.value.file) {
      
    }
  }
 
  upload() {
    const formData = new FormData();
    let file = this.file.nativeElement.files;
    if (file.length > 0) {
      for (let file of this.file.nativeElement.files) {
        formData.append(file.name, file);
      }

      const uploadReq = new HttpRequest('POST', api.uploadCuratedExperienceTemplateUrl, formData, {
        reportProgress: true
      });

      if (formData.has.length > 0) {

        this.http.request(uploadReq).subscribe(event => {
          console.log(event.type + HttpEventType.Response);
          if (event.type === HttpEventType.UploadProgress)
            this.progress = Math.round(100 * event.loaded / event.total);
          else if (event.type === HttpEventType.Response)
            this.message = event.body.toString();
        });
      }
    }
    else {
      this.message = "Please select the file to upload!";
    }
  }
}
