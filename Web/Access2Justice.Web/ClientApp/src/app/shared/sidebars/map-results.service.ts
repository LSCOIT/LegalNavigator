import { Injectable, Input, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
declare var Microsoft: any;

@Injectable()

export class MapResultsService implements OnInit {
  searchResults: any;
  map: any;
  mapResults: any;

  //@Input() searchResource: any;

  constructor(private http: HttpClient) {
  }


  callGeocodeService(credentials) {
    var that = this;
    var address = "Honolulu, Hawaii 96813, United States";
    var searchRequest = 'https://dev.virtualearth.net/REST/v1/Locations/' +
      encodeURI(address) +
      '?output=json&key=' + credentials;

    //that.http.get(searchRequest).toPromise().then(response => {
    //    console.log(response);
    //  });

    //var mapscript = document.createElement('script');
    //mapscript.type = 'text/javascript';
    //mapscript.src = searchRequest;
    //document.getElementById('my-map-results').appendChild(mapscript);
  }


  geocodeServiceCallback(result) {
    let map = new Microsoft.Maps.Map('#my-map-results',
      {
        credentials: environment.bingmap_key
      });
    if (result &&
      result.resourceSets &&
      result.resourceSets.length > 0 &&
      result.resourceSets[0].resources &&
      result.resourceSets[0].resources.length > 0) {
      var results = result.resourceSets[0].resources;
      var locationArray = new Array();
      for (var j = 0; j < results.length; j++) {

        var location = new Microsoft.Maps.Location(results[j].point.coordinates[0],
          results[j].point.coordinates[1]);
        var pushpin = createPushpin(location, (j + 1).toString());

        // get calculation method:
        var calculationMethod = getCalcMethod(results[j]);

        // Add geocoding metadata to pin:
        pushpin.geoname = results[j].name;
        pushpin.geoentitytype = results[j].entityType;
        pushpin.geoaddress = results[j].address;
        pushpin.geoconfidence = results[j].confidence;
        pushpin.geomatchcodes = results[j].matchCodes;
        pushpin.geocalculationmethod = calculationMethod;
        pushpin.geopindragged = false;
        pushpin.geoaddressrevgeo = false;

        // Add pin to map:
        map.entities.push(pushpin);

        // Add location to array for map auto-scaling:
        locationArray.push(location);
      }

      // Set view depending on whether result is unique or not:
      if (results.length == 1) {
        var bbox = results[0].bbox;
        var viewBoundaries = Microsoft.Maps.LocationRect.fromCorners(new Microsoft.Maps.Location(bbox[0], bbox[1]),
          new Microsoft.Maps.Location(bbox[2], bbox[3]));
        map.setView({ bounds: viewBoundaries });
      } else {
        // Show a best view for all locations
        var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(locationArray);
        map.setView({ bounds: viewBoundaries, padding: 75 });
      }
      // Open infobox for top result:
      //showInfoBox(map.entities.get(0));
    }
    else {
      if (result && result.errorDetails) {
        //alert("Message :" + response.errorDetails[0]);
      }
      alert("No results for the query");
    }
  }

  //var latlons = [{ lat: 45, lon: -110 }, ....];

  //for (var i = 0, len = latlons.length; i < len; i++) {
  //  var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(latlongs[I].lat, latlongs[I].lon));
  //  map.entities.push(pin);
  //}

  ngOnInit() {

  }

}

// Callback for REST Locations request:
function geocodeServiceCallback(result) {
  let map = new Microsoft.Maps.Map('#my-map-results',
    {
      credentials: environment.bingmap_key
    });
  if (result &&
    result.resourceSets &&
    result.resourceSets.length > 0 &&
    result.resourceSets[0].resources &&
    result.resourceSets[0].resources.length > 0) {
    var results = result.resourceSets[0].resources;
    var locationArray = new Array();
    for (var j = 0; j < results.length; j++) {

      var location = new Microsoft.Maps.Location(results[j].point.coordinates[0],
        results[j].point.coordinates[1]);
      var pushpin = createPushpin(location, (j + 1).toString());

      // get calculation method:
      var calculationMethod = getCalcMethod(results[j]);

      // Add geocoding metadata to pin:
      pushpin.geoname = results[j].name;
      pushpin.geoentitytype = results[j].entityType;
      pushpin.geoaddress = results[j].address;
      pushpin.geoconfidence = results[j].confidence;
      pushpin.geomatchcodes = results[j].matchCodes;
      pushpin.geocalculationmethod = calculationMethod;
      pushpin.geopindragged = false;
      pushpin.geoaddressrevgeo = false;

      // Add pin to map:
      map.entities.push(pushpin);

      // Add location to array for map auto-scaling:
      locationArray.push(location);
    }

    // Set view depending on whether result is unique or not:
    if (results.length == 1) {
      var bbox = results[0].bbox;
      var viewBoundaries = Microsoft.Maps.LocationRect.fromCorners(new Microsoft.Maps.Location(bbox[0], bbox[1]),
        new Microsoft.Maps.Location(bbox[2], bbox[3]));
      map.setView({ bounds: viewBoundaries });
    } else {
      // Show a best view for all locations
      var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(locationArray);
      map.setView({ bounds: viewBoundaries, padding: 75 });
    }
    // Open infobox for top result:
    //showInfoBox(map.entities.get(0));
  }
  else {
    if (result && result.errorDetails) {
      //alert("Message :" + response.errorDetails[0]);
    }
    alert("No results for the query");
  }
}

function createPushpin(location, label) {
  var pushpin = new Microsoft.Maps.Pushpin(location, { draggable: true, text: label });
  //var pushpinclick = Microsoft.Maps.Events.addHandler(pushpin, 'click', pinClickHandler);
  //var pushpindragend = Microsoft.Maps.Events.addHandler(pushpin, 'drag', dragHandler);
  return pushpin;
}

// obtain calculation method from a location result:
function getCalcMethod(result) {

  // Identify the calculation method for our display point:
  var calculationMethod = "";
  if (result.geocodePoints && result.geocodePoints.length != null) {
    // loop through to find the calculation method for the geocodePoint of type 'Display':
    for (var k = 0; k < result.geocodePoints.length; k++) {
      if (result.geocodePoints[k].usageTypes.toString().match(/Display/)) {
        calculationMethod = result.geocodePoints[k].calculationMethod;
      }
    }
  }
  return calculationMethod;

}


