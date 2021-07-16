import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule , ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { GastosComponent } from './gastos/gastos.component';
import { GastoItemComponent } from './gasto-item/gasto-item.component';
import { GastoFormComponent } from './gasto-form/gasto-form.component';
import { ErrorModalComponent } from './error-modal/error-modal.component';
import { UserGuardGuard } from './user-guard.guard';
import { SignUpComponent } from './sign-up/sign-up.component';
import { UserFormComponent } from './user-form/user-form.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    GastosComponent,
    GastoItemComponent,
    GastoFormComponent,
    LoginComponent,
    ErrorModalComponent,
    SignUpComponent,
    UserFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'gastos', component: GastosComponent, pathMatch: 'full' , canActivate : [UserGuardGuard] },
      { path: 'crear-gasto', component: GastoFormComponent, pathMatch: 'full' , canActivate : [UserGuardGuard] },
      { path: 'sign-up', component: SignUpComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
