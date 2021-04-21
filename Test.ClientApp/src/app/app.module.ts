import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
import { ClipboardModule } from 'ngx-clipboard';
import { TranslateModule } from '@ngx-translate/core';
import { InlineSVGModule } from 'ng-inline-svg';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigurationService } from './modules/auth/_services/configuration.service';
import { environment } from 'src/environments/environment';
// Highlight JS
import { HighlightModule, HIGHLIGHT_OPTIONS } from 'ngx-highlightjs';
import { SplashScreenModule } from './_metronic/partials/layout/splash-screen/splash-screen.module';
// #fake-start#
import { FakeAPIService } from './_fake/fake-api.service';
// #fake-end#

//api module
import { ApiModule } from '../api.module/api.module';
import { Configuration, ConfigurationParameters } from '../api.module/configuration';

function ApiConfigurationFactory(authService: ConfigurationService): Configuration {
  const params: ConfigurationParameters = {
      basePath:environment.apiUrl,
      credentials:{["Bearer"]:authService.getUserByToken.bind(authService)},
      apiKeys:{"Bearer":authService.getUserByToken.bind(authService)},
      withCredentials:false
  }
  return new Configuration(params);
}


@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SplashScreenModule,
    TranslateModule.forRoot(),
    HttpClientModule,
    HighlightModule,
    ClipboardModule,
    // #fake-start#
    environment.isMockEnabled
      ? HttpClientInMemoryWebApiModule.forRoot(FakeAPIService, {
        passThruUnknownUrl: true,
        dataEncapsulation: false,
      })
      : [],
    // #fake-end#
    AppRoutingModule,
    ApiModule,
    InlineSVGModule.forRoot(),
    NgbModule,
  ],
  providers: [
    {
      provide: Configuration,
      useFactory: (authSvc: ConfigurationService) => new Configuration({
        accessToken: authSvc.getUserByToken.bind(authSvc),
        basePath:environment.apiUrl,
        credentials:{["Bearer"]:authSvc.getUserByToken.bind(authSvc)},
        apiKeys:{"Bearer":authSvc.getUserByToken.bind(authSvc)},
      }),
      multi: false,
      deps: [ConfigurationService],
    },
    {
      provide: HIGHLIGHT_OPTIONS,
      useValue: {
        coreLibraryLoader: () => import('highlight.js/lib/core'),
        languages: {
          xml: () => import('highlight.js/lib/languages/xml'),
          typescript: () => import('highlight.js/lib/languages/typescript'),
          scss: () => import('highlight.js/lib/languages/scss'),
          json: () => import('highlight.js/lib/languages/json')
        },
      },
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
