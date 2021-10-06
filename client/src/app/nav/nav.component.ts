import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
// import * as internal from 'stream';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  // loggedIn:boolean | undefined; 
   currentUser$: Observable<User>; //using this method rather loggedIn
  
  
  // constructor(private accountService: AccountService) 
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) 
  { }

  ngOnInit(): void { 
    // this.getCurrentUser();
    //Using Observable currentUser$ directly in nav.components instead of usin getCurrentUser()
    // this.currentUser$ = this.accountService.currentUser$;     --making AccountService public and using it without much coding
  }
  login(){
    this.accountService.login(this.model).subscribe(response =>
      {
        this.router.navigateByUrl('/members')
        console.log(response);
        // this.loggedIn = true;
      },
       error =>
      {
        console.log(error);
        this.toastr.error(error.error);
        
      }
      )
    console.log(this.model);
  }
  logout(){
    this.accountService.logout();
    // this.loggedIn= false;
    this.router.navigateByUrl('/')

  }
//Using Observable currentUser$ directly in nav.components instead of usin getCurrentUser()
  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe(user=>
  //     {
  //       this.loggedIn = !!user;
  //     },
  //     error =>
  //     {
  //       console.log(error);
  //     }
  //     );
  //     // console.log(user)
  //     console.log(this.loggedIn);
  // }
}
