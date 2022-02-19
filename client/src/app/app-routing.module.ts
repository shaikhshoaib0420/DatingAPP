import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestErrosComponent } from './errors/test-erros/test-erros.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberEditComponent } from './member-edit/member-edit.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { PreventUnsavedChangesGuard } from './prevent-unsaved-changes.guard';
// import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'members', component: MemberListComponent},
  {path: 'members/:username', component: MemberDetailComponent}, 
  {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]}, 
  // , canActivate: [AuthGuard]
  {path: 'app-lists', component: ListsComponent},
  {path: 'app-messages', component: MessagesComponent},
  {path: 'app-errors', component: TestErrosComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'},
  
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
