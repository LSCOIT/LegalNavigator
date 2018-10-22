import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http'
import { api } from '../../../api/api';

@Component({
  selector: 'app-upload-curated-experience-template',
  templateUrl: './upload-curated-experience-template.component.html',
  styleUrls: ['./upload-curated-experience-template.component.css']
})
export class UploadCuratedExperienceTemplateComponent implements OnInit {
  progress: number;
  message: string;
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  upload(files) {
    if (files.length === 0)
      return;
    const formData = new FormData();

    for (let file of files)
      formData.append(file.name, file);

    const uploadReq = new HttpRequest('POST', api.uploadCuratedExperienceTemplateUrl, formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress = Math.round(100 * event.loaded / event.total);
      else if (event.type === HttpEventType.Response)
        this.message = event.body.toString();
    });
  }

}
