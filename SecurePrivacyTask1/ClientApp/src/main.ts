import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { userReducer } from './app/store/user.reducer';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideEffects } from '@ngrx/effects';
import { UserEffects } from './app/store/user.effects';
import { UserService } from './app/services/user.service';  // Make sure UserService is provided here
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { importProvidersFrom } from '@angular/core'; // Required for standalone

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),  // Make sure HttpClient is provided
    provideRouter(routes),
    provideStore({ users: userReducer }),
    provideEffects(UserEffects),
    UserService,  
    importProvidersFrom(BrowserAnimationsModule),  
    importProvidersFrom(ToastrModule.forRoot())  
  ]
}).catch(err => console.error(err));
