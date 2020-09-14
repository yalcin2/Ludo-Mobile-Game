import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ConvertToSpacesPipe } from './shared/convert-to-spaces.pipe';
import { PropertyListComponent } from './property/property-list.component';
import { PropertyDetailComponent } from './property/property-detail.component';
import { PropertyGuardService } from './property/services/property-guard.service';
import { AddPropertyComponent } from './add-property/add-property.component';
import { EditPropertyComponent } from './edit-property/edit-property.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { AboutUsComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AgmCoreModule } from '@agm/core';


@NgModule({
  declarations: [
    AppComponent,
    ConvertToSpacesPipe,
    PropertyListComponent,
    PropertyDetailComponent,
    AddPropertyComponent,
    EditPropertyComponent,
    SignUpComponent,
    AboutUsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule, FormsModule, HttpClientModule, HttpModule, NgxPaginationModule, ReactiveFormsModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyBSIIINf7PYVnQDy49SPeJIY8ijdRAKWxA'
    }),
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: 'about', component: AboutUsComponent },
      { path: 'sign-up', component: SignUpComponent },
      { path: 'property', component: PropertyListComponent },
      { path: 'property/:id', canActivate:[PropertyGuardService],
                                  component: PropertyDetailComponent },
      { path: 'add-property', component: AddPropertyComponent},
      { path: 'edit-property/:id', component: EditPropertyComponent},
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: '**', redirectTo: 'home', pathMatch: 'full' }, ])
  ],
  providers: [PropertyGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
