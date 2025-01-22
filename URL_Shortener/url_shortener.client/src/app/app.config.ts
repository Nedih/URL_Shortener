import { ApplicationConfig, ErrorHandler, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { PreloadAllModules, provideRouter, TitleStrategy, UrlSerializer, withPreloading } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app-routing.module';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimations()
  ]
};
