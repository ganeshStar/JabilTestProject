import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { apiUrl } from 'src/apiSettings';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    var postbody = {
      email: "ganeshmadane@gmail.com",
      password: "p@ssw0rd888",
      firstname: "ganesh"
    }
    http.post(baseUrl + apiUrl.cretaUserAPIUrl, postbody).subscribe(result => {
      //this.forecasts = result;
      console.log("result==", result);
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
