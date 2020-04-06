import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CuratedExperience } from "./curatedExperience";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ENV } from "environment";

@Injectable({
  providedIn: "root",
})
export class CuratedExperienceService {
  constructor(private http: HttpClient) {}

  getCuratedExperiences(location: string): Observable<CuratedExperience[]> {
    return this.http.get<CuratedExperience[]>(
      ENV.curatedExperienceUrl + `?location=${location}`
    );
  }
  deleteCuratedExperience(id: string, title: string): Observable<{}> {
    return this.http.delete<{}>(ENV.curatedExperienceUrl + `/${id}/${title}`);
  }
}
