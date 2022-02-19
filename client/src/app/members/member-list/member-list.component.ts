import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  member: Member;
  pagination : Pagination;
  userParams: UserParams;
  user: User;
  genderList = [{value: 'male', display: 'Males'}, {value: 'male', display: 'Females'}]
  // pageNumber = 1;
  // pageSize = 5;

  // members$: Observable<Member[]>
  constructor(private memberService: MembersService, private accountService: AccountService) {
    accountService.currentUser$.pipe(take(1)).subscribe(user =>
      {
      this.user = user;
      console.log(this.user)
      this.userParams = new UserParams(this.user);
      })
   }

  ngOnInit(): void {
    this.loadMembers();
  //  this.members$ = this.memberService.getMembers()

  //  console.log(this.members$)
 

  }

  pageChanged(event: any){
    // this.pageNumber = event.page;
    this.userParams.pageNumber = event.page;
    this.loadMembers()
  }

  resetFilters(){
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }

  loadMembers(){
    // this.userParams = new UserParams(this.user);
    // this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(response =>
    this.memberService.getMembers(this.userParams).subscribe(response =>
      {
        this.members = response.result;
        this.pagination = response.Pagination;
      })
    // this.memberService.getMembers().subscribe(members => {
    //   this.members = members;
    //   console.log(this.members)
    // })
  }


}
