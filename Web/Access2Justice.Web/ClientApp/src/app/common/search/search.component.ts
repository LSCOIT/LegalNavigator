import { Component, Input, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { LocationDetails, MapLocation } from "../map/map";
import { NavigateDataService } from "../services/navigate-data.service";
import { ILuisInput } from "./search-results/search-results.model";
import { SearchService } from "./search.service";

@Component({
  selector: "app-search",
  templateUrl: "./search.component.html",
  styleUrls: ["./search.component.css"]
})
export class SearchComponent implements OnInit {
  inputText: string;
  @Input()
  searchResults: any;
  luisInput: ILuisInput = {
    Sentence: "",
    Location: "",
    TranslateFrom: "",
    TranslateTo: "",
    LuisTopScoringIntent: "",
    OrderByField: "date",
    OrderBy: "DESC"
  };
  mapLocation: MapLocation;
  locationDetails: LocationDetails;
  mock: any;

  constructor(
    private searchService: SearchService,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private spinner: NgxSpinnerService
  ) {}

  onSubmit(searchForm: NgForm): void {
    this.spinner.show();
    this.luisInput.Sentence = searchForm.value.inputText;
    if (sessionStorage.getItem("mapAddress")) {
      const address = JSON.parse(
        sessionStorage.getItem("mapAddress")
      );
      this.mapLocation = {
        state: address.state,
        county: address.county,
        city: address.city,
        zipCode: address.zipCode
      }
    }
    this.luisInput.Location = this.mapLocation;
    sessionStorage.removeItem("cacheSearchResults");
    sessionStorage.removeItem("searchedLocationMap");
    this.searchService.search(this.luisInput).subscribe(
      response => {
        this.spinner.hide();
        if (response != undefined) {
          this.searchResults = response;
          this.navigateDataService.setData(this.searchResults);
          this.router
            .navigateByUrl("/searchRefresh", { skipLocationChange: true })
            .then(() => this.router.navigate(["/search"]));
        }
      },
      error => {
        this.spinner.hide();
        this.router.navigate(["/error"]);
      }
    );
  }

  ngOnInit() {
    this.mock = {
      suggestions: [
        'lol kek',
        'cheburek',
        'chicki-briki'
      ],
        resources: [
          'Lorem ipsum dolor sit amet, consectetur adipisicing',
          'Lorem ipsum dolor sit amet, consectetur adipisicing',
          'Lorem ipsum dolor sit amet, consectetur adipisicing'
        ],
        relatedTopics: [
          'Lorem ipsum dolor sit amet, consectetur adipisicing',
          'Lorem ipsum dolor sit amet, consectetur adipisicing',
          'Lorem ipsum dolor sit amet, consectetur adipisicing'
        ]
    };
  }
}
